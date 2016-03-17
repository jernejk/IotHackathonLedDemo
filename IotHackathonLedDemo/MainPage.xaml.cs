using Windows.Devices.Gpio;
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

        private GpioController controller;
        private GpioPin ledPin1;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += PageLoaded;
        }

        private void PageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            controller = GpioController.GetDefault();
            if (controller != null)
            {
                ledPin1 = InitLed(21);
            }

            // Set to initial value
            ToggleLed(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Toggle between on and off
            ToggleLed(1);
        }

        private GpioPin InitLed(int pinNumber)
        {
            var ledPin = controller.OpenPin(pinNumber);
            ledPin.SetDriveMode(GpioPinDriveMode.Output);

            return ledPin;
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
                    ToggleLed(LedIndicator1, ledPin1);
                    break;
            }
        }
        
        private void ToggleLed(Ellipse rect, GpioPin ledPin)
        {
            var newState = rect.Fill == ledOn ? false : true;

            // Change color of LED indicator.
            rect.Fill = newState ? ledOn : ledOff;

            if (ledPin != null)
            {
                ledPin.Write(newState ? GpioPinValue.Low : GpioPinValue.High);
            }
        }

        private void Grid_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            ToggleLed(1);
        }
    }
}
