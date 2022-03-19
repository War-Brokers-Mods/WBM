using BepInEx.Configuration;

using System.Reflection;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<KeyboardShortcut> clearDeathLogShortcut;

        private MethodInfo clearMessagesFuncRef;

        private void setupClearMessage()
        {
            this.clearMessagesFuncRef = webguyType.GetMethod(MangledNames.clearMessages, bindFlags);
        }

        private void doclearMessage()
        {
            if (this.clearDeathLogShortcut.Value.IsDown()) this.clearMessagesFuncRef.Invoke(this.webguy, new object[] { });
        }
    }
}
