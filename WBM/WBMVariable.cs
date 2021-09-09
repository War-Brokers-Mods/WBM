using BepInEx.Configuration;
using UnityEngine;

using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace WBM
{
	public partial class WBM
	{
		// important boy
		private webguy webguy;
		private IEnumerator UpdateValues;

		// websocket data stuff
		private WebSocketSharp.Server.WebSocketServer server;
		private ushort serverPort = 24601;
		private Data.SerializableData data = new Data.SerializableData();

		// internal or temporary
		private bool _showConfig;

		// Configurations
		private ConfigEntry<bool> showGUI;
		private ConfigEntry<KeyboardShortcut> showGUIShortcut;

		private ConfigEntry<int> GUIOffsetX;
		private ConfigEntry<int> GUIOffsetY;
		private ConfigEntry<KeyboardShortcut> resetGUIShortcut;

		private ConfigEntry<bool> shiftToCrouch;
		private ConfigEntry<KeyboardShortcut> shiftToCrouchShortcut;

		private ConfigEntry<bool> killStreakSFX;
		private ConfigEntry<KeyboardShortcut> killStreakSFXShortcut;

		private ConfigEntry<bool> showPlayerStats;
		private ConfigEntry<KeyboardShortcut> showPlayerStatsShortcut;
		private ConfigEntry<bool> showWeaponStats;
		private ConfigEntry<KeyboardShortcut> showWeaponStatsShortcut;
		private ConfigEntry<bool> showTeamStats;
		private ConfigEntry<KeyboardShortcut> showTeamStatsShortcut;

		private ConfigEntry<bool> showEloOnLeaderboard;
		private void showEloOnLeaderboardChanged(object sender, EventArgs e)
		{
			this.showEloOnLeaderboardRaw = this.showEloOnLeaderboard.Value;
		}
		private ConfigEntry<KeyboardShortcut> showEloOnLeaderboardShortcut;

		private ConfigEntry<bool> showSquadServer;
		private void showSquadServerChanged(object sender, EventArgs e)
		{
			this.showSquadServerRaw = this.showSquadServer.Value;
		}
		private ConfigEntry<KeyboardShortcut> showSquadServerShortcut;

		private ConfigEntry<bool> showTestingServer;
		private void showTestingServerChanged(object sender, EventArgs e)
		{
			this.showTestingServerRaw = this.showTestingServer.Value;
		}
		private ConfigEntry<KeyboardShortcut> showTestingServerShortcut;

		// Audio
		private Dictionary<string, AudioClip> killStreakAudioDict = new Dictionary<string, AudioClip>();
		private string audioPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets/audio");
		private AudioSource killStreakAudioSource;
		private Dictionary<int, string> killStreakSFXDictionary = new Dictionary<int, string>()
		{
			{10, "rampage"},
			{20, "killing spree"},
			{30, "unstoppable"},
			{50, "godlike"},
			{69, "nice"},
		};

		// memory stuff
		private static BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		private FieldInfo showEloOnLeaderboardRef;
		private bool showEloOnLeaderboardRaw
		{
			get
			{
				return (bool)this.showEloOnLeaderboardRef.GetValue(this.webguy);
			}
			set
			{
				this.showEloOnLeaderboardRef.SetValue(this.webguy, value);
			}
		}

		private FieldInfo showSquadServerRef;
		private bool showSquadServerRaw
		{
			get
			{
				return (bool)this.showSquadServerRef.GetValue(this.webguy);
			}
			set
			{
				this.showSquadServerRef.SetValue(this.webguy, value);
			}
		}

		private FieldInfo showTestingServerRef;
		private bool showTestingServerRaw
		{
			get
			{
				return (bool)this.showTestingServerRef.GetValue(this.webguy);
			}
			set
			{
				this.showTestingServerRef.SetValue(this.webguy, value);
			}
		}

		private FieldInfo playerStatsArrayRef;
		private Data.PlayerStatsStruct[] playerStatsArrayRaw
		{
			get
			{
				PDEMAFHPNBD[] rawPlayerStatsArray = (PDEMAFHPNBD[])this.playerStatsArrayRef.GetValue(this.webguy);
				Data.PlayerStatsStruct[] result = new Data.PlayerStatsStruct[rawPlayerStatsArray.Length];

				for (int i = 0; i < rawPlayerStatsArray.Length; i++)
				{
					PDEMAFHPNBD currentlyParsing = rawPlayerStatsArray[i];

					result[i] = new Data.PlayerStatsStruct
					{
						kills = currentlyParsing.CFMGCOGACPA,
						deaths = currentlyParsing.GABHLIIJHBJ,
						damage = currentlyParsing.CECNBFABADA,
						longestKill = currentlyParsing.GDFIBEEKMJA,
						points = currentlyParsing.HNHFAABONHO,
						headShots = currentlyParsing.GJLLOFLEHHD,
						vote = currentlyParsing.JCBAKMONPGC,
						mapVote = currentlyParsing.BOFANBBCNOH,
						gamesElo = currentlyParsing.IBHFIBAOKCB,
						gamesEloDelta = currentlyParsing.JMGOHGIGLPI,
						killsElo = currentlyParsing.GBIABKEEFOC,
						killsEloDelta = currentlyParsing.JAAKOCPIGJL,
					};
				}

				return result;
			}
		}
		private Data.PlayerStatsStruct myPlayerStats;
		private int prevDeaths = 0;
		private int prevKills = 0;
		private int killCountBeforeDeath = 0;
		private int killStreak = 0;

		private FieldInfo currentAreaRef;
		private int currentAreaRaw
		{
			get
			{
				return (int)this.currentAreaRef.GetValue(this.webguy);
			}
		}

		private FieldInfo teamListRef;
		private Data.TeamEnum[] teamListRaw
		{
			get
			{
				return (Data.TeamEnum[])this.teamListRef.GetValue(this.webguy);
			}
		}
		private Data.TeamEnum[] teamList;
		private Data.TeamEnum myTeam;

		private FieldInfo localPlayerIndexRef;
		private int localPlayerIndexRaw
		{
			get
			{
				return (int)this.localPlayerIndexRef.GetValue(this.webguy);
			}
		}

		private FieldInfo personGunRef;
		private NGNJNHEFLHB personGunRaw
		{
			get
			{
				return (NGNJNHEFLHB)this.personGunRef.GetValue(this.webguy);
			}
		}
		private NGNJNHEFLHB personGun;

		private FieldInfo nickListRef;
		private string[] nickListRaw
		{
			get
			{
				return (string[])this.nickListRef.GetValue(this.webguy);
			}
		}

		private FieldInfo gameStateRef;
		private Data.GameStateEnum gameStateRaw
		{
			get
			{
				return (Data.GameStateEnum)this.gameStateRef.GetValue(this.webguy);
			}
		}

		private MethodInfo addMessageFuncRef;
	}
}
