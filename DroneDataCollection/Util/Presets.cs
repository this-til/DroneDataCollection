namespace DroneDataCollection;

public class Presets {

    public static List<string> fieldNames = new List<string> {
        "device_id",
        "time",
        "ux",
        "uy",
        "uz",
        "hx",
        "hy",
        "hz",
        "t",
        "y",
        "x",
        "interpret_y",
        "interpret_x",
        "h",
        "la_h",
        "gyro_1",
        "gyro_2",
        "gyro_3",
        "gps_1",
        "gps_2",
        "gps_3"
    };
    
    public static List<string> dataField = new List<string> {
        "ux",
        "uy",
        "uz",
        "hx",
        "hy",
        "hz",
        "t",
        "utc",
        "y",
        "x",
        "interpret_y",
        "interpret_x",
        "h",
        "la_h"
    };
    
    public static List<string> insertDataField = new List<string> {
        "ux",
        "uy",
        "uz",
        "hx",
        "hy",
        "hz",
        "t",
        "time",
        "y",
        "x",
        "interpret_y",
        "interpret_x",
        "h",
        "la_h"
    };
}
