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
        static BestiaryUI bestiaryUIScreen;
        string keybindText = "";
        KeyCode openMenuKeyCode = KeyCode.B;

        
        void Update()
        {
            if (bestiaryUIScreen == null)
            {
                bestiaryUIScreen = new BestiaryUI(DaggerfallWorkshop.Game.DaggerfallUI.UIManager);
            }

            if (InputManager.Instance.GetKeyDown(openMenuKeyCode) && GameManager.Instance.IsPlayerOnHUD)
            {
                DisplayBestiaryUI();
            }
            else if (bestiaryUIScreen.isShowing && InputManager.Instance.GetKeyDown(openMenuKeyCode))
            {
                bestiaryUIScreen.CloseWindow();
            }
            else if (openMenuKeyCode != KeyCode.None && !InputManager.Instance.IsPaused && InputManager.Instance.GetKeyDown(openMenuKeyCode))
            {
                if (bestiaryUIScreen.isShowing)
                    bestiaryUIScreen.CloseWindow();
            }

        }

        [Invoke(StateManager.StateTypes.Game, 0)]
        public static void Init(InitParams initParams)
        {
            GameObject bestiaryGO = new GameObject("bestiary");
            BestiaryMain bestiary = bestiaryGO.AddComponent<BestiaryMain>();

            ModManager.Instance.GetMod(initParams.ModTitle).IsReady = true;
            
        }

        void Awake()
        {
            ModSettings settings = ModManager.Instance.GetMod("Bestiary").GetSettings();

            keybindText = settings.GetValue<string>("Controls", "Keybind");

            SetKeyFromText(keybindText);
        }
        public static void DisplayBestiaryUI()
        {
            DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
        }
        private void SetKeyFromText(string text) //"Inspired" by code from Mighty Foot from numidium (https://www.nexusmods.com/daggerfallunity/mods/162)
        {
            KeyCode result;
            if (System.Enum.TryParse(text, out result))
                openMenuKeyCode = result;
            else
                openMenuKeyCode = KeyCode.B;
        }
    }
}