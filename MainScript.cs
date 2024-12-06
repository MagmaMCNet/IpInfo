using Spectre.Console;
using static IpInfo.Functions;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.Versioning;

namespace IpInfo;

[SupportedOSPlatform("windows")]
internal class MainScript
{
    static void Main(string[] ConsoleArgs)
    {
        if (ConsoleArgs.Length <= 0)
        {
            AnsiConsole.Write(new FigletText("IpInfo").Color(Color.Orange1).Centered());
            AnsiConsole.MarkupLine("[cyan]IPV4[/]:[yellow] " + GetIPV4() + "[/]");
            AnsiConsole.MarkupLine("[cyan]Gateway[/]:[yellow] " + GetDefaultGateway() + "[/]");
            AnsiConsole.Write("\n\n");
            AnsiConsole.MarkupLine("[orange1]Get [cyan]Ipv4[/][/]:[yellow] " + AppDomain.CurrentDomain.FriendlyName + ".exe ipv4" + "[/]");
            AnsiConsole.MarkupLine("[orange1]Get [cyan]Gateway[/][/]:[yellow] " + AppDomain.CurrentDomain.FriendlyName + ".exe gateway" + "[/]");
            AnsiConsole.MarkupLine("[orange1]Add To [cyan]Path[/][/]:[yellow] " + AppDomain.CurrentDomain.FriendlyName + ".exe install" + "[/]");
            Console.ReadKey();
            return;
        }
        if (ConsoleArgs.Contains("ipv4"))
            Console.WriteLine(GetIPV4());
        else if (ConsoleArgs.Contains("gateway"))
            Console.WriteLine(GetDefaultGateway());
        else if (ConsoleArgs.Contains("install"))
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(AppDomain.CurrentDomain.FriendlyName + ".exe", "install");
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                Process.Start(processStartInfo);
                Environment.Exit(0);
                return;
            }
            string LocalAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            if (!Directory.Exists($@"{LocalAppdata}\MMC_Installed\"))
                Directory.CreateDirectory($@"{LocalAppdata}\MMC_Installed\");

            File.Copy(AppDomain.CurrentDomain.FriendlyName + ".exe", $@"{LocalAppdata}\MMC_Installed\ipinfo.exe", true);

            var EVScope = EnvironmentVariableTarget.Machine;
            var Path = Environment.GetEnvironmentVariable("PATH", EVScope);
            var NewPath = Path + $@";{LocalAppdata}\MMC_Installed\";

            Environment.SetEnvironmentVariable("PATH", NewPath, EVScope);

            AnsiConsole.MarkupLine("[orange1]Installed IpInfo[/]");
            Thread.Sleep(2000);
        }
    }
}