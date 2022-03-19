using BepInEx.Configuration;

using UnityEngine;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showGUI;
        private ConfigEntry<KeyboardShortcut> showGUIShortcut;

        private ConfigEntry<int> GUIOffsetX;
        private ConfigEntry<int> GUIOffsetY;
        private ConfigEntry<KeyboardShortcut> resetGUIShortcut;

        private void drawCoreUI()
        {
            GUI.skin.box.fontSize = 15;
            GUI.skin.label.fontSize = 15;
            GUI.skin.label.wordWrap = false;

            if (this._showConfig) this.showConfig();

            if (!this.showGUI.Value) return;

            this.showWBMVersion();
        }

        private void showConfig()
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

        private void showWBMVersion()
        {
            GUI.Box(
                new Rect(this.GUIOffsetX.Value, this.GUIOffsetY.Value, 220, 60),
                $@"{WBM.programName}
Made by [LP] POMP
v{WBM.programVersion}"
            );
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
