using BepInEx.Configuration;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<KeyboardShortcut> clearDeathLogShortcut;

        private void clearDeathLogOnKeyPress()
        {
            if (this.clearDeathLogShortcut.Value.IsDown()) this.clearMessagesFuncRef.Invoke(this.webguy, new object[] { });
        }
    }
}
