using System.IO;

namespace DroneDataCollection;

public class ConfigService {

    public UserConfig userConfig { get; set; } = new UserConfig();

    public FileInfo fileInfo { get; } = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/Config/user.yaml");

    public ConfigService() {
        App.instance.Startup += (sender, args) => { loadConfig(); };
    }

    public void loadConfig() {
        DirectoryInfo? directoryInfo = fileInfo.Directory;

        if (directoryInfo is null) {
            throw new AggregateException($" file {fileInfo.FullName} does not directory");
        }

        if (!directoryInfo.Exists) {
            directoryInfo.Create();
        }

        if (!fileInfo.Exists) {
            fileInfo.Create();
        }

        using StreamReader streamReader = fileInfo.OpenText();
        userConfig = App.instance.yamlService.deserializer.Deserialize<UserConfig>(streamReader) ?? new UserConfig();
    }

    public void saveConfig() {

        DirectoryInfo? directoryInfo = fileInfo.Directory;

        if (directoryInfo is null) {
            throw new AggregateException($" file {fileInfo.FullName} does not directory");
        }

        if (!directoryInfo.Exists) {
            directoryInfo.Create();
        }

        if (!fileInfo.Exists) {
            fileInfo.Create();
        }

        using (File.Create(fileInfo.FullName)) {
        }

        using StreamWriter streamWriter = fileInfo.CreateText();

        App.instance.yamlService.serializer.Serialize(streamWriter, userConfig, typeof(UserConfig));

    }

}

public class UserConfig {

    public string host = string.Empty;

    public string database = string.Empty;

    public string port = string.Empty;

    public string user = string.Empty;

    public string password = string.Empty;

}
