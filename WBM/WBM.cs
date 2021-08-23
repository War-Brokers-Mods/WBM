using BepInEx;
using UnityEngine;


namespace WBM
{
	[BepInPlugin("com.developomp.wbm", "War Brokers Mods", "1.0.0.0")]
	public partial class WBM : BaseUnityPlugin
	{
		private void Start()
		{
			Logger.LogDebug("WBM: Initializing");
			this._webguy = FindObjectOfType<webguy>();

			System.Type webguyType = typeof(webguy);

			showEloRef = webguyType.GetField("KDOBENAOLLF", bindFlags);
			showSquadServerRef = webguyType.GetField("PHPIBBCFKFI", bindFlags);
			showTestingServerRef = webguyType.GetField("LHHEGFHLNJE", bindFlags);
			playerStatsArrayRef = webguyType.GetField("NAFCGDLLFJC", bindFlags);
			currentAreaRef = webguyType.GetField("FLJLJNLDFAM", bindFlags);
			playersActiveRef = webguyType.GetField("EPNEEBOFKHA", bindFlags);
			killListRef = webguyType.GetField("IKKEILIBONF", bindFlags);
			teamListRef = webguyType.GetField("MNEJLPDLMBH", bindFlags);
			localPlayerIndexRef = webguyType.GetField("ALEJJPEPFOG", bindFlags);

			showElo = true;
			showSquadServer = true;
			showTestingServer = true;

			Logger.LogDebug("WBM: Ready!");
		}

		private void OnGUI()
		{
			int xOffset = 40;
			int yOffset = 325;

			GUI.skin.box.fontSize = 15;

			GUI.Box(
				new Rect(xOffset, yOffset, 200, 60),
				@"War Brokers Mods
Made by [LP] POMP
v1.0.0.0"
			);
			if (localPlayerIndex > 0)
			{
				Data.PlayerStatsStruct playerStat = this.MyPlayerStats;

				string killsEloDeltaSign = playerStat.killsEloDelta >= 0 ? "+" : "";
				string gamesEloDeltaSign = playerStat.gamesEloDelta >= 0 ? "+" : "";

				GUI.Box(
					new Rect(xOffset, yOffset + 65, 200, 130),
					$@"kills Elo: {playerStat.killsElo} {killsEloDeltaSign}{playerStat.killsEloDelta}
games Elo: {playerStat.gamesElo} {gamesEloDeltaSign}{playerStat.gamesEloDelta}
K/D: {playerStat.kills} / {playerStat.deaths}
Damage: {playerStat.damage}
Longest Kill: {playerStat.longestKill}
Points: {playerStat.points}
HeadShots: {playerStat.headShots}"
				);
			}
		}
	}
}
