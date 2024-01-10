using System;
using System.Collections.Generic;

using DaggerfallConnect;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

using UnityEngine;

using Wenzil.Console;

// ReSharper disable CheckNamespace
namespace BestiaryMod
{
    public class BestiaryMain : MonoBehaviour, IHasModSaveData
    {
        #region Mod Messages
        public const string OVERRIDE_TITLE = "OVERRIDE_TITLE";
        public const string OVERRIDE_SUMMARY = "OVERRIDE_SUMMARY";
        public const string OVERRIDE_ADVICE = "OVERRIDE_ADVICE";
        public const string OVERRIDE_MATERIAL = "OVERRIDE_MATERIAL";
        public const string OVERRIDE_LANGUAGE = "OVERRIDE_LANGUAGE";
        public const string OVERRIDE_ABILITIES = "OVERRIDE_ABILITIES";
        public const string OVERRIDE_SPELLS_BY_NAME = "OVERRIDE_SPELLS_BY_NAME";
        public const string OVERRIDE_SPELLS_BY_IDS = "OVERRIDE_SPELLS_BY_IDS";
        #endregion


        private static Mod mod;
        #region saveDataStuff
        [FullSerializer.fsObject("v1")]
        public class MyModSaveData
        {
            public Dictionary<string, uint> KillCounts;
            public bool UnlockedBestiary;
            public bool ConvertedBestiarySaveData;
        }
        public Type SaveDataType { get { return typeof(MyModSaveData); } }
        public static Dictionary<string, uint> killCounts = new Dictionary<string, uint>();
        public static bool UnlockedBestiary { get; set; }
        #endregion
        public static BestiaryMain Instance;
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
            Instance = go.AddComponent<BestiaryMain>();

            readyToOpenUI = false;
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(BestiaryItem.templateIndex, ItemGroups.UselessItems2, typeof(BestiaryItem));

            mod.SaveDataInterface = Instance;
            mod.LoadSettingsCallback = LoadSettings;
            StateManager.OnStartNewGame += OnGameStarted;
            StartGameBehaviour.OnStartGame += OnNewGameStarted;
            PlayerActivate.OnLootSpawned += AddBestiary_OnLootSpawned;
            EnemyDeath.OnEnemyDeath += EnemyDeath_OnEnemyDeath;
            EnemyDeath.OnEnemyDeath += BestiaryLoot_OnEnemyDeath;
        }

        void Awake()
        {
            mod.MessageReceiver = MessageReceiver;
            mod.IsReady = true;
        }

        private void MessageReceiver(string message, object data, DFModMessageCallback callBack)
        {
            try
            {
                object[] paramArray;
                int MonsterCareer;
                switch (message)
                {
                    // Allow to set monster page Title
                    // SendModMessage("OVERRIDE_TITLE", [10, "New Title"]);
                    case OVERRIDE_TITLE:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        string newTitle = (string)paramArray[1];

                        BestiaryTextDB.OverrideTitleTable.Add(MonsterCareer, newTitle);

                        callBack?.Invoke(OVERRIDE_TITLE, true);
                        break;

                    // Allow to set monster Summary
                    // SendModMessage("OVERRIDE_SUMMARY", [10, "New summary text"]);
                    case OVERRIDE_SUMMARY:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        string newSummary = (string)paramArray[1];

                        BestiaryTextDB.OverrideSummaryTable.Add(MonsterCareer, newSummary);

                        callBack?.Invoke(OVERRIDE_SUMMARY, true);
                        break;

                    // Allow to set monster Advice text
                    // SendModMessage("OVERRIDE_ADVICE", [10, "Always hit first!"]);
                    case OVERRIDE_ADVICE:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        string newAdvice = (string)paramArray[1];

                        BestiaryTextDB.OverrideAdviceTable.Add(MonsterCareer, newAdvice);

                        callBack?.Invoke(OVERRIDE_ADVICE, true);
                        break;

                    // Allow to set monster Material requirements text
                    // SendModMessage("OVERRIDE_MATERIAL", [10, "Use only gold weapon"]);
                    case OVERRIDE_MATERIAL:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        string newMaterial = (string)paramArray[1];

                        BestiaryTextDB.OverrideMaterialTable.Add(MonsterCareer, newMaterial);

                        callBack?.Invoke(OVERRIDE_MATERIAL, true);
                        break;

                    // Allow to set monster Language
                    // SendModMessage("OVERRIDE_LANGUAGE", [10, "Sentinel dialect of Orcish"]);
                    case OVERRIDE_LANGUAGE:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        string newLanguage = (string)paramArray[1];

                        BestiaryTextDB.OverrideLanguageTable.Add(MonsterCareer, newLanguage);

                        callBack?.Invoke(OVERRIDE_LANGUAGE, true);
                        break;

                    // Allow to set monster Abilities list
                    // SendModMessage("OVERRIDE_ABILITIES", [10, ["Can swim", "Can fly", "Can run"]]);
                    case OVERRIDE_ABILITIES:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        string[] newAbilities = (string[])paramArray[1];

                        BestiaryTextDB.OverrideAbilitiesTable.Add(MonsterCareer, newAbilities);

                        callBack?.Invoke(OVERRIDE_ABILITIES, true);
                        break;

                    // Allow to set monster Spells using direct names
                    // If spells are overriden by names, then the IDs are ignored
                    // SendModMessage("OVERRIDE_SPELLS_BY_NAME", [10, ["Zipper", "Balina's poison", "Frost fist"]]);
                    case OVERRIDE_SPELLS_BY_NAME:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        string[] newSpellsNames = (string[])paramArray[1];

                        BestiaryTextDB.OverrideSpellsTable.Add(MonsterCareer, newSpellsNames);

                        callBack?.Invoke(OVERRIDE_SPELLS_BY_NAME, true);
                        break;

                    // Allow to set monster Spells using standart IDs
                    // SendModMessage("OVERRIDE_SPELLS_BY_IDS", [10, [11, 23, 13, 8]]);
                    case OVERRIDE_SPELLS_BY_IDS:
                        paramArray = (object[])data;
                        MonsterCareer = (int)paramArray[0];
                        int[] newSpellsIds = (int[])paramArray[1];

                        BestiaryTextDB.OverrideSpellsIdsTable.Add(MonsterCareer, newSpellsIds);

                        callBack?.Invoke(OVERRIDE_SPELLS_BY_IDS, true);
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                callBack?.Invoke("error", "Data passed is invalid for " + message);
            }
        }

        private void Start()
        {
            Debug.Log("Begin mod init: Bestiary");

            RegisterBestiaryCommands();
            mod.LoadSettings();

            Debug.Log("Finished mod init: Bestiary");
        }

        private void Update()
        {
            if (readyToOpenUI)
            {
                if (InputManager.Instance.GetKeyDown(openMenuKeyCode) && !InputManager.Instance.IsPaused && GameManager.Instance.IsPlayerOnHUD)
                {
                    if (SettingEntries == 1 && killCounts.Count < 1)
                    {

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
                                DaggerfallUI.AddHUDText(BestiaryTextDB.YouDontHaveBestiary);
                            break;
                        case 2:
                            if (UnlockedBestiary)
                                DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
                            else
                                DaggerfallUI.AddHUDText(BestiaryTextDB.UnlockBestiaryToUseIt);
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
            bestiaryUIScreen = new BestiaryUI(DaggerfallUI.UIManager);

            readyToOpenUI = true;
        }

        //"Inspired" by code from Mighty Foot from numidium (https://www.nexusmods.com/daggerfallunity/mods/162).
        private static KeyCode SetKeyFromText(string text)
        {
            KeyCode result;
            if (!Enum.TryParse(text, out result))
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
                case (int)MobileTypes.Centaur:
                case (int)MobileTypes.Giant:
                case (int)MobileTypes.Orc:
                case (int)MobileTypes.OrcSergeant:
                case (int)MobileTypes.OrcShaman:
                case (int)MobileTypes.OrcWarlord:
                case (int)MobileTypes.Vampire:
                case (int)MobileTypes.VampireAncient:
                    return true;
            }
            return false;
        }

        public static void EnemyDeath_OnEnemyDeath(object sender, EventArgs e)
        {
            EnemyDeath enemyDeath = (EnemyDeath)sender;

            DaggerfallEntityBehaviour entityBehaviour;

            if (!enemyDeath.TryGetComponent<DaggerfallEntityBehaviour>(out entityBehaviour))
                return;

            EnemyEntity enemyEntity = (EnemyEntity)entityBehaviour.Entity;

            if (enemyEntity == null)
                return;

            // Here we go. We don't need to count Humans
            if (enemyEntity.MobileEnemy.Affinity == MobileAffinity.Human)
                return;

            if (entityBehaviour.GetComponent<EnemySenses>().Target == GameManager.Instance.PlayerEntityBehaviour)
            {
                string mName = TextManager.Instance.GetLocalizedEnemyName(enemyEntity.CareerIndex);

                if (killCounts.ContainsKey(mName))
                {
                    killCounts[mName] += 1;
                }
                else
                {
                    if (SettingEntries == 1)
                        DaggerfallUI.AddHUDText(string.Format(BestiaryTextDB.AddedToTheBestiary, mName));

                    killCounts.Add(mName, 1);
                }
                InitializeUI();
            }
        }

        //Modified, base from here: https://github.com/Ralzar81/SkillBooks/blob/cf024383284c12fbf4f27e6611ba2384c96508b9/SkillBooks/SkillBooks.cs.
        public static void AddBestiary_OnLootSpawned(object sender, ContainerLootSpawnedEventArgs e)
        {
            if (!SettingSpawnItem)
                return;

            DaggerfallInterior interior = GameManager.Instance.PlayerEnterExit.Interior;

            if (interior == null) return;

            DFLocation.BuildingData buildingData = interior.BuildingData;

            if (e.ContainerType == LootContainerTypes.ShopShelves && buildingData.BuildingType == DFLocation.BuildingTypes.Bookseller)
            {
                if (GetLuckRoll())
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

            EnemyDeath enemyDeath = (EnemyDeath)sender;

            DaggerfallEntityBehaviour entityBehaviour;

            if (!enemyDeath.TryGetComponent<DaggerfallEntityBehaviour>(out entityBehaviour))
                return;

            EnemyEntity enemyEntity = entityBehaviour.Entity as EnemyEntity;

            if (enemyEntity == null)
                return;

            if (enemyEntity.MobileEnemy.Affinity == MobileAffinity.Human || HumanoidCheck(enemyEntity.MobileEnemy.ID))
            {
                if (GetLuckRoll())
                {
                    DaggerfallUnityItem bestiaryItem = ItemBuilder.CreateItem(ItemGroups.UselessItems2, BestiaryItem.templateIndex);
                    entityBehaviour.CorpseLootContainer.Items.AddItem(bestiaryItem);
                }
            }
        }

        public static void RegisterBestiaryCommands()
        {
            Debug.Log("[Bestiary] Trying to register console commands.");
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(BestiaryConsoleCommand.Command, BestiaryConsoleCommand.Description, BestiaryConsoleCommand.Usage, BestiaryConsoleCommand.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error Registering Bestiary Console commands: {e.Message}");
            }
        }

        public static void DisplayMessage(string message)
        {
            DaggerfallMessageBox daggerfallMessageBox = new DaggerfallMessageBox(DaggerfallUI.UIManager);
            daggerfallMessageBox.AllowCancel = true;
            daggerfallMessageBox.ClickAnywhereToClose = true;
            daggerfallMessageBox.ParentPanel.BackgroundColor = Color.clear;

            daggerfallMessageBox.SetText(message);
            DaggerfallUI.UIManager.PushWindow(daggerfallMessageBox);
        }

        private static bool GetLuckRoll()
        {
            int luckRollModifier = UnityEngine.Random.Range(0, playerEntity.Stats.LiveLuck);
            int baseRollChance = UnityEngine.Random.Range(0, 100);
            int luckRoll = UnityEngine.Random.Range(0, baseRollChance + luckRollModifier);

            float dropChance = ((float)luckRoll / ((float)playerEntity.Stats.LiveLuck + 100f));

            // Approx. 1 in 120 enemies
            return dropChance > 0.8;
        }

        public object NewSaveData()
        {
            return new MyModSaveData
            {
                KillCounts = new Dictionary<string, uint>(),
                UnlockedBestiary = false,
                ConvertedBestiarySaveData = true,
            };
        }

        public object GetSaveData()
        {
            return new MyModSaveData
            {
                ConvertedBestiarySaveData = true,
                KillCounts = killCounts,
                UnlockedBestiary = UnlockedBestiary
            };
        }

        public void RestoreSaveData(object saveData)
        {
            var myModSaveData = (MyModSaveData)saveData;

            if (!myModSaveData.ConvertedBestiarySaveData)
            {
                try
                {
                    var newKillCounts = new Dictionary<string, uint>();

                    foreach (var KillCount in myModSaveData.KillCounts)
                    {
                        newKillCounts.Add(BestiarySaveConvert.Convert(KillCount.Key), KillCount.Value);
                    }

                    myModSaveData.KillCounts = newKillCounts;
                }
                catch
                { }
            }

            killCounts = myModSaveData.KillCounts;
            UnlockedBestiary = myModSaveData.UnlockedBestiary;
        }

        public Mod GetMod()
        {
            return mod;
        }
    }

    static class BestiarySaveConvert
    {
        public static string Convert(string old)
        {
            switch (old)
            {
                case "entry_ancient_lich":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.AncientLich);
                case "entry_centaur":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Centaur);
                case "entry_daedra_lord":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.DaedraLord);
                case "entry_daedra_seducer":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.DaedraSeducer);
                case "entry_daedroth":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Daedroth);
                case "entry_dragonling":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Dragonling);
                case "entry_dreugh":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Dreugh);
                case "entry_fire_atronach":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.FireAtronach);
                case "entry_fire_daedra":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.FireDaedra);
                case "entry_flesh_atronach":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.FleshAtronach);
                case "entry_gargoyle":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Gargoyle);
                case "entry_ghost":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Ghost);
                case "entry_giant":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Giant);
                case "entry_giant_bat":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.GiantBat);
                case "entry_grizzly_bear":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.GrizzlyBear);
                case "entry_harpy":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Harpy);
                case "entry_ice_atronach":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.IceAtronach);
                case "entry_ice_daedra":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.FrostDaedra);
                case "entry_imp":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Imp);
                case "entry_iron_atronach":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.IronAtronach);
                case "entry_lamia":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Lamia);
                case "entry_lich":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Lich);
                case "entry_mummy":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Mummy);
                case "entry_nymph":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Nymph);
                case "entry_orc":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Orc);
                case "entry_orc_sergeant":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.OrcSergeant);
                case "entry_orc_shaman":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.OrcShaman);
                case "entry_orc_warlord":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.OrcWarlord);
                case "entry_rat":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Rat);
                case "entry_sabertooth_tiger":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.SabertoothTiger);
                case "entry_scorpion":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.GiantScorpion);
                case "entry_skeletal_warrior":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.SkeletalWarrior);
                case "entry_slaughterfish":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Slaughterfish);
                case "entry_spider":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Spider);
                case "entry_spriggan":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Spriggan);
                case "entry_vampire":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Vampire);
                case "entry_vampire_ancient":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.VampireAncient);
                case "entry_wereboar":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Wereboar);
                case "entry_werewolf":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Werewolf);
                case "entry_wraith":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Wraith);
                case "entry_zombie":
                    return TextManager.Instance.GetLocalizedEnemyName((int)MonsterCareers.Zombie);
                default: return string.Empty;
            }
        }
    }
}
