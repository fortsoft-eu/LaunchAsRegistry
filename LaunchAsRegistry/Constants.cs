namespace LaunchAsRegistry {
    public static class Constants {
        public const string ExtensionExe = ".exe";
        public const string ExtensionLnk = ".lnk";
        public const string ExtensionReg = ".reg";

        public const string NotepadExeFileName = "notepad.exe";
        public const string RegeditExeFileName = "regedit.exe";
        public const string ErrorLogFileName = "error.log";
        public const string ErrorLogTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public const string EmDash = "—";
        public const string EnDash = "–";
        public const string Underscore = "_";
        public const string Space = " ";

        public const string ExampleApplicationFilePath = "C:\\Program Files\\Example Application\\example.exe";
        public const string ExampleWorkingFolderPath = "C:\\Program Files\\Example Application";
        public const string ExampleApplicationArguments = "msiexec /i \"C:\\Program Files\\Example Application\\example.msi\" INSTALLLEVEL=3 /l* msi.log PROPERTY=\"Embedded \"\"Quotes\"\" White Space\"";
        public const string ExampleRegFilePath = "C:\\Program Files\\Example Application\\example.reg";
    }
}
