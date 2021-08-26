using BepInEx;
using UnityEngine;

using System;
using System.Collections;

namespace WBM
{
	[BepInPlugin("com.developomp.wbm", "War Brokers Mods", "0.4.0.0")]
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
			this.playersActiveRef = webguyType.GetField("EPNEEBOFKHA", bindFlags);
			this.killListRef = webguyType.GetField("IKKEILIBONF", bindFlags);
			this.teamListRef = webguyType.GetField("MNEJLPDLMBH", bindFlags);
			this.localPlayerIndexRef = webguyType.GetField("ALEJJPEPFOG", bindFlags);
			this.gunTypeRef = webguyType.GetField("JBMCOIGJFMG", bindFlags);
			this.personGunRef = webguyType.GetField("IEGLIMLBDPH", bindFlags);

			// Load configurations
			this.showSquadServerRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showSquadServer, 1));
			this.showTestingServerRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showTestingServer, 1));
			this.GUIOffsetX = PlayerPrefs.GetInt(PrefNames.GUIOffsetX, this.DefaultGUIOffsetX);
			this.GUIOffsetY = PlayerPrefs.GetInt(PrefNames.GUIOffsetY, this.DefaultGUIOffsetY);
			this.showGUI = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showGUI, 1));
			this.showPlayerStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showPlayerStats, 1));
			this.showWeaponStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showWeaponStats, 1));
			this.showEloRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showElo, Convert.ToInt32(this.showEloRaw)));

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
				if (Input.GetKeyDown(KeyCode.E)) this.showEloRaw = !this.showEloRaw;
				if (Input.GetKeyDown(KeyCode.S)) this.showSquadServerRaw = !this.showSquadServerRaw;
				if (Input.GetKeyDown(KeyCode.T)) this.showTestingServerRaw = !this.showTestingServerRaw;
				if (Input.GetKeyDown(KeyCode.C)) this.showConfig = !this.showConfig;
				if (Input.GetKeyDown(KeyCode.R))
				{
					this.GUIOffsetX = this.DefaultGUIOffsetX;
					this.GUIOffsetY = this.DefaultGUIOffsetY;
					this.showGUI = true;
					this.showPlayerStats = true;
					this.showWeaponStats = true;
					this.showEloRaw = true;
					this.showSquadServerRaw = true;
					this.showTestingServerRaw = true;
					this.showConfig = true;
				}

				this.showConfig = true;
			}

			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightShift)) this.showConfig = false;
		}

		private void OnGUI()
		{
			if (this.showConfig)
			{
				GUI.Box(
		new Rect(this.GUIOffsetX + 205, this.GUIOffsetY, 320, 265),
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
show squad server: {this.showSquadServerRaw} (RShift+S)
show testing server: {this.showTestingServerRaw} (RShift+T)
Reset Everything: (RShift+R)"
	);
			}

			if (!this.showGUI) return;
			GUI.skin.box.fontSize = 15;

			GUI.Box(
				new Rect(this.GUIOffsetX, this.GUIOffsetY, 200, 60),
				@"War Brokers Mods
Made by [LP] POMP
v0.4.0.0"
			);

			if (this.localPlayerIndex >= 0)
			{
				if (this.showPlayerStats)
				{
					try
					{
						string killsEloDeltaSign = this.MyPlayerStats.killsEloDelta >= 0 ? "+" : "";
						string gamesEloDeltaSign = this.MyPlayerStats.gamesEloDelta >= 0 ? "+" : "";
						string kdr = this.MyPlayerStats.deaths == 0 ? "inf" : (this.MyPlayerStats.kills / this.MyPlayerStats.deaths).ToString();

						GUI.Box(
							new Rect(this.GUIOffsetX, this.GUIOffsetY + 65, 200, 145),
							$@"Player stats

KDR: {kdr}
kills Elo: {this.MyPlayerStats.killsElo} {killsEloDeltaSign}{this.MyPlayerStats.killsEloDelta}
games Elo: {this.MyPlayerStats.gamesElo} {gamesEloDeltaSign}{this.MyPlayerStats.gamesEloDelta}
Damage dealt: {this.MyPlayerStats.damage}
Longest Kill: {this.MyPlayerStats.longestKill}
Points: {this.MyPlayerStats.points}
HeadShots: {this.MyPlayerStats.headShots}"
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
							new Rect(this.GUIOffsetX, this.GUIOffsetY + 215, 220, 145),
							$@"Weapon stats

Weapon: {Util.getGunName(this.gunType)}
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
			}
		}

		private IEnumerator UpdateValuesFunction(float time)
		{
			try
			{
				this.localPlayerIndex = this.localPlayerIndexRaw;

				if (this.localPlayerIndex >= 0)
				{
					this.MyPlayerStats = this.MyPlayerStatsRaw;
					this.gunType = this.gunTypeRaw;
					this.personGun = this.personGunRaw;
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
			PlayerPrefs.SetInt(PrefNames.showElo, Convert.ToInt32(this.showEloRaw));
			PlayerPrefs.Save();
		}
	}
}
