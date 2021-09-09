using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.Networking;

using System;
using System.IO;
using System.Threading.Tasks;

namespace WBM
{
	[BepInPlugin("com.developomp.wbm", "War Brokers Mods", "1.5.0.0")]
	public partial class WBM : BaseUnityPlugin
	{
		private async void Start()
		{
			Logger.LogDebug("Initializing");
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
			this.chatListRef = webguyType.GetField("MOOBJBOCANE", bindFlags);

			this.addMessageFuncRef = webguyType.GetMethod("NBPKLIOLLEI", bindFlags);
			this.clearMessagesFuncRef = webguyType.GetMethod("IOCHBBACKFA", bindFlags);
			this.drawChatMessageFuncRef = webguyType.GetMethod("EBDKFEJMEMB", bindFlags);

			// Configurations
			this.showGUI = Config.Bind("Config", "show GUI", true);
			this.showGUIShortcut = Config.Bind("Hotkeys", "show GUI Shortcut", new KeyboardShortcut(KeyCode.A, KeyCode.RightShift));

			this.GUIOffsetX = Config.Bind("Config", "GUI Horizontal position", 38, new ConfigDescription("WBM GUI Horizontal position", new AcceptableValueRange<int>(0, Screen.width)));
			this.GUIOffsetY = Config.Bind("Config", "GUI Vertical position", 325, new ConfigDescription("WBM GUI Vertical position", new AcceptableValueRange<int>(0, Screen.height)));
			this.resetGUIShortcut = Config.Bind("Hotkeys", "reset GUI position", new KeyboardShortcut(KeyCode.R, KeyCode.LeftControl));

			this.shiftToCrouch = Config.Bind("Config", "shift to crouch", true);
			this.shiftToCrouchShortcut = Config.Bind("Hotkeys", "shift to crouch", new KeyboardShortcut(KeyCode.C, KeyCode.RightShift));

			this.killStreakSFX = Config.Bind("Config", "kill streak sound effect", true);
			this.killStreakSFXShortcut = Config.Bind("Hotkeys", "kill streak sound effect", new KeyboardShortcut(KeyCode.F, KeyCode.RightShift));

			this.showPlayerStats = Config.Bind("Config", "show player statistics", true);
			this.showPlayerStatsShortcut = Config.Bind("Hotkeys", "show player statistics", new KeyboardShortcut(KeyCode.P, KeyCode.RightShift));

			this.showWeaponStats = Config.Bind("Config", "show weapon statistics", true);
			this.showWeaponStatsShortcut = Config.Bind("Hotkeys", "show weapon statistics", new KeyboardShortcut(KeyCode.W, KeyCode.RightShift));

			this.showTeamStats = Config.Bind("Config", "show team statistics", true);
			this.showTeamStatsShortcut = Config.Bind("Hotkeys", "show team statistics", new KeyboardShortcut(KeyCode.L, KeyCode.RightShift));

			this.showEloOnLeaderboard = Config.Bind("Config", "show Elo on leaderboard", true);
			this.showEloOnLeaderboard.SettingChanged += this.showEloOnLeaderboardChanged;
			this.showEloOnLeaderboardShortcut = Config.Bind("Hotkeys", "show Elo on leaderboard", new KeyboardShortcut(KeyCode.E, KeyCode.RightShift));
			this.showEloOnLeaderboardRaw = this.showEloOnLeaderboard.Value;

			this.showSquadServer = Config.Bind("Config", "show squad server", true);
			this.showSquadServer.SettingChanged += this.showSquadServerChanged;
			this.showSquadServerShortcut = Config.Bind("Hotkeys", "show squad server", new KeyboardShortcut(KeyCode.S, KeyCode.RightShift));
			this.showSquadServerRaw = this.showSquadServer.Value;

			this.showTestingServer = Config.Bind("Config", "show testing server", true);
			this.showTestingServer.SettingChanged += this.showTestingServerChanged;
			this.showTestingServerShortcut = Config.Bind("Hotkeys", "show testing server", new KeyboardShortcut(KeyCode.T, KeyCode.RightShift));
			this.showTestingServerRaw = this.showTestingServer.Value;

			this.clearChatShortcut = Config.Bind("Hotkeys", "clear chat", new KeyboardShortcut(KeyCode.Z, KeyCode.RightShift));
			this.clearDeathLogShortcut = Config.Bind("Hotkeys", "clear messages", new KeyboardShortcut(KeyCode.X, KeyCode.RightShift));

			// Audio

			this.killStreakAudioSource = this.gameObject.AddComponent<AudioSource>();

			if (!Directory.Exists(this.audioPath))
			{
				Logger.LogError($"Directory {this.audioPath} does not exist. Aborting!");
				GameObject.Destroy(this);
			}

			foreach (string fileName in Directory.GetFiles(this.audioPath))
			{
				Logger.LogDebug("Loading AudioClip " + Path.GetFileNameWithoutExtension(fileName));

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

			// Websocket

			server = new WebSocketSharp.Server.WebSocketServer($"ws://127.0.0.1:{this.serverPort}");
			server.AddWebSocketService<WSJSONService>("/json");
			server.Start();

			StartCoroutine(UpdateValuesFunction());

			Logger.LogDebug("Ready!");
		}

		private void Update()
		{
			// Move UI
			if (Input.GetKey(KeyCode.LeftControl))
			{
				// move GUI
				if (Input.GetKey(KeyCode.LeftShift))
				{
					if (Input.GetKey(KeyCode.UpArrow)) this.GUIOffsetY.Value -= 1;
					if (Input.GetKey(KeyCode.DownArrow)) this.GUIOffsetY.Value += 1;
					if (Input.GetKey(KeyCode.LeftArrow)) this.GUIOffsetX.Value -= 1;
					if (Input.GetKey(KeyCode.RightArrow)) this.GUIOffsetX.Value += 1;
				}
				else
				{
					if (Input.GetKeyDown(KeyCode.UpArrow)) this.GUIOffsetY.Value -= 1;
					if (Input.GetKeyDown(KeyCode.DownArrow)) this.GUIOffsetY.Value += 1;
					if (Input.GetKeyDown(KeyCode.LeftArrow)) this.GUIOffsetX.Value -= 1;
					if (Input.GetKeyDown(KeyCode.RightArrow)) this.GUIOffsetX.Value += 1;
				}
			}

			// reset GUI position
			if (this.resetGUIShortcut.Value.IsDown())
			{
				this.GUIOffsetX.Value = (int)this.GUIOffsetX.DefaultValue;
				this.GUIOffsetY.Value = (int)this.GUIOffsetY.DefaultValue;
			}

			if (this.showGUIShortcut.Value.IsDown()) this.showGUI.Value = !this.showGUI.Value;
			if (this.shiftToCrouchShortcut.Value.IsDown()) this.shiftToCrouch.Value = !this.shiftToCrouch.Value;
			if (this.killStreakSFXShortcut.Value.IsDown()) this.killStreakSFX.Value = !this.killStreakSFX.Value;
			if (this.showPlayerStatsShortcut.Value.IsDown()) this.showPlayerStats.Value = !this.showPlayerStats.Value;
			if (this.showWeaponStatsShortcut.Value.IsDown()) this.showWeaponStats.Value = !this.showWeaponStats.Value;
			if (this.showTeamStatsShortcut.Value.IsDown()) this.showTeamStats.Value = !this.showTeamStats.Value;
			if (this.showEloOnLeaderboardShortcut.Value.IsDown()) this.showEloOnLeaderboard.Value = !this.showEloOnLeaderboard.Value;
			if (this.showSquadServerShortcut.Value.IsDown()) this.showSquadServer.Value = !this.showSquadServer.Value;
			if (this.showTestingServerShortcut.Value.IsDown()) this.showTestingServer.Value = !this.showTestingServer.Value;
			if (this.clearChatShortcut.Value.IsDown()) this.clearChat();
			if (this.clearDeathLogShortcut.Value.IsDown()) this.clearMessagesFuncRef.Invoke(this.webguy, new object[] { });

			// config visibility
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift)) this._showConfig = true;
			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightShift)) this._showConfig = false;

			// only if right buttton is not held
			if (this.shiftToCrouch.Value && !Input.GetMouseButton(1))
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

			if (this._showConfig)
			{
				GUI.Box(
		new Rect(Screen.width - 340, 70, 320, 340),
		$@"Configuration

move GUI: LCtrl+LShift+Arrow
move GUI by pixel: LCtrl+Arrow
reset GUI position: {this.resetGUIShortcut.Value}
clear chat: {this.clearChatShortcut.Value}
clear death log: {this.clearDeathLogShortcut.Value}

GUI X offset: {this.GUIOffsetX.Value}
GUI Y offset: {this.GUIOffsetY.Value}
Show WBM GUI: {this.showGUI.Value} ({this.showGUIShortcut.Value})
Show Elo on leaderboard: {this.showEloOnLeaderboard.Value} ({this.showEloOnLeaderboardShortcut.Value})
Show player stats: {this.showPlayerStats.Value} ({this.showPlayerStatsShortcut.Value})
Show weapon stats: {this.showWeaponStats.Value} ({this.showWeaponStatsShortcut.Value})
Show teammate stats: {this.showTeamStats.Value} ({this.showTeamStatsShortcut.Value})
show squad server: {this.showSquadServer.Value} ({this.showSquadServerShortcut.Value})
show testing server: {this.showTestingServer.Value} ({this.showTestingServerShortcut.Value})
shift to crouch: {this.shiftToCrouch.Value} ({this.shiftToCrouchShortcut.Value})
kill streak SFX: {this.killStreakSFX.Value} ({this.killStreakSFXShortcut.Value})"
	);
			}

			if (!this.showGUI.Value) return;

			GUI.Box(
				new Rect(this.GUIOffsetX.Value, this.GUIOffsetY.Value, 220, 60),
				@"War Brokers Mods
Made by [LP] POMP
v1.5.0.0"
			);

			if (this.data.localPlayerIndex >= 0)
			{
				if (this.showPlayerStats.Value)
				{
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

				if (this.showWeaponStats.Value)
				{
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

				if (this.showTeamStats.Value)
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

						int teamStatOffset = (this.data.gameState == Data.GameStateEnum.Results) ? 310 : 0;
						GUI.Box(new Rect(Screen.width - 320, 415 + teamStatOffset, 300, 270), "Team Stats");
						GUI.Label(new Rect(Screen.width - 315, 440 + teamStatOffset, 105, 190), teamNames);
						GUI.Label(new Rect(Screen.width - 200, 440 + teamStatOffset, 40, 190), teamKDR);
						GUI.Label(new Rect(Screen.width - 150, 440 + teamStatOffset, 40, 190), teamPoints);
						GUI.Label(new Rect(Screen.width - 100, 440 + teamStatOffset, 70, 190), teamDamage);

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
	}
}
