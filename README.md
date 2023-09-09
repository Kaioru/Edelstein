# Edelstein [![Build](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml/badge.svg)](https://github.com/Kaioru/Edelstein/actions/workflows/build.yaml)
A v.95.1 Mushroom game server emulator written in C#.

## 🚀 Getting started

### ✨ Usage

#### Prerequisites
* A running [PostgreSQL](https://www.postgresql.org) server!
* That's mostly it..

#### Download a release
1. Check the [releases](https://github.com/Kaioru/Edelstein/releases) tab and download the correct bundle based on your OS!

#### Download required assets 
2. Download the data from [Server.NX](https://github.com/Kaioru/Server.NX/releases)
3. Download the scripts from [Server.Scripts](https://github.com/Kaioru/Server.Scripts/releases)
4. Unzip both into the `data` and `scripts` folder respectively

#### Update configuration and migrations
5. Edit the `appsettings.json` file to the appropriate settings
6. Run the scripts in with the `migrate` prefix in sequence (Use either the bundled binary files or SQL scripts)

#### Running the server
7. Run the `Edelstein.Daemon.Server` executable

### 📦 Docker

#### Download required assets 
1. Download the data from [Server.NX](https://github.com/Kaioru/Server.NX/releases)
2. Download the scripts from [Server.Scripts](https://github.com/Kaioru/Server.Scripts/releases)
3. Unzip both into the `data` and `scripts` folder respectively

#### Running `docker compose up`
4. Run the `HOST=127.0.0.1 docker compose up` command
5. Do substitute or omit the HOST environment key accordingly

### 🏗️ Builds
A nightly build is published at 00:00 UTC when there are changes to the 'dev' branch.

* Executables are available under [releases](https://github.com/Kaioru/Edelstein/releases/tag/nightly) tab with the `nightly` tag
* Protocol and Common libraries are pushed to [packages](https://github.com/Kaioru?tab=packages&repo_name=Edelstein)

#### Setting up your project for Github Packages
1. Create a Personal Access Token with the 'read:packages' scope
2. Create a `nuget.config` file on your project root with the following contents:
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
3. Remember to set your Github Username and Personal Access Token!

Check the [here](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry) for more on how to setup the NuGet registry.

## 📦 Extra Stuff
* [Server.NX](https://github.com/kaioru/server.nx) - the source for the Server.nx file.
* [Server.Scripts](https://github.com/kaioru/server.scripts) - various scripts for use with Edelstein.

## ⭐️ Acknowledgements
* [Rebirth](https://github.com/RajanGrewal/Rebirth) - lot's of referencing from here.
* [Destiny](https://github.com/Fraysa/Destiny) - even more referencing from here.
