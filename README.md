<p align="center">
  <img width="200" alt="NosCore" src="icon.png"/>
</p>

<h1 align="center">NosCore.ParserInputGenerator</h1>

<p align="center">
  <a href="https://www.nuget.org/packages/NosCore.ParserInputGenerator/"><img alt="NuGet" src="https://img.shields.io/nuget/v/NosCore.ParserInputGenerator.svg?style=flat-square"/></a>
  <a href="https://github.com/NosCoreIO/NosCore.ParserInputGenerator/actions/workflows/dotnet.yml"><img alt="Build" src="https://github.com/NosCoreIO/NosCore.ParserInputGenerator/actions/workflows/dotnet.yml/badge.svg?branch=master"/></a>
  <a href="https://discord.gg/Eu3ETSw"><img alt="Discord" src="https://img.shields.io/badge/Discord-join-5865F2?style=flat-square&logo=discord&logoColor=white"/></a>
  <a href="LICENSE"><img alt="MIT License" src="https://img.shields.io/badge/license-MIT-blue?style=flat-square"/></a>
</p>

Downloads the NosTale client's data archives from the official patch server and unpacks them into the folder layout
that the NosCore parser reads: the `.dat` files, the language files and the maps.

## Usage

```bash
dotnet build
cd build/net11.0
dotnet NosCore.ParserInputGenerator.Launcher.dll
```

The result is `build/net11.0/output/parser`, with the binary map files in `output/parser/map`, plus a
`parser-input-files.tar.bz2` of the same folder. Give that folder to the NosCore parser with `--folder`, as described
in NosCore's [getting started guide](https://github.com/NosCoreIO/NosCore/blob/master/documentation/getting-started.md).
Archives whose checksum has not changed since the last run are not downloaded again.

### In a container

```bash
podman build -t noscore-parser-input-generator .
mkdir -p output
podman run --rm -v "$PWD/output:/app/build/net11.0/output:Z" noscore-parser-input-generator
```

### As a library

```bash
dotnet add package NosCore.ParserInputGenerator
```

`services.AddParserInputGenerator()` registers `ParserInputBuilder`, whose `BuildAsync(workFolder)` downloads into
`workFolder/client` and unpacks into `workFolder/parser`.

## Building

```bash
dotnet build
dotnet test
```

## Contributing

Questions, ideas and pull requests are welcome; most of the discussion happens on [Discord](https://discord.gg/Eu3ETSw).
Any editor that handles .NET 11 works: `dotnet build` and `dotnet test` run on Windows, Linux and macOS.
In Visual Studio, [Roslynator](https://github.com/dotnet/roslynator) and
[ReSharper](https://www.jetbrains.com/resharper/) help keep to the code style.

## Supporting NosCore

NosCore is a spare-time open source project. Sponsoring buys time spent on the emulator and the packages around it,
and gives the **Supporter** role on Discord.

<p>
  <a href="https://github.com/sponsors/0Lucifer0"><img alt="Sponsor on GitHub" src="https://img.shields.io/badge/GitHub-sponsor-EA4AAA?style=for-the-badge&logo=githubsponsors&logoColor=white"/></a>
  <a href="https://ko-fi.com/A3562BQV"><img alt="Support on Ko-fi" src="https://img.shields.io/badge/Ko--fi-support-FF5E5B?style=for-the-badge&logo=kofi&logoColor=white"/></a>
  <a href="https://www.patreon.com/bePatron?u=6503887"><img alt="Become a patron" src="https://img.shields.io/badge/Patreon-become_a_patron-F96854?style=for-the-badge&logo=patreon&logoColor=white"/></a>
</p>

Thanks to [Navicat](https://www.navicat.com) for supporting the project.

<a href="https://www.navicat.com"><img height="60" alt="Navicat" src="https://user-images.githubusercontent.com/35202750/207230064-dcf23adc-9e96-4481-9a53-cd212f5bd60e.png"/></a>

## Legal

NosCore is an independent, unofficial project for educational use. It is not affiliated with, authorized, maintained,
sponsored or endorsed by Gameforge or any of its affiliates; NosTale is a trademark of Gameforge. Using it may be
against the terms of service of NosTale servers.

Released under the [MIT License](LICENSE).
