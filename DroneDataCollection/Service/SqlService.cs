﻿using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using HandyControl.Controls;
using log4net;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DroneDataCollection;

public partial class SqlService : DependencyObject {

    public ILog log = LogManager.GetLogger(typeof(SqlService));

    public MySqlConnection? sqlConnection { get; set; }

    public event Action? linkedDatabaseEvent;

    public event Action? closeConnectionDatabaseEvent;

    public void connectDatabase(string connectionString) {
        if (sqlConnection is not null) {
            return;
        }
        log.Info($"try Connecting to database: {connectionString}");
        try {
            MySqlConnection sqlConnection = new MySqlConnection(connectionString);
            sqlConnection.Open();
            log.Info($"Connected to database");
            Growl.SuccessGlobal("连接数据库成功。");
            this.sqlConnection = sqlConnection;
            linkedDatabaseEvent?.Invoke();
        }
        catch (Exception exception) {
            log.Error("数据库连接失败，错误信息：", exception);
        }
    }

    public void closeDatabase() {
        if (sqlConnection is null) {
            return;
        }
        sqlConnection.Close();
        sqlConnection.Dispose();
        sqlConnection = null;
        closeConnectionDatabaseEvent?.Invoke();
        log.Info($"Closed database");
        Growl.SuccessGlobal("数据库链接已关闭。");
    }

    public async Task query(string sql, Action<DbDataReader> readerAction) {
        if (sqlConnection is null) {
            throw new InvalidOperationException("sqlConnection is null");
        }
        await using MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);

        DbDataReader reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            readerAction(reader);
        }

    }

    public async Task<List<C>> query<C>(string sql) where C : class {
        List<C> result = new List<C>();
        await query(sql, reader => result.Add((C)Activator.CreateInstance(typeof(C), reader)!));
        return result;
    }

}
