using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

namespace WBM
{
    /// patch for onFPSChanged function
    [HarmonyPatch(typeof(webguy))]
    [HarmonyPatch(MangledNames.onFPSChanged)]
    class FPSSliderPatch
    {
        private static int defaultTargetFrameRate = -1;
        private static int maxTargetFrameRate = 1000;

        private static GameObject fpsSliderTextObj = GameObject.Find("fpsSlideFuckText");
        private static Slider slider = GameObject.Find("fpsSlider").GetComponent<Slider>();
        private static AccessTools.FieldRef<webguy, float> fpsValueRef = AccessTools.FieldRefAccess<webguy, float>(MangledNames.fpsValue);

        static bool Prefix(webguy __instance, float HJPKONHFKEC)
        {
            fpsValueRef(__instance) = HJPKONHFKEC;
            int targetFrameRate = (int)(HJPKONHFKEC * maxTargetFrameRate);

            if (targetFrameRate == 0)
            {
                ((InfernalBehaviour)__instance).CBBJNPBJGEO(fpsSliderTextObj, __instance.JFNBHCLAPNI("Disabled"));
                targetFrameRate = defaultTargetFrameRate;
            }
            else
            {
                ((InfernalBehaviour)__instance).CBBJNPBJGEO(fpsSliderTextObj, targetFrameRate.ToString());
            }

            if (targetFrameRate > 0 && targetFrameRate < 5) targetFrameRate = 5;

            Application.targetFrameRate = targetFrameRate;
            return false;
        }
    }
}
