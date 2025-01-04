using System.IO;
using log4net.Config;

namespace DroneDataCollection;

public class LogService {

    public LogService() {
        FileInfo logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/Config/log4net.config");
        XmlConfigurator.ConfigureAndWatch(logCfg);
    }

}
