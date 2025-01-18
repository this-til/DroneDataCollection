using Window = System.Windows.Window;

namespace DroneDataCollection;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    public static MainWindow mainWindow { get; private set; } = null!;


    public MainWindow() {
        mainWindow = this;
        InitializeComponent();
        this.DataContext = this;
    }
    

}
