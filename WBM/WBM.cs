using BepInEx;

namespace WBM
{
    /// <summary>
    /// Class <c>WBM</c> is a regular unity script component (<c>GameObject</c>).
    /// The functions <c>Awake</c>, <c>Start</c>, <c>Update</c>, <c>OnGUI</c>, and <c>onDestroy</c>
    /// are event functions that gets called on specific stages of the component's lifecycle. 
    /// More information can be found in the <see href="https://docs.unity3d.com/Manual/ExecutionOrder.html">unity's documentation page</see>.
    /// </summary>
    [BepInPlugin("com.developomp.wbm", "War Brokers Mods", "1.8.0.0")]
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
            this.setupClearChat();
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
            this.doCore();

            this.doKillStreakSFX();
            this.doPlayerStats();
            this.doWeaponStats();
            this.doTeamStats();
            this.doLeaderboardElo();
            this.doShowSquadServer();
            this.doTestingServer();
            this.doClearChat();
            this.doclearMessage();
            this.doShiftToCrouch();
        }

        /// Called multiple times per frame in response to GUI events.
        /// The Layout and Repaint events are processed first,
        /// followed by a Layout and keyboard/mouse event for each input event.
        private void OnGUI()
        {
            this.drawCoreUI();

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
