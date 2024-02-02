using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Query;
using WindowsGSM.GameServer.Engine;

namespace WindowsGSM.Plugins {
  public class PalworldServerInject : SteamCMDAgent { // SteamCMDAgent is used because Palworld relies on SteamCMD for installation and update process

    // Plugin Details
    public Plugin Plugin = new Plugin {
      name = "WindowsGSM.PalworldServerInject", // WindowsGSM.XXXX
      author = "ohmcodes, JourneyOver",
      description = "WindowsGSM plugin for supporting Palworld Dedicated Server",
      version = "1.0",
      url = "https://github.com/JourneyOver/WindowsGSM.PalworldServerInject", // Github repository link (Best practice)
      color = "#1E8449" // Color Hex
    };

    // Settings properties for SteamCMD installer
    public override bool loginAnonymous => true; // Palworld does not require a steam account to install the server, so loginAnonymous = true
    public override string AppId => "2394010"; // Game server appId

    // Standard Constructor and properties
    public PalworldServerInject(ServerConfig serverData) : base(serverData) => base.serverData = _serverData = serverData;
    private readonly ServerConfig _serverData; // Store server start metadata, such as start ip, port, start param, etc
    public string Error, Notice;

    // Game server Fixed variables
    public override string StartPath =>  @"Pal\Binaries\Win64\PalServerInject.exe"; // Game server start path
    public string FullName = "Palworld Dedicated Server (Modded)"; // Game server FullName
    public bool AllowsEmbedConsole = false;  // Does this server support output redirect?
    public int PortIncrements = 1; // This tells WindowsGSM how many ports should skip after installation
    public object QueryMethod = new A2S(); // Query method should be use on current server type. Accepted value: null or new A2S() or new FIVEM() or new UT3()

    // Game server default values
    public string ServerName = "Palworld"; // Default server name
    public string Port = "8211"; // Default port
    public string QueryPort = "8212"; // Default query port
    public string Defaultmap = "MainWorld5"; // Default map name
    public string Maxplayers = "32"; // Default maxplayers
    public string Additional = "EpicApp=PalServer -useperfthreads -NoAsyncLoadingThread -UseMultithreadForDS"; // Additional server start parameter

    private Dictionary<string, string> configData = new Dictionary<string, string>();

    // Create a default cfg for the game server after installation
    public async void CreateServerCFG() {
      // Implementation of creating a default CFG for the game server after installation
    }

    // Start server function, return its Process to WindowsGSM
    public async Task<Process> Start() {
      // Path to the game server executable
      string shipExePath = Functions.ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath);
      if (!File.Exists(shipExePath)) {
        Error = $"{Path.GetFileName(shipExePath)} not found ({shipExePath})";
        return null;
      }

      string param = $" {_serverData.ServerParam} ";
      param += $"-publicip=\"{_serverData.ServerIP}\" ";
      param += $"-port={_serverData.ServerPort} ";
      param += $"-publicport={_serverData.ServerPort} ";
      param += $"-queryport={_serverData.ServerQueryPort} ";
      param += $"-publicqueryport={_serverData.ServerQueryPort} ";
      param += $"-players={_serverData.ServerMaxPlayer} ";
      param += $"-servername=\"\"\"{_serverData.ServerName}\"\"\"";


      // Prepare Process
      var p = new Process {
        StartInfo = {
          WorkingDirectory = ServerPath.GetServersServerFiles(_serverData.ServerID, @"Pal\Binaries\Win64"),
          FileName = shipExePath,
          Arguments = param.ToString(),
          WindowStyle = ProcessWindowStyle.Hidden,
          UseShellExecute = false
        },
        EnableRaisingEvents = true
      };

      // Set up Redirect Input and Output to WindowsGSM Console if EmbedConsole is on
      if (AllowsEmbedConsole) {
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        var serverConsole = new ServerConsole(_serverData.ServerID);
        p.OutputDataReceived += serverConsole.AddOutput;
        p.ErrorDataReceived += serverConsole.AddOutput;
      }

      // Start Process
      try {
        p.Start();
        if (AllowsEmbedConsole) {
          p.BeginOutputReadLine();
          p.BeginErrorReadLine();
        }

        return p;
      } catch (Exception e) {
        Error = e.Message;
        return null; // return null if fail to start
      }
    }

    // Stop server function
    public async Task Stop(Process p) {
        // Send a Ctrl+C command to the main window of the provided process
        await Task.Run(() => {
            Functions.ServerConsole.SetMainWindow(p.MainWindowHandle);
            Functions.ServerConsole.SendWaitToMainWindow("^c");
        });
    
        // Wait for a short duration
        await Task.Delay(2000);
    }


    // Update server function
    public async Task<Process> Update(bool validate = false, string custom = null) {
      // Update the server using SteamCMD
      var (p, error) = await Installer.SteamCMD.UpdateEx(serverData.ServerID, AppId, validate, custom: custom, loginAnonymous: loginAnonymous);
      Error = error;
      await Task.Run(() => { p.WaitForExit(); });
      return p;
    }

    // Check if the installation is valid
    public bool IsInstallValid() {
      return File.Exists(Functions.ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath));
    }

    // Check if the import path is valid
    public bool IsImportValid(string path) {
      string exePath = Path.Combine(path, "PackageInfo.bin");
      Error = $"Invalid Path! Fail to find {Path.GetFileName(exePath)}";
      return File.Exists(exePath);
    }

    // Get the local build version
    public string GetLocalBuild() {
      var steamCMD = new Installer.SteamCMD();
      return steamCMD.GetLocalBuild(_serverData.ServerID, AppId);
    }

    // Get the remote build version
    public async Task<string> GetRemoteBuild() {
      var steamCMD = new Installer.SteamCMD();
      return await steamCMD.GetRemoteBuild(AppId);
    }
  }
}
