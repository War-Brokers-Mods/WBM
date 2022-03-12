using BepInEx.Configuration;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showPlayerStats;
        private ConfigEntry<KeyboardShortcut> showPlayerStatsShortcut;

        private void togglePlayerStatsOnKeyPress()
        {
            if (this.showPlayerStatsShortcut.Value.IsDown()) this.showPlayerStats.Value = !this.showPlayerStats.Value;
        }

        private void doPlayerStats()
        {
            this.togglePlayerStatsOnKeyPress();
        }
    }
}
