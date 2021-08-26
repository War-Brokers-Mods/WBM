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

		public enum GunEnum
		{
			Invalid = -1,
			ARRifle,
			AKRifle,
			Pistol,
			HuntingRifle,
			RPG,
			Shotgun,
			Sniper,
			UZI,
			GRPG,
			Briefcase,
			Grenade,
			BGM,
			AirStrike,
			Knife,
			Parachute,
			Revolver,
			Minigun,
			GrenadeLauncher,
			SmokeGrenade,
			Fist,
			VSS,
			Sniper50Cal,
			Crossbow,
			SCAR,
			Shotgun2,
			Max
		}

		public static string[] gunNames = new string[]{
			"AR Rifle",
			"AK Rifle",
			"Pistol",
			"Hunting Rifle",
			"RPG",
			"Shotgun",
			"Sniper",
			"UZI",
			"GRPG",
			"Briefcase",
			"Grenade",
			"BGM",
			"Air Strike",
			"Knife",
			"Parachute",
			"Revolver",
			"Minigun",
			"Grenade Launcher",
			"Smoke Grenade",
			"Fist",
			"VSS",
			"50 Cal Sniper",
			"Crossbow",
			"SCAR",
			"Shotgun2",
			"Max"
		};

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

		public struct TeamInfoStruct
		{
			public int index;
			public int score;
			public int deaths;
		}
	}
}
