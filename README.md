# WindowsGSM.PalworldServerInject Plugin

🧩 This WindowsGSM plugin simplifies the installation of Palworld Dedicated servers with mod support.

## WindowsGSM Installation:
1. Download [WindowsGSM](https://windowsgsm.com/).
2. Create a folder at your desired location for server installations and run WindowsGSM.Exe.
3. Drag WindowsGSM.Exe into the created folder and execute it.

## Plugin Installation:
1. Download the latest release.
2. Extract and move the **PalworldServerInject.cs** folder to the **plugins** folder.
   - OR -
   Press the Puzzle Icon in the bottom left corner, navigate to the plugin zip file, and install it.
3. Click the **[RELOAD PLUGINS]** button or restart WindowsGSM.
4. Navigate to "Servers," click "Install Game Server," and select "Palworld Dedicated Server (Modded) [PalworldServerInject.cs]."

## Mod Installation (Required before starting server):
1. Download [UE4SS Xinput](https://github.com/UE4SS-RE/RE-UE4SS/releases)
2. Download [Palworld-ServerInjector](https://github.com/N00byKing/PalWorld-ServerInjector/releases)
3. Extract `UE4SS Xinput` and copy the contents to the `Pal\Binaries\Win64` folder.
4. Extract `Palworld-ServerInjector` and copy only `PalServerInject.exe`, `UE4SS_Signatures` and `UE4SS.dll`
5. Grab any mods that will work on dedicated servers from [NexusMods/Palworld](https://www.nexusmods.com/palworld) and follow installation guides on nexus for each mod you install (pathing on where the mods need to be installed are different for various types of mods)

### Official Documentation
🗃️ [Palworld Dedicated Server Documentation](https://tech.palworldgame.com/dedicated-server-guide)

### The Game
🕹️ [Palworld on Steam](https://store.steampowered.com/app/1623730/Palworld/)

### Dedicated Server Info
🖥️ [SteamDB - Palworld Dedicated Server](https://steamdb.info/app/2394010/info/)

### Modding Documentation 
⚙️ [Palworld Modding Documentation](https://palworldforums.net/resources/how-to-run-mods-lua-pak-on-a-steam-client-and-dedicated-server-inject-character-into-server.8/)

### Port Forwarding
- 8211 UDP - Default
- 8212 TCP - If you are using 8211, it automatically uses 8211 +1 = 8212 for QueryPort, so you have to port forward this.
- RCONPort can be changed to anything; another forwarding TCP/UDP is required.

### Available Params
All these params are available to be set automatically by WGSM
- EpicApp=PalServer	            Set up server as a community server.
- -publicip=192.168.xxx.xxx     Usually the local port of the server (Change via WGSM settings)
- -publicport=8211              Cannot be changed and is not working (Change via WGSM settings)
- -port=8211                    Can be changed and is working (Change via WGSM settings)
- -queryport=8212               This is a test not confirmed if it's working or not
- -players=32                   Can be set as much as you want, as far as I know it can be 128 (Change via WGSM settings)
- -servername=""                Can override via Server Start Param box (WGSM Edit button)
- -serverdescription=""         Can override via Server Start Param box (WGSM Edit button)
- -adminpassword=""             Can override via Server Start Param box (WGSM Edit button)
- -serverpassword=""            Can override via Server Start Param box (WGSM Edit button)
- -rconenabled=true             Can override via Server Start Param box (WGSM Edit button)
- -rconport=25572               Default can override via Server Start Param box (WGSM Edit button)

### Config Guide
I've tried setting up PublicPort and PublicIP; from here, it looks like it gets overridden by launch parameters, and it's better than changing here
- Run the server to generate the Saved Folder files and Stop
- Copy all the contents of `\DefaultPalWorldSettings.ini` to `\Pal\Saved\Config\WindowsServer\PalWorldSettings.ini`
- Change ServerName
- Change PublicIP= x.x.x.x (Local Server IP e.g., 192.168.x.x)
- AdminPassword (Console can be performed in Chatbox using /AdminPassword then /Broadcast Test)
- RCONEnabled (if you are using third-party apps this is useful for saving the game outside, for example, discord slash commands /Save)
- RCONPort (Can be set any port you prefer and must be forwarded as well)
- Any other settings can be set

### Blank Console?
- Try installing Required Redist inside `_CommonRedist` folder
- VC++ latest  DirectX offline version or latest
- Download and install steam launcher on your server will do the fix
- You should be seeing `Setting breakpad minidump AppID = 1623730`

### Stuck in loading?
- Delete profile (For now other fix)
- To get the profile either open up a new server and let corrupted player join and copy the save hex
- Or backup all the players and start the server, let corrupted player join and remember the hex and restore profiles then delete the corrupted profile save
- If you have the player id you can convert it to hex [here](https://www.binaryhexconverter.com/decimal-to-hex-converter)
- [YouTube Guide](https://www.youtube.com/watch?v=fwLamiy30Qc&ab_channel=EpicLazyPanda)
- It is advisable to put your pal in palbox and items in storages before logging out

# Other notes
- Server listing - Please be aware that the whole world is listing their own dedicated server. It is advisable to use direct connect instead of looking for it. The game is not region-locked, and the max is 200 items.
- The game is currently in Early Access Stage. WGSM and this plugin are not liable if something happens to your server; the app is only for managing your server easily.
- To add GLST to help steam list your server [here](https://steamcommunity.com/dev/managegameservers)

### How can you play with your friends without port forwarding?
- Use [zerotier](https://www.zerotier.com/) follow the basic guide and create a network
- Download the client app and join to your network
- Create a static IP address for your host machine
- Edit WGSM IP Address to your recently created static IP address
- Give your network ID to your friends
- After they've joined to your network
- They can connect using the IP you've created e.g., 10.123.17.1:8211
- Enjoy

### Support

- [Palworld Discord](https://discord.com/channels/505994577942151180/1196354410868117525)
- [WGSM Discord](https://discord.com/channels/590590698907107340/645730252672335893)
