
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace DroneDataCollection;


public class Data {

    public int id { get; set; }

    public int date { get; set; }

    public int time { get; set; }

    public int device_id { get; set; }

    public float? ux { get; set; } // 三轴磁通门电压

    public float? uy { get; set; } // 三轴磁通门电压

    public float? uz { get; set; } // 三轴磁通门电压

    public float? hx { get; set; } // 根据电压解算的磁场分量

    public float? hy { get; set; } // 根据电压解算的磁场分量

    public float? hz { get; set; } // 根据电压解算的磁场分量

    public float? t { get; set; } // 磁场总量

    public float? x { get; set; } // gps读数

    public float? y { get; set; } // gps读数

    public float? h { get; set; } // gps读数

    public float? x1 { get; set; } // 解算的坐标

    public float? x2 { get; set; } // 解算的坐标

    public float? la_h { get; set; } // 激光高度计

    public float? gyro_1 { get; set; } // 陀螺仪记录姿态数据

    public float? gyro_2 { get; set; } // 陀螺仪记录姿态数据

    public float? gyro_3 { get; set; } // 陀螺仪记录姿态数据

    public float? gps_1 { get; set; } // 飞机GPS数据

    public float? gps_2 { get; set; } // 飞机GPS数据

    public float? gps_3 { get; set; } // 飞机GPS数据

    // 默认构造函数

    public Data() {

    }

    // 使用MySqlDataReader作为参数的构造函数

    public Data(DbDataReader reader) {

        id = reader.GetInt32("id");
        
        date = reader.GetInt32("date");
        
        time = reader.GetInt32("time");

        device_id = reader.GetInt32("device_id");

        ux = reader.IsDBNull("ux")
            ? null
            : reader.GetFloat("ux");

        uy = reader.IsDBNull("uy")
            ? null
            : reader.GetFloat("uy");

        uz = reader.IsDBNull("uz")
            ? null
            : reader.GetFloat("uz");

        hx = reader.IsDBNull("hx")
            ? null
            : reader.GetFloat("hx");

        hy = reader.IsDBNull("hy")
            ? null
            : reader.GetFloat("hy");

        hz = reader.IsDBNull("hz")
            ? null
            : reader.GetFloat("hz");

        t = reader.IsDBNull("t")
            ? null
            : reader.GetFloat("t");

        x = reader.IsDBNull("x")
            ? null
            : reader.GetFloat("x");

        y = reader.IsDBNull("y")
            ? null
            : reader.GetFloat("y");

        h = reader.IsDBNull("h")
            ? null
            : reader.GetFloat("h");

        x1 = reader.IsDBNull("x1")
            ? null
            : reader.GetFloat("x1");

        x2 = reader.IsDBNull("x2")
            ? null
            : reader.GetFloat("x2");

        la_h = reader.IsDBNull("la_h")
            ? null
            : reader.GetFloat("la_h");

        gyro_1 = reader.IsDBNull("gyro_1")
            ? null
            : reader.GetFloat("gyro_1");

        gyro_2 = reader.IsDBNull("gyro_2")
            ? null
            : reader.GetFloat("gyro_2");

        gyro_3 = reader.IsDBNull("gyro_3")
            ? null
            : reader.GetFloat("gyro_3");

        gps_1 = reader.IsDBNull("gps_1")
            ? null
            : reader.GetFloat("gps_1");

        gps_2 = reader.IsDBNull("gps_2")
            ? null
            : reader.GetFloat("gps_2");

        gps_3 = reader.IsDBNull("gps_3")
            ? null
            : reader.GetFloat("gps_3");

    }

}
