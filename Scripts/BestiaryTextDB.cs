using DaggerfallConnect;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Entity;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace BestiaryMod
{
    internal class BestiaryTextDB
    {
        public static Dictionary<int, string> OverrideTitleTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideSummaryTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideAdviceTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideMaterialTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideLanguageTable = new Dictionary<int, string>();
        public static Dictionary<int, string[]> OverrideAbilitiesTable = new Dictionary<int, string[]>();
        public static Dictionary<int, string[]> OverrideSpellsTable = new Dictionary<int, string[]>();
        public static Dictionary<int, int[]> OverrideSpellsIdsTable = new Dictionary<int, int[]>();

        #region Strings
        // General
        public static string NoneLabel = BestiaryMain.Instance.Localize("NoneLabel");
        public static string AbilitiesLabel = BestiaryMain.Instance.Localize("AbilitiesLabel");
        public static string AdviceLabel = BestiaryMain.Instance.Localize("AdviceLabel");
        public static string KillcountLabel = BestiaryMain.Instance.Localize("KillcountLabel");
        public static string LanguageLabel = BestiaryMain.Instance.Localize("LanguageLabel");
        public static string MaterialLabel = BestiaryMain.Instance.Localize("MaterialLabel");
        public static string OverviewLabel = BestiaryMain.Instance.Localize("OverviewLabel");
        public static string SpellsLabel = BestiaryMain.Instance.Localize("SpellsLabel");
        public static string SummaryLabel = BestiaryMain.Instance.Localize("SummaryLabel");
        public static string BestiaryTitle = BestiaryMain.Instance.Localize("BestiaryTitle");
        public static string SummarySubTitle = BestiaryMain.Instance.Localize("SummarySubTitle");
        public static string UnlockBestiaryToUseIt = BestiaryMain.Instance.Localize("UnlockBestiaryToUseIt");
        public static string YouDontHaveBestiary = BestiaryMain.Instance.Localize("YouDontHaveBestiary");
        public static string YouHaveUnlockedTheBestiary = BestiaryMain.Instance.Localize("YouHaveUnlockedTheBestiary");
        public static string AddedToTheBestiary = BestiaryMain.Instance.Localize("AddedToTheBestiary");

        // Materials
        public static string NoMaterial = BestiaryMain.Instance.Localize("NoMaterial");
        public static string MaterialMithril = BestiaryMain.Instance.Localize("MaterialMithril");
        public static string MaterialSilver = BestiaryMain.Instance.Localize("MaterialSilver");
        public static string MaterialDwarven = BestiaryMain.Instance.Localize("MaterialDwarven");
        public static string MaterialSteel = BestiaryMain.Instance.Localize("MaterialSteel");
        public static string MaterialElven = BestiaryMain.Instance.Localize("MaterialElven");

        /*** PAGES ***/
        // Animals
        public static string AnimalsTitle = BestiaryMain.Instance.Localize("AnimalsTitle");
        public static string AnimalsOverview = BestiaryMain.Instance.Localize("AnimalsOverview");

        // Atronachs
        public static string AtronachsTitle = BestiaryMain.Instance.Localize("AtronachsTitle");
        public static string AtronachsOverview = BestiaryMain.Instance.Localize("AtronachsOverview");

        // Daedra
        public static string DaedraTitle = BestiaryMain.Instance.Localize("DaedraTitle");
        public static string DaedraOverview = BestiaryMain.Instance.Localize("DaedraOverview");

        // Lycanthropes
        public static string LycanthropesTitle = BestiaryMain.Instance.Localize("LycanthropesTitle");
        public static string LycanthropesOverview = BestiaryMain.Instance.Localize("LycanthropesOverview");

        // Monsters
        public static string MonstersTitle = BestiaryMain.Instance.Localize("MonstersTitle");
        public static string MonstersOverview = BestiaryMain.Instance.Localize("MonstersOverview");

        // Orcs
        public static string OrcsTitle = BestiaryMain.Instance.Localize("OrcsTitle");
        public static string OrcsOverview = BestiaryMain.Instance.Localize("OrcsOverview");

        // Undead
        public static string UndeadTitle = BestiaryMain.Instance.Localize("UndeadTitle");
        public static string UndeadOverview = BestiaryMain.Instance.Localize("UndeadOverview");

        /*** ENTRIES ***/
        //AncientLich
        public static string AncientLichSummary = BestiaryMain.Instance.Localize("AncientLichSummary");
        public static string AncientLichAdvice = BestiaryMain.Instance.Localize("AncientLichAdvice");
        public static string AncientLichMaterial = MaterialMithril;
        public static string AncientLichLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);

        // Centaur
        public static string CentaurSummary = BestiaryMain.Instance.Localize("CentaurSummary");
        public static string CentaurAdvice = BestiaryMain.Instance.Localize("CentaurAdvice");
        public static string CentaurAbility = BestiaryMain.Instance.Localize("CentaurAbility");
        public static string CentaurLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Centaurian);

        // Daedra Lord
        public static string DaedraLordSummary = BestiaryMain.Instance.Localize("DaedraLordSummary");
        public static string DaedraLordAdvice = BestiaryMain.Instance.Localize("DaedraLordAdvice");
        public static string DaedraLordMaterial = MaterialMithril;
        public static string DaedraLordLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string DaedraLordAbilities = BestiaryMain.Instance.Localize("DaedraLordAbilities");

        // Daedra Seducer
        public static string DaedraSeducerSummary = BestiaryMain.Instance.Localize("DaedraSeducerSummary");
        public static string DaedraSeducerAdvice = BestiaryMain.Instance.Localize("DaedraSeducerAdvice");
        public static string DaedraSeducerMaterial = MaterialMithril;
        public static string DaedraSeducerLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string DaedraSeducerAbilities = BestiaryMain.Instance.Localize("DaedraSeducerAbilities");

        // Daedroth
        public static string DaedrothSummary = BestiaryMain.Instance.Localize("DaedrothSummary");
        public static string DaedrothAdvice = BestiaryMain.Instance.Localize("DaedrothAdvice");
        public static string DaedrothMaterial = MaterialMithril;
        public static string DaedrothLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string DaedrothAbilities = BestiaryMain.Instance.Localize("DaedrothAbilities");

        // Dragonling
        public static string DragonlingSummary = BestiaryMain.Instance.Localize("DragonlingSummary");
        public static string DragonlingAdvice = BestiaryMain.Instance.Localize("DragonlingAdvice");
        public static string DragonlingLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Dragonish);
        public static string DragonlingAbilities = BestiaryMain.Instance.Localize("DragonlingAbilities");

        // Dreugh
        public static string DreughSummary = BestiaryMain.Instance.Localize("DreughSummary");
        public static string DreughAdvice = BestiaryMain.Instance.Localize("DreughAdvice");
        public static string DreughLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Impish);
        public static string DreughAbilities = BestiaryMain.Instance.Localize("DreughAbilities");

        // FireAtronach
        public static string FireAtronachSummary = BestiaryMain.Instance.Localize("FireAtronachSummary");
        public static string FireAtronachAdvice = BestiaryMain.Instance.Localize("FireAtronachAdvice");
        public static string FireAtronachAbilities = BestiaryMain.Instance.Localize("FireAtronachAbilities");

        // Fire Daedra
        public static string FireDaedraSummary = BestiaryMain.Instance.Localize("FireDaedraSummary");
        public static string FireDaedraAdvice = BestiaryMain.Instance.Localize("FireDaedraAdvice");
        public static string FireDaedraMaterial = MaterialMithril;
        public static string FireDaedraLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string FireDaedraAbilities = BestiaryMain.Instance.Localize("FireDaedraAbilities");

        // Flesh Atronach
        public static string FleshAtronachSummary = BestiaryMain.Instance.Localize("FleshAtronachSummary");
        public static string FleshAtronachAdvice = BestiaryMain.Instance.Localize("FleshAtronachAdvice");
        public static string FleshAtronachAbilities = BestiaryMain.Instance.Localize("FleshAtronachAbilities");

        // Gargoyle
        public static string GargoyleSummary = BestiaryMain.Instance.Localize("GargoyleSummary");
        public static string GargoyleAdvice = BestiaryMain.Instance.Localize("GargoyleAdvice");
        public static string GargoyleMaterial = MaterialMithril;
        public static string GargoyleLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Giantish);
        public static string GargoyleAbilities = BestiaryMain.Instance.Localize("GargoyleAbilities");

        // Ghost
        public static string GhostSummary = BestiaryMain.Instance.Localize("GhostSummary");
        public static string GhostAdvice = BestiaryMain.Instance.Localize("GhostAdvice");
        public static string GhostMaterial = MaterialSilver;

        // Giant
        public static string GiantSummary = BestiaryMain.Instance.Localize("GiantSummary");
        public static string GiantAdvice = BestiaryMain.Instance.Localize("GiantAdvice");
        public static string GiantLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Giantish);

        // Giant Bat
        public static string GiantBatSummary = BestiaryMain.Instance.Localize("GiantBatSummary");
        public static string GiantBatAdvice = BestiaryMain.Instance.Localize("GiantBatAdvice");
        public static string GiantBatAbilities = BestiaryMain.Instance.Localize("GiantBatAbilities");

        // Grizzly bear
        public static string GrizzlyBearSummary = BestiaryMain.Instance.Localize("GrizzlyBearSummary");
        public static string GrizzlyBearAdvice = BestiaryMain.Instance.Localize("GrizzlyBearAdvice");

        // Harpy
        public static string HarpySummary = BestiaryMain.Instance.Localize("HarpySummary");
        public static string HarpyAdvice = BestiaryMain.Instance.Localize("HarpyAdvice");
        public static string HarpyMaterial = MaterialDwarven;
        public static string HarpyLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Harpy);

        // Ice atronach
        public static string IceAtronachSummary = BestiaryMain.Instance.Localize("IceAtronachSummary");
        public static string IceAtronachAdvice = BestiaryMain.Instance.Localize("IceAtronachAdvice");
        public static string IceAtronachAbilities = BestiaryMain.Instance.Localize("IceAtronachAbilities");

        // Frost Daedra
        public static string FrostDaedraSummary = BestiaryMain.Instance.Localize("FrostDaedraSummary");
        public static string FrostDaedraAdvice = BestiaryMain.Instance.Localize("FrostDaedraAdvice");
        public static string FrostDaedraMaterial = MaterialMithril;
        public static string FrostDaedraLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string FrostDaedraAbilities = BestiaryMain.Instance.Localize("FrostDaedraAbilities");

        // Imp
        public static string ImpSummary = BestiaryMain.Instance.Localize("ImpSummary");
        public static string ImpAdvice = BestiaryMain.Instance.Localize("ImpAdvice");
        public static string ImpMaterial = MaterialSteel;
        public static string ImpLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Impish);
        public static string ImpAbilities = BestiaryMain.Instance.Localize("ImpAbilities");

        // Iron Atronach
        public static string IronAtronachSummary = BestiaryMain.Instance.Localize("IronAtronachSummary");
        public static string IronAtronachAdvice = BestiaryMain.Instance.Localize("IronAtronachAdvice");
        public static string IronAtronachAbilities = BestiaryMain.Instance.Localize("IronAtronachAbilities");

        // Lamia
        public static string LamiaSummary = BestiaryMain.Instance.Localize("LamiaSummary");
        public static string LamiaAdvice = BestiaryMain.Instance.Localize("LamiaAdvice");
        public static string LamiaLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Nymph);
        public static string LamiaAbilities = BestiaryMain.Instance.Localize("LamiaAbilities");

        // Lich
        public static string LichSummary = BestiaryMain.Instance.Localize("LichSummary");
        public static string LichAdvice = BestiaryMain.Instance.Localize("LichAdvice");
        public static string LichMaterial = MaterialMithril;
        public static string LichLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);
        public static string LichAbilities = BestiaryMain.Instance.Localize("LichAbilities");

        // Mummy
        public static string MummySummary = BestiaryMain.Instance.Localize("MummySummary");
        public static string MummyAdvice = BestiaryMain.Instance.Localize("MummyAdvice");
        public static string MummyMaterial = MaterialSilver;
        public static string MummyAbilities = BestiaryMain.Instance.Localize("MummyAbilities");

        // Nymph
        public static string NymphSummary = BestiaryMain.Instance.Localize("NymphSummary");
        public static string NymphAdvice = BestiaryMain.Instance.Localize("NymphAdvice");
        public static string NymphMaterial = MaterialElven;
        public static string NymphLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Nymph);
        public static string NymphAbilities = BestiaryMain.Instance.Localize("NymphAbilities");

        // Orc
        public static string OrcSummary = BestiaryMain.Instance.Localize("OrcSummary");
        public static string OrcAdvice = BestiaryMain.Instance.Localize("OrcAdvice");
        public static string OrcLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);
        public static string OrcAbilities = BestiaryMain.Instance.Localize("OrcAbilities");

        // Orc Sergeant
        public static string OrcSergeantSummary = BestiaryMain.Instance.Localize("OrcSergeantSummary");
        public static string OrcSergeantAdvice = BestiaryMain.Instance.Localize("OrcSergeantAdvice");
        public static string OrcSergeantLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);

        // Orc Shaman
        public static string OrcShamanSummary = BestiaryMain.Instance.Localize("OrcShamanSummary");
        public static string OrcShamanAdvice = BestiaryMain.Instance.Localize("OrcShamanAdvice");
        public static string OrcShamanLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);

        // Orc Warlord
        public static string OrcWarlordSummary = BestiaryMain.Instance.Localize("OrcWarlordSummary");
        public static string OrcWarlordAdvice = BestiaryMain.Instance.Localize("OrcWarlordAdvice");
        public static string OrcWarlordLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);

        // Giant Rat
        public static string RatSummary = BestiaryMain.Instance.Localize("RatSummary");
        public static string RatAdvice = BestiaryMain.Instance.Localize("RatAdvice");
        public static string RatAbilities = BestiaryMain.Instance.Localize("RatAbilities");

        // Sabertooth Tiger
        public static string SabertoothTigerSummary = BestiaryMain.Instance.Localize("SabertoothTigerSummary");
        public static string SabertoothTigerAdvice = BestiaryMain.Instance.Localize("SabertoothTigerAdvice");

        // Scorpion
        public static string GiantScorpionSummary = BestiaryMain.Instance.Localize("GiantScorpionSummary");
        public static string GiantScorpionAdvice = BestiaryMain.Instance.Localize("GiantScorpionAdvice");
        public static string GiantScorpionAbilities = BestiaryMain.Instance.Localize("GiantScorpionAbilities");

        // Skeletal Warrior
        public static string SkeletalWarriorSummary = BestiaryMain.Instance.Localize("SkeletalWarriorSummary");
        public static string SkeletalWarriorAdvice = BestiaryMain.Instance.Localize("SkeletalWarriorAdvice");
        public static string SkeletalWarriorAbilities = BestiaryMain.Instance.Localize("SkeletalWarriorAbilities");

        // Slaughterfish
        public static string SlaughterfishSummary = BestiaryMain.Instance.Localize("SlaughterfishSummary");
        public static string SlaughterfishAdvice = BestiaryMain.Instance.Localize("SlaughterfishAdvice");

        // Spider
        public static string SpiderSummary = BestiaryMain.Instance.Localize("SpiderSummary");
        public static string SpiderAdvice = BestiaryMain.Instance.Localize("SpiderAdvice");
        public static string SpiderAbilities = BestiaryMain.Instance.Localize("SpiderAbilities");

        // Spriggan
        public static string SprigganSummary = BestiaryMain.Instance.Localize("SprigganSummary");
        public static string SprigganAdvice = BestiaryMain.Instance.Localize("SprigganAdvice");
        public static string SprigganLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Spriggan);
        public static string SprigganAbilities = BestiaryMain.Instance.Localize("SprigganAbilities");

        // Vampire
        public static string VampireSummary = BestiaryMain.Instance.Localize("VampireSummary");
        public static string VampireAdvice = BestiaryMain.Instance.Localize("VampireAdvice");
        public static string VampireMaterial = MaterialSilver;
        public static string VampireLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);
        public static string VampireAbilities = BestiaryMain.Instance.Localize("VampireAbilities");

        // Vampire Ancient
        public static string VampireAncientSummary = BestiaryMain.Instance.Localize("VampireAncientSummary");
        public static string VampireAncientAdvice = BestiaryMain.Instance.Localize("VampireAncientAdvice");
        public static string VampireAncientMaterial = MaterialMithril;
        public static string VampireAncientLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);
        public static string VampireAncientAbilities = BestiaryMain.Instance.Localize("VampireAncientAbilities");

        // Wereboar
        public static string WereboarSummary = BestiaryMain.Instance.Localize("WereboarSummary");
        public static string WereboarAdvice = BestiaryMain.Instance.Localize("WereboarAdvice");
        public static string WereboarMaterial = MaterialSilver;
        public static string WereboarAbilities = BestiaryMain.Instance.Localize("WereboarAbilities");

        // Werewolf
        public static string WerewolfSummary = BestiaryMain.Instance.Localize("WerewolfSummary");
        public static string WerewolfAdvice = BestiaryMain.Instance.Localize("WerewolfAdvice");
        public static string WerewolfMaterial = MaterialSilver;
        public static string WerewolfAbilities = BestiaryMain.Instance.Localize("WerewolfAbilities");

        // Wraith
        public static string WraithSummary = BestiaryMain.Instance.Localize("WraithSummary");
        public static string WraithAdvice = BestiaryMain.Instance.Localize("WraithAdvice");
        public static string WraithMaterial = MaterialSilver;
        public static string WraithAbilities = BestiaryMain.Instance.Localize("WraithAbilities");

        // Zombie
        public static string ZombieSummary = BestiaryMain.Instance.Localize("ZombieSummary");
        public static string ZombieAdvice = BestiaryMain.Instance.Localize("ZombieAdvice");
        public static string ZombieAbilities = BestiaryMain.Instance.Localize("ZombieAbilities");
        #endregion


        #region Spells
        public static int[] ImpSpells = { 0x07, 0x0A, 0x1D, 0x2C };
        public static int[] GhostSpells = { 0x22 };
        public static int[] OrcShamanSpells = { 0x06, 0x07, 0x16, 0x19, 0x1F };
        public static int[] WraithSpells = { 0x1C, 0x1F };
        public static int[] FrostDaedraSpells = { 0x10, 0x14 };
        public static int[] FireDaedraSpells = { 0x0E, 0x19 };
        public static int[] DaedrothSpells = { 0x16, 0x17, 0x1F };
        public static int[] VampireSpells = { 0x33 };
        public static int[] SeducerSpells = { 0x34, 0x43 };
        public static int[] VampireAncientSpells = { 0x08, 0x32 };
        public static int[] DaedraLordSpells = { 0x08, 0x0A, 0x0E, 0x3C, 0x43 };
        public static int[] LichSpells = { 0x08, 0x0A, 0x0E, 0x22, 0x3C };
        public static int[] AncientLichSpells = { 0x08, 0x0A, 0x0E, 0x1D, 0x1F, 0x22, 0x3C };

        #endregion


        #region Entries
        public static Entry AncientLich()
        {
            return new Entry(MonsterCareers.AncientLich)
            {
                ButtonTextureName = "button_ancient_lich",
                Summary = AncientLichSummary,
                Advice = AncientLichAdvice,
                Material = AncientLichMaterial,
                Language = AncientLichLanguage,
                Spells = AncientLichSpells,
            };
        }

        public static Entry Centaur()
        {
            return new Entry(MonsterCareers.Centaur)
            {
                ButtonTextureName = "button_centaur",
                Summary = CentaurSummary,
                Advice = CentaurAdvice,
                Material = NoMaterial,
                Abilities = CentaurAbility.Split('|'),
                Language = CentaurLanguage,
            };
        }

        public static Entry DaedraLord()
        {
            return new Entry(MonsterCareers.DaedraLord)
            {
                ButtonTextureName = "button_daedra_lord",
                Summary = DaedraLordSummary,
                Advice = DaedraLordAdvice,
                Material = DaedraLordMaterial,
                Abilities = DaedraLordAbilities.Split('|'),
                Language = DaedraLordLanguage,
                Spells = DaedraLordSpells,
            };
        }

        public static Entry DaedraSeducer()
        {
            return new Entry(MonsterCareers.DaedraSeducer)
            {
                ButtonTextureName = "button_daedra_seducer",
                Summary = DaedraSeducerSummary,
                Advice = DaedraSeducerAdvice,
                Material = DaedraSeducerMaterial,
                Abilities = DaedraSeducerAbilities.Split('|'),
                Language = DaedraSeducerLanguage,
                Spells = SeducerSpells,
            };
        }

        public static Entry Daedroth()
        {
            return new Entry(MonsterCareers.Daedroth)
            {
                ButtonTextureName = "button_daedroth",
                Summary = DaedrothSummary,
                Advice = DaedrothAdvice,
                Material = DaedrothMaterial,
                Abilities = DaedrothAbilities.Split('|'),
                Language = DaedrothLanguage,
                Spells = DaedrothSpells
            };
        }

        public static Entry Dragonling()
        {
            return new Entry(MonsterCareers.Dragonling)
            {
                ButtonTextureName = "button_dragonling",
                Summary = DragonlingSummary,
                Advice = DragonlingAdvice,
                Material = NoMaterial,
                Language = DragonlingLanguage,
                Abilities = DragonlingAbilities.Split('|'),
            };
        }

        public static Entry Dreugh()
        {
            return new Entry(MonsterCareers.Dreugh)
            {
                ButtonTextureName = "button_dreugh",
                Summary = DreughSummary,
                Advice = DreughAdvice,
                Material = NoMaterial,
                Language = DreughLanguage,
                Abilities = DreughAbilities.Split('|'),
            };
        }

        public static Entry FireAtronach()
        {
            return new Entry(MonsterCareers.FireAtronach)
            {
                ButtonTextureName = "button_fire_atronach",
                Summary = FireAtronachSummary,
                Advice = FireAtronachAdvice,
                Material = NoMaterial,
                Abilities = FireAtronachAbilities.Split('|'),
            };
        }

        public static Entry FireDaedra()
        {
            return new Entry(MonsterCareers.FireDaedra)
            {
                ButtonTextureName = "button_fire_daedra",
                Summary = FireDaedraSummary,
                Advice = FireDaedraAdvice,
                Material = FireDaedraMaterial,
                Language = FireDaedraLanguage,
                Abilities = FireDaedraAbilities.Split('|'),
                Spells = FireDaedraSpells,
            };
        }

        public static Entry FleshAtronach()
        {
            return new Entry(MonsterCareers.FleshAtronach)
            {
                ButtonTextureName = "button_flesh_atronach",
                Summary = FleshAtronachSummary,
                Advice = FleshAtronachAdvice,
                Material = NoMaterial,
                Abilities = FleshAtronachAbilities.Split('|'),
            };
        }

        public static Entry Gargoyle()
        {
            return new Entry(MonsterCareers.Gargoyle)
            {
                ButtonTextureName = "button_gargoyle",
                Summary = GargoyleSummary,
                Advice = GargoyleAdvice,
                Material = GargoyleMaterial,
                Language = GargoyleLanguage,
                Abilities = GargoyleAbilities.Split('|'),
            };
        }

        public static Entry Ghost()
        {
            return new Entry(MonsterCareers.Ghost)
            {
                ButtonTextureName = "button_ghost",
                Summary = GhostSummary,
                Advice = GhostAdvice,
                Material = GhostMaterial,
                Spells = GhostSpells,
            };
        }

        public static Entry Giant()
        {
            return new Entry(MonsterCareers.Giant)
            {
                ButtonTextureName = "button_giant",
                Summary = GiantSummary,
                Advice = GiantAdvice,
                Material = NoMaterial,
                Language = GiantLanguage,
            };
        }

        public static Entry GiantBat()
        {
            return new Entry(MonsterCareers.GiantBat)
            {
                ButtonTextureName = "button_giant_bat",
                Summary = GiantBatSummary,
                Advice = GiantBatAdvice,
                Material = NoMaterial,
                Abilities = GiantBatAbilities.Split('|'),
            };
        }

        public static Entry GrizzlyBear()
        {
            return new Entry(MonsterCareers.GrizzlyBear)
            {
                ButtonTextureName = "button_grizzly_bear",
                Summary = GrizzlyBearSummary,
                Advice = GrizzlyBearAdvice,
                Material = NoMaterial,
            };
        }

        public static Entry Harpy()
        {
            return new Entry(MonsterCareers.Harpy)
            {
                ButtonTextureName = "button_harpy",
                Summary = HarpySummary,
                Advice = HarpyAdvice,
                Material = HarpyMaterial,
                Language = HarpyLanguage,
            };
        }

        public static Entry IceAtronach()
        {
            return new Entry(MonsterCareers.IceAtronach)
            {
                ButtonTextureName = "button_ice_atronach",
                Summary = IceAtronachSummary,
                Advice = IceAtronachAdvice,
                Material = NoMaterial,
                Abilities = IceAtronachAbilities.Split('|'),
            };
        }

        public static Entry FrostDaedra()
        {
            return new Entry(MonsterCareers.FrostDaedra)
            {
                ButtonTextureName = "button_ice_daedra",
                Summary = FrostDaedraSummary,
                Advice = FrostDaedraAdvice,
                Material = FrostDaedraMaterial,
                Language = FrostDaedraLanguage,
                Abilities = FrostDaedraAbilities.Split('|'),
                Spells = FrostDaedraSpells,
            };
        }

        public static Entry Imp()
        {
            return new Entry(MonsterCareers.Imp)
            {
                ButtonTextureName = "button_imp",
                Summary = ImpSummary,
                Advice = ImpAdvice,
                Material = ImpMaterial,
                Language = ImpLanguage,
                Abilities = ImpAbilities.Split('|'),
                Spells = ImpSpells,
            };
        }

        public static Entry IronAtronach()
        {
            return new Entry(MonsterCareers.IronAtronach)
            {
                ButtonTextureName = "button_iron_atronach",
                Summary = IronAtronachSummary,
                Advice = IronAtronachAdvice,
                Material = NoMaterial,
                Abilities = IronAtronachAbilities.Split('|'),
            };
        }

        public static Entry Lamia()
        {
            return new Entry(MonsterCareers.Lamia)
            {
                ButtonTextureName = "button_lamia",
                Summary = LamiaSummary,
                Advice = LamiaAdvice,
                Material = NoMaterial,
                Language = LamiaLanguage,
                Abilities = LamiaAbilities.Split('|'),
            };
        }

        public static Entry Lich()
        {
            return new Entry(MonsterCareers.Lich)
            {
                ButtonTextureName = "button_lich",
                Summary = LichSummary,
                Advice = LichAdvice,
                Material = LichMaterial,
                Language = LichLanguage,
                Abilities = LichAbilities.Split('|'),
                Spells = LichSpells,
            };
        }

        public static Entry Mummy()
        {
            return new Entry(MonsterCareers.Mummy)
            {
                ButtonTextureName = "button_mummy",
                Summary = MummySummary,
                Advice = MummyAdvice,
                Material = MummyMaterial,
                Abilities = MummyAbilities.Split('|'),
            };
        }

        public static Entry Nymph()
        {
            return new Entry(MonsterCareers.Nymph)
            {
                ButtonTextureName = "button_nymph",
                Summary = NymphSummary,
                Advice = NymphAdvice,
                Material = NymphMaterial,
                Language = NymphLanguage,
                Abilities = NymphAbilities.Split('|'),
            };
        }

        public static Entry Orc()
        {
            return new Entry(MonsterCareers.Orc)
            {
                ButtonTextureName = "button_orc",
                Summary = OrcSummary,
                Advice = OrcAdvice,
                Material = NoMaterial,
                Language = OrcLanguage,
                Abilities = OrcAbilities.Split('|'),
            };
        }

        public static Entry OrcSergeant()
        {
            return new Entry(MonsterCareers.OrcSergeant)
            {
                ButtonTextureName = "button_orc_sergeant",
                Summary = OrcSergeantSummary,
                Advice = OrcSergeantAdvice,
                Material = NoMaterial,
                Language = OrcSergeantLanguage,
            };
        }

        public static Entry OrcShaman()
        {
            return new Entry(MonsterCareers.OrcShaman)
            {
                ButtonTextureName = "button_orc_shaman",
                Summary = OrcShamanSummary,
                Advice = OrcShamanAdvice,
                Material = NoMaterial,
                Language = OrcShamanLanguage,
                Spells = OrcShamanSpells,
            };
        }

        public static Entry OrcWarlord()
        {
            return new Entry(MonsterCareers.OrcWarlord)
            {
                ButtonTextureName = "button_orc_warlord",
                Summary = OrcWarlordSummary,
                Advice = OrcWarlordAdvice,
                Material = NoMaterial,
                Language = OrcWarlordLanguage,
            };
        }

        public static Entry Rat()
        {
            return new Entry(MonsterCareers.Rat)
            {
                ButtonTextureName = "button_rat",
                Summary = RatSummary,
                Advice = RatAdvice,
                Material = NoMaterial,
                Abilities = RatAbilities.Split('|'),
            };
        }

        public static Entry SabertoothTiger()
        {
            return new Entry(MonsterCareers.SabertoothTiger)
            {
                ButtonTextureName = "button_sabertooth_tiger",
                Summary = SabertoothTigerSummary,
                Advice = SabertoothTigerAdvice,
                Material = NoMaterial,
            };
        }

        public static Entry GiantScorpion()
        {
            return new Entry(MonsterCareers.GiantScorpion)
            {
                ButtonTextureName = "button_scorpion",
                Summary = GiantScorpionSummary,
                Advice = GiantScorpionAdvice,
                Material = NoMaterial,
                Abilities = GiantScorpionAbilities.Split('|'),
            };
        }

        public static Entry SkeletalWarrior()
        {
            return new Entry(MonsterCareers.SkeletalWarrior)
            {
                ButtonTextureName = "button_skeletal_warrior",
                Summary = SkeletalWarriorSummary,
                Advice = SkeletalWarriorAdvice,
                Material = NoMaterial,
                Abilities = SkeletalWarriorAbilities.Split('|'),
            };
        }

        public static Entry Slaughterfish()
        {
            return new Entry(MonsterCareers.Slaughterfish)
            {
                ButtonTextureName = "button_slaughterfish",
                Summary = SlaughterfishSummary,
                Advice = SlaughterfishAdvice,
                Material = NoMaterial,
            };
        }

        public static Entry Spider()
        {
            return new Entry(MonsterCareers.Spider)
            {
                ButtonTextureName = "button_spider",
                Summary = SpiderSummary,
                Advice = SpiderAdvice,
                Material = NoMaterial,
                Abilities = SpiderAbilities.Split('|'),
            };
        }

        public static Entry Spriggan()
        {
            return new Entry(MonsterCareers.Spriggan)
            {
                ButtonTextureName = "button_spriggan",
                Summary = SprigganSummary,
                Advice = SprigganAdvice,
                Material = NoMaterial,
                Language = SprigganLanguage,
                Abilities = SprigganAbilities.Split('|'),
            };
        }

        public static Entry Vampire()
        {
            return new Entry(MonsterCareers.Vampire)
            {
                ButtonTextureName = "button_vampire",
                Summary = VampireSummary,
                Advice = VampireAdvice,
                Material = VampireMaterial,
                Language = VampireLanguage,
                Abilities = VampireAbilities.Split('|'),
                Spells = VampireSpells,
            };
        }

        public static Entry VampireAncient()
        {
            return new Entry(MonsterCareers.VampireAncient)
            {
                ButtonTextureName = "button_vampire_ancient",
                Summary = VampireAncientSummary,
                Advice = VampireAncientAdvice,
                Material = VampireAncientMaterial,
                Language = VampireAncientLanguage,
                Abilities = VampireAncientAbilities.Split('|'),
                Spells = VampireAncientSpells,
            };
        }

        public static Entry Wereboar()
        {
            return new Entry(MonsterCareers.Wereboar)
            {
                ButtonTextureName = "button_wereboar",
                Summary = WereboarSummary,
                Advice = WereboarAdvice,
                Material = WereboarMaterial,
                Abilities = WereboarAbilities.Split('|'),
            };
        }

        public static Entry Werewolf()
        {
            return new Entry(MonsterCareers.Werewolf)
            {
                ButtonTextureName = "button_werewolf",
                Summary = WerewolfSummary,
                Advice = WerewolfAdvice,
                Material = WerewolfMaterial,
                Abilities = WerewolfAbilities.Split('|'),
            };
        }

        public static Entry Wraith()
        {
            return new Entry(MonsterCareers.Wraith)
            {
                ButtonTextureName = "button_wraith",
                Summary = WraithSummary,
                Advice = WraithAdvice,
                Material = WraithMaterial,
                Abilities = WraithAbilities.Split('|'),
                Spells = WraithSpells,
            };
        }

        public static Entry Zombie()
        {
            return new Entry(MonsterCareers.Zombie)
            {
                ButtonTextureName = "button_zombie",
                Summary = ZombieSummary,
                Advice = ZombieAdvice,
                Material = NoMaterial,
                Abilities = ZombieAbilities.Split('|'),
            };
        }
        #endregion


        #region Pages
        public static Page Animals()
        {
            return new Page(
                AnimalsTitle,
                AnimalsOverview,
                MonsterCareers.GiantBat,
                MonsterCareers.Rat,
                MonsterCareers.GrizzlyBear,
                MonsterCareers.SabertoothTiger,
                MonsterCareers.GiantScorpion,
                MonsterCareers.Slaughterfish,
                MonsterCareers.Spider
            );
        }

        public static Page Atronachs()
        {
            return new Page(
                AtronachsTitle,
                AtronachsOverview,
                MonsterCareers.FireAtronach,
                MonsterCareers.FleshAtronach,
                MonsterCareers.IceAtronach,
                MonsterCareers.IronAtronach
            );
        }

        public static Page Daedra()
        {
            return new Page(
                DaedraTitle,
                DaedraOverview,
                MonsterCareers.DaedraLord,
                MonsterCareers.DaedraSeducer,
                MonsterCareers.Daedroth,
                MonsterCareers.FireDaedra,
                MonsterCareers.FrostDaedra
            );
        }

        public static Page Lycanthropes()
        {
            return new Page(
                LycanthropesTitle,
                LycanthropesOverview,
                MonsterCareers.Wereboar,
                MonsterCareers.Werewolf
            );
        }

        public static Page Monsters1()
        {
            return new Page(
                MonstersTitle,
                MonstersOverview,
                MonsterCareers.Centaur,
                MonsterCareers.Dragonling,
                MonsterCareers.Dreugh,
                MonsterCareers.Gargoyle,
                MonsterCareers.Giant
            );
        }

        public static Page Monsters2()
        {
            return new Page(
                MonstersTitle,
                MonstersOverview,
                MonsterCareers.Harpy,
                MonsterCareers.Imp,
                MonsterCareers.Lamia,
                MonsterCareers.Nymph,
                MonsterCareers.Spriggan
            );
        }

        public static Page Orcs()
        {
            return new Page(
                OrcsTitle,
                OrcsOverview,
                MonsterCareers.Orc,
                MonsterCareers.OrcSergeant,
                MonsterCareers.OrcShaman,
                MonsterCareers.OrcWarlord
            );
        }

        public static Page Undead()
        {
            return new Page(
                UndeadTitle,
                UndeadOverview,
                MonsterCareers.Ghost,
                MonsterCareers.Lich,
                MonsterCareers.AncientLich,
                MonsterCareers.Mummy,
                MonsterCareers.SkeletalWarrior,
                MonsterCareers.Vampire,
                MonsterCareers.VampireAncient,
                MonsterCareers.Wraith,
                MonsterCareers.Zombie
            );
        }

        public static Page Classic()
        {
            return new Page(
                string.Empty,
                string.Empty,
                MonsterCareers.Centaur,
                MonsterCareers.Daedroth,
                MonsterCareers.Dreugh,
                MonsterCareers.FireDaedra,
                MonsterCareers.FrostDaedra,
                MonsterCareers.Orc,
                MonsterCareers.GiantScorpion,
                MonsterCareers.Spriggan,
                MonsterCareers.Vampire
            );
        }

        #endregion

        public static Page GetPage(string pageID)
        {
            switch (pageID)
            {
                case "page_animals":
                    return Animals();
                case "page_atronachs":
                    return Atronachs();
                case "page_daedra":
                    return Daedra();
                case "page_lycanthropes":
                    return Lycanthropes();
                case "page_monsters1":
                    return Monsters1();
                case "page_monsters2":
                    return Monsters2();
                case "page_orcs":
                    return Orcs();
                case "page_undead":
                    return Undead();
                case "page_classic":
                    return Classic();
                default:
                    return null;
            }
        }

        public static Entry GetEntryByMonster(MonsterCareers id)
        {
            switch (id)
            {
                case MonsterCareers.AncientLich:
                    return AncientLich();
                case MonsterCareers.Centaur:
                    return Centaur();
                case MonsterCareers.DaedraLord:
                    return DaedraLord();
                case MonsterCareers.DaedraSeducer:
                    return DaedraSeducer();
                case MonsterCareers.Daedroth:
                    return Daedroth();
                case MonsterCareers.Dragonling:
                    return Dragonling();
                case MonsterCareers.Dreugh:
                    return Dreugh();
                case MonsterCareers.FireAtronach:
                    return FireAtronach();
                case MonsterCareers.FireDaedra:
                    return FireDaedra();
                case MonsterCareers.FleshAtronach:
                    return FleshAtronach();
                case MonsterCareers.Gargoyle:
                    return Gargoyle();
                case MonsterCareers.Ghost:
                    return Ghost();
                case MonsterCareers.Giant:
                    return Giant();
                case MonsterCareers.GiantBat:
                    return GiantBat();
                case MonsterCareers.GrizzlyBear:
                    return GrizzlyBear();
                case MonsterCareers.Harpy:
                    return Harpy();
                case MonsterCareers.IceAtronach:
                    return IceAtronach();
                case MonsterCareers.FrostDaedra:
                    return FrostDaedra();
                case MonsterCareers.Imp:
                    return Imp();
                case MonsterCareers.IronAtronach:
                    return IronAtronach();
                case MonsterCareers.Lamia:
                    return Lamia();
                case MonsterCareers.Lich:
                    return Lich();
                case MonsterCareers.Mummy:
                    return Mummy();
                case MonsterCareers.Nymph:
                    return Nymph();
                case MonsterCareers.Orc:
                    return Orc();
                case MonsterCareers.OrcSergeant:
                    return OrcSergeant();
                case MonsterCareers.OrcShaman:
                    return OrcShaman();
                case MonsterCareers.OrcWarlord:
                    return OrcWarlord();
                case MonsterCareers.Rat:
                    return Rat();
                case MonsterCareers.SabertoothTiger:
                    return SabertoothTiger();
                case MonsterCareers.GiantScorpion:
                    return GiantScorpion();
                case MonsterCareers.SkeletalWarrior:
                    return SkeletalWarrior();
                case MonsterCareers.Slaughterfish:
                    return Slaughterfish();
                case MonsterCareers.Spider:
                    return Spider();
                case MonsterCareers.Spriggan:
                    return Spriggan();
                case MonsterCareers.Vampire:
                    return Vampire();
                case MonsterCareers.VampireAncient:
                    return VampireAncient();
                case MonsterCareers.Wereboar:
                    return Wereboar();
                case MonsterCareers.Werewolf:
                    return Werewolf();
                case MonsterCareers.Wraith:
                    return Wraith();
                case MonsterCareers.Zombie:
                    return Zombie();
                default:
                    return null;
            }
        }
    }
}
