# Edelstein [![CircleCI](https://circleci.com/gh/Kaioru/Edelstein.svg?style=svg)](https://circleci.com/gh/Kaioru/Edelstein)
A MapleStory Global v.95 server emulator written in C#.

**this project is a work-in-progress**

## 👨‍💻 Cloning the repo
1. ```git clone https://github.com/Kaioru/Edelstein && cd Edelstein```
2. ```git submodule update --init --recursive```

## 🐳 Building from Docker

### Running with Docker Compose
1. ```docker-compose up```
yup. it's that easy.

### Configuring .env
By default, the predefined Container service runs on 127.0.0.1. 

To change this, create a .env file and add ```APP_Host={ip}``` to change the host the service runs on.

## 🔨 Building from scratch

### Build with your favourite tool/ide
1. On Visual Studio and Rider it should be pretty straightforward
2. Use ```dotnet build``` if not using an IDE

### Running the Container service
The Container service is the quick and easy way to spin up Edelstein
1. ```cd src/Edelstein.Service.All```
2. ```cp appsettings appsettings.Production.json```
3. Edit the appsettings.Production.json appropriately
4. ```dotnet run```

## 📦 Extra Stuff
* [Server.NX](https://github.com/kaioru/server.nx) - the source for the Server.nx file.
* [Server.Scripts](https://github.com/kaioru/server.scripts) - various scripts for use with Edelstein.

## ⭐️ Acknowledgements
* [Rebirth](https://github.com/RajanGrewal/Rebirth) - lot's of referencing from here.
* [Destiny](https://github.com/Fraysa/Destiny) - even more referencing from here.
* [@kwakery](https://github.com/kwakery) - for bein a nub, jk ily.
