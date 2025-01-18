using CommunityToolkit.Mvvm.ComponentModel;

namespace DroneDataCollection;

public partial class Pack<V> : ObservableObject {

    [ObservableProperty]
    public partial V? value { get; set; }

    public Pack(V? value) {
        this.value = value;
    }

    public Pack() {
    }

}
