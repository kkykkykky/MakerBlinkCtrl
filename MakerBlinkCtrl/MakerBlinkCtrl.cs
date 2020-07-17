using System;
using BepInEx;
using KKAPI.Maker;
using KKAPI.Maker.UI.Sidebar;
using UniRx;

namespace MakerBlinkCtrl
{
    [BepInPlugin(GUID, "Maker Blink Controller", Version)]
    [BepInDependency("marco.kkapi")]
    [BepInProcess("AI-Syoujyo.exe")]
    public class MakerBlinkCtrl : BaseUnityPlugin
    {
        public const string GUID = "kky.il.makerblinkctrl";
        public const string Version = "0.1.0";

        //public static ConfigEntry<BepInEx.Configuration.KeyboardShortcut> BlinkCtrltHotkey { get; private set; }
        private bool blinkFlag;
        private AIChara.ChaControl cc;
        private SidebarToggle sidebarToggle;

        internal void Start()
        {
            //BlinkCtrltHotkey = Config.Bind("Config", "Hotkey", new BepInEx.Configuration.KeyboardShortcut(KeyCode.B, new KeyCode[] { KeyCode.RightControl }), "Hotkey for toggling blinking");

            MakerAPI.MakerBaseLoaded += MakerAPI_Enter;
            MakerAPI.MakerExiting += (_, __) => OnDestroy();
        }

        private void MakerAPI_Enter(object sender, RegisterCustomControlsEvent e)
        {
            cc = MakerAPI.GetCharacterControl();
            blinkFlag = cc.GetEyesBlinkFlag();
            //Logger.LogDebug("on enter: " + blinkFlag);

            sidebarToggle = e.AddSidebarControl(new SidebarToggle("Disable eye blink", true, this));
            sidebarToggle.Value = blinkFlag;
            sidebarToggle.ValueChanged.Subscribe(b => BlinkToggle()); //need to fix so BlinkToggle doesn't get called when control is created

        }
        private void OnDestroy()
        {
            sidebarToggle = null;
            Logger.LogDebug("blinkCtrl sidebarToggle destroyed");
        }

        private void BlinkToggle()
        {
            if (cc != null)
            {
                Logger.LogDebug("BlinkToggle start");
                blinkFlag = cc.GetEyesBlinkFlag();
                Logger.LogDebug($"before: {blinkFlag}");

                cc.ChangeEyesBlinkFlag(!blinkFlag);

                blinkFlag = cc.GetEyesBlinkFlag();
                Logger.LogDebug($"after: {blinkFlag}");
            }
            else
            {
                Logger.LogInfo("Not in Maker and/or Character not loaded.");
            }
        }
        /*
        void Update()
        {
            if (BlinkCtrltHotkey.Value.IsDown())
            {
                Logger.LogDebug("BlinkCtrltHotkey down");
                BlinkToggle();
                //sidebarToggle.Value = !blinkFlag;
            }

        }
        */
    }
}
