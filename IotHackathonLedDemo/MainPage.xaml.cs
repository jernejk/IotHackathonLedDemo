using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IotHackathonLedDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SolidColorBrush ledOn = new SolidColorBrush(Colors.Green);
        private SolidColorBrush ledOff = new SolidColorBrush(Colors.Gray);

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += PageLoaded;
        }

        private void PageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Set to initial value
            ToggleLed(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Toggle between on and off
            ToggleLed(1);
        }

        /// <summary>
        /// Helper method for switching LEDs
        /// </summary>
        /// <param name="ledId">LED ID</param>
        private void ToggleLed(int ledId)
        {
            switch (ledId)
            {
                case 1:
                    ToggleLed(LedIndicator1);
                    break;
            }
        }
        
        private void ToggleLed(Ellipse rect)
        {
            var newState = rect.Fill == ledOn ? false : true;

            // Change color of LED indicator.
            rect.Fill = newState ? ledOn : ledOff;
        }
    }
}
