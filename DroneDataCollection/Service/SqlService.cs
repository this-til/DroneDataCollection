using System.Diagnostics;
using System.Windows;
using HandyControl.Controls;
using log4net;
using Microsoft.Data.SqlClient;

namespace DroneDataCollection;

public partial class SqlService {

    public ILog log = LogManager.GetLogger(typeof(SqlService));

    public Visibility connectDatabaseButtonVisibility => sqlConnection is null
        ? Visibility.Visible
        : Visibility.Hidden;

    public Visibility closeDatabaseButtonVisibility => sqlConnection is null
        ? Visibility.Hidden
        : Visibility.Visible;

    public SqlConnection? sqlConnection;

    public void connectDatabase(string connectionString) {
        try {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
        catch (Exception exception) {
            log.Error("数据库连接失败，错误信息：", exception);
            sqlConnection = null;
            Growl.Error("数据库连接失败，错误信息：" + exception.Message);
        }
    }

    public void closeDatabase() {
        if (sqlConnection is null) {
            return;
        }
        sqlConnection.Close();
        sqlConnection.Dispose();
        sqlConnection = null;
    }

}
