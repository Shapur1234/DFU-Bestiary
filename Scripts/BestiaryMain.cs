using System;
using System.Collections;
using System.Collections.Generic;

using DaggerfallConnect;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
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
        }
        
        public Type SaveDataType { get { return typeof(MyModSaveData); } }
        private static Mod mod;
        public static BestiaryMain instance;
    
        static BestiaryUI bestiaryUIScreen;
        static KeyCode openMenuKeyCode;

        static bool firstSetting = true;
        public static Dictionary<string, uint> killCounts;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            
            var go = new GameObject(mod.Title);
            instance = go.AddComponent<BestiaryMain>();

            mod.SaveDataInterface = instance;
            mod.LoadSettingsCallback = LoadSettings;

            EnemyDeath.OnEnemyDeath += EnemyDeath_OnEnemyDeath;

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
                DaggerfallUI.UIManager.PushWindow(bestiaryUIScreen);
            }
            else if (bestiaryUIScreen.isShowing && InputManager.Instance.GetKeyDown(openMenuKeyCode))
                bestiaryUIScreen.CloseWindow();
            else if (openMenuKeyCode != KeyCode.None && !InputManager.Instance.IsPaused && InputManager.Instance.GetKeyDown(openMenuKeyCode))
                if (bestiaryUIScreen.isShowing)
                    bestiaryUIScreen.CloseWindow();
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

        public static void EnemyDeath_OnEnemyDeath(object sender, EventArgs e)
        {
            EnemyDeath enemyDeath = sender as EnemyDeath;
            if (enemyDeath != null)
            {
                DaggerfallEntityBehaviour entityBehaviour = enemyDeath.GetComponent<DaggerfallEntityBehaviour>();
                if (entityBehaviour != null)
                {
                    EnemyEntity enemyEntity = entityBehaviour.Entity as EnemyEntity;
                    if (true || enemyEntity != null && enemyEntity.GetEnemyGroup() != DFCareer.EnemyGroups.Humanoid)
                    {
                        if (entityBehaviour.GetComponent<EnemySenses>().Target == GameManager.Instance.PlayerEntityBehaviour)
                        {
                            string monsterName = MonsterCareerIndexToString(enemyEntity.CareerIndex);

                            if(!killCounts.ContainsKey(monsterName))
                                killCounts.Add(monsterName, 1);
                            else
                                killCounts[monsterName] += 1;
                        }
                    }
                }
            }
        }

        public static string MonsterCareerIndexToString(int index)
        {
            string output;

            switch (index)
            {
                case (int)MonsterCareers.AncientLich:
                    output = "entry_ancient_lich";
                    break;
                case (int)MonsterCareers.Centaur:
                    output = "entry_centaur";
                    break;
                case (int)MonsterCareers.DaedraLord:
                    output = "entry_daedra_lord";
                    break;
                case (int)MonsterCareers.DaedraSeducer:
                    output = "entry_daedra_seducer";
                    break;
                case (int)MonsterCareers.Daedroth:
                    output = "entry_daedroth";
                    break;
                case (int)MonsterCareers.Dragonling:
                    output = "entry_dragonling";
                    break;
                case (int)MonsterCareers.Dragonling_Alternate:
                    output = "entry_dragonling";
                    break;
                case (int)MonsterCareers.Dreugh:
                    output = "entry_dreugh";
                    break;
                case (int)MonsterCareers.FireAtronach:
                    output = "entry_fire_atronach";
                    break;
                case (int)MonsterCareers.FireDaedra:
                    output = "entry_fire_daedra";
                    break;
                case (int)MonsterCareers.FleshAtronach:
                    output = "entry_flesh_atronach";
                    break;
                case (int)MonsterCareers.FrostDaedra:
                    output = "entry_ice_daedra";
                    break;
                case (int)MonsterCareers.Gargoyle:
                    output = "entry_gargoyle";
                    break;
                case (int)MonsterCareers.Ghost:
                    output = "entry_ghost";
                    break;
                case (int)MonsterCareers.Giant:
                    output = "entry_giant";
                    break;
                case (int)MonsterCareers.GiantBat:
                    output = "entry_giant_bat";
                    break;
                case (int)MonsterCareers.GiantScorpion:
                    output = "entry_scorpion";
                    break;
                case (int)MonsterCareers.GrizzlyBear:
                    output = "entry_grizzly_bear";
                    break;
                case (int)MonsterCareers.Harpy:
                    output = "entry_harpy";
                    break;
                case (int)MonsterCareers.IceAtronach:
                    output = "entry_ice_atronach";
                    break;
                case (int)MonsterCareers.Imp:
                    output = "entry_imp";
                    break;
                case (int)MonsterCareers.IronAtronach:
                    output = "entry_iron_atronach";
                    break;
                case (int)MonsterCareers.Lamia:
                    output = "entry_lamia";
                    break;
                case (int)MonsterCareers.Lich:
                    output = "entry_lich";
                    break;
                case (int)MonsterCareers.Mummy:
                    output = "entry_mummy";
                    break;
                case (int)MonsterCareers.Nymph:
                    output = "entry_nymph";
                    break;
                case (int)MonsterCareers.Orc:
                    output = "entry_orc";
                    break;
                case (int)MonsterCareers.OrcSergeant:
                    output = "entry_orc_sergeant";
                    break;
                case (int)MonsterCareers.OrcShaman:
                    output = "entry_orc_shaman";
                    break;
                case (int)MonsterCareers.OrcWarlord:
                    output = "entry_orc_warlord";
                    break;
                case (int)MonsterCareers.Rat:
                    output = "entry_rat";
                    break;
                case (int)MonsterCareers.SabertoothTiger:
                    output = "entry_sabertooth_tiger";
                    break;
                case (int)MonsterCareers.SkeletalWarrior:
                    output = "entry_skeletal_warrior";
                    break;
                case (int)MonsterCareers.Slaughterfish:
                    output = "entry_slaughterfish";
                    break;
                case (int)MonsterCareers.Spider:
                    output = "entry_spider";
                    break;
                case (int)MonsterCareers.Spriggan:
                    output = "entry_spriggan";
                    break;
                case (int)MonsterCareers.Vampire:
                    output = "entry_vampire";
                    break;
                case (int)MonsterCareers.VampireAncient:
                    output = "entry_vampire_ancient";
                    break;
                case (int)MonsterCareers.Wereboar:
                    output = "entry_wereboar";
                    break;
                case (int)MonsterCareers.Werewolf:
                    output = "entry_werewolf";
                    break;
                case (int)MonsterCareers.Wraith:
                    output = "entry_wraith";
                    break;
                case (int)MonsterCareers.Zombie:
                    output = "entry_zombie";
                    break;
                default:
                    output = "false";
                    break;
            }

            return output;
        }

        public object NewSaveData()
        {
            return new MyModSaveData
            {
                KillCounts = new Dictionary<string, uint>()
            };
        }

        public object GetSaveData()
        {
            return new MyModSaveData
            {
                KillCounts = killCounts
            };
        }

        public void RestoreSaveData(object saveData)
        {
            var myModSaveData = (MyModSaveData)saveData;
            killCounts = myModSaveData.KillCounts;
        }
    }
}
