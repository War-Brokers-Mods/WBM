using BepInEx.Configuration;

using UnityEngine;

using System;
using System.Reflection;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showSquadServer;
        private ConfigEntry<KeyboardShortcut> showSquadServerShortcut;

        private FieldInfo showSquadServerRef;
        private bool showSquadServerRaw
        {
            get
            {
                return (bool)this.showSquadServerRef.GetValue(this.webguy);
            }
            set
            {
                this.showSquadServerRef.SetValue(this.webguy, value);
            }
        }

        private void setupShowSquadServer()
        {
            this.showSquadServerRef = webguyType.GetField("IKOJAEHBIJI", bindFlags);

            this.showSquadServer = Config.Bind("Config", "show squad server", true);
            this.showSquadServer.SettingChanged += this.onShowSquadServerChange;
            this.showSquadServerShortcut = Config.Bind("Hotkeys", "show squad server", new KeyboardShortcut(KeyCode.S, KeyCode.RightShift));
            this.showSquadServerRaw = this.showSquadServer.Value;
        }

        private void doShowSquadServer()
        {
            this.toggleShowSquadServerOnKeyPress();
        }

        private void toggleShowSquadServerOnKeyPress()
        {
            if (this.showSquadServerShortcut.Value.IsDown()) this.showSquadServer.Value = !this.showSquadServer.Value;
        }

        private void onShowSquadServerChange(object sender, EventArgs e)
        {
            this.showSquadServerRaw = this.showSquadServer.Value;
        }
    }
}