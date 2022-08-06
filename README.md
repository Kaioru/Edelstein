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

### 🏗️ Builds
A nightly build is published on Github Packages at 00:00 UTC when there are changes to the 'dev' branch.

#### Setting up your project for Github Packages
1. Create a Personal Access Token with the 'read:packages' scope
2. Create a 'nuget.config' file on your project root with the following contents:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <clear />
        <add key="github" value="https://nuget.pkg.github.com/Kaioru/index.json" />
    </packageSources>
    <packageSourceCredentials>
        <github>
            <add key="Username" value="GITHUB_USERNAME" />
            <add key="ClearTextPassword" value="GITHUB_PERSONAL_ACCESS_TOKEN" />
        </github>
    </packageSourceCredentials>
</configuration>
```
3. Remember to se your Github Username and Personal Access Token!

Check the [here](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry) for more on how to setup the NuGet registry.

#### Installing packages
Now that your NuGet sources is all set up and authenticated, you can now install nightly builds of Edelstein.

See the [packages](https://github.com/Kaioru?tab=packages&repo_name=Edelstein) tab for an index of the packages!

## 📦 Extra Stuff
* [Server.NX](https://github.com/kaioru/server.nx) - the source for the Server.nx file.
* [Server.Scripts](https://github.com/kaioru/server.scripts) - various scripts for use with Edelstein.

## ⭐️ Acknowledgements
* [Rebirth](https://github.com/RajanGrewal/Rebirth) - lot's of referencing from here.
* [Destiny](https://github.com/Fraysa/Destiny) - even more referencing from here.
