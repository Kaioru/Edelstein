# Edelstein [![Build](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml/badge.svg)](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml)
A v.95.1 Mushroom game server emulator written in C#.

**this project is a work-in-progress**

## 👨‍💻 Cloning the repo
1. ```git clone https://github.com/Kaioru/Edelstein && cd Edelstein```

## 🔨 Building from scratch

### Build with your favourite tool/ide
1. On Visual Studio and Rider it should be pretty straightforward
2. Use ```dotnet build``` if not using an IDE

### Downloading required assets
1. Download the scripts from [Server.Scripts](https://github.com/kaioru/server.scripts/releases)
2. Download the data from [Server.NX](https://github.com/kaioru/server.nx/releases)
3. Unzip both into their own folders

### Running the Standalone app
The Standalone app is the quick and easy way to spin up Edelstein
1. ```cd src/app/Edelstein.App.Standalone```
2. ```cp appsettings.json appsettings.Production.json```
3. Edit the appsettings.Production.json appropriately (remember to set the scripts and data path!)
4. ```dotnet run```

## 📦 Extra Stuff
* [Server.NX](https://github.com/kaioru/server.nx) - the source for the Server.nx file.
* [Server.Scripts](https://github.com/kaioru/server.scripts) - various scripts for use with Edelstein.

## ⭐️ Acknowledgements
* [Rebirth](https://github.com/RajanGrewal/Rebirth) - lot's of referencing from here.
* [Destiny](https://github.com/Fraysa/Destiny) - even more referencing from here.
