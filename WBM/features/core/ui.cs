using BepInEx.Configuration;

using UnityEngine;

using System;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showGUI;
        private ConfigEntry<KeyboardShortcut> showGUIShortcut;

        private ConfigEntry<int> GUIOffsetX;
        private ConfigEntry<int> GUIOffsetY;
        private ConfigEntry<KeyboardShortcut> resetGUIShortcut;

        private void setupGUI()
        {
            GUI.skin.box.fontSize = 15;
            GUI.skin.label.fontSize = 15;
            GUI.skin.label.wordWrap = false;
        }

        private void drawConfig()
        {
            GUI.Box(
                    new Rect(Screen.width - 370, 60, 360, 370), $@"Configuration

move GUI: LCtrl+LShift+Arrow
move GUI by pixel: LCtrl+Arrow
reset GUI position: {this.resetGUIShortcut.Value}
clear chat: {this.clearChatShortcut.Value}
clear death log: {this.clearDeathLogShortcut.Value}

GUI X offset: {this.GUIOffsetX.Value}
GUI Y offset: {this.GUIOffsetY.Value}
Show WBM GUI: {this.showGUI.Value} ({this.showGUIShortcut.Value})
Show Elo on leaderboard: {this.showEloOnLeaderboard.Value} ({this.showEloOnLeaderboardShortcut.Value})
Show player stats: {this.showPlayerStats.Value} ({this.showPlayerStatsShortcut.Value})
Show weapon stats: {this.showWeaponStats.Value} ({this.showWeaponStatsShortcut.Value})
Show teammate stats: {this.showTeamStats.Value} ({this.showTeamStatsShortcut.Value})
show squad server: {this.showSquadServer.Value} ({this.showSquadServerShortcut.Value})
show testing server: {this.showTestingServer.Value} ({this.showTestingServerShortcut.Value})
shift to crouch: {this.shiftToCrouch.Value} ({this.shiftToCrouchShortcut.Value})
kill streak SFX: {this.killStreakSFX.Value} ({this.killStreakSFXShortcut.Value})"
                );
        }

        private void drawWBMVersion()
        {
            GUI.Box(
                new Rect(this.GUIOffsetX.Value, this.GUIOffsetY.Value, 220, 60),
                @"War Brokers Mods
Made by [LP] POMP
v1.7.1.0"
            );
        }

        private void drawPlayerStats()
        {
            if (!this.showPlayerStats.Value) return;

            try
            {
                string killsEloDeltaSign = this.myPlayerStats.killsEloDelta >= 0 ? "+" : "";
                string gamesEloDeltaSign = this.myPlayerStats.gamesEloDelta >= 0 ? "+" : "";

                GUI.Box(
                    new Rect(this.GUIOffsetX.Value, this.GUIOffsetY.Value + 65, 220, 180),
                    $@"Player stats

KDR: {Util.formatKDR(this.myPlayerStats.kills, this.myPlayerStats.deaths)}
kills Elo: {this.myPlayerStats.killsElo} {killsEloDeltaSign}{Util.formatDecimal((float)this.myPlayerStats.killsEloDelta / 10)}
games Elo: {this.myPlayerStats.gamesElo} {gamesEloDeltaSign}{Util.formatDecimal((float)this.myPlayerStats.gamesEloDelta / 10)}
Damage dealt: {this.myPlayerStats.damage}
Longest Kill: {this.myPlayerStats.longestKill}m
Points: {this.myPlayerStats.points}
Headshots: {this.myPlayerStats.headShots}
Kill streak: {this.killStreak}"
                );
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        private void drawWeaponStats()
        {

            if (!this.showWeaponStats.Value) return;

            try
            {
                GUI.Box(
                    new Rect(this.GUIOffsetX.Value, this.GUIOffsetY.Value + 250, 230, 130),
                    $@"Weapon stats

fire Timer: {String.Format("{0:0.00}", Util.getGunFireTimer(this.personGun))}s (max: {String.Format("{0:0.00}", Util.getGunFireRate(this.personGun))}s)
reload Timer: {Util.getGunReloadTimer(this.personGun)}
cooldown Timer: {Util.getGunCooldownTimer(this.personGun)}
speed: {Util.getGunFireVelocity(this.personGun)}
zoom: {Util.getGunZoom(this.personGun)}"
                );
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        private void drawTeamStats()
        {
            if (!this.showTeamStats.Value) return;

            try
            {
                string teamNames = "Nickname\n\n";
                string teamKDR = "KDR\n\n";
                string teamPoints = "pts\n\n";
                string teamDamage = "Damage\n\n";

                int teamTotalKills = 0;
                int teamTotalDeaths = 0;
                int teamTotalDamage = 0;

                for (int i = 0; i < this.data.playerStatsArray.Length; i++)
                {
                    Data.PlayerStatsStruct stat = this.data.playerStatsArray[i];

                    // if player is not a bot and if player is in my team
                    if ((stat.killsElo != 0) && (this.teamList[i] == this.myTeam))
                    {
                        teamNames += $"{this.data.nickList[i]}\n";
                        teamKDR += $"{Util.formatKDR(stat.kills, stat.deaths)}\n";
                        teamPoints += $"{stat.points}\n";
                        teamDamage += $"{stat.damage}\n";

                        teamTotalKills += stat.kills;
                        teamTotalDeaths += stat.deaths;
                        teamTotalDamage += stat.damage;
                    }
                }

                int teamStatOffset = (this.data.gameState == Data.GameStateEnum.Results) ? 280 : 0;
                GUI.Box(new Rect(Screen.width - 320, 445 + teamStatOffset, 300, 270), "Team Stats");
                GUI.Label(new Rect(Screen.width - 315, 470 + teamStatOffset, 105, 190), teamNames);
                GUI.Label(new Rect(Screen.width - 200, 470 + teamStatOffset, 40, 190), teamKDR);
                GUI.Label(new Rect(Screen.width - 150, 470 + teamStatOffset, 40, 190), teamPoints);
                GUI.Label(new Rect(Screen.width - 100, 470 + teamStatOffset, 70, 190), teamDamage);

                GUI.Label(
                    new Rect(Screen.width - 315, 655 + teamStatOffset, 300, 55),
                    $@"total damage: {teamTotalDamage}
total deaths: {teamTotalDeaths}
total kills: {teamTotalKills}"
                );
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        private void moveUIOnKeyPress()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                // move GUI
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKey(KeyCode.UpArrow)) this.GUIOffsetY.Value -= 1;
                    if (Input.GetKey(KeyCode.DownArrow)) this.GUIOffsetY.Value += 1;
                    if (Input.GetKey(KeyCode.LeftArrow)) this.GUIOffsetX.Value -= 1;
                    if (Input.GetKey(KeyCode.RightArrow)) this.GUIOffsetX.Value += 1;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow)) this.GUIOffsetY.Value -= 1;
                    if (Input.GetKeyDown(KeyCode.DownArrow)) this.GUIOffsetY.Value += 1;
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) this.GUIOffsetX.Value -= 1;
                    if (Input.GetKeyDown(KeyCode.RightArrow)) this.GUIOffsetX.Value += 1;
                }
            }
        }

        private void resetUIOnKeyPress()
        {
            if (this.resetGUIShortcut.Value.IsDown())
            {
                this.GUIOffsetX.Value = (int)this.GUIOffsetX.DefaultValue;
                this.GUIOffsetY.Value = (int)this.GUIOffsetY.DefaultValue;
            }
        }

        private void toggleUIOnKeyPress()
        {
            if (this.showGUIShortcut.Value.IsDown()) this.showGUI.Value = !this.showGUI.Value;
        }

        private void showConfigOnKeyPress()
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift)) this._showConfig = true;
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightShift)) this._showConfig = false;
        }
    }
}
