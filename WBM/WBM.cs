using BepInEx;
using UnityEngine;

using System;
using System.Collections;

namespace WBM
{
	[BepInPlugin("com.developomp.wbm", "War Brokers Mods", "0.9.0.0")]
	public partial class WBM : BaseUnityPlugin
	{
		private void Start()
		{
			Logger.LogDebug("WBM: Initializing");
			this.webguy = FindObjectOfType<webguy>();

			System.Type webguyType = typeof(webguy);

			this.showEloRef = webguyType.GetField("KDOBENAOLLF", bindFlags);
			this.showSquadServerRef = webguyType.GetField("PHPIBBCFKFI", bindFlags);
			this.showTestingServerRef = webguyType.GetField("LHHEGFHLNJE", bindFlags);
			this.playerStatsArrayRef = webguyType.GetField("NAFCGDLLFJC", bindFlags);
			this.currentAreaRef = webguyType.GetField("FLJLJNLDFAM", bindFlags);
			this.teamListRef = webguyType.GetField("MNEJLPDLMBH", bindFlags);
			this.localPlayerIndexRef = webguyType.GetField("ALEJJPEPFOG", bindFlags);
			this.personGunRef = webguyType.GetField("IEGLIMLBDPH", bindFlags);
			this.nickListRef = webguyType.GetField("CLLDJOMEKIP", bindFlags);
			// modList

			// Load configurations
			this.showSquadServerRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showSquadServer, 1));
			this.showTestingServerRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showTestingServer, 1));
			this.GUIOffsetX = PlayerPrefs.GetInt(PrefNames.GUIOffsetX, this.DefaultGUIOffsetX);
			this.GUIOffsetY = PlayerPrefs.GetInt(PrefNames.GUIOffsetY, this.DefaultGUIOffsetY);
			this.showGUI = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showGUI, 1));
			this.showPlayerStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showPlayerStats, 1));
			this.showWeaponStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showWeaponStats, 1));
			this.showTeammateStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showTeammateStats, 1));
			this.showEloRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showElo, Convert.ToInt32(this.showEloRaw)));
			this.shiftToCrouch = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.shiftToCrouch, 1));

			StartCoroutine(UpdateValuesFunction(0f));

			Logger.LogDebug("WBM: Ready!");
		}

		private void Update()
		{
			// Move UI
			if (Input.GetKey(KeyCode.LeftControl))
			{
				// move GUI
				if (Input.GetKey(KeyCode.LeftShift))
				{
					if (Input.GetKey(KeyCode.UpArrow)) this.GUIOffsetY -= 1;
					if (Input.GetKey(KeyCode.DownArrow)) this.GUIOffsetY += 1;
					if (Input.GetKey(KeyCode.LeftArrow)) this.GUIOffsetX -= 1;
					if (Input.GetKey(KeyCode.RightArrow)) this.GUIOffsetX += 1;
				}
				else
				{
					if (Input.GetKeyDown(KeyCode.UpArrow)) this.GUIOffsetY -= 1;
					if (Input.GetKeyDown(KeyCode.DownArrow)) this.GUIOffsetY += 1;
					if (Input.GetKeyDown(KeyCode.LeftArrow)) this.GUIOffsetX -= 1;
					if (Input.GetKeyDown(KeyCode.RightArrow)) this.GUIOffsetX += 1;
				}

				// reset GUI location
				if (Input.GetKeyDown(KeyCode.R))
				{
					this.GUIOffsetX = this.DefaultGUIOffsetX;
					this.GUIOffsetY = this.DefaultGUIOffsetY;
				}
				this.showConfig = true;
			}
			if (Input.GetKey(KeyCode.RightShift))
			{
				if (Input.GetKeyDown(KeyCode.A)) this.showGUI = !this.showGUI;
				if (Input.GetKeyDown(KeyCode.P)) this.showPlayerStats = !this.showPlayerStats;
				if (Input.GetKeyDown(KeyCode.W)) this.showWeaponStats = !this.showWeaponStats;
				if (Input.GetKeyDown(KeyCode.L)) this.showTeammateStats = !this.showTeammateStats;
				if (Input.GetKeyDown(KeyCode.E)) this.showEloRaw = !this.showEloRaw;
				if (Input.GetKeyDown(KeyCode.S)) this.showSquadServerRaw = !this.showSquadServerRaw;
				if (Input.GetKeyDown(KeyCode.T)) this.showTestingServerRaw = !this.showTestingServerRaw;
				if (Input.GetKeyDown(KeyCode.C)) this.shiftToCrouch = !this.shiftToCrouch;
				if (Input.GetKeyDown(KeyCode.R))
				{
					this.GUIOffsetX = this.DefaultGUIOffsetX;
					this.GUIOffsetY = this.DefaultGUIOffsetY;

					this.showGUI = true;
					this.showPlayerStats = true;
					this.showWeaponStats = true;
					this.showTeammateStats = true;
					this.showEloRaw = true;
					this.showSquadServerRaw = true;
					this.showTestingServerRaw = true;
					this.shiftToCrouch = true;

					this.showConfig = true;
				}

				this.showConfig = true;
			}

			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightShift)) this.showConfig = false;

			// only if right buttton is not held
			if (this.shiftToCrouch && !Input.GetMouseButton(1))
			{
				if (Input.GetKeyDown(KeyCode.LeftShift)) OMOJPGNNKFN.NEELEHFDKBP.EGACOOOGDDC = true;
				if (Input.GetKeyUp(KeyCode.LeftShift)) OMOJPGNNKFN.NEELEHFDKBP.EGACOOOGDDC = false;
			}
		}

		private void OnGUI()
		{
			GUI.skin.box.fontSize = 15;
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.wordWrap = false;

			if (this.showConfig)
			{
				GUI.Box(
		new Rect(Screen.width - 340, 80, 320, 300),
		$@"Configuration

(LCtrl+Arrow) to move GUI one step at a time
(LCtrl+LShift+Arrow) to move long distance
(LCtrl+R) to reset GUI position

GUI X offset: {this.GUIOffsetX}
GUI Y offset: {this.GUIOffsetY}
Show WBM GUI: {this.showGUI} (RShift+A)
Show Elo on leaderboard: {this.showEloRaw} (RShift+E)
Show player stats: {this.showPlayerStats} (RShift+P)
Show weapon stats: {this.showWeaponStats} (RShift+W)
Show teammate stats: {this.showTeammateStats} (RShift+L)
show squad server: {this.showSquadServerRaw} (RShift+S)
show testing server: {this.showTestingServerRaw} (RShift+T)
shift to crouch: {this.shiftToCrouch} (RShift+C)
Reset Everything: (RShift+R)"
	);
			}

			if (!this.showGUI) return;

			GUI.Box(
				new Rect(this.GUIOffsetX, this.GUIOffsetY, 220, 60),
				@"War Brokers Mods
Made by [LP] POMP
v0.9.0.0"
			);

			if (this.localPlayerIndex >= 0)
			{
				if (this.showPlayerStats)
				{
					try
					{
						string killsEloDeltaSign = this.myPlayerStats.killsEloDelta >= 0 ? "+" : "";
						string gamesEloDeltaSign = this.myPlayerStats.gamesEloDelta >= 0 ? "+" : "";

						GUI.Box(
							new Rect(this.GUIOffsetX, this.GUIOffsetY + 65, 220, 160),
							$@"Player stats

KDR: {Util.formatKDR(this.myPlayerStats.kills, this.myPlayerStats.deaths)}
kills Elo: {this.myPlayerStats.killsElo} {killsEloDeltaSign}{this.myPlayerStats.killsEloDelta}
games Elo: {this.myPlayerStats.gamesElo} {gamesEloDeltaSign}{this.myPlayerStats.gamesEloDelta}
Damage dealt: {this.myPlayerStats.damage}
Longest Kill: {this.myPlayerStats.longestKill}m
Points: {this.myPlayerStats.points}
HeadShots: {this.myPlayerStats.headShots}"
						);
					}
					catch (Exception e)
					{
						Logger.LogDebug(e);
					}
				}

				if (this.showWeaponStats)
				{
					try
					{
						GUI.Box(
							new Rect(this.GUIOffsetX, this.GUIOffsetY + 230, 230, 130),
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
						Logger.LogDebug(e);
					}
				}

				if (this.showTeammateStats)
				{
					try
					{
						string teamNames = "Nickname\n\n";
						string teamKDR = "KDR\n\n";
						string teamPoints = "pts\n\n";
						string teamDamage = "Damage\n\n";

						int teamTotalKills = 0;
						int teamTotalDeaths = 0;
						int teamTotalDamage = 0;

						for (int i = 0; i < this.playerStatsArray.Length; i++)
						{
							Data.PlayerStatsStruct stat = this.playerStatsArray[i];

							// if player is not a bot and if player is in my team
							if ((stat.killsElo != 0) && (this.teamList[i] == this.myTeam))
							{
								teamNames += $"{this.nickList[i]}\n";
								teamKDR += $"{Util.formatKDR(stat.kills, stat.deaths)}\n";
								teamPoints += $"{stat.points}\n";
								teamDamage += $"{stat.damage}\n";

								teamTotalKills += stat.kills;
								teamTotalDeaths += stat.deaths;
								teamTotalDamage += stat.damage;
							}
						}

						GUI.Box(new Rect(Screen.width - 320, this.GUIOffsetY + 60, 300, 270), "Team Stats");
						GUI.Label(new Rect(Screen.width - 315, this.GUIOffsetY + 85, 105, 190), teamNames);
						GUI.Label(new Rect(Screen.width - 200, this.GUIOffsetY + 85, 40, 190), teamKDR);
						GUI.Label(new Rect(Screen.width - 150, this.GUIOffsetY + 85, 40, 190), teamPoints);
						GUI.Label(new Rect(Screen.width - 100, this.GUIOffsetY + 85, 70, 190), teamDamage);

						GUI.Label(
							new Rect(Screen.width - 315, this.GUIOffsetY + 270, 300, 55),
							$@"total damage: {teamTotalDamage}
total deaths: {teamTotalDeaths}
total kills: {teamTotalKills}"
						);
					}
					catch (Exception e)
					{
						Logger.LogDebug(e);
					}
				}
			}
		}

		private IEnumerator UpdateValuesFunction(float time)
		{
			try
			{
				this.localPlayerIndex = this.localPlayerIndexRaw;

				if (this.localPlayerIndex >= 0)
				{
					this.playerStatsArray = this.playerStatsArrayRaw;
					this.myPlayerStats = this.playerStatsArray[this.localPlayerIndex];
					this.teamList = this.teamListRaw;
					this.myTeam = this.teamList[localPlayerIndex];
					this.personGun = this.personGunRaw;
					this.nickList = this.nickListRaw;
				}
			}
			catch (Exception e)
			{
				Logger.LogDebug(e);
			}

			yield return new WaitForSeconds(time);

			this.UpdateValues = UpdateValuesFunction(0.1f);
			StartCoroutine(this.UpdateValues);
		}

		private void OnDestroy()
		{
			// save Configuration
			PlayerPrefs.SetInt(PrefNames.showSquadServer, Convert.ToInt32(this.showSquadServerRaw));
			PlayerPrefs.SetInt(PrefNames.showTestingServer, Convert.ToInt32(this.showTestingServerRaw));
			PlayerPrefs.SetInt(PrefNames.GUIOffsetX, this.GUIOffsetX);
			PlayerPrefs.SetInt(PrefNames.GUIOffsetY, this.GUIOffsetY);
			PlayerPrefs.SetInt(PrefNames.showGUI, Convert.ToInt32(this.showGUI));
			PlayerPrefs.SetInt(PrefNames.showPlayerStats, Convert.ToInt32(this.showPlayerStats));
			PlayerPrefs.SetInt(PrefNames.showWeaponStats, Convert.ToInt32(this.showWeaponStats));
			PlayerPrefs.SetInt(PrefNames.showTeammateStats, Convert.ToInt32(this.showTeammateStats));
			PlayerPrefs.SetInt(PrefNames.showElo, Convert.ToInt32(this.showEloRaw));
			PlayerPrefs.SetInt(PrefNames.shiftToCrouch, Convert.ToInt32(this.shiftToCrouch));

			PlayerPrefs.Save();
		}
	}
}
