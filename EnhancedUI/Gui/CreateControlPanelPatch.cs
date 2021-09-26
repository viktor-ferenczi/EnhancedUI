using System.Collections.Generic;
using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage.Game.ModAPI;

namespace EnhancedUI.Gui
{
    [HarmonyPatch(typeof(MyGuiScreenTerminal))]
    internal static class CreateControlPanelPatch
    {
        private static ChromiumGuiControl? _currentControl;
        private const string NAME = "Terminal";
        private static WebContent Content = new WebContent();
      
        //Replaces the controls on the Control Panel section of the terminal.
        [HarmonyPatch("CreateControlPanelPageControls")]
        [HarmonyPrefix]
        private static bool CreatePrefix(MyGuiScreenTerminal __instance, MyGuiControlTabPage page, Dictionary<MyTerminalPageEnum, MyGuiControlBase> ___m_defaultFocusedControlKeyboard)
        {
            // Code for a reload button
            //MyGuiControlButton refreshButton = new MyGuiControlButton(new Vector2(0, 0.0f), VRage.Game.MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, "Reload HTML page.", new System.Text.StringBuilder("Reload Page"), onButtonClick: new Action<MyGuiControlButton>(ReloadAction));
            //page.Controls.Add(refreshButton);

            page.Name = "PageControlPanel";
            page.TextEnum = MySpaceTexts.ControlPanel;
            page.TextScale = 0.7005405f;

            var control = new ChromiumGuiControl(Content, NAME)
            {
                Position = new(0f, 0.005f),
                Size = new(0.9f, 0.7f)
            };
            //Adds the gui elements to the screen
            page.Controls.Add(_currentControl);
            page.Controls.Add(_currentControl.Wheel);

            //Sets m_defaultFocusedControlKeyboard to control
            ___m_defaultFocusedControlKeyboard[MyTerminalPageEnum.ControlPanel] = _currentControl;


            __instance.Closed += InstanceOnClosed;

            return false;
        }

        [HarmonyPatch("HandleUnhandledInput")]
        [HarmonyPrefix]
        private static bool HandleUnhandledPrefix() => !(_currentControl?.CheckMouseOver() ?? false);

        private static void InstanceOnClosed(MyGuiScreenBase source, bool isUnloading)
        {
            _currentControl = null;
            source.Closed -= InstanceOnClosed;
        }

        //Code for reloading the page with the reload button
        /*
            // Adds the GUI elements to the screen
            page.Controls.Add(control);
            page.Controls.Add(control.Wheel);

            ___m_defaultFocusedControlKeyboard[MyTerminalPageEnum.ControlPanel] = control;
            return false;
        }

        /* Code for reloading the page with the reload button
        private static void ReloadAction(MyGuiControlButton myGuiControlButton)
        {
            control.ReloadPage();
        }
        */
    }
}