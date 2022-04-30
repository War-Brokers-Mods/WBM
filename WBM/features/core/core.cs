using BepInEx.Configuration;

using HarmonyLib;

using UnityEngine;

using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using CPersonGun = BOKJJMJCNGM;
using SPlayerStats = AOCINPBCGCH;

namespace WBM
{
    partial class WBM
    {
        private webguy webguy;
        private System.Type webguyType;
        private Harmony harmony;
        private IEnumerator UpdateValues;

        //
        // internal or temporary
        //

        private bool _showConfig;

        //
        // memory stuff
        //

        private static BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        //
        // Methods
        //

        private MethodInfo addMessageFuncRef;

        private IEnumerator UpdateValuesFunction()
        {
            try
            {
                this.data.localPlayerIndex = this.localPlayerIndexRaw;

                if (this.data.localPlayerIndex >= 0)
                {
                    this.data.playerStatsArray = this.playerStatsArrayRaw;
                    this.myPlayerStats = this.data.playerStatsArray[this.data.localPlayerIndex];
                    this.teamList = this.teamListRaw;
                    this.myTeam = this.teamList[this.data.localPlayerIndex];
                    this.personGun = this.personGunRaw;
                    this.data.nickList = this.nickListRaw;
                    this.data.gameState = this.gameStateRaw;

                    // check if deaths has changed since the last value update
                    if (this.prevDeaths == this.myPlayerStats.deaths)
                    {
                        this.killStreak = this.myPlayerStats.kills - this.killCountBeforeDeath;

                        if (this.prevKills != this.myPlayerStats.kills)
                        {
                            if (this.killStreakSFX.Value && this.killStreakSFXDictionary.ContainsKey(this.killStreak))
                            {
                                this.killStreakAudioSource.clip = this.AudioDict[this.killStreakSFXDictionary[this.killStreak]];
                                this.killStreakAudioSource.Play();

                                this.addMessageFuncRef.Invoke(this.webguy, new object[] { $"You are on a {this.killStreak} kill streak", -1 });
                            }
                        }
                    }
                    else
                    {
                        // reset kill streak when death count changes

                        this.killCountBeforeDeath = this.myPlayerStats.kills;
                        this.prevDeaths = this.myPlayerStats.deaths;
                        this.killStreak = 0;
                    }
                    this.prevKills = this.myPlayerStats.kills;
                }

                this.server.WebSocketServices["/json"].Sessions.Broadcast(Util.data2JSON(data));
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            yield return new WaitForSeconds(0.1f);

            this.UpdateValues = UpdateValuesFunction();
            StartCoroutine(this.UpdateValues);
        }

        private FieldInfo playerStatsArrayRef;
        private Data.PlayerStatsStruct[] playerStatsArrayRaw
        {
            get
            {
                SPlayerStats[] rawPlayerStatsArray = (SPlayerStats[])this.playerStatsArrayRef.GetValue(this.webguy);
                Data.PlayerStatsStruct[] result = new Data.PlayerStatsStruct[rawPlayerStatsArray.Length];

                for (int i = 0; i < rawPlayerStatsArray.Length; i++)
                {
                    SPlayerStats currentlyParsing = rawPlayerStatsArray[i];

                    result[i] = new Data.PlayerStatsStruct
                    {
                        kills = currentlyParsing.GDAAGFMFLNI,
                        deaths = currentlyParsing.CCJMDIHCBDP,
                        damage = currentlyParsing.OCGEPPGBDNH,
                        longestKill = currentlyParsing.CBALDOLFIHK,
                        points = currentlyParsing.AIEMHCIHIFN,
                        headShots = currentlyParsing.JGFCAIPDMIF,
                        vote = currentlyParsing.KHEHPPEJLFP,
                        mapVote = currentlyParsing.DDIINLJFLFD,
                        gamesElo = currentlyParsing.MHLFOIJHBAC,
                        gamesEloDelta = currentlyParsing.HFHOJIAHLIH,
                        killsElo = currentlyParsing.MKKAAMMOBJB,
                        killsEloDelta = currentlyParsing.CJLEKFIHJBM,
                    };
                }

                return result;
            }
        }
        private Data.PlayerStatsStruct myPlayerStats;
        private int prevDeaths = 0;
        private int prevKills = 0;
        private int killCountBeforeDeath = 0;

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
        private CPersonGun personGunRaw
        {
            get
            {
                return (CPersonGun)this.personGunRef.GetValue(this.webguy);
            }
        }
        private CPersonGun personGun;

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

        private FieldInfo chatListRef;
        private string[] chatListRaw
        {
            get
            {
                return (string[])this.chatListRef.GetValue(this.webguy);
            }
            set
            {
                this.chatListRef.SetValue(this.webguy, value);
            }
        }

        //
        // Audio
        //

        private Dictionary<string, AudioClip> AudioDict = new Dictionary<string, AudioClip>();
        private string audioPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets/audio");

        private void initCore()
        {
            this.harmony = new Harmony(WBM.programID);
            this.harmony.PatchAll();
        }

        private async System.Threading.Tasks.Task setupCore()
        {
            this.webguy = FindObjectOfType<webguy>();
            this.webguyType = typeof(webguy);

            //
            // References
            //

            this.playerStatsArrayRef = webguyType.GetField(MangledNames.statsList, bindFlags);
            this.teamListRef = webguyType.GetField(MangledNames.teamList, bindFlags);
            this.localPlayerIndexRef = webguyType.GetField(MangledNames.localPlayerIndex, bindFlags);
            this.personGunRef = webguyType.GetField(MangledNames.personGun, bindFlags);
            this.nickListRef = webguyType.GetField(MangledNames.nickList, bindFlags);
            this.gameStateRef = webguyType.GetField(MangledNames.gameState, bindFlags);
            this.chatListRef = webguyType.GetField(MangledNames.chatList, bindFlags);

            this.addMessageFuncRef = webguyType.GetMethod(MangledNames.addMessage, bindFlags);

            //
            // Configurations
            //

            this.showGUI = Config.Bind("Config", "show GUI", true);
            this.showGUIShortcut = Config.Bind("Hotkeys", "show GUI Shortcut", new KeyboardShortcut(KeyCode.A, KeyCode.RightShift));

            this.GUIOffsetX = Config.Bind("Config", "GUI Horizontal position", 38, new ConfigDescription("WBM GUI Horizontal position", new AcceptableValueRange<int>(0, Screen.width)));
            this.GUIOffsetY = Config.Bind("Config", "GUI Vertical position", 325, new ConfigDescription("WBM GUI Vertical position", new AcceptableValueRange<int>(0, Screen.height)));
            this.resetGUIShortcut = Config.Bind("Hotkeys", "reset GUI position", new KeyboardShortcut(KeyCode.R, KeyCode.LeftControl));

            this.showPlayerStats = Config.Bind("Config", "show player statistics", true);
            this.showPlayerStatsShortcut = Config.Bind("Hotkeys", "show player statistics", new KeyboardShortcut(KeyCode.P, KeyCode.RightShift));

            this.showWeaponStats = Config.Bind("Config", "show weapon statistics", true);
            this.showWeaponStatsShortcut = Config.Bind("Hotkeys", "show weapon statistics", new KeyboardShortcut(KeyCode.W, KeyCode.RightShift));

            this.showTeamStats = Config.Bind("Config", "show team statistics", true);
            this.showTeamStatsShortcut = Config.Bind("Hotkeys", "show team statistics", new KeyboardShortcut(KeyCode.L, KeyCode.RightShift));

            this.clearDeathLogShortcut = Config.Bind("Hotkeys", "clear messages", new KeyboardShortcut(KeyCode.X, KeyCode.RightShift));

            //
            // Audio
            //

            this.killStreakAudioSource = this.gameObject.AddComponent<AudioSource>();

            if (!Directory.Exists(this.audioPath))
            {
                Logger.LogError($"Directory {this.audioPath} does not exist. Aborting!");
                GameObject.Destroy(this);
            }

            foreach (string fileName in Directory.GetFiles(this.audioPath))
            {
                Logger.LogDebug("Loading AudioClip " + Path.GetFileNameWithoutExtension(fileName));

                this.AudioDict.Add(
                    Path.GetFileNameWithoutExtension(fileName),
                    await Util.fetchAudioClip(Path.Combine(this.audioPath, fileName))
                );
            }
        }

        private void doCore()
        {
            this.moveUIOnKeyPress();
            this.resetUIOnKeyPress();
            this.toggleUIOnKeyPress();
            this.showConfigOnKeyPress();
        }
    }
}
