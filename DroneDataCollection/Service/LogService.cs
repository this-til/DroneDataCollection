using System.IO;
using log4net.Config;

namespace DroneDataCollection;

public class LogService {

    public LogService() {

        //获取配置文件全称
        string str = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".Config.log4net.config";
        //读取配置文件
        Stream? stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(str);
        XmlConfigurator.Configure(stream!);
    }

}
