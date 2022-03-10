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

        public enum GameStateEnum
        {
            WaitingOnPlayers,
            Countdown,
            GameInProgress,
            Results
        }

        [DataContract]
        public class SerializableData
        {
            // game version
            // gamemode
            // teammate list
            // team rank (array of player index)

            [DataMember] public int localPlayerIndex = -1;
            [DataMember] public string[] nickList = new string[] { };
            [DataMember] public PlayerStatsStruct[] playerStatsArray = new PlayerStatsStruct[] { };
            [DataMember] public Data.GameStateEnum gameState;
        }
    }
}
