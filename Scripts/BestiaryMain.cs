using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string REGISTER_CUSTOM_ENTITY = "REGISTER_CUSTOM_ENTITY";
        public const string REGISTER_CUSTOM_PAGE = "REGISTER_CUSTOM_PAGE";
        public const string ADD_ENTITY_TO_EXISTING_PAGE = "ADD_ENTITY_TO_EXISTING_PAGE";
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
        public static int SettingItemSpawningExtraChance { get; set; }
        public static int SettingDefaultRotation { get; set; }
        public static int SettingAnimationUpdateDelay { get; set; }
        public static bool SettingAnimate { get; set; }
        public static bool SettingEnableAllDirectionRotation { get; set; }
        public static Color32 SettingFontColor { get; set; }
        public static Color32 SettingFontShadowColor { get; set; }
        public static Color32 SettingHeaderFontColor { get; set; }
        public static Color32 SettingHeaderFontShadowColor { get; set; }
        #endregion
        private static PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
        private static KeyCode openMenuKeyCode;
        private static bool readyToOpenUI;

        Dictionary<string, string> baseText = null;
        Dictionary<string, string> localization = null;

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
            LoadTextData();

            mod.MessageReceiver = MessageReceiver;
            mod.IsReady = true;
        }

        private void LoadTextData()
        {
            const string csvFilename = "BestiaryModData.csv";

            if (baseText == null)
                baseText = BestiaryModCSVParser.LoadDictionary(csvFilename);

            if (localization == null)
                localization = BestiaryModCSVParser.LoadDictionary(csvFilename, true);

            return;
        }

        private void MessageReceiver(string message, object data, DFModMessageCallback callBack)
        {
            try
            {
                object[] paramArray;
                string MonsterCareer;
                switch (message)
                {
                    // Allow to set monster page Title
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_TITLE", new object[] { "Rat", "New Title" });
                    case OVERRIDE_TITLE:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        string newTitle = (string)paramArray[1];

                        BestiaryTextDB.OverrideTitleTable.Add(MonsterCareer, newTitle);

                        callBack?.Invoke(OVERRIDE_TITLE, true);
                        break;

                    // Allow to set monster Summary
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_SUMMARY", new object[] { "Rat", "New summary text" });
                    case OVERRIDE_SUMMARY:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        string newSummary = (string)paramArray[1];

                        BestiaryTextDB.OverrideSummaryTable.Add(MonsterCareer, newSummary);

                        callBack?.Invoke(OVERRIDE_SUMMARY, true);
                        break;

                    // Allow to set monster Advice text
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_ADVICE", new object[] { "Rat", "Always hit first!" });
                    case OVERRIDE_ADVICE:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        string newAdvice = (string)paramArray[1];

                        BestiaryTextDB.OverrideAdviceTable.Add(MonsterCareer, newAdvice);

                        callBack?.Invoke(OVERRIDE_ADVICE, true);
                        break;

                    // Allow to set monster Material requirements text
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_MATERIAL", new object[] { "Rat", "Use only gold weapon" });
                    case OVERRIDE_MATERIAL:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        string newMaterial = (string)paramArray[1];

                        BestiaryTextDB.OverrideMaterialTable.Add(MonsterCareer, newMaterial);

                        callBack?.Invoke(OVERRIDE_MATERIAL, true);
                        break;

                    // Allow to set monster Language
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_LANGUAGE", new object[] { "Rat", "Sentinel dialect of Orcish" });
                    case OVERRIDE_LANGUAGE:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        string newLanguage = (string)paramArray[1];

                        BestiaryTextDB.OverrideLanguageTable.Add(MonsterCareer, newLanguage);

                        callBack?.Invoke(OVERRIDE_LANGUAGE, true);
                        break;

                    // Allow to set monster Abilities list
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_ABILITIES", new object[] { "Rat", new string[] { "Can swim", "Can fly", "Can run" } });
                    case OVERRIDE_ABILITIES:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        string[] newAbilities = (string[])paramArray[1];

                        BestiaryTextDB.OverrideAbilitiesTable.Add(MonsterCareer, newAbilities);

                        callBack?.Invoke(OVERRIDE_ABILITIES, true);
                        break;

                    // Allow to set monster Spells using direct names
                    // If spells are overriden by names, then the IDs are ignored
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_SPELLS_BY_NAME", new object[] { "Rat", new string[] { "Zipper", "Balina's poison", "Frost fist" } });
                    case OVERRIDE_SPELLS_BY_NAME:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        string[] newSpellsNames = (string[])paramArray[1];

                        BestiaryTextDB.OverrideSpellsTable.Add(MonsterCareer, newSpellsNames);

                        callBack?.Invoke(OVERRIDE_SPELLS_BY_NAME, true);
                        break;

                    // Allow to set monster Spells using standart IDs
                    // ModManager.Instance.SendModMessage("Bestiary", "OVERRIDE_SPELLS_BY_IDS", new object[] { "Rat", new int[] { 11, 23, 13, 8 } });
                    case OVERRIDE_SPELLS_BY_IDS:
                        paramArray = (object[])data;
                        MonsterCareer = (string)paramArray[0];
                        int[] newSpellsIds = (int[])paramArray[1];

                        BestiaryTextDB.OverrideSpellsIdsTable.Add(MonsterCareer, newSpellsIds);

                        callBack?.Invoke(OVERRIDE_SPELLS_BY_IDS, true);
                        break;

                    // Register new monster records
                    // The data will be attempted to read from MonsterName.csv
                    // Your mod have to provide these files
                    // Like: Cockroach.csv, Walrus.csv, Raccoon.csv
                    // See ExampleMonster.csv for details
                    // ModManager.Instance.SendModMessage("Bestiary", "REGISTER_CUSTOM_ENTITY", new object[] { "Cockroach", "Walrus", "Raccoon" });
                    case REGISTER_CUSTOM_ENTITY:
                        paramArray = (object[])data;

                        for (int i = 0; i < paramArray.Length; i++)
                        {
                            string monsterName = (string)paramArray[i];
                            if (BestiaryTextDB.CustomEntries.ContainsKey(monsterName)) { continue; }

                            var monsterData = BestiaryModCSVParser.LoadDictionary($"{monsterName}.csv");

                            if (monsterData == null) continue;

                            var monsterEntry = new Entry(monsterName)
                            {
                                Title = monsterData["Title"],
                                ButtonTextureName = monsterData["ButtonTextureName"],
                                Summary = monsterData["Summary"],
                                Advice = monsterData["Advice"],
                                Material = monsterData["Material"],
                                Language = monsterData["Language"],
                                Abilities = monsterData["Abilities"].Split('\n'),
                                NamedSpells = monsterData["NamedSpells"].Split('\n'),
                            };
                            int TextureArchive;
                            if (int.TryParse(monsterData["TextureArchive"], out TextureArchive))
                            {
                                monsterEntry.TextureArchive = TextureArchive;
                            }

                            BestiaryTextDB.CustomEntries.Add(monsterName, monsterEntry);

                            callBack?.Invoke(REGISTER_CUSTOM_ENTITY, true);
                        }
                        break;

                    // Add one more record to existing page, up to 9 per page
                    // ModManager.Instance.SendModMessage("Bestiary", "ADD_ENTITY_TO_EXISTING_PAGE", new object[] { "PAGE_ID", "CUSTOM_ENTITY_NAME" });
                    case ADD_ENTITY_TO_EXISTING_PAGE:
                        paramArray = (object[])data;

                        string pageid = (string)paramArray[0];
                        string entryid = (string)paramArray[1];

                        var page = BestiaryTextDB.GetPage(pageid);
                        if (page == null)
                        {
                            callBack?.Invoke("error", "PAGE_ID does not exist: " + message);
                            break;
                        }

                        if (BestiaryTextDB.CustomEntries.ContainsKey(entryid))
                        {
                            if (page.Entries.Count == 9)
                            {
                                callBack?.Invoke("error", "Page with this PAGE_ID is full: " + message);
                            }
                            else
                            {
                                if (BestiaryTextDB.ExtraForExistingPages.ContainsKey(pageid))
                                {
                                    BestiaryTextDB.ExtraForExistingPages[pageid].Add(BestiaryTextDB.CustomEntries[entryid]);
                                }
                                else
                                {
                                    BestiaryTextDB.ExtraForExistingPages.Add(pageid, new List<Entry> { BestiaryTextDB.CustomEntries[entryid] });
                                }
                                callBack?.Invoke(ADD_ENTITY_TO_EXISTING_PAGE, true);
                            }
                        }
                        else
                        {
                            callBack?.Invoke("error", "CUSTOM_ENTITY does not exist: " + message);
                        }

                        break;

                    // Create and register new page with up to 9 records
                    // ModManager.Instance.SendModMessage("Bestiary", "REGISTER_CUSTOM_PAGE", new object[] { "Page Title", "Page summary", new string[] { "Cockroach", "Walrus", "Raccoon" } });
                    case REGISTER_CUSTOM_PAGE:
                        paramArray = (object[])data;
                        string newPageTitle = (string)paramArray[0];
                        string newPageSummary = (string)paramArray[1];
                        string[] newPageEntryIds = (string[])paramArray[2];

                        var newPageEntries = newPageEntryIds
                        .Where(id => BestiaryTextDB.CustomEntries.ContainsKey(id))
                        .Take(9)
                        .Select(id => BestiaryTextDB.CustomEntries[id]);

                        BestiaryTextDB.ExtraPagesSummary.Add(newPageTitle, newPageSummary);
                        BestiaryTextDB.ExtraPagesEntries.Add(newPageTitle, newPageEntries);

                        callBack?.Invoke(REGISTER_CUSTOM_PAGE, true);

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
            SettingItemSpawningExtraChance = modSettings.GetInt("Gameplay", "ItemSpawningExtraChance");
            SettingDefaultRotation = modSettings.GetValue<int>("UserInterface", "DefaultMobOrientation");
            SettingAnimationUpdateDelay = modSettings.GetValue<int>("UserInterface", "DelayBetweenAnimationFrames");
            SettingAnimate = modSettings.GetBool("UserInterface", "EnableAnimations");
            SettingEnableAllDirectionRotation = modSettings.GetBool("UserInterface", "EnableEightDirectionRotation");
            SettingFontShadowColor = modSettings.GetColor("UserInterface", "FontShadowColor");
            SettingHeaderFontShadowColor = modSettings.GetColor("UserInterface", "HeaderFontShadowColor");
            SettingFontColor = modSettings.GetColor("UserInterface", "FontColor");
            SettingHeaderFontColor = modSettings.GetColor("UserInterface", "HeaderFontColor");

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
            // Base roll 0 to 10
            int baseRollChance = UnityEngine.Random.Range(0, 10);
            // Added chance 1 to 50
            int addedFromSettings = (int)Math.Ceiling(SettingItemSpawningExtraChance / 2f);
            // Player luck 1 to 10
            int luckRollModifier = (int)Math.Ceiling(UnityEngine.Random.Range(0, playerEntity.Stats.LiveLuck) / 10f);

            // from 2:100 to 70:100 chance of success
            return Dice100.SuccessRoll(baseRollChance + addedFromSettings + luckRollModifier);
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
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }

            killCounts = myModSaveData.KillCounts;
            UnlockedBestiary = myModSaveData.UnlockedBestiary;
        }

        public string Localize(string Key)
        {
            if (localization.ContainsKey(Key))
                return localization[Key];

            if (baseText.ContainsKey(Key))
                return baseText[Key];

            return string.Empty;
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
