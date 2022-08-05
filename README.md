# Edelstein [![Build](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml/badge.svg)](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml)
A v.95.1 Mushroom game server emulator written in C#.

## 🚀 Getting started

### 👨🏼‍💻 Developers
This project is built on .NET! make sure you have it installed before continuing.

#### Downloading required assets
1. Download the scripts from [Server.Scripts](https://github.com/kaioru/server.scripts/releases)
2. Download the data from [Server.NX](https://github.com/kaioru/server.nx/releases)
3. Unzip both into their own folders

#### Running the server
1. ```cd src/app/Edelstein.Daemon.Server```
2. ```cp appsettings.json appsettings.Production.json```
3. Edit the appsettings.Production.json appropriately (remember to set the scripts and data path!)
4. ```dotnet run```

## 📦 Extra Stuff
* [Server.NX](https://github.com/kaioru/server.nx) - the source for the Server.nx file.
* [Server.Scripts](https://github.com/kaioru/server.scripts) - various scripts for use with Edelstein.

## ⭐️ Acknowledgements
* [Rebirth](https://github.com/RajanGrewal/Rebirth) - lot's of referencing from here.
* [Destiny](https://github.com/Fraysa/Destiny) - even more referencing from here.
