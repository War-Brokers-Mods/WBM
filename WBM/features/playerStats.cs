using BepInEx.Configuration;

using UnityEngine;

using System;

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
    }
}
