using BepInEx.Configuration;

using UnityEngine;

using System.Reflection;

namespace WBM
{
    partial class WBM
    {
        private MethodInfo drawChatMessageFuncRef;

        private ConfigEntry<KeyboardShortcut> clearChatShortcut;

        private void setupClearChat()
        {
            this.drawChatMessageFuncRef = webguyType.GetMethod("EBDKFEJMEMB", bindFlags);
            this.clearChatShortcut = Config.Bind("Hotkeys", "clear chat", new KeyboardShortcut(KeyCode.Z, KeyCode.RightShift));
        }

        private void doClearChat()
        {
            if (this.clearChatShortcut.Value.IsDown()) this.clearChat();
        }

        private void clearChat()
        {
            for (int i = 0; i < this.chatListRaw.Length; i++) this.chatListRaw[i] = string.Empty;
            this.drawChatMessageFuncRef.Invoke(this.webguy, new object[] { "" });
        }
    }
}
