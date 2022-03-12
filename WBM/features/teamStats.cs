using BepInEx.Configuration;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showTeamStats;
        private ConfigEntry<KeyboardShortcut> showTeamStatsShortcut;

        private void doTeamStats()
        {
            this.toggleTeamStatsOnKeyPress();
        }

        private void toggleTeamStatsOnKeyPress()
        {
            if (this.showTeamStatsShortcut.Value.IsDown()) this.showTeamStats.Value = !this.showTeamStats.Value;
        }
    }
}
