using UnityEngine;

using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using WebSocketSharp.Server;

namespace WBM
{
	public partial class WBM
	{
		private static BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
		private webguy webguy;
		private IEnumerator UpdateValues;
		private WebSocketServer server;
		private ushort serverPort = 24601;

		public Data.SerializableData data = new Data.SerializableData();

		private bool showConfig = false;
		private int GUIOffsetX;
		private int DefaultGUIOffsetX = 38;
		private int GUIOffsetY;
		private int DefaultGUIOffsetY = 325;

		private Dictionary<string, AudioClip> killStreakAudioDict = new Dictionary<string, AudioClip>();
		private string audioPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets/audio");
		private AudioSource killStreakAudioSource;
		private Dictionary<int, string> killStreakSFXDictionary = new Dictionary<int, string>()
		{
			{10, "rampage"},
			{20,"killing spree"},
			{30, "unstoppable"},
			{50, "godlike"}
		};

		private FieldInfo showEloOnLeaderboardRef;
		private bool showEloOnLeaderboardRaw
		{
			get
			{
				return (bool)showEloOnLeaderboardRef.GetValue(this.webguy);
			}
			set
			{
				showEloOnLeaderboardRef.SetValue(this.webguy, value);
			}
		}

		private FieldInfo showSquadServerRef;
		private bool showSquadServerRaw
		{
			get
			{
				return (bool)showSquadServerRef.GetValue(this.webguy);
			}
			set
			{
				showSquadServerRef.SetValue(this.webguy, value);
			}
		}

		private FieldInfo showTestingServerRef;
		private bool showTestingServerRaw
		{
			get
			{
				return (bool)showTestingServerRef.GetValue(this.webguy);
			}
			set
			{
				showTestingServerRef.SetValue(this.webguy, value);
			}
		}

		private FieldInfo playerStatsArrayRef;
		private Data.PlayerStatsStruct[] playerStatsArrayRaw
		{
			get
			{
				PDEMAFHPNBD[] rawPlayerStatsArray = (PDEMAFHPNBD[])playerStatsArrayRef.GetValue(this.webguy);
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
				return (int)currentAreaRef.GetValue(this.webguy);
			}
		}

		private FieldInfo teamListRef;
		private Data.TeamEnum[] teamListRaw
		{
			get
			{
				return (Data.TeamEnum[])teamListRef.GetValue(this.webguy);
			}
		}
		private Data.TeamEnum[] teamList;
		private Data.TeamEnum myTeam;

		private FieldInfo localPlayerIndexRef;
		private int localPlayerIndexRaw
		{
			get
			{
				return (int)localPlayerIndexRef.GetValue(this.webguy);
			}
		}

		private FieldInfo personGunRef;
		private NGNJNHEFLHB personGunRaw
		{
			get
			{
				return (NGNJNHEFLHB)personGunRef.GetValue(this.webguy);
			}
		}
		private NGNJNHEFLHB personGun;

		private FieldInfo nickListRef;
		private string[] nickListRaw
		{
			get
			{
				return (string[])nickListRef.GetValue(this.webguy);
			}
		}

		private FieldInfo gameStateRef;
		private Data.GameStateEnum gameStateRaw
		{
			get
			{
				return (Data.GameStateEnum)gameStateRef.GetValue(this.webguy);
			}
		}

		private MethodInfo addMessageFuncRef;
	}
}
