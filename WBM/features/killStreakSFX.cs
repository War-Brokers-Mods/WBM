using BepInEx.Configuration;

using UnityEngine;

using System.Collections.Generic;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> killStreakSFX;
        private ConfigEntry<KeyboardShortcut> killStreakSFXShortcut;

        private int killStreak = 0;

        private AudioSource killStreakAudioSource;
        private Dictionary<int, string> killStreakSFXDictionary = new Dictionary<int, string>()
        {
            {10, "rampage"},
            {20, "killing spree"},
            {30, "unstoppable"},
            {50, "godlike"},
            {69, "nice"},
        };

        private void setupKillStreakSFX()
        {
            this.killStreakSFX = Config.Bind("Config", "kill streak sound effect", true);
            this.killStreakSFXShortcut = Config.Bind("Hotkeys", "kill streak sound effect", new KeyboardShortcut(KeyCode.F, KeyCode.RightShift));
        }

        private void doKillStreakSFX()
        {
            this.toggleKillStreakSFXOnKeyPress();
        }

        private void toggleKillStreakSFXOnKeyPress()
        {
            if (this.killStreakSFXShortcut.Value.IsDown()) this.killStreakSFX.Value = !this.killStreakSFX.Value;
        }
    }
}
