# [War Brokers Mods (WBM)](https://github.com/developomp/WBM)

![License: MIT](https://img.shields.io/github/license/developomp/WBM?style=flat-square&color=blue)

> **IF YOU USE THIS TO DEVELOP HACKS YOUR MOM IS GAY.**

<p align="center">
  <img src="images/WBM.png" alt="WBM logo"/>
</p>

> **WARNING**<br />
> WBM only works with steam clients. It is **NOT compatible with browsers**.

**W**ar **B**rokers **M**ods, AKA **WBM** is a unofficial mod for [War Brokers](https://store.steampowered.com/app/750470).<br />

<details>
<summary>Example Images (click to unfold)</summary>

![Example 1](./images/example1.png)

![Example 2](./images/example2.png)

![Example 3](./images/example3.png)

</details>

## Installation

Officially supported platforms: Windows, MacOS, and Linux

> **WARNING**<br />
> I do not upload WBM anywhere other than github. If you find it elsewhere, IT IS NOT UPLOADED BY ME.

### Installing BepInEx

1. Download the latest version of BepInEx **version 5** from [here](https://github.com/BepInEx/BepInEx/releases).

   |      Platform | Filename                      |
   | ------------: | :---------------------------- |
   | Linux & MacOS | BepInEx_unix\_**5**.Y.Z.W.zip |
   |       Windows | BepInEx_x86\_**5**.Y.Z.W.zip  |

2. Extract (Unzip) the content**s** to where the game executable is located.

   How to find game location:
   ![how to find game location](./images/local_files.png)

3. If you are using Linux or MacOS, you must also perform the following setup:

   https://docs.bepinex.dev/v5.4.11/articles/advanced/steam_interop.html

4. **IMPORTNT** Run the game at least once to generate the plugins folder as well as other necessary files.

### Installing WBM

1. [Download](https://github.com/developomp/WBM/releases/latest) the latest version of WBM (a .dll file).
2. Put the dll file in the `<Game folder>/BepInEx/plugins` folder.

### Setting up OBS

WIP...

That's it! Now you're a proud user of WBM!

### Updating

Simply replace the existing dll file with the latest version.

## Usage

- Hold down <kbd>LCtrl</kbd> or <kbd>RShift</kbd> to show shortcuts in-game.

| Function                             | Shortcut                                            |
| ------------------------------------ | --------------------------------------------------- |
| Move GUI by one pixel at a time      | <kbd>LCtrl</kbd>+<kbd>Arrow</kbd>                   |
| Move GUI (long press)                | <kbd>LCtrl</kbd>+<kbd>LShift</kbd>+<kbd>Arrow</kbd> |
| Reset GUI position                   | <kbd>LCtrl</kbd>+<kbd>R</kbd>                       |
|                                      |                                                     |
| Toggle All GUI visibility            | <kbd>RShift</kbd>+<kbd>A</kbd>                      |
| Toggle Player statistics visibility  | <kbd>RShift</kbd>+<kbd>P</kbd>                      |
| Toggle Weapon statistics visibility  | <kbd>RShift</kbd>+<kbd>W</kbd>                      |
| Toggle Team statistics visibility    | <kbd>RShift</kbd>+<kbd>L</kbd>                      |
| Toggle Elo visibility on leaderboard | <kbd>RShift</kbd>+<kbd>E</kbd>                      |
| Squad server visibility              | <kbd>RShift</kbd>+<kbd>S</kbd>                      |
| Testing servers visibility           | <kbd>RShift</kbd>+<kbd>T</kbd>                      |
|                                      |                                                     |
| Toggle shift to crouch               | <kbd>RShift</kbd>+<kbd>C</kbd>                      |
|                                      |                                                     |
| Reset everything                     | <kbd>RShift</kbd>+<kbd>R</kbd>                      |

## Features

Full list of features:

- [x] in-game overlays
- [x] quick shortcuts
- [ ] OBS overlays

- in-game overlay

  - Tab Leaderboard

    - show kills Elo

  - Player statistics

    - KDR
    - kills Elo
    - kills Elo earned/lost
    - games Elo
    - games Elo earned/lost
    - total damage dealt
    - longest kill
    - points earned

  - Weapon statistics

    - total headshot count

- OBS (includes everything in in-game overlay)

  - design custom OBS overlays
  - **WIP**

  <!-- - [ ] top player per criteria (kills, longest kills, points, etc.)
  - [ ] kill streak
  - [ ] hit accuracy
  - [ ] game mode
  - [ ] team score
  - [ ] server id
  - [ ] streamer ID
  - [ ] survivors left in a BR match
  - [ ] teammate name
  - [ ] played game history (win, lose, and by how much)
  - [ ] ping in millisecond per player
  - [ ] if a player is a bot or not
  - [ ] Daily and history record -->

- Controls

  - Shift to crouch

# Building

If you are a casual user, this is completely unnecessary. **This is only recommended for developers.**

> Assumes that working directory is project root.

1. Install .NET sdk.

   MacOS:

   https://docs.microsoft.com/en-us/dotnet/core/install/macos

   Windows:

   https://docs.microsoft.com/en-us/dotnet/core/install/windows

   Linux:

   https://docs.microsoft.com/en-us/dotnet/core/install/linux

   Arch linux:

   ```bash
   pacman -S dotnet-sdk
   ```

2. Clone this repository.
3. Copy the following DLL files from `<WB install path>/war_brokers_Data/Managed` (in WB) to `./WBM/dll`. Create directory if it does not exist.

   - `Assembly-CSharp.dll`
   - `Assembly-CSharp-firstpass.dll`
   - `UnityEngine.*.dll`

4. Run the build command.

   Build in debug mode

   ```bash
   dotnet build
   ```

   Build in release mode

   ```bash
   dotnet build --configuration Release
   ```

5. The built dll can be found at:

   - `./WBM/bin/Debug/net48/WBM.dll`
   - `./WBM/bin/Release/net48/WBM.dll`

## Bug reports / Suggestions

If you have a cool idea that will make WBM better, or if WBM misbehaves in any way (no matter how minor the problem is), feel free go to the [Issues page](https://github.com/developomp/WBM/issues) and open a new issue!

## Contributing

- use GH pull request
- use vscode and install [recommended extensions](.vscode/extensions.json). This is required for code formatting.

## License

This project is licenced under the [MIT License](https://opensource.org/licenses/MIT).

Fonts:

- https://fonts.google.com/specimen/Architects+Daughter : OFL (used in WBM logo)
