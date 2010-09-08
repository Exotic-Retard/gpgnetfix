namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG.Logging;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class LuaUtil
    {
        public static string ModToXml(string modFile, out string errors)
        {
            try
            {
                string str = AppDomain.CurrentDomain.BaseDirectory + @"lua\Mod2Xml2.lua";
                string arguments = string.Format("\"{0}\" \"{1}\"", str, modFile);
                string xmlString = "";
                string err = "";
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "lua.exe", arguments);
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;
                process.StartInfo = info;
                process.OutputDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    xmlString = xmlString + e.Data;
                };
                process.ErrorDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    err = err + e.Data;
                };
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                errors = err;
                return xmlString;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                errors = exception.Message;
                return null;
            }
        }

        public static string ScenarioToXml(string scenarioFile)
        {
            string str;
            return ScenarioToXml(scenarioFile, out str);
        }

        public static string ScenarioToXml(string scenarioFile, out string errors)
        {
            try
            {
                string str = AppDomain.CurrentDomain.BaseDirectory + @"lua\Scenario2Xml.lua";
                string arguments = string.Format("\"{0}\" \"{1}\"", str, scenarioFile);
                string xmlString = "";
                string err = "";
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "lua.exe", arguments);
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;
                process.StartInfo = info;
                process.OutputDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    xmlString = xmlString + e.Data;
                };
                process.ErrorDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    err = err + e.Data;
                };
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                errors = err;
                return xmlString;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                errors = exception.Message;
                return null;
            }
        }

        public static string ValidateMod(string modFile, string guid, int version, out string errors)
        {
            try
            {
                string str = AppDomain.CurrentDomain.BaseDirectory + @"lua\VaultifyMod.lua";
                string arguments = string.Format("\"{0}\" \"{1}\" {2} {3}", new object[] { str, modFile, guid, version });
                string output = "";
                string err = "";
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "lua.exe", arguments);
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;
                process.StartInfo = info;
                process.OutputDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    if ((e.Data != null) && (e.Data.Length > 0))
                    {
                        output = output + e.Data + "\r\n";
                    }
                };
                process.ErrorDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    if ((e.Data != null) && (e.Data.Length > 0))
                    {
                        err = err + e.Data + "\r\n";
                    }
                };
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                errors = err;
                return output;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                errors = exception.Message;
                return null;
            }
        }

        public static bool VerifyScenario(string scenarioFile, out string errors)
        {
            try
            {
                string str = AppDomain.CurrentDomain.BaseDirectory + @"lua\VerifyScenario.lua";
                string arguments = string.Format("\"{0}\" \"{1}\"", str, scenarioFile);
                string output = "";
                string err = "";
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "lua.exe", arguments);
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;
                process.StartInfo = info;
                process.OutputDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    output = output + e.Data + "\r\n";
                };
                process.ErrorDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    if ((e.Data != null) && (e.Data.Length > 0))
                    {
                        err = err + e.Data + "\r\n";
                    }
                };
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                errors = err + output;
                return (process.ExitCode < 1);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                errors = exception.Message;
                return false;
            }
        }

        public static string VersionScenario(string scenarioFile, int version)
        {
            string str;
            return VersionScenario(scenarioFile, version, out str);
        }

        public static string VersionScenario(string scenarioFile, int version, out string errors)
        {
            try
            {
                string str = AppDomain.CurrentDomain.BaseDirectory + @"lua\VaultifyScenario.lua";
                string arguments = string.Format("\"{0}\" \"{1}\" {2}", str, scenarioFile, version);
                string output = "";
                string err = "";
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "lua.exe", arguments);
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;
                process.StartInfo = info;
                process.OutputDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    if ((e.Data != null) && (e.Data.Length > 0))
                    {
                        output = output + e.Data + "\r\n";
                    }
                };
                process.ErrorDataReceived += delegate (object s, DataReceivedEventArgs e) {
                    if ((e.Data != null) && (e.Data.Length > 0))
                    {
                        err = err + e.Data + "\r\n";
                    }
                };
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                errors = err;
                return output;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                errors = exception.Message;
                return null;
            }
        }
    }
}

