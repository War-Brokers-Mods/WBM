# [War Brokers Mods (WBM)](https://github.com/War-Brokers-Mods/WBM)

![License: MIT](https://img.shields.io/github/license/War-Brokers-Mods/WBM?style=for-the-badge&color=blue)
[![discord invite](https://img.shields.io/badge/Discord-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/aQqamSCUcS)

<p align="center">
   <strong>IF YOU USE THIS TO DEVELOP HACKS YOUR MOM IS GAY.</strong>
</p>

<p align="center">
  <img src="images/WBM.png" alt="WBM logo"/>
</p>

**W**ar **B**rokers **M**ods, AKA **WBM** is a mod for [War Brokers](https://store.steampowered.com/app/750470).<br />

<details>
<summary>Example Images (click to unfold)</summary>

![Example 1](./images/example1.png)

![Example 2](./images/example2.png)

</details>

## Installation

Only Windows, MacOS, and Linux are officially supported. It does **NOT** work on browsers.

Usage of the [WBM Installer](https://github.com/War-Brokers-Mods/WBM-installer/releases) is recommended.

<details>
<summary>Manual Installation Instruction (not recommended) (click to unfold)</summary>

### 1. Install BepInEx

1.  Download the latest version of [BepInEx](https://github.com/BepInEx/BepInEx/releases) **version 5**.

    |      Platform | Filename                      |
    | ------------: | :---------------------------- |
    | Linux & MacOS | BepInEx\_**unix_5**.Y.Z.W.zip |
    |       Windows | BepInEx\_**x86_5**.Y.Z.W.zip  |

2.  Extract all the contents to where the game is installed.

    How to find game location:<br />
    ![how to find game location](./images/local_files.png)

    Now the folder structure should look like this:

    ```
    WarBrokers/
    ├── BepInEx/
    │   ├── core/
    │   └── ...
    └── ...
    ```

3.  **If you are using Linux or MacOS:**

    1. make `run_bepinex.sh` executable: `chmod u+x run_bepinex.sh`
    2. Add launch option

       where to find game properties:<br />
       ![where to find game properties](images/properties.png)

       **If you're using linux**, set the launch option to:

       ```bash
       ./run_bepinex.sh %command%
       ```

       **If you're using Mac**, open a terminal in the game folder and run

       ```bash
       pwd
       ```

       This will print the full path to the game folder. Copy it, then set the launch option to:

       ```bash
       "PWD_RESULT_HERE/run_bepinex.sh" %command%
       ```

### 2. Install WBM

1. [Download](https://github.com/War-Brokers-Mods/WBM/releases/latest) the latest version of WBM. (`WBM.zip` file)
2. Unzip it in the `<game folder>/BepInEx/plugins` folder. Create one if it doesn't exist.

   The folder structure should look like this after unzipping the file:

   ```
   WarBrokers/
   ├── BepInEx/
   │   ├── plugins/
   │   │   └── WBM
   │   │       ├── assets/
   │   │       ├── WBM.dll
   │   │       └── ...
   │   ├── core/
   │   └── ...
   └── ...
   ```

That's it! You can open War Brokers now.

### Updating

Simply go through the installation process again and replace existing files. You don't have to reinstall BepInEx to reinstall WBM.

</details>

## Usage

### Warning

The order of keystroke matters.<br />
For example, pressing <kbd>RShift</kbd>+<kbd>A</kbd> is different from pressing <kbd>A</kbd>+<kbd>RShift</kbd>.<br/>
This is to prevent situation where <kbd>RShift</kbd>+<kbd>A</kbd> fires when the user intended to press <kbd>LCtrl</kbd>+<kbd>RShift</kbd>+<kbd>A</kbd>.

### Default shortcuts

- Hold down <kbd>LCtrl</kbd> or <kbd>RShift</kbd> to show shortcuts in-game.
- Press F1 to show menu. Click outside the menu to close it.

| Function                                   | Default Shortcut                                    |
| ------------------------------------------ | --------------------------------------------------- |
| Show Menu                                  | <kbd>F1</kbd>                                       |
| <br />                                     |                                                     |
| Move GUI (long press)                      | <kbd>LCtrl</kbd>+<kbd>LShift</kbd>+<kbd>Arrow</kbd> |
| Move GUI by one pixel                      | <kbd>LCtrl</kbd>+<kbd>Arrow</kbd>                   |
| Reset GUI position                         | <kbd>LCtrl</kbd>+<kbd>R</kbd>                       |
| <br />                                     |                                                     |
| Toggle All GUI visibility                  | <kbd>RShift</kbd>+<kbd>A</kbd>                      |
| Toggle player statistics visibility        | <kbd>RShift</kbd>+<kbd>P</kbd>                      |
| Toggle weapon statistics visibility        | <kbd>RShift</kbd>+<kbd>W</kbd>                      |
| Toggle team statistics visibility          | <kbd>RShift</kbd>+<kbd>L</kbd>                      |
| Toggle kills Elo visibility on leaderboard | <kbd>RShift</kbd>+<kbd>E</kbd>                      |
| Toggle squad server visibility             | <kbd>RShift</kbd>+<kbd>S</kbd>                      |
| Toggle Testing servers visibility          | <kbd>RShift</kbd>+<kbd>T</kbd>                      |
| Toggle kill streak sound effect            | <kbd>RShift</kbd>+<kbd>F</kbd>                      |
| Clear chat                                 | <kbd>RShift</kbd>+<kbd>Z</kbd>                      |
| Clear kills and death log                  | <kbd>RShift</kbd>+<kbd>X</kbd>                      |
| <br />                                     |                                                     |
| Toggle shift to crouch                     | <kbd>RShift</kbd>+<kbd>C</kbd>                      |

## Features

<details>
<summary>A list of all the features in WBM (click to unfold)</summary>

- in-game menu
- custom shortcut keys
- clear chat
- clear game messages (kills, deaths, missile launch, bomb diffuse, etc.)
- Extended fps limit (5\~240 => disabled\~1000) (may be buggy)

### in-game overlays

- Leaderboard

  - kills Elo for each player

- Player statistics

  - KDR
  - kills Elo
  - kills Elo earned/lost
  - games Elo
  - games Elo earned/lost
  - total damage dealt
  - longest kill
  - points earned
  - headshot count
  - kill streak

- Weapon statistics

  - fire timer
  - reload timer
  - cooldown timer
  - bullet speed
  - current zoom

- Team statistics

  - in-game nick
  - kdr
  - points earned
  - damage dealt
  - total damage dealt
  - total deaths
  - total kills

### Controls

- Shift to crouch

### Sound effects

- 10 kill streak: ["rampage"](./assets/audio/rampage.wav)
- 20 kill streak: ["killing spree"](<./assets/audio/killing spree.wav>)
- 30 kill streak: ["unstoppable"](./assets/audio/unstoppable.wav)
- 50 kill streak: ["godlike"](./assets/audio/godlike.wav)
- 69 kill streak: ["nice"](./assets/audio/nice.wav)

### [OBS overlays](https://github.com/War-Brokers-Mods/WBM-Overlays)

### Etc

- kill streak sound effect
- Quickly change settings with keyboard shortcuts

</details>

## Limitations

WBM is not a hack.
WBM will not include any features that will give unfair advantages.
These features include but are not limited to:
extended minimap zoom, quick weapon swap, instant zoom, extended field of view, audio filter, etc.

WBM will not include any custom skins.
Micro-transaction accounts for a significant portion of the developers' income,
and WBM will not include any feature that will affect it.

## Building

If you are a casual user, this is completely unnecessary.
**This is only intended for developers.**

<details>
   <summary>Building instructions (click to unfold)</summary>

<br />

The guide is intentionally left incomplete.
To prevent any regular developers from using this mod to develop hacks,
I won't provide any help when it comes to building the mod from scratch.

This guide is only useful to people who's already familiar with reverse engineering,
and can create hacks on their own anyway.

> Assumes that working directory is project root.

1. Install .NET sdk.
2. Copy all DLL files from `<WB install path>/war_brokers_Data/Managed/` and `<WB install path>/BepInEx/core` to `WBM/dll/`. Create directory if it does not exist.
3. Download and unzip [BepInEx configuration manager v16](https://github.com/BepInEx/BepInEx.ConfigurationManager/releases) then copy the dll file to the `WBM/dll` directory.
4. Create `scripts/config.sh`. This will be used to quickly test the mod without having to manually install it.

   ```bash
   #!/bin/bash

   WB_PLUGINS_DIR="<PATH_TO_PLUGIN_INSTALL_DIRECTORY>"
   ```

5. Now you can run the scripts.

   - `scripts/debug.sh`: Builds WBM in debug mode and copy the files to the plugins directory.
   - `scripts/release.sh`: Creates a zip file that can be uploaded to the gh release section.

</details>

## Bug reports / Suggestions

If you have a cool idea that will make WBM better, or if WBM misbehaves in any way (no matter how minor the problem is), feel free go to the [Issues page](https://github.com/War-Brokers-Mods/WBM/issues) and open a new issue! Alternatively, you can report the bug in my [discord server](https://discord.gg/aQqamSCUcS).

## Special thanks

- [l3lackShark](https://github.com/l3lackShark) for [inspiration](https://github.com/l3lackShark/gosumemory)

## License

The source code for this project is available under the [MIT License](https://opensource.org/licenses/MIT).

Fonts:

- https://fonts.google.com/specimen/Architects+Daughter : OFL (used in WBM logo)
