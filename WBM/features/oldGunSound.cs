using BepInEx.Configuration;

using System;
using System.Reflection;

using CAudioClip = DMJGLPCLOPG;

namespace WBM
{
    public partial class WBM
    {
        private ConfigEntry<bool> useOldGunSoundConf;

        private FieldInfo oldGunSoundRef;
        private CAudioClip oldGunSoundRaw
        {
            get
            {
                return (CAudioClip)this.oldGunSoundRef.GetValue(this.webguy);
            }
        }
        private CAudioClip oldGunSound;

        private FieldInfo AKSoundRef;
        private CAudioClip AKSoundRaw
        {
            get
            {
                return (CAudioClip)this.AKSoundRef.GetValue(this.webguy);
            }
            set
            {
                this.AKSoundRef.SetValue(this.webguy, value);
            }
        }
        private CAudioClip newAKSound;

        private FieldInfo SMGSoundRef;
        private CAudioClip SMGSoundRaw
        {
            get
            {
                return (CAudioClip)this.SMGSoundRef.GetValue(this.webguy);
            }
            set
            {
                this.SMGSoundRef.SetValue(this.webguy, value);
            }
        }
        private CAudioClip newSMGSound;

        private void setupOldGunSound()
        {
            this.oldGunSoundRef = webguyType.GetField(MangledNames.gunShotClip, bindFlags);
            this.AKSoundRef = webguyType.GetField(MangledNames.AKRifleShotClip, bindFlags);
            this.SMGSoundRef = webguyType.GetField(MangledNames.SMGShotClip, bindFlags);

            this.useOldGunSoundConf = Config.Bind("Config", "use old gun sound", true);
            this.useOldGunSoundConf.SettingChanged += this.onOldGunSoundChange;
            this.onOldGunSoundChange(new object(), new EventArgs());

            this.oldGunSound = this.oldGunSoundRaw;
            this.newAKSound = this.AKSoundRaw;
            this.newSMGSound = this.SMGSoundRaw;
        }

        private void onOldGunSoundChange(object sender, EventArgs e)
        {
            if (this.useOldGunSoundConf.Value)
            {
                this.AKSoundRaw = this.oldGunSound;
                this.SMGSoundRaw = this.oldGunSound;
            }
            else
            {
                this.AKSoundRaw = this.newAKSound;
                this.SMGSoundRaw = this.newSMGSound;
            }
        }
    }
}
