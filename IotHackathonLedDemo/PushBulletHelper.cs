using PushbulletSharp;
using PushbulletSharp.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IotHackathonLedDemo
{
    class PushBulletHelper
    {
        public const string Token = "<INSERT-YOUR-TOKEN>";

        private bool isRunning;
        private PushbulletClient client { get; set; }
        private string lastIden;

        public event Action<int> ToggleLed;

        public PushBulletHelper()
        {
            client = new PushbulletClient(Token);
        }

        public async void Start()
        {
            var filter = new PushResponseFilter
            {
                Limit = 1
            };

            isRunning = true;
            while (isRunning)
            {
                try
                {
                    var pushes = client.GetPushes(filter);

                    var push = pushes?.Pushes?.FirstOrDefault();
                    if (push != null && lastIden != push.Iden)
                    {
                        lastIden = push.Iden;

                        string startKeyword = "LED ";
                        if (push.Body.StartsWith(startKeyword, StringComparison.CurrentCultureIgnoreCase))
                        {
                            int ledId = 0;
                            string number = push.Body.Substring(startKeyword.Length);

                            if (int.TryParse(number, out ledId))
                            {
                                ToggleLed?.Invoke(ledId);
                            }
                        }
                        else if (push.Body.Length > 0 && (push.Body[0] >= '0' && push.Body[0] <= '9'))
                        {
                            int ledId = 0;
                            if (int.TryParse(push.Body, out ledId))
                            {
                                ToggleLed?.Invoke(ledId);
                            }
                        }
                        else if (push.Body.Equals("all", StringComparison.CurrentCultureIgnoreCase))
                        {
                            for (int i = 0; i < 10; ++i)
                            {
                                ToggleLed?.Invoke(i);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ToggleLed?.Invoke(1);
                    await Task.Delay(20000);
                }

                await Task.Delay(1000);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
