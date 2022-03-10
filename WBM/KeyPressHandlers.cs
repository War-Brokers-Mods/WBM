using UnityEngine;


namespace WBM
{
    public partial class WBM
    {
        private void handleKeyPresses()
        {
            this.moveUIOnKeyPress();
            this.resetUIOnKeyPress();
            this.toggleUIOnKeyPress();
            this.toggleShiftToCrouchOnKeyPress();
            this.toggleKillStreakSFXOnKeyPress();
            this.togglePlayerStatsOnKeyPress();
            this.toggleWeaponStatsOnKeyPress();
            this.toggleTeamStatsOnKeyPress();
            this.toggleLeaderboardEloOnKeyPress();
            this.toggleSquadServerOnKeyPress();
            this.toggleTestingServerOnKeyPress();
            this.clearChatOnKeyPress();
            this.clearDeathLogOnKeyPress();
            this.showConfigOnKeyPress();
            this.crouchOnKeyPress();
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

        private void toggleShiftToCrouchOnKeyPress()
        {
            if (this.shiftToCrouchShortcut.Value.IsDown()) this.shiftToCrouch.Value = !this.shiftToCrouch.Value;
        }

        private void toggleKillStreakSFXOnKeyPress()
        {
            if (this.killStreakSFXShortcut.Value.IsDown()) this.killStreakSFX.Value = !this.killStreakSFX.Value;
        }

        private void togglePlayerStatsOnKeyPress()
        {
            if (this.showPlayerStatsShortcut.Value.IsDown()) this.showPlayerStats.Value = !this.showPlayerStats.Value;
        }

        private void toggleWeaponStatsOnKeyPress()
        {
            if (this.showWeaponStatsShortcut.Value.IsDown()) this.showWeaponStats.Value = !this.showWeaponStats.Value;
        }

        private void toggleTeamStatsOnKeyPress()
        {
            if (this.showTeamStatsShortcut.Value.IsDown()) this.showTeamStats.Value = !this.showTeamStats.Value;
        }

        private void toggleLeaderboardEloOnKeyPress()
        {
            if (this.showEloOnLeaderboardShortcut.Value.IsDown()) this.showEloOnLeaderboard.Value = !this.showEloOnLeaderboard.Value;
        }

        private void toggleSquadServerOnKeyPress()
        {
            if (this.showSquadServerShortcut.Value.IsDown()) this.showSquadServer.Value = !this.showSquadServer.Value;
        }

        private void toggleTestingServerOnKeyPress()
        {
            if (this.showTestingServerShortcut.Value.IsDown()) this.showTestingServer.Value = !this.showTestingServer.Value;
        }

        private void clearChatOnKeyPress()
        {
            if (this.clearChatShortcut.Value.IsDown()) this.clearChat();
        }

        private void clearDeathLogOnKeyPress()
        {
            if (this.clearDeathLogShortcut.Value.IsDown()) this.clearMessagesFuncRef.Invoke(this.webguy, new object[] { });
        }

        private void showConfigOnKeyPress()
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift)) this._showConfig = true;
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightShift)) this._showConfig = false;
        }

        private void crouchOnKeyPress()
        {
            // only if right buttton is not held
            if (this.shiftToCrouch.Value && !Input.GetMouseButton(1))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift)) OMOJPGNNKFN.NEELEHFDKBP.EGACOOOGDDC = true;
                if (Input.GetKeyUp(KeyCode.LeftShift)) OMOJPGNNKFN.NEELEHFDKBP.EGACOOOGDDC = false;
            }
        }
    }
}
