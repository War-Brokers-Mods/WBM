using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace WBM
{
	public class Util
	{
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
			return deaths == 0 ? "inf" : formatDecimal((float)kills / (float)deaths);
		}

		public static string formatDecimal(float number)
		{
			return String.Format("{0:0.0}", number);
		}

		public static float getGunZoom(NGNJNHEFLHB gun)
		{
			return gun.ADLGCCMDNED;
		}

		public static float getGunFireTimer(NGNJNHEFLHB gun)
		{
			return gun.MAKBOBOAAHG;
		}

		public static float getGunFireVelocity(NGNJNHEFLHB gun)
		{
			return gun.HOIKHOJJBOG;
		}

		public static float getGunFireRate(NGNJNHEFLHB gun)
		{
			return gun.IHEEIAIOABG;
		}

		public static float getGunReloadTimer(NGNJNHEFLHB gun)
		{
			return gun.NBLDKJAKFIB;
		}

		public static float getGunCooldownTimer(NGNJNHEFLHB gun)
		{
			return gun.LBOBALHJBDM;
		}
	}
}
