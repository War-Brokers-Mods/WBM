using BepInEx.Configuration;

using UnityEngine;

using System;

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

        private void toggleTeamStatsOnKeyPress()
        {
            if (this.showTeamStatsShortcut.Value.IsDown()) this.showTeamStats.Value = !this.showTeamStats.Value;
        }
    }
}
