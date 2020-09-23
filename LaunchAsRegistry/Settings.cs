using FSTools;

namespace LaunchAsRegistry {
    public class Settings {
        private PersistentSettings persistentSettings;

        public Settings() {
            persistentSettings = new PersistentSettings();
            Load();
        }

        public string ApplicationFilePath { get; set; }

        public string Arguments { get; set; }

        public string WorkingFolderPath { get; set; }

        public string RegFilePath { get; set; }

        public string ShortcutName { get; set; }

        public bool OneInstance { get; set; }

        public bool DisableThemes { get; set; }

        private void Load() {
            ApplicationFilePath = persistentSettings.Load("Path", ApplicationFilePath);
            Arguments = persistentSettings.Load("Arguments", Arguments);
            WorkingFolderPath = persistentSettings.Load("Folder", WorkingFolderPath);
            RegFilePath = persistentSettings.Load("RegFile", RegFilePath);
            ShortcutName = persistentSettings.Load("Shortcut", ShortcutName);
            OneInstance = persistentSettings.Load("OneInstance", OneInstance);
            DisableThemes = persistentSettings.Load("DisableThemes", DisableThemes);
        }

        public void Save() {
            persistentSettings.Save("Path", ApplicationFilePath);
            persistentSettings.Save("Arguments", Arguments);
            persistentSettings.Save("Folder", WorkingFolderPath);
            persistentSettings.Save("RegFile", RegFilePath);
            persistentSettings.Save("Shortcut", ShortcutName);
            persistentSettings.Save("OneInstance", OneInstance);
            persistentSettings.Save("DisableThemes", DisableThemes);
        }

        public bool RenderWithVisualStyles { get; set; }
    }
}
