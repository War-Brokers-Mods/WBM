using BepInEx.Configuration;

using UnityEngine;

using System;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showWeaponStats;
        private ConfigEntry<KeyboardShortcut> showWeaponStatsShortcut;

        private void doWeaponStats()
        {
            this.toggleWeaponStatsOnKeyPress();
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

        private void toggleWeaponStatsOnKeyPress()
        {
            if (this.showWeaponStatsShortcut.Value.IsDown()) this.showWeaponStats.Value = !this.showWeaponStats.Value;
        }
    }
}
