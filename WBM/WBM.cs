using BepInEx;
using BepInEx.Configuration;

using HarmonyLib;

using UnityEngine;

using System;
using System.IO;

namespace WBM
{
    [BepInPlugin("com.developomp.wbm", "War Brokers Mods", "1.7.1.0")]
    public partial class WBM : BaseUnityPlugin
    {
        private void Awake()
        {
            this.harmony = new Harmony("com.developomp.wbm");
            this.harmony.PatchAll();
        }

        private async void Start()
        {
            Logger.LogDebug("Initializing");

            this.webguy = FindObjectOfType<webguy>();

            System.Type webguyType = typeof(webguy);

            this.fetchReferences(webguyType);
            this.setupConfiguration();

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

            this.oldGunSound = this.oldGunSoundRaw;
            this.newAKSound = this.AKSoundRaw.ADCOCHNNCHM;
            this.newSMGSound = this.SMGSoundRaw.ADCOCHNNCHM;

            //
            // Websocket
            //

            server = new WebSocketSharp.Server.WebSocketServer($"ws://127.0.0.1:{this.serverPort}");
            server.AddWebSocketService<WSJSONService>("/json");
            server.Start();

            StartCoroutine(UpdateValuesFunction());

            Logger.LogDebug("Ready!");
        }

        private void Update()
        {
            this.handleKeyPresses();
        }

        private void OnGUI()
        {
            this.setupGUI();

            if (this._showConfig) this.drawConfig();

            if (!this.showGUI.Value) return;

            this.drawWBMVersion();

            // don't draw if player is not in a games
            if (this.data.localPlayerIndex < 0) return;

            this.drawPlayerStats();
            this.drawWeaponStats();
            this.drawTeamStats();
        }

        private void onDestroy()
        {
            // properly stop websocket server
            this.server.Stop();
        }

        //
        // setup functions
        //

        private void fetchReferences(System.Type webguyType)
        {
            this.showEloOnLeaderboardRef = webguyType.GetField("KDOBENAOLLF", bindFlags);
            this.showSquadServerRef = webguyType.GetField("PHPIBBCFKFI", bindFlags);
            this.showTestingServerRef = webguyType.GetField("LHHEGFHLNJE", bindFlags);
            this.playerStatsArrayRef = webguyType.GetField("NAFCGDLLFJC", bindFlags);
            this.currentAreaRef = webguyType.GetField("FLJLJNLDFAM", bindFlags);
            this.teamListRef = webguyType.GetField("MNEJLPDLMBH", bindFlags);
            this.localPlayerIndexRef = webguyType.GetField("ALEJJPEPFOG", bindFlags);
            this.personGunRef = webguyType.GetField("IEGLIMLBDPH", bindFlags);
            this.nickListRef = webguyType.GetField("CLLDJOMEKIP", bindFlags);
            this.gameStateRef = webguyType.GetField("MCGMEPGBCKK", bindFlags);
            this.chatListRef = webguyType.GetField("MOOBJBOCANE", bindFlags);

            this.addMessageFuncRef = webguyType.GetMethod("NBPKLIOLLEI", bindFlags);
            this.clearMessagesFuncRef = webguyType.GetMethod("IOCHBBACKFA", bindFlags);
            this.drawChatMessageFuncRef = webguyType.GetMethod("EBDKFEJMEMB", bindFlags);

            this.oldGunSoundRef = webguyType.GetField("PINGEJAHHDI", bindFlags);
            this.AKSoundRef = webguyType.GetField("BJFBGCMEELH", bindFlags);
            this.SMGSoundRef = webguyType.GetField("HKDDIMFIHCE", bindFlags);
        }

        private void setupConfiguration()
        {
            this.showGUI = Config.Bind("Config", "show GUI", true);
            this.showGUIShortcut = Config.Bind("Hotkeys", "show GUI Shortcut", new KeyboardShortcut(KeyCode.A, KeyCode.RightShift));

            this.GUIOffsetX = Config.Bind("Config", "GUI Horizontal position", 38, new ConfigDescription("WBM GUI Horizontal position", new AcceptableValueRange<int>(0, Screen.width)));
            this.GUIOffsetY = Config.Bind("Config", "GUI Vertical position", 325, new ConfigDescription("WBM GUI Vertical position", new AcceptableValueRange<int>(0, Screen.height)));
            this.resetGUIShortcut = Config.Bind("Hotkeys", "reset GUI position", new KeyboardShortcut(KeyCode.R, KeyCode.LeftControl));

            this.shiftToCrouch = Config.Bind("Config", "shift to crouch", true);
            this.shiftToCrouchShortcut = Config.Bind("Hotkeys", "shift to crouch", new KeyboardShortcut(KeyCode.C, KeyCode.RightShift));

            this.killStreakSFX = Config.Bind("Config", "kill streak sound effect", true);
            this.killStreakSFXShortcut = Config.Bind("Hotkeys", "kill streak sound effect", new KeyboardShortcut(KeyCode.F, KeyCode.RightShift));

            this.showPlayerStats = Config.Bind("Config", "show player statistics", true);
            this.showPlayerStatsShortcut = Config.Bind("Hotkeys", "show player statistics", new KeyboardShortcut(KeyCode.P, KeyCode.RightShift));

            this.showWeaponStats = Config.Bind("Config", "show weapon statistics", true);
            this.showWeaponStatsShortcut = Config.Bind("Hotkeys", "show weapon statistics", new KeyboardShortcut(KeyCode.W, KeyCode.RightShift));

            this.showTeamStats = Config.Bind("Config", "show team statistics", true);
            this.showTeamStatsShortcut = Config.Bind("Hotkeys", "show team statistics", new KeyboardShortcut(KeyCode.L, KeyCode.RightShift));

            this.showEloOnLeaderboard = Config.Bind("Config", "show Elo on leaderboard", true);
            this.showEloOnLeaderboard.SettingChanged += this.showEloOnLeaderboardChanged;
            this.showEloOnLeaderboardShortcut = Config.Bind("Hotkeys", "show Elo on leaderboard", new KeyboardShortcut(KeyCode.E, KeyCode.RightShift));
            this.showEloOnLeaderboardRaw = this.showEloOnLeaderboard.Value;

            this.showSquadServer = Config.Bind("Config", "show squad server", true);
            this.showSquadServer.SettingChanged += this.showSquadServerChanged;
            this.showSquadServerShortcut = Config.Bind("Hotkeys", "show squad server", new KeyboardShortcut(KeyCode.S, KeyCode.RightShift));
            this.showSquadServerRaw = this.showSquadServer.Value;

            this.showTestingServer = Config.Bind("Config", "show testing server", true);
            this.showTestingServer.SettingChanged += this.showTestingServerChanged;
            this.showTestingServerShortcut = Config.Bind("Hotkeys", "show testing server", new KeyboardShortcut(KeyCode.T, KeyCode.RightShift));
            this.showTestingServerRaw = this.showTestingServer.Value;

            this.clearChatShortcut = Config.Bind("Hotkeys", "clear chat", new KeyboardShortcut(KeyCode.Z, KeyCode.RightShift));
            this.clearDeathLogShortcut = Config.Bind("Hotkeys", "clear messages", new KeyboardShortcut(KeyCode.X, KeyCode.RightShift));

            this.useOldGunSoundConf = Config.Bind("Config", "use old gun sound", true);
            this.useOldGunSoundConf.SettingChanged += this.useOldGunSoundChanged;
            this.useOldGunSoundChanged(new object(), new EventArgs());
        }
    }
}
