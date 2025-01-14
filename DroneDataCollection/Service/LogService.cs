using System.IO;
using System.Text;
using HandyControl.Controls;
using log4net.Appender;
using log4net.Config;
using log4net.Core;

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

public class MemoryAppender : AppenderSkeleton {

    private StringBuilder stringBuilder;

    private TextWriter textWriter;

    public MemoryAppender() {
        stringBuilder = new StringBuilder();
        textWriter = new StringWriter(stringBuilder);

    }

    protected override void Append(LoggingEvent loggingEvent) {
        Layout?.Format(textWriter, loggingEvent);
        string? exceptionString = loggingEvent.GetExceptionString();
        if (exceptionString is not null) {
            stringBuilder.Append(exceptionString);
        }
        string item = stringBuilder.ToString().Trim();
        MainWindow.mainWindow.consolePanel.Dispatcher.Invoke(() => MainWindow.mainWindow.consolePanel.logMessage.Add(item));
        stringBuilder.Clear();
    }

}

public class GrowlAppender : AppenderSkeleton {

    protected override void Append(LoggingEvent loggingEvent) {
        if (loggingEvent.Level is null) {
            return;
        }
        if (loggingEvent.Level.Value >= Level.Error.Value) {
            Growl.ErrorGlobal(loggingEvent.RenderedMessage + (loggingEvent.ExceptionObject as Exception)?.Message);
        }
        else if (loggingEvent.Level.Value >= Level.Warn.Value) {
            Growl.WarningGlobal(loggingEvent.RenderedMessage + (loggingEvent.ExceptionObject as Exception)?.Message);
        }

    }

}
