using System.Reflection;
using System.Collections;

namespace WBM
{
	public partial class WBM
	{
		private webguy _webguy;
		private IEnumerator UpdateValues;

		// private ushort serverPort = 24601;
		private static BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		private int GUIOffsetX = 40;
		private int GUIOffsetY = 325;

		private FieldInfo showEloRef;
		private bool showEloRaw
		{
			get
			{
				return (bool)showEloRef.GetValue(this._webguy);
			}
			set
			{
				showEloRef.SetValue(this._webguy, value);
			}
		}

		private FieldInfo showSquadServerRef;
		private bool showSquadServerRaw
		{
			get
			{
				return (bool)showSquadServerRef.GetValue(this._webguy);
			}
			set
			{
				showSquadServerRef.SetValue(this._webguy, value);
			}
		}

		private FieldInfo showTestingServerRef;
		private bool showTestingServerRaw
		{
			get
			{
				return (bool)showTestingServerRef.GetValue(this._webguy);
			}
			set
			{
				showTestingServerRef.SetValue(this._webguy, value);
			}
		}

		private FieldInfo playerStatsArrayRef;
		private Data.PlayerStatsStruct[] playerStatsArrayRaw
		{
			get
			{
				PDEMAFHPNBD[] rawPlayerStatsArray = (PDEMAFHPNBD[])playerStatsArrayRef.GetValue(this._webguy);
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
				PDEMAFHPNBD[] rawPlayerStatsArray = (PDEMAFHPNBD[])playerStatsArrayRef.GetValue(this._webguy); ;
				PDEMAFHPNBD currentlyParsing = rawPlayerStatsArray[localPlayerIndexRaw];

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
				return (int)currentAreaRef.GetValue(this._webguy);
			}
			set
			{
				currentAreaRef.SetValue(this._webguy, value);
			}
		}

		private FieldInfo playersActiveRef;
		private bool[] playersActiveRaw
		{
			get
			{
				return (bool[])playersActiveRef.GetValue(this._webguy);
			}
			set
			{
				playersActiveRef.SetValue(this._webguy, value);
			}
		}

		private FieldInfo killListRef;
		private int[] killListRaw
		{
			get
			{
				return (int[])killListRef.GetValue(this._webguy);
			}
			set
			{
				killListRef.SetValue(this._webguy, value);
			}
		}

		private FieldInfo teamListRef;
		private Data.TeamEnum[] teamListRaw
		{
			get
			{
				return (Data.TeamEnum[])teamListRef.GetValue(this._webguy);
			}
			set
			{
				teamListRef.SetValue(this._webguy, value);
			}
		}

		private FieldInfo localPlayerIndexRef;
		private int localPlayerIndexRaw
		{
			get
			{
				return (int)localPlayerIndexRef.GetValue(this._webguy);
			}
			set
			{
				localPlayerIndexRef.SetValue(this._webguy, value);
			}
		}
		private int localPlayerIndex;
	}
}
