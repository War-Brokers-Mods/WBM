using BepInEx;
using UnityEngine;

using System.Collections;

namespace WBM
{
	[BepInPlugin("com.developomp.wbm", "War Brokers Mods", "0.2.0.0")]
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

			showEloRaw = true;
			showSquadServerRaw = true;
			showTestingServerRaw = true;

			StartCoroutine(UpdateValuesFunction(0f));

			Logger.LogDebug("WBM: Ready!");
		}

		private void OnGUI()
		{
			GUI.skin.box.fontSize = 15;

			GUI.Box(
				new Rect(this.GUIOffsetX, this.GUIOffsetY, 200, 60),
				@"War Brokers Mods
Made by [LP] POMP
v0.2.0.0"
			);
			if (this.localPlayerIndex > 0)
			{
				string killsEloDeltaSign = this.MyPlayerStats.killsEloDelta >= 0 ? "+" : "";
				string gamesEloDeltaSign = this.MyPlayerStats.gamesEloDelta >= 0 ? "+" : "";

				GUI.Box(
					new Rect(this.GUIOffsetX, this.GUIOffsetY + 65, 200, 130),
					$@"kills Elo: {this.MyPlayerStats.killsElo} {killsEloDeltaSign}{this.MyPlayerStats.killsEloDelta}
games Elo: {this.MyPlayerStats.gamesElo} {gamesEloDeltaSign}{this.MyPlayerStats.gamesEloDelta}
K/D: {this.MyPlayerStats.kills} / {this.MyPlayerStats.deaths}
Damage: {this.MyPlayerStats.damage}
Longest Kill: {this.MyPlayerStats.longestKill}
Points: {this.MyPlayerStats.points}
HeadShots: {this.MyPlayerStats.headShots}"
				);
			}
		}

		private IEnumerator UpdateValuesFunction(float time)
		{
			this.localPlayerIndex = this.localPlayerIndexRaw;

			if (this.localPlayerIndexRaw > 0)
			{
				this.MyPlayerStats = this.MyPlayerStatsRaw;
			}

			yield return new WaitForSeconds(time);

			this.UpdateValues = UpdateValuesFunction(1f);
			StartCoroutine(this.UpdateValues);
		}
	}
}
