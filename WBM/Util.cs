using UnityEngine;
using UnityEngine.Networking;

using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using CPersonGun = BOKJJMJCNGM;

namespace WBM
{
    public class Util
    {
        public async static Task<AudioClip> fetchAudioClip(string where)
        {
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + where, AudioType.WAV))
            {
                uwr.SendWebRequest();

                while (!uwr.isDone) await Task.Delay(10);

                return DownloadHandlerAudioClip.GetContent(uwr);
            }
        }

        public static string data2JSON(Data.SerializableData data)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Data.SerializableData));

            serializer.WriteObject(stream, data);
            byte[] json = stream.ToArray();
            stream.Close();

            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public static string formatKDR(int kills, int deaths)
        {
            return deaths == 0 ? "inf" : formatDecimal((float)kills / deaths);
        }

        public static string formatDecimal(float number)
        {
            return String.Format("{0:0.0}", number);
        }

        public static float getGunZoom(CPersonGun gun)
        {
            // CPersonGun.cameraZoom
            return gun.KJOHIDPMFGJ;
        }

        public static float getGunFireTimer(CPersonGun gun)
        {
            // CPersonGun.fireTimer
            return gun.LKMIHJFMGMP;
        }

        public static float getGunFireVelocity(CPersonGun gun)
        {
            // CPersonGun.fireVel
            return gun.KNBPIMGMICO;
        }

        public static float getGunFireRate(CPersonGun gun)
        {
            // CPersonGun.fireRate
            return gun.AMBMNGKBKHL;
        }

        public static float getGunReloadTimer(CPersonGun gun)
        {
            // CPersonGun.reloadTimer
            return gun.MEEIEBOPDGF;
        }
    }
}
