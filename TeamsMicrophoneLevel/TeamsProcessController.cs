using System;
using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;

namespace TeamsMicrophoneLevel
{
    internal static class TeamsProcessController
    {
        /// <summary>
        /// Start teams using the debugging port specified.
        /// Close and re-open if not using the port wanted.
        /// </summary>
        /// <remarks>
        public static void StartTeams(int debuggingPort)
        {
            var port = GetTeamsDebugPort();
            if (port == debuggingPort)
            {
                return;
            }

            KillTeamsProcesses();
            StartTeamsProcess(debuggingPort);
        }

        /// <summary>
        /// Start teams using the debugging port specified, or return the debugging port in use.
        /// </summary>
        /// <remarks>
        /// Leaves the existing teams process running, and returns the used port if it is already
        /// running in debug mode.
        /// </remarks>
        public static int StartOrCheckTeams(int debuggingPort)
        {
            var port = GetTeamsDebugPort();
            if (port != null)
            {
                // already running in debug mode
                return port.Value;
            }

            // no teams processes / not running in debug mode
            KillTeamsProcesses();
            StartTeamsProcess(debuggingPort);
            return debuggingPort;
        }

        /// <summary>
        /// Try to get the debug port of the teams process.
        /// </summary>
        /// <returns>Null if no teams processes or not running with the debug port</returns>
        public static int? GetTeamsDebugPort()
        {
            int? debugPort = null;
            foreach (var process in Process.GetProcessesByName(Constants.TeamsProcessName))
            {
                var commandLine = GetCommandLine(process);
                var port = ParseDebugPort(commandLine);
                if (port != null)
                {
                    debugPort = port;
                    break;
                }
            }

            return debugPort;
        }

        /// <summary>
        /// Get the command line of a process (using WMI)
        /// </summary>
        private static string? GetCommandLine(Process process)
        {
            using var searcher = new ManagementObjectSearcher($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}");
            using var objects = searcher.Get();
            return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
        }

        /// <summary>
        /// Try to parse out the debug port from a (teams.exe) process command line
        /// </summary>
        /// <returns>Null if debug port argument is not present</returns>
        private static int? ParseDebugPort(string? commandLine)
        {
            if (commandLine == null)
            {
                return null;
            }

            var match = Regex.Match(commandLine, "--remote-debugging-port=([0-9]+)");
            if (!match.Success || match.Groups.Count != 2)
            {
                return null;
            }

            var portString = match.Groups[1].Value;
            if (!int.TryParse(portString, out int port))
            {
                return null;
            }

            return port;
        }
    
        /// <summary>
        /// Kill all teams processes.
        /// </summary>
        private static void KillTeamsProcesses()
        {
            var processes = Process.GetProcessesByName(Constants.TeamsProcessName);
            while (processes.Any())
            {
                TryKill(processes.First());

                // yield, then update list
                Thread.Sleep(0);
                processes = Process.GetProcessesByName(Constants.TeamsProcessName);
            }

        }

        /// <summary>
        /// Try to kill the process and descendents. Ignore all exceptions.
        /// </summary>
        /// <remarks>
        /// Waits for the main process to exit, child processes may still be running after exit.
        /// </remarks>
        private static void TryKill(Process process)
        {
            try
            {
                process.Kill(true);
                process.WaitForExit();
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// Start a new instance of teams in debug mode.
        /// </summary>
        public static void StartTeamsProcess(int debuggingPort)
        {
            var processPath = Environment.ExpandEnvironmentVariables(Constants.TeamsExecutablePath);
            Process.Start(processPath, $"--remote-debugging-port={debuggingPort}");
        }
    }
}
