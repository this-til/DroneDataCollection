using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DroneDataCollection;

public partial class ConfigService : ObservableObject {

    [ObservableProperty]
    public partial UserConfig userConfig { get; set; } = new UserConfig();

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

public partial class UserConfig : ObservableObject {

    [ObservableProperty]
    public partial string host { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string database { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string port { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string user { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string password { get; set; } = string.Empty;

}
