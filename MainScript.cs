using Spectre.Console;
using static IpInfo.Functions;
using MagmaMc.JEF;
using static IpInfo.MainScript;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace IpInfo
{
    internal class MainScript
    {
        public static _ConsoleInfo_ ConsoleInfo = new();
        public class _ConsoleInfo_
        {
            public bool DebugMode { get; set; } = false;
            public bool SentArgs { get; set; } = false;
        }
        static void MainLoop(string[] ConsoleArgs)
        {
            Task.Run(() => 
            {
                while(true)
                {
                    // Inf Loop No-Stop Code
                }
            });
        }
        static void Main(string[] ConsoleArgs)
        {
            MainLoop(ConsoleArgs);
            ConsoleInfo.DebugMode = (Debugger.IsAttached == true ? true : false);
            ConsoleInfo.SentArgs = (ConsoleArgs.Length > 0 ? true : false);


            // Main Program
            if (!ConsoleInfo.SentArgs)
            { // Ran Normaly
                AnsiConsole.Write(new FigletText("IpInfo").Color(Color.Orange1).Centered()); // title
                AnsiConsole.MarkupLine("[cyan]IPV4[/]:[yellow] " + GetIPV4() + "[/]"); // ipv4
                AnsiConsole.MarkupLine("[cyan]Gateway[/]:[yellow] " + GetDefaultGateway() + "[/]"); // gateway
                AnsiConsole.Write("\n\n");

                AnsiConsole.MarkupLine("[orange1]Get [cyan]Ipv4[/][/]:[yellow] " + AppDomain.CurrentDomain.FriendlyName + ".exe ipv4" + "[/]");
                AnsiConsole.MarkupLine("[orange1]Get [cyan]Gateway[/][/]:[yellow] " + AppDomain.CurrentDomain.FriendlyName + ".exe gateway" + "[/]");
                AnsiConsole.MarkupLine("[orange1]Add To [cyan]Path[/][/]:[yellow] " + AppDomain.CurrentDomain.FriendlyName + ".exe install" + "[/]");
                Console.ReadKey(); // Pause
            } else if (ConsoleInfo.SentArgs)
            { // Sent Arguments
                if (JEF.Lists.IsInList(ConsoleArgs.ToList(), "ipv4", false))
                {
                    Console.WriteLine(GetIPV4());
                }
                else if (JEF.Lists.IsInList(ConsoleArgs.ToList(), "gateway", false))
                {
                    Console.WriteLine(GetDefaultGateway());
                }
                else if (JEF.Lists.IsInList(ConsoleArgs.ToList(), "install", false))
                {
                    if (!JEF.Administrator.IsElevated())
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo(AppDomain.CurrentDomain.FriendlyName + ".exe", "install");
                        processStartInfo.Verb = "runas";
                        processStartInfo.UseShellExecute = true;
                        Process.Start(processStartInfo);
                        Environment.Exit(0);
                    }
                    else
                    {
                        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + $@"\MMC_Installed\"))
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + $@"\MMC_Installed\");

                        File.Copy(
                            AppDomain.CurrentDomain.FriendlyName + ".exe",
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + $@"\MMC_Installed\ipinfo.exe", true);

                        var scope = EnvironmentVariableTarget.Machine;
                        var Path = Environment.GetEnvironmentVariable("PATH", scope);
                        var NewPath = Path + ";" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + $@"\MMC_Installed\";
                        Environment.SetEnvironmentVariable("PATH", NewPath, scope);

                        AnsiConsole.MarkupLine("[orange1]Installed IpInfo[/]");
                        Thread.Sleep(2000);
                    }
                }
            }


            
        }
    }
}