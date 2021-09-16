using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

namespace WBM
{
	[HarmonyPatch(typeof(webguy))]
	[HarmonyPatch("FHBMKCDMGII")]
	class FPSSliderPatch
	{
		private static int defaultTargetFrameRate = -1;
		private static int maxTargetFrameRate = 1000;

		private static GameObject fpsSliderTextObj = GameObject.Find("fpsSlideFuckText");
		private static Slider slider = GameObject.Find("fpsSlider").GetComponent<Slider>();
		private static AccessTools.FieldRef<webguy, float> fpsValueRef = AccessTools.FieldRefAccess<webguy, float>("CLLNACDIPHE");

		static bool Prefix(webguy __instance, float JKNNNLEEIAO)
		{
			fpsValueRef(__instance) = JKNNNLEEIAO;
			int targetFrameRate = (int)(JKNNNLEEIAO * maxTargetFrameRate);

			if (targetFrameRate == 0)
			{
				((InfernalBehaviour)__instance).KKFJBNFGKEP(fpsSliderTextObj, __instance.HNDAMJPNGAE("Disabled"));
				targetFrameRate = defaultTargetFrameRate;
			}
			else
			{
				((InfernalBehaviour)__instance).KKFJBNFGKEP(fpsSliderTextObj, targetFrameRate.ToString());
			}

			if (targetFrameRate > 0 && targetFrameRate < 5) targetFrameRate = 5;

			Application.targetFrameRate = targetFrameRate;
			return false;
		}
	}
}
