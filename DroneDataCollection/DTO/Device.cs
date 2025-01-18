using System.Data;
using System.Data.Common;

namespace DroneDataCollection;

public class Device {

    public int id { get; set; }

    public string host_name { get; set; } = String.Empty;

    public bool deleted { get; set; }

    public DateTime synchronization_time { get; set; }

    // 默认构造函数
    public Device() {
    }

    // 从MySqlDataReader构造的构造函数
    public Device(DbDataReader reader) {
        id = reader.GetInt32("id");
        host_name = reader.GetString("host_name");

        deleted = !reader.IsDBNull("deleted") && reader.GetBoolean("deleted");

        synchronization_time = reader.IsDBNull("synchronization_time")
            ? default
            : reader.GetDateTime("synchronization_time");
    }

}
