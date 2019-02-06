# Edelstein [![CircleCI](https://circleci.com/gh/Kaioru/Edelstein.svg?style=svg)](https://circleci.com/gh/Kaioru/Edelstein)
A MapleStory Global v.95 server emulator written in C#.

**this project is a work-in-progress**

## 🔨 Building and Running
### Clone the repo
1. ```git clone https://github.com/Kaioru/Edelstein && cd Edelstein```
2. ```git submodule update --init --recursive```
### Build with your favourite tool/ide
1. On Visual Studio and Rider it should be pretty straightforward
2. Use ```dotnet build``` if not using an IDE
### Running database migrations
1. ```cd src/Edelstein.Data```
2. ```cp Database.Example.json Database.json```
3. Edit Database.json to the appropriate connection string
4. ```dotnet ef database update```
### Running WvsContainer
WvsContainer is the quick and easy way to spin up Edelstein
1. ```cd src/Edelstein.Service.All```
2. ```cp WvsContainer.example.json WvsContainer.json```
3. Edit the WvsContainer.json appropriately
4. ```dotnet run```

## 📦 Extra Stuff
* [Server.NX](https://github.com/kaioru/server.nx) - the source for the Server.nx file.
* [Server.Scripts](https://github.com/kaioru/server.scripts) - various scripts for use with Edelstein.

## ⭐️ Acknowledgements
* [Rebirth](https://github.com/RajanGrewal/Rebirth) - lot's of referencing from here.
* [Destiny](https://github.com/Fraysa/Destiny) - even more referencing from here.
* [@kwakery](https://github.com/kwakery) - for bein a nub, jk ily.
