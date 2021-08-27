using System.Reflection;
using System.Collections;
using UnityEngine;

namespace WBM
{
	public partial class WBM
	{
		private static BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
		private webguy webguy;
		private IEnumerator UpdateValues;
		// private ushort serverPort = 24601;

		public Texture2D crouchIcon;

		private bool showConfig = false;
		private int GUIOffsetX;
		private int DefaultGUIOffsetX = 38;
		private int GUIOffsetY;
		private int DefaultGUIOffsetY = 325;
		private bool showGUI;
		private bool showPlayerStats;
		private bool showWeaponStats;
		private bool showTeammateStats;

		private FieldInfo showEloRef;
		private bool showEloRaw
		{
			get
			{
				return (bool)showEloRef.GetValue(this.webguy);
			}
			set
			{
				showEloRef.SetValue(this.webguy, value);
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
		private Data.PlayerStatsStruct[] playerStatsArray;
		private Data.PlayerStatsStruct myPlayerStats;

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
		private Data.TeamEnum myTeam;

		private Data.TeamEnum[] teamList;

		private FieldInfo localPlayerIndexRef;
		private int localPlayerIndexRaw
		{
			get
			{
				return (int)localPlayerIndexRef.GetValue(this.webguy);
			}
		}
		private int localPlayerIndex;

		private FieldInfo gunTypeRef;
		private Data.GunEnum gunTypeRaw
		{
			get
			{
				return (Data.GunEnum)gunTypeRef.GetValue(this.webguy);
			}
		}
		private Data.GunEnum gunType;

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
		private string[] nickList;
	}
}
