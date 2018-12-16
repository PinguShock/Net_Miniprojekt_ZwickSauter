using System.Windows;

namespace AutoReservation.GUI {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    /// 


    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
        }

        private void AppOnStartup(object sender, StartupEventArgs e) {
            var screen = new SplashScreen("media/SplashScreen2.jpg");
            screen.Show(true);
        }

    }
}
