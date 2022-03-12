using BepInEx.Configuration;

using UnityEngine;

using System;
using System.Reflection;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> showTestingServer;
        private ConfigEntry<KeyboardShortcut> showTestingServerShortcut;

        private FieldInfo showTestingServerRef;
        private bool showTestingServerRaw
        {
            get
            {
                return (bool)this.showTestingServerRef.GetValue(this.webguy);
            }
            set
            {
                this.showTestingServerRef.SetValue(this.webguy, value);
            }
        }

        private void setupShowTestingServer()
        {
            this.showTestingServerRef = webguyType.GetField("LHHEGFHLNJE", bindFlags);

            this.showTestingServer = Config.Bind("Config", "show testing server", true);
            this.showTestingServer.SettingChanged += this.onShowTestingServerChange;
            this.showTestingServerShortcut = Config.Bind("Hotkeys", "show testing server", new KeyboardShortcut(KeyCode.T, KeyCode.RightShift));
            this.showTestingServerRaw = this.showTestingServer.Value;
        }

        private void doTestingServer()
        {
            this.toggleTestingServerOnKeyPress();
        }

        private void toggleTestingServerOnKeyPress()
        {
            if (this.showTestingServerShortcut.Value.IsDown()) this.showTestingServer.Value = !this.showTestingServer.Value;
        }

        private void onShowTestingServerChange(object sender, EventArgs e)
        {
            this.showTestingServerRaw = this.showTestingServer.Value;
        }
    }
}
