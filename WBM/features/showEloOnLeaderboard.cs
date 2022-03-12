using BepInEx.Configuration;

using UnityEngine;

using System;
using System.Reflection;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showEloOnLeaderboard;
        private ConfigEntry<KeyboardShortcut> showEloOnLeaderboardShortcut;

        private FieldInfo showEloOnLeaderboardRef;
        private bool showEloOnLeaderboardRaw
        {
            get
            {
                return (bool)this.showEloOnLeaderboardRef.GetValue(this.webguy);
            }
            set
            {
                this.showEloOnLeaderboardRef.SetValue(this.webguy, value);
            }
        }

        private void setupShowEloOnLeaderBoard()
        {
            this.showEloOnLeaderboardRef = webguyType.GetField("KDOBENAOLLF", bindFlags);

            this.showEloOnLeaderboard = Config.Bind("Config", "show Elo on leaderboard", true);
            this.showEloOnLeaderboard.SettingChanged += this.onShowEloOnLeaderboardChanged;
            this.showEloOnLeaderboardShortcut = Config.Bind("Hotkeys", "show Elo on leaderboard", new KeyboardShortcut(KeyCode.E, KeyCode.RightShift));
            this.showEloOnLeaderboardRaw = this.showEloOnLeaderboard.Value;
        }

        private void toggleLeaderboardEloOnKeyPress()
        {
            if (this.showEloOnLeaderboardShortcut.Value.IsDown()) this.showEloOnLeaderboard.Value = !this.showEloOnLeaderboard.Value;
        }

        private void onShowEloOnLeaderboardChanged(object sender, EventArgs e)
        {
            this.showEloOnLeaderboardRaw = this.showEloOnLeaderboard.Value;
        }
    }
}