using BepInEx.Configuration;

using UnityEngine;
using CFPSGuy = MPFNCCPKLMG;

namespace WBM
{
    partial class WBM
    {
        private ConfigEntry<bool> shiftToCrouch;
        private ConfigEntry<KeyboardShortcut> shiftToCrouchShortcut;

        private void setupShiftToCrouch()
        {
            this.shiftToCrouch = Config.Bind("Config", "shift to crouch", true);
            this.shiftToCrouchShortcut = Config.Bind("Hotkeys", "shift to crouch", new KeyboardShortcut(KeyCode.C, KeyCode.RightShift));
        }

        private void doShiftToCrouch()
        {
            this.crouchOnKeyPress();
            this.toggleShiftToCrouchOnKeyPress();
        }

        private void crouchOnKeyPress()
        {
            // Skip if this setting is not activated
            if (!this.shiftToCrouch.Value) return;

            // Skip if right buttton is being pressed (if weapon is zoomed)
            if (Input.GetMouseButton(1)) return;

            if (Input.GetKeyDown(KeyCode.LeftShift)) setCrouchState(true);
            if (Input.GetKeyUp(KeyCode.LeftShift)) setCrouchState(false);
        }

        private void toggleShiftToCrouchOnKeyPress()
        {
            if (this.shiftToCrouchShortcut.Value.IsDown()) this.shiftToCrouch.Value = !this.shiftToCrouch.Value;
        }

        private void setCrouchState(bool crouchState)
        {
            // CFPSGuy.inSt.isCrouching
            MPFNCCPKLMG.MPPOBOJEEKB.BEJAPOIAHJA = crouchState;
        }
    }
}
