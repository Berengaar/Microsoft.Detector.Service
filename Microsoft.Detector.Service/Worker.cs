using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Detector.Service
{
    public class Worker : BackgroundService
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        private string GetActiveScreen()
        {
            IntPtr hWnd = GetForegroundWindow();
            const int nChars = 256;
            StringBuilder title = new StringBuilder(nChars);

            if (GetWindowText(hWnd, title, nChars) > 0)
            {
                return title.ToString();
            }

            return string.Empty;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!Directory.Exists(AppSettings.logDirectoryPath))
                    {
                        Directory.CreateDirectory(AppSettings.logDirectoryPath);
                    }
                    if (!File.Exists(AppSettings.logFilePath))
                    {
                        using (File.Create(AppSettings.logFilePath)) { }
                    }
                    string activeScreen = GetActiveScreen();
                    if (!string.IsNullOrEmpty(activeScreen))
                    {
                        Console.WriteLine($"Aktif pencere: {activeScreen}");

                        // Log writer
                        using (TextWriter tw = new StreamWriter(AppSettings.logFilePath, true))
                        {
                            tw.WriteLine($"{DateTime.Now}: {activeScreen}");
                        }
                    }

                    // Each 5 seconds
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally { }
                
            }
        }
    }
}
