using System.Diagnostics;
using System.Threading;

namespace LaunchAsRegistry {
    public class LauncherAsRegistry {
        private string applicationFilePath, arguments, workingFolderPath, regFilePath;
        private bool oneInstance;
        private Process process1, process2;

        public LauncherAsRegistry() {
            process1 = new Process();
            process2 = new Process();
            process2.StartInfo.FileName = Constants.RegeditExeFileName;
        }

        public void Launch() {
            if (oneInstance) {
                if (FocusApplication.SwitchToRunningInstance(applicationFilePath)) {
                    return;
                }
            }
            process1.StartInfo.FileName = applicationFilePath;
            process1.StartInfo.Arguments = arguments;
            process1.StartInfo.WorkingDirectory = workingFolderPath;
            process2.StartInfo.Arguments = "/s " + ArgumentParser.EscapeArgument(regFilePath);
            process2.Start();
            do {
                Thread.Sleep(100);
            } while (!process2.HasExited);
            process1.Start();
        }

        public string ApplicationFilePath {
            get {
                return applicationFilePath;
            }
            set {
                applicationFilePath = value;
            }
        }

        public string Arguments {
            get {
                return arguments;
            }
            set {
                arguments = value;
            }
        }

        public string WorkingFolderPath {
            get {
                return workingFolderPath;
            }
            set {
                workingFolderPath = value;
            }
        }

        public bool OneInstance {
            get {
                return oneInstance;
            }
            set {
                oneInstance = value;
            }
        }

        public string RegFilePath {
            get {
                return regFilePath;
            }
            set {
                regFilePath = value;
            }
        }
    }
}
