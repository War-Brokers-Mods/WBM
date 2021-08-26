using System.Reflection;
using System.Collections;
using UnityEngine;

namespace WBM
{
	public partial class WBM
	{
		private webguy webguy;
		private IEnumerator UpdateValues;

		// private ushort serverPort = 24601;
		private static BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		private bool showConfig = false;
		private int GUIOffsetX;
		private int DefaultGUIOffsetX = 40;
		private int GUIOffsetY;
		private int DefaultGUIOffsetY = 325;
		private bool showGUI;
		private bool showPlayerStats;
		private bool showWeaponStats;

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
		private Data.PlayerStatsStruct MyPlayerStatsRaw
		{
			get
			{
				PDEMAFHPNBD currentlyParsing = ((PDEMAFHPNBD[])playerStatsArrayRef.GetValue(this.webguy))[localPlayerIndexRaw];

				return new Data.PlayerStatsStruct
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
		}
		private Data.PlayerStatsStruct MyPlayerStats;

		private FieldInfo currentAreaRef;
		private int currentAreaRaw
		{
			get
			{
				return (int)currentAreaRef.GetValue(this.webguy);
			}
		}

		private FieldInfo playersActiveRef;
		private bool[] playersActiveRaw
		{
			get
			{
				return (bool[])playersActiveRef.GetValue(this.webguy);
			}
		}

		private FieldInfo killListRef;
		private int[] killListRaw
		{
			get
			{
				return (int[])killListRef.GetValue(this.webguy);
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
	}
}
