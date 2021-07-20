using UnityEngine;
using System.Collections;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility.ModSupport;


namespace BestiaryMod
{
    public class BestiaryMain : MonoBehaviour
    {
        static BestiaryUI bestiaryUIScreen;

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
        }
        public static void DisplayBestiaryUI()
        {
            DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
        }
    }
}