using System.Data;
using MySql.Data.MySqlClient;

namespace DroneDataCollection;

public class Device {

    public int Id { get; set; }

    public string HostName { get; set; }

    public int? SynchronizationDate { get; set; }

    public int? SynchronizationTime { get; set; }

    // 默认构造函数
    public Device() {
    }

    // 从MySqlDataReader构造的构造函数
    public Device(MySqlDataReader reader) {
        Id = reader.GetInt32("id");
        HostName = reader.GetString("host_name");
        
        SynchronizationDate = reader.IsDBNull("synchronization_date")
            ? null
            : reader.GetInt32("synchronization_date");
        
        SynchronizationTime = reader.IsDBNull("synchronization_time")
            ? null
            : reader.GetInt32("synchronization_time");
    }

}
