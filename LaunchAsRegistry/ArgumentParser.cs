using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LaunchAsRegistry {
    public class ArgumentParser {
        private List<string> arguments;
        private string argumentString, applicationFilePath, applicationArguments, workingFolderPath, regFilePath;
        private bool expectingFilePath, expectingArguments, expectingFolderPath, expectingRegFilePath, filePathSet, argumentsSet, folderPathSet, regFilePathSet, helpSet, hasArguments, oneInstanceSet, thisTestSet;

        public ArgumentParser() {
            Reset();
        }

        private void Reset() {
            applicationFilePath = string.Empty;
            applicationArguments = string.Empty;
            workingFolderPath = string.Empty;
            regFilePath = string.Empty;
            expectingFilePath = false;
            expectingArguments = false;
            expectingFolderPath = false;
            expectingRegFilePath = false;
            filePathSet = false;
            argumentsSet = false;
            folderPathSet = false;
            regFilePathSet = false;
            helpSet = false;
            hasArguments = false;
            oneInstanceSet = false;
            thisTestSet = false;
        }

        private void Evaluate() {
            foreach (string arg in arguments) {
                string argument = arg;
                hasArguments = true;
                if (argument == "-i" || argument == "/i") {             //Input file path: Application to launch.
                    if (filePathSet || expectingFilePath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageI);
                    }
                    if (expectingArguments || expectingFolderPath || expectingRegFilePath || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingFilePath = true;
                } else if (argument == "-a" || argument == "/a") {      //Arguments passed to the launched application.
                    if (argumentsSet || expectingArguments) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageA);
                    }
                    if (expectingFilePath || expectingFolderPath || expectingRegFilePath || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingArguments = true;
                } else if (argument == "-w" || argument == "/w") {      //Working folder path.
                    if (folderPathSet || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageW);
                    }
                    if (expectingFilePath || expectingArguments || expectingRegFilePath || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingFolderPath = true;
                } else if (argument == "-r" || argument == "/r") {      //Registry file path.
                    if (regFilePathSet || expectingRegFilePath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageR);
                    }
                    if (expectingFilePath || expectingArguments || expectingFolderPath || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingRegFilePath = true;
                } else if (argument == "-o" || argument == "/o") {      //Allows only one instance.
                    if (oneInstanceSet || helpSet || thisTestSet || expectingFilePath || expectingArguments || expectingFolderPath || expectingRegFilePath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    oneInstanceSet = true;
                } else if (argument == "-h" || argument == "/h" || argument == "-?" || argument == "/?") {      //Will show help.
                    if (filePathSet || argumentsSet || folderPathSet || oneInstanceSet || helpSet || thisTestSet || expectingFilePath || expectingArguments || expectingRegFilePath || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    helpSet = true;
                } else if (argument == "-T" || argument == "/T") {      //Test mode (ArgumentParser test).
                    if (filePathSet || argumentsSet || folderPathSet || oneInstanceSet || helpSet || thisTestSet || expectingFilePath || expectingArguments || expectingRegFilePath || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    thisTestSet = true;
                } else if (expectingFilePath) {
                    applicationFilePath = argument;
                    expectingFilePath = false;
                    filePathSet = true;
                } else if (expectingArguments) {
                    applicationArguments = argument;
                    expectingArguments = false;
                    argumentsSet = true;
                } else if (expectingFolderPath) {
                    workingFolderPath = argument;
                    expectingFolderPath = false;
                    folderPathSet = true;
                } else if (expectingRegFilePath) {
                    regFilePath = argument;
                    expectingRegFilePath = false;
                    regFilePathSet = true;
                } else if (argument.StartsWith("-") || argument.StartsWith("/")) {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageU);
                } else {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                }
            }
            if (expectingFilePath || expectingArguments || expectingFolderPath || expectingRegFilePath) {
                throw new ApplicationException(Properties.Resources.ExceptionMessageM);
            }
        }

        public static string EscapeArgument(string argument) {
            argument = Regex.Replace(argument, @"(\\*)" + "\"", @"$1$1\" + "\"");
            return "\"" + Regex.Replace(argument, @"(\\+)$", @"$1$1") + "\"";
        }

        public bool HasArguments {
            get {
                return hasArguments;
            }
        }

        public bool OneInstance {
            get {
                return oneInstanceSet;
            }
        }

        public bool IsHelp {
            get {
                return helpSet;
            }
        }

        public bool IsThisTest {
            get {
                return thisTestSet;
            }
        }

        public string ApplicationFilePath {
            get {
                return applicationFilePath;
            }
        }

        public string WorkingFolderPath {
            get {
                return workingFolderPath;
            }
        }

        public string ApplicationArguments {
            get {
                return applicationArguments;
            }
        }

        public string RegFilePath {
            get {
                return regFilePath;
            }
        }

        public string[] Arguments {
            get {
                return arguments.ToArray();
            }
            set {
                Reset();
                arguments = new List<string>(value.Length);
                arguments.AddRange(value);
                try {
                    Evaluate();
                } catch (Exception exception) {
                    Reset();
                    throw exception;
                }
            }
        }

        public string ArgumentString {
            get {
                if (string.IsNullOrEmpty(argumentString) && arguments.Count > 0) {
                    return string.Join(Constants.Space, arguments);
                }
                return argumentString;
            }
            set {
                Reset();
                argumentString = value;
                arguments = Parse(argumentString);
                try {
                    Evaluate();
                } catch (Exception exception) {
                    Reset();
                    throw exception;
                }
            }
        }

        private static List<string> Parse(string str) {
            List<string> arguments = new List<string>();
            StringBuilder c = new StringBuilder();
            bool e = false, d = false, s = false;
            for (int i = 0; i < str.Length; i++) {
                if (!s) {
                    if (str[i] == ' ') {
                        continue;
                    }
                    d = str[i] == '"';
                    s = true;
                    e = false;
                    if (d) {
                        continue;
                    }
                }
                if (d) {
                    if (str[i] == '\\') {
                        if (i + 1 < str.Length && str[i + 1] == '"') {
                            c.Append(str[++i]);
                        } else {
                            c.Append(str[i]);
                        }
                    } else if (str[i] == '"') {
                        if (i + 1 < str.Length && str[i + 1] == '"') {
                            c.Append(str[++i]);
                        } else {
                            d = false;
                            e = true;
                        }
                    } else {
                        c.Append(str[i]);
                    }
                } else if (s) {
                    if (str[i] == ' ') {
                        s = false;
                        arguments.Add(e ? c.ToString() : c.ToString().TrimEnd(' '));
                        c = new StringBuilder();
                    } else if (!e) {
                        c.Append(str[i]);
                    }
                }
            }
            if (c.Length > 0) {
                arguments.Add(c.ToString());
            }
            return arguments;
        }
    }
}
