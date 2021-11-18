using System;
using System.Collections;
using System.Collections.Generic;

using DaggerfallConnect;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

using UnityEngine;

using Wenzil.Console;

namespace BestiaryMod
{
    public class BestiaryMain : MonoBehaviour, IHasModSaveData
    {
        private static Mod mod;
        #region saveDataStuff
        [FullSerializer.fsObject("v1")]
        public class MyModSaveData
        {
            public Dictionary<string, uint> KillCounts;
            public bool UnlockedBestiary;
        }
        public Type SaveDataType { get { return typeof(MyModSaveData); } }
        public static Dictionary<string, uint> killCounts = new Dictionary<string, uint>();
        public static bool UnlockedBestiary { get; set; }
        #endregion
        public static BestiaryMain instance;
        public static BestiaryUI bestiaryUIScreen;
        public static AllTextClass AllText { get; set; }

        #region settingsVars
        public static int SettingMenuUnlock { get; set; }
        public static int SettingEntries { get; set; }
        public static bool SettingSpawnItem { get; set; }
        public static int SettingDefaultRotation { get; set; }
        public static int SettingAnimationUpdateDelay { get; set; }
        public static bool SettingAnimate { get; set; }
        public static bool SettingEnableAllDirectionRotation { get; set; }
        #endregion
        private static PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
        private static KeyCode openMenuKeyCode;
        private static bool readyToOpenUI;

        private static readonly List<string> pagesFull = new List<string> { "page_animals", "page_atronachs", "page_daedra", "page_lycanthropes", "page_monsters1", "page_monsters2", "page_orcs", "page_undead" };
        private static readonly List<string> pagesClassic = new List<string> { "page_classic" };

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;

            var go = new GameObject(mod.Title);
            instance = go.AddComponent<BestiaryMain>();

            readyToOpenUI = false;
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(BestiaryItem.templateIndex, ItemGroups.UselessItems2, typeof(BestiaryItem));

            mod.SaveDataInterface = instance;
            mod.LoadSettingsCallback = LoadSettings;
            StateManager.OnStartNewGame += OnGameStarted;
            StartGameBehaviour.OnStartGame += OnNewGameStarted;
            EnemyDeath.OnEnemyDeath += EnemyDeath_OnEnemyDeath;
            PlayerActivate.OnLootSpawned += AddBestiary_OnLootSpawned;
            EnemyDeath.OnEnemyDeath += BestiaryLoot_OnEnemyDeath;

            mod.IsReady = true;
        }

        private void Start()
        {
            Debug.Log("Begin mod init: Bestiary");

            RegisterBestiaryCommands();
            mod.LoadSettings();

            Debug.Log("Finished mod init: Bestiary");
        }

        void Update()
        {
            if (readyToOpenUI)
            {
                if (InputManager.Instance.GetKeyDown(openMenuKeyCode) && !InputManager.Instance.IsPaused && GameManager.Instance.IsPlayerOnHUD)
                {
                    if (SettingEntries == 1 && killCounts.Count < 1)
                    {
                        DaggerfallWorkshop.Game.DaggerfallUI.AddHUDText("You have no entries to display. Slay Something first, weakling.");
                        return;
                    }
                    if (AllText.Pages.Count < 1)
                        return;

                    switch (SettingMenuUnlock)
                    {
                        case 0:
                            DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
                            break;
                        case 1:
                            if (playerEntity.Items.SearchItems(ItemGroups.UselessItems2, BestiaryItem.templateIndex).Count >= 1)
                                DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
                            else
                                DaggerfallUI.AddHUDText("You do not to have the Bestiary item in your inventory.");
                            break;
                        case 2:
                            if (UnlockedBestiary)
                                DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
                            else
                                DaggerfallUI.AddHUDText("You have not yet unlocked the Bestiary. Find the Bestiary book item and click USE on it.");
                            break;
                    }
                }
                else if (bestiaryUIScreen.isShowing && openMenuKeyCode != KeyCode.None && InputManager.Instance.GetKeyDown(openMenuKeyCode))
                    bestiaryUIScreen.CloseWindow();
            }
        }

        private static void InitializeUI()
        {
            if (SettingEntries == 2)
                AllText = new AllTextClass(pagesClassic);
            else
                AllText = new AllTextClass(pagesFull);
            bestiaryUIScreen = new BestiaryUI(DaggerfallWorkshop.Game.DaggerfallUI.UIManager);

            readyToOpenUI = true;
        }

        //"Inspired" by code from Mighty Foot from numidium (https://www.nexusmods.com/daggerfallunity/mods/162).
        private static KeyCode SetKeyFromText(string text)
        {
            KeyCode result;
            if (!System.Enum.TryParse(text, out result))
                result = KeyCode.B;

            return result;
        }
        static void OnGameStarted(object sender, EventArgs e)
        {
            mod.LoadSettings();
        }
        static void OnNewGameStarted(object sender, EventArgs e)
        {
            mod.LoadSettings();
        }

        static void LoadSettings(ModSettings modSettings, ModSettingsChange change)
        {
            SettingMenuUnlock = modSettings.GetValue<int>("Gameplay", "MenuUnlock");
            SettingEntries = modSettings.GetValue<int>("Gameplay", "Entries");
            SettingSpawnItem = modSettings.GetBool("Gameplay", "ItemSpawning");
            SettingDefaultRotation = modSettings.GetValue<int>("UserInterface", "DefaultMobOrientation");
            SettingAnimationUpdateDelay = modSettings.GetValue<int>("UserInterface", "DelayBetweenAnimationFrames");
            SettingAnimate = modSettings.GetBool("UserInterface", "EnableAnimations");
            SettingEnableAllDirectionRotation = modSettings.GetBool("UserInterface", "EnableEightDirectionRotation");

            openMenuKeyCode = SetKeyFromText(modSettings.GetValue<string>("Controls", "Keybind"));
            InitializeUI();
        }

        //From here: https://github.com/Ralzar81/SkillBooks/blob/cf024383284c12fbf4f27e6611ba2384c96508b9/SkillBooks/SkillBooks.cs.
        static bool HumanoidCheck(int enemyID)
        {
            switch (enemyID)
            {
                case (int)MobileTypes.Orc:
                case (int)MobileTypes.Centaur:
                case (int)MobileTypes.OrcSergeant:
                case (int)MobileTypes.Giant:
                case (int)MobileTypes.OrcShaman:
                case (int)MobileTypes.OrcWarlord:
                    return true;
            }
            return false;
        }

        public static void EnemyDeath_OnEnemyDeath(object sender, EventArgs e)
        {
            EnemyDeath enemyDeath = sender as EnemyDeath;
            if (enemyDeath != null)
            {
                DaggerfallEntityBehaviour entityBehaviour = enemyDeath.GetComponent<DaggerfallEntityBehaviour>();
                if (entityBehaviour != null)
                {
                    EnemyEntity enemyEntity = entityBehaviour.Entity as EnemyEntity;
                    if (enemyEntity != null)
                    {
                        if (entityBehaviour.GetComponent<EnemySenses>().Target == GameManager.Instance.PlayerEntityBehaviour)
                        {
                            string monsterName = AllTextClass.MonsterCareerIndexToString(enemyEntity.CareerIndex);
                            if (killCounts.ContainsKey(monsterName))
                            {
                                killCounts[monsterName] += 1;
                                for (int i = 0; i < AllText.Pages.Count; i++)
                                    AllText.Pages[i].PageSummary = new Summary(AllText.Pages[i].PageSummary.Name);
                            }
                            else
                            {
                                if (SettingEntries == 1 && monsterName != "false")
                                    DaggerfallUI.AddHUDText(String.Format("{0} has been added to the Bestiary.", new List<string>(mod.GetAsset<TextAsset>(monsterName).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))[2]));

                                killCounts.Add(monsterName, 1);
                                InitializeUI();
                            }
                        }
                    }
                }
            }
        }

        //Modified, base from here: https://github.com/Ralzar81/SkillBooks/blob/cf024383284c12fbf4f27e6611ba2384c96508b9/SkillBooks/SkillBooks.cs.
        public static void AddBestiary_OnLootSpawned(object sender, ContainerLootSpawnedEventArgs e)
        {
            if (!SettingSpawnItem)
                return;

            DaggerfallInterior interior = GameManager.Instance.PlayerEnterExit.Interior;
            if (interior != null &&
                e.ContainerType == LootContainerTypes.ShopShelves &&
                interior.BuildingData.BuildingType == DFLocation.BuildingTypes.Bookseller)
            {
                int numBooks = UnityEngine.Random.Range(0, interior.BuildingData.Quality / 5);

                if (UnityEngine.Random.Range(1, 4) > 2)
                {
                    DaggerfallUnityItem bestiaryItem = ItemBuilder.CreateItem(ItemGroups.UselessItems2, BestiaryItem.templateIndex);
                    e.Loot.AddItem(bestiaryItem);
                }
            }
        }

        //Modified, base from here: https://github.com/Ralzar81/SkillBooks/blob/cf024383284c12fbf4f27e6611ba2384c96508b9/SkillBooks/SkillBooks.cs.
        static void BestiaryLoot_OnEnemyDeath(object sender, EventArgs e)
        {
            if (!SettingSpawnItem)
                return;

            EnemyDeath enemyDeath = sender as EnemyDeath;
            if (enemyDeath != null)
            {
                DaggerfallEntityBehaviour entityBehaviour = enemyDeath.GetComponent<DaggerfallEntityBehaviour>();
                if (entityBehaviour != null)
                {
                    EnemyEntity enemyEntity = entityBehaviour.Entity as EnemyEntity;
                    if (enemyEntity != null)
                    {
                        if (enemyEntity.MobileEnemy.Affinity == MobileAffinity.Human || HumanoidCheck(enemyEntity.MobileEnemy.ID))
                        {
                            int luckRoll = UnityEngine.Random.Range(1, 20) + ((playerEntity.Stats.LiveLuck / 10) - 5);
                            if (luckRoll > 18)
                            {
                                DaggerfallUnityItem bestiaryItem = ItemBuilder.CreateItem(ItemGroups.UselessItems2, BestiaryItem.templateIndex);
                                entityBehaviour.CorpseLootContainer.Items.AddItem(bestiaryItem);
                            }
                        }
                    }
                }
            }
        }

        public static void RegisterBestiaryCommands()
        {
            Debug.Log("[Bestiary] Trying to register console commands.");
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(AddBestiaryItem.command, AddBestiaryItem.description, AddBestiaryItem.usage, AddBestiaryItem.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error Registering Bestiary Console commands: {0}", e.Message));
            }
        }

        private static class AddBestiaryItem
        {
            public static readonly string command = "add_bestiaryitem";
            public static readonly string description = "Put the Bestiary (item) in players inventory";
            public static readonly string usage = "add_bestiaryitem";

            public static string Execute(params string[] args)
            {
                if (args.Length > 0)
                    return usage;

                int index = 900;
                DaggerfallUnityItem bestiaryItem = ItemBuilder.CreateItem(ItemGroups.UselessItems2, index);
                GameManager.Instance.PlayerEntity.Items.AddItem(bestiaryItem);
                return "BestiaryItem added";
            }
        }
        public static void DisplayMessage(string message)
        {
            DaggerfallMessageBox daggerfallMessageBox = new DaggerfallMessageBox(DaggerfallWorkshop.Game.DaggerfallUI.UIManager);
            daggerfallMessageBox.AllowCancel = true;
            daggerfallMessageBox.ClickAnywhereToClose = true;
            daggerfallMessageBox.ParentPanel.BackgroundColor = Color.clear;

            daggerfallMessageBox.SetText(message);
            DaggerfallUI.UIManager.PushWindow(daggerfallMessageBox);
        }

        public object NewSaveData()
        {
            return new MyModSaveData
            {
                KillCounts = new Dictionary<string, uint>(),
                UnlockedBestiary = false
            };
        }
        public object GetSaveData()
        {
            return new MyModSaveData
            {
                KillCounts = killCounts,
                UnlockedBestiary = UnlockedBestiary
            };
        }
        public void RestoreSaveData(object saveData)
        {
            var myModSaveData = (MyModSaveData)saveData;

            killCounts = myModSaveData.KillCounts;
            UnlockedBestiary = myModSaveData.UnlockedBestiary;
        }
    }
}
