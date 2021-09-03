using BepInEx;
using UnityEngine;
using UnityEngine.Networking;

using System;
using System.IO;
using System.Collections;
using System.Threading.Tasks;

using WebSocketSharp.Server;

namespace WBM
{
	[BepInPlugin("com.developomp.wbm", "War Brokers Mods", "1.3.0.0")]
	public partial class WBM : BaseUnityPlugin
	{
		private async void Start()
		{
			Logger.LogDebug("WBM: Initializing");
			this.webguy = FindObjectOfType<webguy>();

			System.Type webguyType = typeof(webguy);

			this.showEloOnLeaderboardRef = webguyType.GetField("KDOBENAOLLF", bindFlags);
			this.showSquadServerRef = webguyType.GetField("PHPIBBCFKFI", bindFlags);
			this.showTestingServerRef = webguyType.GetField("LHHEGFHLNJE", bindFlags);
			this.playerStatsArrayRef = webguyType.GetField("NAFCGDLLFJC", bindFlags);
			this.currentAreaRef = webguyType.GetField("FLJLJNLDFAM", bindFlags);
			this.teamListRef = webguyType.GetField("MNEJLPDLMBH", bindFlags);
			this.localPlayerIndexRef = webguyType.GetField("ALEJJPEPFOG", bindFlags);
			this.personGunRef = webguyType.GetField("IEGLIMLBDPH", bindFlags);
			this.nickListRef = webguyType.GetField("CLLDJOMEKIP", bindFlags);
			this.gameStateRef = webguyType.GetField("MCGMEPGBCKK", bindFlags);
			this.addMessageFuncRef = webguyType.GetMethod("NBPKLIOLLEI", bindFlags);

			// Load configurations
			this.showSquadServerRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showSquadServer, 1));
			this.showTestingServerRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showTestingServer, 1));
			this.GUIOffsetX = PlayerPrefs.GetInt(PrefNames.GUIOffsetX, this.DefaultGUIOffsetX);
			this.GUIOffsetY = PlayerPrefs.GetInt(PrefNames.GUIOffsetY, this.DefaultGUIOffsetY);
			this.data.config.showGUI = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showGUI, 1));
			this.data.config.showPlayerStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showPlayerStats, 1));
			this.data.config.showWeaponStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showWeaponStats, 1));
			this.data.config.showTeammateStats = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showTeammateStats, 1));
			this.showEloOnLeaderboardRaw = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.showElo, 1));
			this.data.config.shiftToCrouch = Convert.ToBoolean(PlayerPrefs.GetInt(PrefNames.shiftToCrouch, 1));

			this.killStreakAudioSource = this.gameObject.AddComponent<AudioSource>();

			if (!Directory.Exists(this.audioPath))
			{
				Logger.LogError($"Directory {this.audioPath} does not exist. Aborting!");
				GameObject.Destroy(this);
			}

			foreach (string fileName in Directory.GetFiles(this.audioPath))
			{
				Logger.LogDebug(Path.GetFileNameWithoutExtension(fileName));

				using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + Path.Combine(this.audioPath, fileName), AudioType.WAV))
				{
					uwr.SendWebRequest();

					try
					{
						while (!uwr.isDone) await Task.Delay(5);

						if (uwr.result == UnityWebRequest.Result.ProtocolError) Logger.LogError($"{uwr.error}");
						else
						{
							this.killStreakAudioDict.Add(
								Path.GetFileNameWithoutExtension(fileName),
								DownloadHandlerAudioClip.GetContent(uwr)
							);

						}
					}
					catch (Exception err)
					{
						Logger.LogError($"{err.Message}, {err.StackTrace}");
					}
				}
			}

			server = new WebSocketServer($"ws://127.0.0.1:{this.serverPort}");
			server.AddWebSocketService<WSJSONService>("/json");
			server.Start();

			StartCoroutine(UpdateValuesFunction());

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

			// Configuration shortbut
			if (Input.GetKey(KeyCode.RightShift))
			{
				if (Input.GetKeyDown(KeyCode.A)) this.data.config.showGUI = !this.data.config.showGUI;
				if (Input.GetKeyDown(KeyCode.P)) this.data.config.showPlayerStats = !this.data.config.showPlayerStats;
				if (Input.GetKeyDown(KeyCode.W)) this.data.config.showWeaponStats = !this.data.config.showWeaponStats;
				if (Input.GetKeyDown(KeyCode.L)) this.data.config.showTeammateStats = !this.data.config.showTeammateStats;
				if (Input.GetKeyDown(KeyCode.E)) this.showEloOnLeaderboardRaw = !this.showEloOnLeaderboardRaw;
				if (Input.GetKeyDown(KeyCode.S)) this.showSquadServerRaw = !this.showSquadServerRaw;
				if (Input.GetKeyDown(KeyCode.T)) this.showTestingServerRaw = !this.showTestingServerRaw;
				if (Input.GetKeyDown(KeyCode.C)) this.data.config.shiftToCrouch = !this.data.config.shiftToCrouch;
				if (Input.GetKeyDown(KeyCode.R))
				{
					this.GUIOffsetX = this.DefaultGUIOffsetX;
					this.GUIOffsetY = this.DefaultGUIOffsetY;

					this.data.config.showGUI = true;
					this.data.config.showPlayerStats = true;
					this.data.config.showWeaponStats = true;
					this.data.config.showTeammateStats = true;
					this.showEloOnLeaderboardRaw = true;
					this.showSquadServerRaw = true;
					this.showTestingServerRaw = true;
					this.data.config.shiftToCrouch = true;
				}

				this.showConfig = true;
			}

			// hide config
			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightShift)) this.showConfig = false;

			// only if right buttton is not held
			if (this.data.config.shiftToCrouch && !Input.GetMouseButton(1))
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
Show WBM GUI: {this.data.config.showGUI} (RShift+A)
Show Elo on leaderboard: {this.showEloOnLeaderboardRaw} (RShift+E)
Show player stats: {this.data.config.showPlayerStats} (RShift+P)
Show weapon stats: {this.data.config.showWeaponStats} (RShift+W)
Show teammate stats: {this.data.config.showTeammateStats} (RShift+L)
show squad server: {this.showSquadServerRaw} (RShift+S)
show testing server: {this.showTestingServerRaw} (RShift+T)
shift to crouch: {this.data.config.shiftToCrouch} (RShift+C)
Reset Everything: (RShift+R)"
	);
			}

			if (!this.data.config.showGUI) return;

			GUI.Box(
				new Rect(this.GUIOffsetX, this.GUIOffsetY, 220, 60),
				@"War Brokers Mods
Made by [LP] POMP
v1.3.0.0"
			);

			if (this.data.localPlayerIndex >= 0)
			{
				if (this.data.config.showPlayerStats)
				{
					try
					{
						string killsEloDeltaSign = this.myPlayerStats.killsEloDelta >= 0 ? "+" : "";
						string gamesEloDeltaSign = this.myPlayerStats.gamesEloDelta >= 0 ? "+" : "";

						GUI.Box(
							new Rect(this.GUIOffsetX, this.GUIOffsetY + 65, 220, 180),
							$@"Player stats

KDR: {Util.formatKDR(this.myPlayerStats.kills, this.myPlayerStats.deaths)}
kills Elo: {this.myPlayerStats.killsElo} {killsEloDeltaSign}{Util.formatDecimal(this.myPlayerStats.killsEloDelta / 10)}
games Elo: {this.myPlayerStats.gamesElo} {gamesEloDeltaSign}{Util.formatDecimal(this.myPlayerStats.gamesEloDelta / 10)}
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

				if (this.data.config.showWeaponStats)
				{
					try
					{
						GUI.Box(
							new Rect(this.GUIOffsetX, this.GUIOffsetY + 250, 230, 130),
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

				if (this.data.config.showTeammateStats)
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

						int teamStatOffset = (this.data.gameState == Data.GameStateEnum.Results) ? 340 : 0;
						GUI.Box(new Rect(Screen.width - 320, 385 + teamStatOffset, 300, 270), "Team Stats");
						GUI.Label(new Rect(Screen.width - 315, 410 + teamStatOffset, 105, 190), teamNames);
						GUI.Label(new Rect(Screen.width - 200, 410 + teamStatOffset, 40, 190), teamKDR);
						GUI.Label(new Rect(Screen.width - 150, 410 + teamStatOffset, 40, 190), teamPoints);
						GUI.Label(new Rect(Screen.width - 100, 410 + teamStatOffset, 70, 190), teamDamage);

						GUI.Label(
							new Rect(Screen.width - 315, 595 + teamStatOffset, 300, 55),
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
			}
		}

		private IEnumerator UpdateValuesFunction()
		{
			try
			{
				this.data.localPlayerIndex = this.localPlayerIndexRaw;

				if (this.data.localPlayerIndex >= 0)
				{
					this.data.playerStatsArray = this.playerStatsArrayRaw;
					this.myPlayerStats = this.data.playerStatsArray[this.data.localPlayerIndex];
					this.teamList = this.teamListRaw;
					this.myTeam = this.teamList[this.data.localPlayerIndex];
					this.personGun = this.personGunRaw;
					this.data.nickList = this.nickListRaw;
					this.data.gameState = this.gameStateRaw;

					// check if deaths has changed since the last value update
					if (this.prevDeaths == this.myPlayerStats.deaths)
					{
						this.killStreak = this.myPlayerStats.kills - this.killCountBeforeDeath;

						if (this.prevKills != this.myPlayerStats.kills)
						{
							Logger.LogDebug(this.killStreakAudioDict);

							if (this.killStreakSFXDictionary.ContainsKey(this.killStreak))
							{
								this.killStreakAudioSource.clip = this.killStreakAudioDict[this.killStreakSFXDictionary[this.killStreak]];
								this.killStreakAudioSource.Play();

								this.addMessageFuncRef.Invoke(this.webguy, new object[] { $"You are on a {this.killStreak} kill streak", -1 });
							}
						}
					}
					else
					{
						// reset kill streak when death count changes

						this.killCountBeforeDeath = this.myPlayerStats.kills;
						this.prevDeaths = this.myPlayerStats.deaths;
						this.killStreak = 0;
					}
					this.prevKills = this.myPlayerStats.kills;
				}

				this.data.config.showSquadServer = this.showSquadServerRaw;
				this.data.config.showTestingServer = this.showTestingServerRaw;
				this.data.config.showEloOnLeaderboard = this.showEloOnLeaderboardRaw;

				this.server.WebSocketServices["/json"].Sessions.Broadcast(Util.data2JSON(data));
			}
			catch (Exception e)
			{
				Logger.LogError(e);
			}

			yield return new WaitForSeconds(0.1f);

			this.UpdateValues = UpdateValuesFunction();
			StartCoroutine(this.UpdateValues);
		}

		private void OnDestroy()
		{
			// save Configuration
			PlayerPrefs.SetInt(PrefNames.showSquadServer, Convert.ToInt32(this.showSquadServerRaw));
			PlayerPrefs.SetInt(PrefNames.showTestingServer, Convert.ToInt32(this.showTestingServerRaw));
			PlayerPrefs.SetInt(PrefNames.GUIOffsetX, this.GUIOffsetX);
			PlayerPrefs.SetInt(PrefNames.GUIOffsetY, this.GUIOffsetY);
			PlayerPrefs.SetInt(PrefNames.showGUI, Convert.ToInt32(this.data.config.showGUI));
			PlayerPrefs.SetInt(PrefNames.showPlayerStats, Convert.ToInt32(this.data.config.showPlayerStats));
			PlayerPrefs.SetInt(PrefNames.showWeaponStats, Convert.ToInt32(this.data.config.showWeaponStats));
			PlayerPrefs.SetInt(PrefNames.showTeammateStats, Convert.ToInt32(this.data.config.showTeammateStats));
			PlayerPrefs.SetInt(PrefNames.showElo, Convert.ToInt32(this.showEloOnLeaderboardRaw));
			PlayerPrefs.SetInt(PrefNames.shiftToCrouch, Convert.ToInt32(this.data.config.shiftToCrouch));

			PlayerPrefs.Save();
		}
	}
}
