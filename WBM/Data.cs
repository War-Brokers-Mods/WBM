using System.Runtime.Serialization;

namespace WBM
{
	public class Data
	{
		public enum TeamEnum
		{
			None,
			Red,
			Blue
		}

		private enum QuestTypeEnum
		{
			Kill,
			Damage,
			Package,
			Missile,
			Capture,
			Play,
			Travel,
			Assist,
			Finish
		}

		public struct PlayerStatsStruct
		{
			public int kills;
			public int deaths;
			public int damage;
			public int longestKill;
			public int points;
			public int headShots;
			public int vote;
			public int mapVote;
			public int gamesElo;
			public int gamesEloDelta;
			public int killsElo;
			public int killsEloDelta;
		}

		public enum GameModeEnum
		{
			DeathMatch,
			DemolitionDerby,
			ProtectLeader,
			ResourceCapture,
			Race,
			TankBattle,
			TankKing,
			CapturePoint,
			VehicleEscort,
			PackageDrop,
			ScudLaunch,
			BattleRoyale,
			Competitive,
			LobbyCompetitive,
			LobbyBR,
			Count
		}

		[DataContract]
		public class WBMConfig
		{
			[DataMember] public bool showSquadServer = true;
			[DataMember] public bool showTestingServer = true;
			[DataMember] public bool showGUI = true;
			[DataMember] public bool showPlayerStats = true;
			[DataMember] public bool showWeaponStats = true;
			[DataMember] public bool showTeammateStats = true;
			[DataMember] public bool showEloOnLeaderboard = true;
			[DataMember] public bool shiftToCrouch = true;
		}

		[DataContract]
		public class SerializableData
		{
			[DataMember] public int localPlayerIndex = -1;
			[DataMember] public string[] nickList;
			[DataMember] public PlayerStatsStruct[] playerStatsArray = new PlayerStatsStruct[] { };
			[DataMember] public WBMConfig config = new WBMConfig();

			/*
			{
				gameVersion: "",
				gameMode: "",
				teammates: {
					"": {
						nick: "",
						isBot: false,
						stats: {
							kills: 0,
							deaths: 0,
							kdr: 0.0,
							killsElo: 0,
							gamesElo: 0,
							killsEloDelta: 0,
							gamesEloDelta: 0,
						}
					}
				},
				teamRank: {
					"kills": [
						0,
						0,
					]
				}
			}
			*/
		}
	}
}
