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
