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
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

using UnityEngine;

namespace BestiaryMod
{
    public class BestiaryMain : MonoBehaviour, IHasModSaveData
    {
        [FullSerializer.fsObject("v1")]
        public class MyModSaveData
        {
            public Dictionary<string, uint> KillCounts;
            public bool UnlockedBestiary;
        }
        public Type SaveDataType { get { return typeof(MyModSaveData); } }

        private static Mod mod;
        public static BestiaryMain instance;
        static BestiaryUI bestiaryUIScreen;

        static PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
        static KeyCode openMenuKeyCode;

        public static int menuUnlock;

        public static bool unlockedBestiary;
        public static Dictionary<string, uint> killCounts = new Dictionary<string, uint>();

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;

            var go = new GameObject(mod.Title);
            instance = go.AddComponent<BestiaryMain>();

            mod.SaveDataInterface = instance;
            mod.LoadSettingsCallback = LoadSettings;

            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(BestiaryItem.templateIndex, ItemGroups.UselessItems2, typeof(BestiaryItem));

            EnemyDeath.OnEnemyDeath += EnemyDeath_OnEnemyDeath;
            PlayerActivate.OnLootSpawned += AddBestiary_OnLootSpawned;
            EnemyDeath.OnEnemyDeath += BestiaryLoot_OnEnemyDeath;

            mod.IsReady = true;
        }

        private void Start()
        {
            Debug.Log("Begin mod init: Bestiary");

            bestiaryUIScreen = new BestiaryUI(DaggerfallWorkshop.Game.DaggerfallUI.UIManager);
            mod.LoadSettings();

            Debug.Log("Finished mod init: Bestiary");
        }

        void Update()
        {
            if (InputManager.Instance.GetKeyDown(openMenuKeyCode) && GameManager.Instance.IsPlayerOnHUD)
            {
                switch (menuUnlock)
                {
                    case 0:
                        DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
                        break;
                    case 1:
                        if (unlockedBestiary)
                            DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
                        else
                            DaggerfallUI.AddHUDText("You have not yet unlocked the Bestiary. Find the Bestiary book item and click USE on it.");
                        break;
                    case 2:
                        if (killCounts.Count > 0)
                            DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
                        else
                            DaggerfallWorkshop.Game.DaggerfallUI.AddHUDText("You have no entries to display. Slay Something first, weakling.");
                        break;
                }

            }
            else if (bestiaryUIScreen.isShowing && InputManager.Instance.GetKeyDown(openMenuKeyCode))
                bestiaryUIScreen.CloseWindow();
            else if (openMenuKeyCode != KeyCode.None && !InputManager.Instance.IsPaused && InputManager.Instance.GetKeyDown(openMenuKeyCode))
                if (bestiaryUIScreen.isShowing)
                    bestiaryUIScreen.CloseWindow();
        }

        //"Inspired" by code from Mighty Foot from numidium (https://www.nexusmods.com/daggerfallunity/mods/162).
        private static KeyCode SetKeyFromText(string text)
        {
            KeyCode result;
            if (!System.Enum.TryParse(text, out result))
                result = KeyCode.B;

            return result;
        }

        static void LoadSettings(ModSettings modSettings, ModSettingsChange change)
        {
            DaggerfallUnityItem skillBook = ItemBuilder.CreateItem(ItemGroups.Books, BestiaryItem.templateIndex);
            GameManager.Instance.PlayerEntity.Items.AddItem(skillBook);

            BestiaryUI.classicMode = modSettings.GetBool("General", "ClassicMode");
            menuUnlock = modSettings.GetValue<int>("General", "MenuUnlock");
            BestiaryUI.animate = modSettings.GetBool("General", "EnableAnimations");
            BestiaryUI.animationUpdateDelay = modSettings.GetValue<int>("General", "DelayBetweenAnimationFrames");
            BestiaryUI.defaultRotation = modSettings.GetValue<int>("General", "DefaultMobOrientation");
            BestiaryUI.rotate8 = modSettings.GetBool("General", "EnableEightDirectionRotation");

            openMenuKeyCode = SetKeyFromText(modSettings.GetValue<string>("Controls", "Keybind"));

            bestiaryUIScreen = new BestiaryUI(DaggerfallWorkshop.Game.DaggerfallUI.UIManager);
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
                    if (enemyEntity != null && enemyEntity.GetEnemyGroup() != DFCareer.EnemyGroups.Humanoid)
                    {
                        if (entityBehaviour.GetComponent<EnemySenses>().Target == GameManager.Instance.PlayerEntityBehaviour)
                        {
                            string monsterName = MonsterCareerIndexToString(enemyEntity.CareerIndex);

                            if (killCounts.ContainsKey(monsterName))
                                killCounts[monsterName] += 1;
                            else
                            {
                                killCounts.Add(monsterName, 1);
                                if (menuUnlock == 2)
                                {
                                    DaggerfallUI.AddHUDText(String.Format("{0} has been added to the Bestiary.", new List<string>(mod.GetAsset<TextAsset>(monsterName).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))[3]));
                                    bestiaryUIScreen = new BestiaryUI(DaggerfallWorkshop.Game.DaggerfallUI.UIManager);
                                }
                            }
                        }
                    }
                }
            }
        }

        //Modified, base from here: https://github.com/Ralzar81/SkillBooks/blob/cf024383284c12fbf4f27e6611ba2384c96508b9/SkillBooks/SkillBooks.cs.
        public static void AddBestiary_OnLootSpawned(object sender, ContainerLootSpawnedEventArgs e)
        {
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

        public static string MonsterCareerIndexToString(int index)
        {
            switch (index)
            {
                case (int)MonsterCareers.AncientLich:
                    return "entry_ancient_lich";
                case (int)MonsterCareers.Centaur:
                    return "entry_centaur";
                case (int)MonsterCareers.DaedraLord:
                    return "entry_daedra_lord";
                case (int)MonsterCareers.DaedraSeducer:
                    return "entry_daedra_seducer";
                case (int)MonsterCareers.Daedroth:
                    return "entry_daedroth";
                case (int)MonsterCareers.Dragonling:
                    return "entry_dragonling";
                case (int)MonsterCareers.Dragonling_Alternate:
                    return "entry_dragonling";
                case (int)MonsterCareers.Dreugh:
                    return "entry_dreugh";
                case (int)MonsterCareers.FireAtronach:
                    return "entry_fire_atronach";
                case (int)MonsterCareers.FireDaedra:
                    return "entry_fire_daedra";
                case (int)MonsterCareers.FleshAtronach:
                    return "entry_flesh_atronach";
                case (int)MonsterCareers.FrostDaedra:
                    return "entry_ice_daedra";
                case (int)MonsterCareers.Gargoyle:
                    return "entry_gargoyle";
                case (int)MonsterCareers.Ghost:
                    return "entry_ghost";
                case (int)MonsterCareers.Giant:
                    return "entry_giant";
                case (int)MonsterCareers.GiantBat:
                    return "entry_giant_bat";
                case (int)MonsterCareers.GiantScorpion:
                    return "entry_scorpion";
                case (int)MonsterCareers.GrizzlyBear:
                    return "entry_grizzly_bear";
                case (int)MonsterCareers.Harpy:
                    return "entry_harpy";
                case (int)MonsterCareers.IceAtronach:
                    return "entry_ice_atronach";
                case (int)MonsterCareers.Imp:
                    return "entry_imp";
                case (int)MonsterCareers.IronAtronach:
                    return "entry_iron_atronach";
                case (int)MonsterCareers.Lamia:
                    return "entry_lamia";
                case (int)MonsterCareers.Lich:
                    return "entry_lich";
                case (int)MonsterCareers.Mummy:
                    return "entry_mummy";
                case (int)MonsterCareers.Nymph:
                    return "entry_nymph";
                case (int)MonsterCareers.Orc:
                    return "entry_orc";
                case (int)MonsterCareers.OrcSergeant:
                    return "entry_orc_sergeant";
                case (int)MonsterCareers.OrcShaman:
                    return "entry_orc_shaman";
                case (int)MonsterCareers.OrcWarlord:
                    return "entry_orc_warlord";
                case (int)MonsterCareers.Rat:
                    return "entry_rat";
                case (int)MonsterCareers.SabertoothTiger:
                    return "entry_sabertooth_tiger";
                case (int)MonsterCareers.SkeletalWarrior:
                    return "entry_skeletal_warrior";
                case (int)MonsterCareers.Slaughterfish:
                    return "entry_slaughterfish";
                case (int)MonsterCareers.Spider:
                    return "entry_spider";
                case (int)MonsterCareers.Spriggan:
                    return "entry_spriggan";
                case (int)MonsterCareers.Vampire:
                    return "entry_vampire";
                case (int)MonsterCareers.VampireAncient:
                    return "entry_vampire_ancient";
                case (int)MonsterCareers.Wereboar:
                    return "entry_wereboar";
                case (int)MonsterCareers.Werewolf:
                    return "entry_werewolf";
                case (int)MonsterCareers.Wraith:
                    return "entry_wraith";
                case (int)MonsterCareers.Zombie:
                    return "entry_zombie";
                default:
                    return "false";
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
                UnlockedBestiary = unlockedBestiary
            };
        }

        public void RestoreSaveData(object saveData)
        {
            var myModSaveData = (MyModSaveData)saveData;

            killCounts = myModSaveData.KillCounts;
            unlockedBestiary = myModSaveData.UnlockedBestiary;
        }
    }
}
