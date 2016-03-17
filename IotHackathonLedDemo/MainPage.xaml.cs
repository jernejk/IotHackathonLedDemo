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
        private GpioPin ledPin2;
        private GpioPin ledPin3;

        private PushBulletHelper pushBullet = new PushBulletHelper();

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
                ledPin2 = InitLed(20);
                ledPin3 = InitLed(26);
            }

            // Set to initial value
            for (int i = 1; i <= 3; ++i)
            {
                ToggleLed(i);
            }

            pushBullet.ToggleLed += ToggleLed;
            pushBullet.Start();
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

                case 2:
                    ToggleLed(LedIndicator2, ledPin2);
                    break;

                case 3:
                    ToggleLed(LedIndicator3, ledPin3);
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
            int key = (int)e.Key;
            if (key >= 48 && key <= 57)
            {
                // Keys 0-9 above character keys (present also on laptops)
                ToggleLed(key - 48);
            }
            else if (key >= 96 && key <= 105)
            {
                // Keys 0-9 on numeric keyboard
                ToggleLed(key - 96);
            }
        }
    }
}
