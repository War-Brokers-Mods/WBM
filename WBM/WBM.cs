using BepInEx;

namespace WBM
{
    /// <summary>
    /// Class <c>WBM</c> is a regular unity script component (<c>GameObject</c>).
    /// The functions <c>Awake</c>, <c>Start</c>, <c>Update</c>, <c>OnGUI</c>, and <c>onDestroy</c>
    /// are event functions that gets called on specific stages of the component's lifecycle. 
    /// More information can be found in the <see href="https://docs.unity3d.com/Manual/ExecutionOrder.html">unity's documentation page</see>.
    /// </summary>
    [BepInPlugin("com.developomp.wbm", "War Brokers Mods", "1.7.1.0")]
    public partial class WBM : BaseUnityPlugin
    {
        /// This function is called as soon as the component becomes active.
        /// It is the first event function that's called during the component's lifecycle.
        private void Awake()
        {
            Logger.LogDebug("Initializing");

            this.initCore();
        }

        /// This function is called only once before the first frame update.
        /// The component is more or less initialized at this point,
        /// and it is this function that completes the initialization process.
        private async void Start()
        {
            await this.setupCore();

            this.setupOldGunSound();
            this.setupWSSever();
            this.setupClearChatOnKeyPress();
            this.setupShiftToCrouch();
            this.setupShowEloOnLeaderBoard();
            this.setupShowSquadServer();
            this.setupShowTestingServer();
            this.setupKillStreakSFX();

            StartCoroutine(UpdateValuesFunction());

            Logger.LogDebug("Ready!");
        }

        /// This function is called on each frame.
        private void Update()
        {
            this.moveUIOnKeyPress();
            this.resetUIOnKeyPress();
            this.toggleUIOnKeyPress();
            this.toggleKillStreakSFXOnKeyPress();
            this.togglePlayerStatsOnKeyPress();
            this.toggleWeaponStatsOnKeyPress();
            this.toggleTeamStatsOnKeyPress();
            this.toggleLeaderboardEloOnKeyPress();
            this.toggleShowSquadServerOnKeyPress();
            this.toggleTestingServerOnKeyPress();
            this.clearChatOnKeyPress();
            this.clearDeathLogOnKeyPress();
            this.showConfigOnKeyPress();

            this.toggleShiftToCrouchOnKeyPress();
            this.crouchOnKeyPress();
        }

        /// Called multiple times per frame in response to GUI events.
        /// The Layout and Repaint events are processed first,
        /// followed by a Layout and keyboard/mouse event for each input event.
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

        /// This function is called after the component has been disabled and is ready to be destroyed.
        /// It is the last event function that's called during the component's lifecycle.
        private void onDestroy()
        {
            this.destroyWSSever();
        }
    }
}
