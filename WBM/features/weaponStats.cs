using BepInEx.Configuration;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showWeaponStats;
        private ConfigEntry<KeyboardShortcut> showWeaponStatsShortcut;

        private void toggleWeaponStatsOnKeyPress()
        {
            if (this.showWeaponStatsShortcut.Value.IsDown()) this.showWeaponStats.Value = !this.showWeaponStats.Value;
        }
    }
}
