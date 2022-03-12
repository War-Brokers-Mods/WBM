using BepInEx.Configuration;

using UnityEngine;

using System;
using System.Reflection;

namespace WBM
{
    public partial class WBM
    {
        private ConfigEntry<bool> useOldGunSoundConf;

        private FieldInfo oldGunSoundRef;
        private AudioClip oldGunSoundRaw
        {
            get
            {
                return ((LPJKBALIFCC)this.oldGunSoundRef.GetValue(this.webguy)).ADCOCHNNCHM;
            }
        }
        private AudioClip oldGunSound;

        private FieldInfo AKSoundRef;
        private LPJKBALIFCC AKSoundRaw
        {
            get
            {
                return (LPJKBALIFCC)this.AKSoundRef.GetValue(this.webguy);
            }
            set
            {
                this.AKSoundRef.SetValue(this.webguy, value);
            }
        }
        private AudioClip newAKSound;

        private FieldInfo SMGSoundRef;
        private LPJKBALIFCC SMGSoundRaw
        {
            get
            {
                return (LPJKBALIFCC)this.SMGSoundRef.GetValue(this.webguy);
            }
            set
            {
                this.SMGSoundRef.SetValue(this.webguy, value);
            }
        }
        private AudioClip newSMGSound;

        private void setupOldGunSound()
        {
            this.oldGunSoundRef = webguyType.GetField("PINGEJAHHDI", bindFlags);
            this.AKSoundRef = webguyType.GetField("BJFBGCMEELH", bindFlags);
            this.SMGSoundRef = webguyType.GetField("HKDDIMFIHCE", bindFlags);

            this.useOldGunSoundConf = Config.Bind("Config", "use old gun sound", true);
            this.useOldGunSoundConf.SettingChanged += this.onOldGunSoundChange;
            this.onOldGunSoundChange(new object(), new EventArgs());

            this.oldGunSound = this.oldGunSoundRaw;
            this.newAKSound = this.AKSoundRaw.ADCOCHNNCHM;
            this.newSMGSound = this.SMGSoundRaw.ADCOCHNNCHM;
        }

        private void onOldGunSoundChange(object sender, EventArgs e)
        {
            if (this.useOldGunSoundConf.Value)
            {
                this.AKSoundRaw.ADCOCHNNCHM = this.oldGunSound;
                this.SMGSoundRaw.ADCOCHNNCHM = this.oldGunSound;
            }
            else
            {
                this.AKSoundRaw.ADCOCHNNCHM = this.newAKSound;
                this.SMGSoundRaw.ADCOCHNNCHM = this.newSMGSound;
            }
        }
    }
}
