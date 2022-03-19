using BepInEx.Configuration;

using System;
using System.Reflection;

using CAudioClip = DMJGLPCLOPG;

namespace WBM
{
    public partial class WBM
    {
        private ConfigEntry<bool> useOldGunSoundConf;

        private CAudioClip oldGunSound;

        private FieldInfo AKSoundRef;
        private CAudioClip AKSoundRaw
        {
            set
            {
                this.AKSoundRef.SetValue(this.webguy, value);
            }
        }
        private CAudioClip newAKSound;

        private FieldInfo SMGSoundRef;
        private CAudioClip SMGSoundRaw
        {
            set
            {
                this.SMGSoundRef.SetValue(this.webguy, value);
            }
        }
        private CAudioClip newSMGSound;

        private void setupOldGunSound()
        {
            this.AKSoundRef = webguyType.GetField(MangledNames.AKRifleShotClip, bindFlags);
            this.SMGSoundRef = webguyType.GetField(MangledNames.SMGShotClip, bindFlags);

            this.useOldGunSoundConf = Config.Bind("Config", "use old gun sound", true);
            this.useOldGunSoundConf.SettingChanged += this.onOldGunSoundChange;
            this.onOldGunSoundChange(new object(), new EventArgs());

            this.oldGunSound = new DMJGLPCLOPG("Sound/gun_shot", 1f, 0f);
            this.oldGunSound.GPBDJPDFDMJ(50f, 1_000f);

            this.newAKSound = new CAudioClip("Sound/AK47_Krinkov_Close_Single", 1f, 0f);
            this.newAKSound.GPBDJPDFDMJ(50f, 1_000f);

            this.newSMGSound = new DMJGLPCLOPG("Sound/smg_gun_shot", 1f, 0f);
            this.newSMGSound.GPBDJPDFDMJ(50f, 1_000f);
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
