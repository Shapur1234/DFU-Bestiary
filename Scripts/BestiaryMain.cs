using System.Collections;

using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

using UnityEngine;

namespace BestiaryMod
{
    public class BestiaryMain : MonoBehaviour
    {
        private static Mod mod;

        static BestiaryUI bestiaryUIScreen;
        static KeyCode openMenuKeyCode;

        static bool firstSetting = true;

        void Update()
        {
            if (bestiaryUIScreen == null) 
            {
                bestiaryUIScreen = new BestiaryUI(DaggerfallWorkshop.Game.DaggerfallUI.UIManager);
                mod.LoadSettings();
            }
            if (InputManager.Instance.GetKeyDown(openMenuKeyCode) && GameManager.Instance.IsPlayerOnHUD)
                DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
            else if (bestiaryUIScreen.isShowing && InputManager.Instance.GetKeyDown(openMenuKeyCode))
                bestiaryUIScreen.CloseWindow();
            else if (openMenuKeyCode != KeyCode.None && !InputManager.Instance.IsPaused && InputManager.Instance.GetKeyDown(openMenuKeyCode))
                if (bestiaryUIScreen.isShowing)
                    bestiaryUIScreen.CloseWindow();
        }

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;

            var go = new GameObject(mod.Title);
            go.AddComponent<BestiaryMain>();

            mod.LoadSettingsCallback = LoadSettings;

            mod.IsReady = true;
        }

        private void Start()
        {
            Debug.Log("Begin mod init: Bestiary");

            mod.LoadSettings();

            Debug.Log("Finished mod init: Bestiary");
        }

        private static KeyCode SetKeyFromText(string text) //"Inspired" by code from Mighty Foot from numidium (https://www.nexusmods.com/daggerfallunity/mods/162)
        {
            KeyCode result;
            if (!System.Enum.TryParse(text, out result))
                result = KeyCode.B;
            
            return result;
        }

        static void LoadSettings(ModSettings modSettings, ModSettingsChange change)
        {
            if(firstSetting)
            {
                string keybindText;
                keybindText = modSettings.GetValue<string>("Controls", "Keybind");
                openMenuKeyCode = SetKeyFromText(keybindText);

                BestiaryUI.animate = modSettings.GetBool("General", "EnableAnimations");
                BestiaryUI.animationUpdateDelay = modSettings.GetValue<int>("General", "DelayBetweenAnimationFrames");
                BestiaryUI.classicMode = modSettings.GetBool("General", "ClassicMode");
                BestiaryUI.defaultRotation = modSettings.GetValue<int>("General", "DefaultMobOrientation");
                BestiaryUI.rotate8 = modSettings.GetBool("General", "EnableEightDirectionRotation");

                firstSetting = false;
            }
            else
            {
                if(change.HasChanged("General"))
                {
                    BestiaryUI.animate = modSettings.GetBool("General", "EnableAnimations");
                    BestiaryUI.animationUpdateDelay = modSettings.GetValue<int>("General", "DelayBetweenAnimationFrames");
                    BestiaryUI.classicMode = modSettings.GetBool("General", "ClassicMode");
                    BestiaryUI.defaultRotation = modSettings.GetValue<int>("General", "DefaultMobOrientation");
                    BestiaryUI.rotate8 = modSettings.GetBool("General", "EnableEightDirectionRotation");
                }
                
                if(change.HasChanged("Controls"))
                {
                    string keybindText;
                    keybindText = modSettings.GetValue<string>("Controls", "Keybind");
                    openMenuKeyCode = SetKeyFromText(keybindText);
                }
            }
        }
    }
}
