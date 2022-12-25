using System.Diagnostics;
using System.Threading;

namespace LaunchAsRegistry {
    public class LauncherAsRegistry {
        private Process process1, process2;

        public LauncherAsRegistry() {
            process1 = new Process();
            process2 = new Process();
            process2.StartInfo.FileName = Constants.RegeditExeFileName;
        }

        public void Launch() {
            if (OneInstance) {
                if (SingleInstance.FocusRunning(ApplicationFilePath)) {
                    return;
                }
            }
            process1.StartInfo.FileName = ApplicationFilePath;
            process1.StartInfo.Arguments = Arguments;
            process1.StartInfo.WorkingDirectory = WorkingFolderPath;
            process2.StartInfo.Arguments = "/s" + Constants.Space + ArgumentParser.EscapeArgument(RegFilePath);
            process2.Start();
            do {
                Thread.Sleep(100);
            } while (!process2.HasExited);
            process1.Start();
        }

        public bool OneInstance { get; set; }

        public string ApplicationFilePath { get; set; }

        public string Arguments { get; set; }

        public string RegFilePath { get; set; }

        public string WorkingFolderPath { get; set; }
    }
}
