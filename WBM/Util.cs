using System;

namespace WBM
{
	public class Util
	{
		public static string formatKDR(int kills, int deaths)
		{
			return deaths == 0 ? "inf" : String.Format("{0:0.0}", (float)kills / (float)deaths);
		}

		public static string getGunName(Data.GunEnum input)
		{
			return (int)input > 0 ? Data.gunNames[(int)input] : "None";
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
