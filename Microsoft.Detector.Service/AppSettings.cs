using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Detector.Service
{
    public static class AppSettings
    {
        public static string logDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft Detector");
        public static string logFilePath = Path.Combine(logDirectoryPath, "log.txt");
    }
}
