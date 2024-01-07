using DaggerfallConnect;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Entity;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace BestiaryMod
{
    internal class BestiaryTextDB
    {
        public static Dictionary<string, string> stringTable = null;

        public static Dictionary<int, string> OverrideTitleTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideSummaryTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideAdviceTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideMaterialTable = new Dictionary<int, string>();
        public static Dictionary<int, string> OverrideLanguageTable = new Dictionary<int, string>();
        public static Dictionary<int, string[]> OverrideAbilitiesTable = new Dictionary<int, string[]>();
        public static Dictionary<int, string[]> OverrideSpellsTable = new Dictionary<int, string[]>();
        public static Dictionary<int, int[]> OverrideSpellsIdsTable = new Dictionary<int, int[]>();

        public static void Setup()
        {
            LoadText();
        }

        #region Strings
        // General
        public static string NoneLabel = "None";
        public static string AbilitiesLabel = "Abilities:";
        public static string AdviceLabel = "Advice:";
        public static string KillcountLabel = "Killcount:";
        public static string LanguageLabel = "Language:";
        public static string MaterialLabel = "Material:";
        public static string OverviewLabel = "Overview:";
        public static string SpellsLabel = "Spells:";
        public static string SummaryLabel = "Summary:";
        public static string BestiaryTitle = "Bestiary";
        public static string SummarySubTitle = "Summary";
        public static string UnlockBestiaryToUseIt = "You have not yet unlocked the Bestiary. Find the Bestiary book item and click USE on it.";
        public static string YouDontHaveBestiary = "You do not to have the Bestiary item in your inventory.";
        public static string YouHaveUnlockedTheBestiary = "You study the contents of the book closely. You have unlocked the Bestiary.";
        public static string AddedToTheBestiary = "{0} has been added to the Bestiary.";

        // Materials
        public static string NoMaterial = "No special material is required";
        public static string MaterialMithril = "Mithril or higher is required";
        public static string MaterialSilver = "Silver or higher is required";
        public static string MaterialDwarven = "Dwarven or higher is required";
        public static string MaterialSteel = "Steel or higher is required";
        public static string MaterialElven = "Elven or higher is required";

        /*** PAGES ***/
        // Animals
        public static string AnimalsTitle = "Animals";
        public static string AnimalsOverview = "Animals are mundane creatures, or oversized versions of them. They never carry any treasure, though you may still retrieve arrows from their corpses. Certain animals are carriers of disease. No special materials are required to harm them.";

        // Atronachs
        public static string AtronachsTitle = "Atronachs";
        public static string AtronachsOverview = "Some of the atronachs in the Iliac Bay are Daedric in origin, however, some are golem-like constructs created by mages in the Iliac Bay. Though they can be hit by any material, they are tied to a certain element and are thus weak to the opposing element. Lastly, they carry no treasure.";

        // Daedra
        public static string DaedraTitle = "Daedra";
        public static string DaedraOverview = "\"Daedra\" is a term for any of the numerous varieties of otherworldly beings from the plane of Oblivion. They are fairly challenging adversaries, and can only be harmed by weapons of mithril quality or greater. All daedra can detect invisible foes.";

        // Lycanthropes
        public static string LycanthropesTitle = "Lycanthropes";
        public static string LycanthropesOverview = "Lycanthropes are mortals who have been affected by Lycanthropy, a condition that transforms them into a large hybrid of humanoid and animal. Two types of lycanthropes exist in the Iliac Bay area - werewolves and wereboars, out of the eight total recorded species of tamriel. Weapons of at least silver quality are needed to harm them, though they're not exceptionally dangerous and carry no treasure. However, there is always a risk of contracting lycanthropy from combat.";

        // Monsters
        public static string MonstersTitle = "Monsters";
        public static string MonstersOverview = "Monsters are fantastical creatures, often possessing various magical abilities. These can range from magical attacks to various magical resistances, and many monsters can only be harmed by weapons made of special materials. Some monsters have an associated language skill. Also, unlike animals, you can find some treasure on the corpses of most monsters.";

        // Orcs
        public static string OrcsTitle = "Orcs";
        public static string OrcsOverview = "Hailing from Orsinium, these savage brutes can be found across the Iliac Bay. Like humanoids, their spoils include a good deal of equipment. No special materials are required to harm them.";

        // Undead
        public static string UndeadTitle = "Undead";
        public static string UndeadOverview = "Undead creatures consist of spirits, reanimated skeletons and corpses, and vampires. They are a recurring menace in dungeons, and some can be very dangerous. Some types of undead are carriers of disease. Most undead are able to detect invisible foes. Ghosts, Skeletal Warriors, Wraiths and Zombies are also often found underwater.";

        /*** ENTRIES ***/
        //AncientLich
        public static string AncientLichSummary = "Ancient Liches are undead beings of immense power; stronger, smarter, faster, and tougher to destroy than their lesser brethren. They are one of the most dangerous opponents any adventurer will ever face.";
        public static string AncientLichAdvice = "Due to the extreme danger of the Ancient Lich's spellcasting abilities, the use of Spell Reflection, Spell Resistance, or Spell Absorption is a definite must when fighting one. Your safest bets are to attack with ranged weapons or have plenty of healing potions and spells on hand. Ancient Liches will sometimes use area affect damage spells at too close a range and harm or even destroy themselves.";
        public static string AncientLichMaterial = MaterialMithril;
        public static string AncientLichLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);

        // Centaur
        public static string CentaurSummary = "Centaurs are half-man, half-horse monsters wielding spears or other weapons.";
        public static string CentaurAdvice = "As centaurs are resistant to magic, melee attacks work best. The Centaurian language skill can sometimes render an enemy non-hostile.";
        public static string CentaurAbility = "Resistance to Magic";
        public static string CentaurLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Centaurian);

        // Daedra Lord
        public static string DaedraLordSummary = "With weapons and magic at their disposal, Daedra Lords are powerful and dangerous opponents.";
        public static string DaedraLordAdvice = "One of the most dangerous foes in the game, the Daedra Lord possesses formidable fighting abilities in close combat. When fighting a Daedra Lord, close in quickly to prevent him from using magic too frequently, and backpedal after striking to avoid his attacks. Spell Reflection, Spell Resistance, or Spell Absorption is a must, especially when fighting at distance as he will use his area effect Fireball spell. A Daedra Lord will sometimes use this spell too close, resulting in his own harm or destruction from its blast.";
        public static string DaedraLordMaterial = MaterialMithril;
        public static string DaedraLordLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string DaedraLordAbilities = "Can detect camouflaged or invisible foes|Immunity to Paralysis, Poison, and Disease|Regenerates Health (General)|Takes Damage from Holy Places";

        // Daedra Seducer
        public static string DaedraSeducerSummary = "Daedra Seducers appear to be alluring spellcasters at first, but when attacked reveal their true bat-winged forms.";
        public static string DaedraSeducerAdvice = "Daedra Seducers are best fought with ranged weapons or magic, as their spells can be devastating at close range (see Bugs). Spell Reflection, Spell Resistance, or Spell Absorption are recommended when fighting one.";
        public static string DaedraSeducerMaterial = MaterialMithril;
        public static string DaedraSeducerLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string DaedraSeducerAbilities = "Can detect camouflaged or invisible foes";

        // Daedroth
        public static string DaedrothSummary = "Daedroths (also known as Lesser Daedra) are crocodile-headed warriors wielding deadly axes and sinister spells.";
        public static string DaedrothAdvice = "Close in quickly and use melee attacks to deny the Daedroth an opportunity to cast its ranged spells. You may also wish to protect yourself with Spell Reflection, Spell Resistance, or Spell Absorption. Daedroths are especially dangerous to spellcasters, as they make frequent use of their Silence spell. However, they are in most cases inferior in combat, so if a Daedroth is itself silenced melee attacks should result in the creature's quick death.";
        public static string DaedrothMaterial = MaterialMithril;
        public static string DaedrothLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string DaedrothAbilities = "Can detect camouflaged or invisible foes";

        // Dragonling
        public static string DragonlingSummary = "Dragonlings are winged reptiles that resemble dragons and attack by breathing fire.";
        public static string DragonlingAdvice = "Melee attacks are best. Beware its fiery breath, for it is not a spell. Dragonlings are heavily armored and can take a lot of hits.";
        public static string DragonlingLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Dragonish);
        public static string DragonlingAbilities = "Resistance to Magic|Immunity to Paralysis, Poison, and Disease|Regenerates Health in Darkness|Adrenaline Rush|Bonus to Hit Humanoids|Low Tolerance to Frost|Takes Damage from Holy Places";

        // Dreugh
        public static string DreughSummary = "Dreughs are abominations resembling a human-octopus hybrid with powerful crab claws. They are only found underwater.";
        public static string DreughAdvice = "Melee attacks.";
        public static string DreughLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Impish);
        public static string DreughAbilities = "Resistance to Magic|Immunity to Paralysis, Poison, and Disease|Regenerates Health in Darkness and in Water|Adrenaline Rush|Bonus to Hit Humanoids|Low Tolerance to Frost|Takes Damage from Holy Places";

        // FireAtronach
        public static string FireAtronachSummary = "Fire Atronachs are golems constructed of flames and possess a fire-based damage aura.";
        public static string FireAtronachAdvice = "Melee attacks are recommended. Lower level characters who wish to avoid melee may use cold-based spells.";
        public static string FireAtronachAbilities = "Healed by fire-based spells|Immunity to Paralysis, Poison, Fire and Disease|Regenerates Health in Darkness|Adrenaline Rush|Bonus to Hit Humanoids|Low Tolerance to Frost|Takes Damage from Holy Places";

        // Fire Daedra
        public static string FireDaedraSummary = "Fire Daedra are flame-clad horrors from Oblivion. In addition to their burning swords, these creatures can cast fire-based spells from afar.";
        public static string FireDaedraAdvice = "Close in quickly and use melee attacks to deny the Fire Daedra an opportunity to cast its ranged spells. You may also wish to protect yourself with Spell Reflection, Spell Resistance, or Spell Absorption, or attack with cold-based offensive spells.";
        public static string FireDaedraMaterial = MaterialMithril;
        public static string FireDaedraLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string FireDaedraAbilities = "Can detect camouflaged or invisible foes";

        // Flesh Atronach
        public static string FleshAtronachSummary = "Flesh Atronachs are golems made of stitched-together cadavers and possess a poison-based damage aura.";
        public static string FleshAtronachAdvice = "Melee attacks are recommended. Lower level characters who wish to avoid melee may use electricity-based spells.";
        public static string FleshAtronachAbilities = "Healed by poison-based spells|Immunity to Paralysis, Poison and Disease|Regenerates Health in Darkness|Adrenaline Rush|Bonus to Hit Humanoids|Takes Damage from Holy Places";

        // Gargoyle
        public static string GargoyleSummary = "Gargoyles are creatures of living stone.";
        public static string GargoyleAdvice = "Melee attacks.";
        public static string GargoyleMaterial = MaterialMithril;
        public static string GargoyleLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Giantish);
        public static string GargoyleAbilities = "Resistance to Magic";

        // Ghost
        public static string GhostSummary = "Ghosts are the angry spirits of the dead.";
        public static string GhostAdvice = "At lower levels, ghosts are best attacked from a distance, or in melee while protected by Spell Reflection, Spell Resistance, or Spell Absorption. Higher level characters can easily destroy a ghost with melee attacks.";
        public static string GhostMaterial = MaterialSilver;

        // Giant
        public static string GiantSummary = "Giants are huge, territorial brutes that like to smash (and eat) their enemies.";
        public static string GiantAdvice = "Melee attacks are the quickest.";
        public static string GiantLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Giantish);

        // Giant Bat
        public static string GiantBatSummary = "The giant bat is a flying animal commonly encountered at low levels, but not so often at higher levels.";
        public static string GiantBatAdvice = "Melee attacks can quickly dispatch these beasts, though attacking with arrows or spells will greatly reduce your odds of catching a disease.";
        public static string GiantBatAbilities = "There is a 5% chance per hit of catching a disease from a giant bat.";

        // Grizzly bear
        public static string GrizzlyBearSummary = "The grizzly bear is a large animal that fights with tooth and claw.";
        public static string GrizzlyBearAdvice = "Melee attacks.";

        // Harpy
        public static string HarpySummary = "Harpies are humanoid-bird creatures with taloned feet.";
        public static string HarpyAdvice = "Melee attacks are recommended.";
        public static string HarpyMaterial = MaterialDwarven;
        public static string HarpyLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Harpy);

        // Ice atronach
        public static string IceAtronachSummary = "Ice Atronachs are golems constructed of ice shards and possess a cold-based damage aura. Unlike other atronachs, they may also be encountered underwater.";
        public static string IceAtronachAdvice = "Melee attacks are recommended. Lower level characters who wish to avoid melee may use fire-based spells.";
        public static string IceAtronachAbilities = "Healed by cold-based spells|Immunity to Paralysis, Poison, Frost and Disease|Regenerates Health in Darkness|Adrenaline Rush|Bonus to Hit Humanoids|Low Tolerance to Magic|Takes Damage from Holy Places";

        // Frost Daedra
        public static string FrostDaedraSummary = "Frost Daedra are wreathed in the cold mists of Oblivion. Not only do their warhammers pack a devastating punch, but Frost Daedra can cast cold-based spells.";
        public static string FrostDaedraAdvice = "Close in quickly and use melee attacks to deny the Frost Daedra an opportunity to cast its ranged spells. You may also wish to protect yourself with Spell Reflection, Spell Resistance, or Spell Absorption, or attack with fire-based offensive spells.";
        public static string FrostDaedraMaterial = MaterialMithril;
        public static string FrostDaedraLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Daedric);
        public static string FrostDaedraAbilities = "Can detect camouflaged or invisible foes";

        // Imp
        public static string ImpSummary = "Imps are flying creatures that cast spells at their foes.";
        public static string ImpAdvice = "Kill imps as quickly as possible, especially at low levels. Spell Reflection, Spell Resistance, or Spell Absorption are helpful.";
        public static string ImpMaterial = MaterialSteel;
        public static string ImpLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Impish);
        public static string ImpAbilities = "Can detect camouflaged or invisible foes";

        // Iron Atronach
        public static string IronAtronachSummary = "Iron Atronachs are powerful metal golems that possess an electricity-based damage aura.";
        public static string IronAtronachAdvice = "Melee attacks are recommended. Lower level characters who wish to avoid melee may use poison-based spells.";
        public static string IronAtronachAbilities = "Healed by electricity-based spells|Immunity to Paralysis, Poison, Shock and Disease|Regenerates Health in Darkness|Adrenaline Rush|Bonus to Hit Humanoids|Low Tolerance to Magic|Takes Damage from Holy Places";

        // Lamia
        public static string LamiaSummary = "Lamias are monsters with the upper body of a human and a snakelike lower section. They are only encountered underwater.";
        public static string LamiaAdvice = "Use ranged weapons to avoid the lamia's fatigue drain.";
        public static string LamiaLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Nymph);
        public static string LamiaAbilities = "Lamias drain 2 points of fatigue for every 1 point of health damage|Resistance to Magic|Immunity to Paralysis, Poison, and Disease|Regenerates Health in Darkness and in Water|Adrenaline Rush|Bonus to Hit Humanoids|Low Tolerance to Frost|Takes Damage from Holy Places";

        // Lich
        public static string LichSummary = "Liches were powerful mages in life and achieved undeath of their own accord. They have retained their intellect - including their spellcasting abilities - while becoming much more physically powerful at the same time. As such, a lich is a dangerous adversary for even the most seasoned adventurers—provided they don't first annihilate themselves with their own magic, which they frequently do.";
        public static string LichAdvice = "The use of Spell Reflection, Spell Resistance, or Spell Absorption is a definite must; a lich can easily kill you if you aren't prepared. Your safest bets are to attack with ranged weapons or have plenty of healing potions and spells on hand. Liches will sometimes use area affect damage spells at too close a range and harm or even destroy themselves.";
        public static string LichMaterial = MaterialMithril;
        public static string LichLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);
        public static string LichAbilities = "Can detect camouflaged or invisible foes";

        // Mummy
        public static string MummySummary = "Mummies are preserved corpses given new life. They pack a powerful punch, and their nature makes them vectors for disease.";
        public static string MummyAdvice = "Mummies are best attacked with ranged weapons to avoid the chance of contracting a disease.";
        public static string MummyMaterial = MaterialSilver;
        public static string MummyAbilities = "There is a 5% chance per hit of contracting a disease from a mummy|Immunity to Disease";

        // Nymph
        public static string NymphSummary = "Nymphs are fae creatures of unearthly beauty that attack with mystical energy.";
        public static string NymphAdvice = "Nymphs can be easily dispatched with a melee attack.";
        public static string NymphMaterial = MaterialElven;
        public static string NymphLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Nymph);
        public static string NymphAbilities = "Nymphs drain 2 points of fatigue for every 1 point of health damage|Resistance to Paralysis and Poison";

        // Orc
        public static string OrcSummary = "Orcs are the basic foot soldiers of the Orcish race.";
        public static string OrcAdvice = "Melee attacks.";
        public static string OrcLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);
        public static string OrcAbilities = "Resistance to Disease|Athleticism|Rapid Healing in Darkness";

        // Orc Sergeant
        public static string OrcSergeantSummary = "Orc Sergeants are Orcish fighters who are more powerful and well-equipped than normal Orcs. Orc Sergeants can be distinguished from their lesser brethren by their double-bladed axes, boots, and heavier armor.";
        public static string OrcSergeantAdvice = "Melee attacks.";
        public static string OrcSergeantLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);

        // Orc Shaman
        public static string OrcShamanSummary = "Orc Shamans are the spellcasters of Orsinium. They attack with powerful destruction spells, and make frequent use of their Invisibility spells in combat.";
        public static string OrcShamanAdvice = "Use ranged attacks to avoid the damaging spell effects encountered in melee. The use of Spell Reflection, Spell Resistance, or Spell Absorption is advised.";
        public static string OrcShamanLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);

        // Orc Warlord
        public static string OrcWarlordSummary = "Orc Warlords are elite warriors, by far the strongest and most dangerous members of the Orcish hordes. They appear similar to Orc Sergeants, with the addition of a tassel on their helms, capes, and gold bracers.";
        public static string OrcWarlordAdvice = "Use ranged attacks at lower levels, melee attacks at higher levels.";
        public static string OrcWarlordLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Orcish);

        // Giant Rat
        public static string RatSummary = "The rat is a small animal that attacks by biting. Rats can be hit by weapons of any material.";
        public static string RatAdvice = "Higher level characters can quickly dispatch rats with a single melee attack, but low level characters may wish to use ranged weapons to avoid disease.";
        public static string RatAbilities = "There is a 5% chance per hit of catching a disease from a rat|Rats are immune to poison and disease";

        // Sabertooth Tiger
        public static string SabertoothTigerSummary = "The sabertooth tiger is a large, fanged animal.";
        public static string SabertoothTigerAdvice = "Melee attacks.";

        // Scorpion
        public static string GiantScorpionSummary = "The giant scorpion is a large arachnid that attacks with claws and stinger, and has the ability to paralyze its foes.";
        public static string GiantScorpionAdvice = "If you lack immunity to paralysis, ranged attacks are safer, especially at lower levels. Giant scorpions are quickly dispatched at higher levels with melee attacks.";
        public static string GiantScorpionAbilities = "Paralyzing attacks";

        // Skeletal Warrior
        public static string SkeletalWarriorSummary = "While they may appear fragile, Skeletal Warriors are tough, skilled fighters. Lacking flesh, these creatures take less damage from edged weapons.";
        public static string SkeletalWarriorAdvice = "Skeletal Warriors are not much of a challenge for higher level characters. However, lower level characters should attack with blunt weapons to avoid the creature's damage reduction.";
        public static string SkeletalWarriorAbilities = "Can detect camouflaged or invisible foes|There is a 2% chance of contracting a disease from a skeletal warrior|Takes normal damage from blunt weapons, and half damage from all the others|Tales double damage from silver weapons";

        // Slaughterfish
        public static string SlaughterfishSummary = "A slaughterfish is a predatory, eel-like fish that dwells in underwater areas and attacks prey with its gaping maw.";
        public static string SlaughterfishAdvice = "Melee attacks.";

        // Spider
        public static string SpiderSummary = "The spider is a large arachnid that can paralyze its victims.";
        public static string SpiderAdvice = "If you lack immunity to paralysis, ranged attacks are preferable, especially at low levels. Spiders are quickly dispatched at higher levels with melee attacks.";
        public static string SpiderAbilities = "Paralyzing attacks";

        // Spriggan
        public static string SprigganSummary = "Spriggans are plant-like monsters.";
        public static string SprigganAdvice = "Melee attacks work best.";
        public static string SprigganLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Spriggan);
        public static string SprigganAbilities = "Due to their nature, spriggans are almost impossible to hit with arrows.";

        // Vampire
        public static string VampireSummary = "Cruel, cunning, and immortal predators, vampires hunt the night, sometimes singly, sometimes in packs. Their powers and strength are certainly the stuff of legends, as is their ability to pass the curse of undeath onto unwitting victims.";
        public static string VampireAdvice = "Spell Reflection, Spell Resistance, or Spell Absorption are recommended when fighting a vampire. Use ranged weapons if you want to avoid the chance of contracting a disease or vampirism. It is best to avoid direct melee attacks at lower levels.";
        public static string VampireMaterial = MaterialSilver;
        public static string VampireLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);
        public static string VampireAbilities = "Can detect camouflaged or invisible foes|There is a 2% chance per hit of contracting a disease from a vampire|There is a 0.6% chance per hit of being infected with vampirism|Immunity to Poison and Disease|Bonus to Hit Humanoids|Takes Damage from Sunlight and Holy Places";

        // Vampire Ancient
        public static string VampireAncientSummary = "The Vampire Ancient (sometimes styled as Ancient Vampire) is possibly the most dangerous foe one will encounter. Unlike Ancient Liches and Daedra Lords, they appear very frequently at higher levels and have no ranged spells - therefore, they can't blast themselves into Oblivion with their own attacks. The Vampire Ancient (sometimes styled Ancient Vampire) is possibly the most dangerous foe in the game. Unlike Ancient Liches and Daedra Lords, they appear very frequently at higher levels and have no ranged spells—therefore, they can't blast themselves into Oblivion with their own attacks.";
        public static string VampireAncientAdvice = "Spell Reflection is a life saver, but Shock or Spell Resistance are of great use. Having healing spells at the ready is also recommended, provided you have the chance to use them. Ranged weapons are almost useless, since Vampire Ancients are very fast and can be within melee range before you have reloaded your weapon. Spell Absorption is little help, unless you have a lot of empty Magicka which can be restored by the Ancient's Shock spells, and these attacks are quite powerful. Characters with a capacity of at most 50 Magicka will probably not be able to absorb even a single Shock spell before it causes damage. On the other hand, if you have a powerful Spell Reflection spell, the Vampire Ancient will kill itself easily with its own Shock attacks. And although they are immune to paralysis, they can be paralyzed if their own spells are reflected back at them.";
        public static string VampireAncientMaterial = MaterialMithril;
        public static string VampireAncientLanguage = DaggerfallUnity.Instance.TextProvider.GetSkillName(DFCareer.Skills.Etiquette);
        public static string VampireAncientAbilities = "Can detect camouflaged or invisible foes|There is a 2% chance per hit of contracting a disease from a Vampire Ancient|There is a 0.6% chance per hit of being infected with Vampirism|Immunity to Paralysis, Poison, and Disease|Adrenaline Rush|Regenerates Health in darkness|Takes Damage in Holy Places|Bonus to hit Humanoids";

        // Wereboar
        public static string WereboarSummary = "Wereboars are half-man, half-boar lycanthropes, whose attacks have a slight chance of passing their curse onto you.";
        public static string WereboarAdvice = "If you don't wish to become a wereboar yourself, ranged attacks are safer at lower levels. Higher levels can quickly dispatch a wereboar with melee attacks.";
        public static string WereboarMaterial = MaterialSilver;
        public static string WereboarAbilities = "There is a 0.6% chance of contracting lycanthropy from the attack|Adrenaline Rush";

        // Werewolf
        public static string WerewolfSummary = "Werewolves are half-man, half-wolf lycanthropes, whose attacks have a slight chance of passing their curse onto you.";
        public static string WerewolfAdvice = "If you don't wish to become a werewolf yourself, ranged attacks are safer at lower levels. Higher levels can quickly dispatch a werewolf with melee attacks.";
        public static string WerewolfMaterial = MaterialSilver;
        public static string WerewolfAbilities = "There is a 0.6% chance of contracting lycanthropy from the attack|Adrenaline Rush";

        // Wraith
        public static string WraithSummary = "Like ghosts, wraiths are the spirits of the dead, but are much more powerful than their counterparts - both in melee and with magic.";
        public static string WraithAdvice = "Wraiths can do massive melee damage so the use of ranged attacks is recommended, except at higher levels where they can be easily killed with melee attacks.";
        public static string WraithMaterial = MaterialSilver;
        public static string WraithAbilities = "Can detect camouflaged or invisible foes|Resistance to Magic";

        // Zombie
        public static string ZombieSummary = "Zombies are animate, rotting corpses. They can deliver a powerful blow, and their decaying flesh harbors festering diseases.";
        public static string ZombieAdvice = "At lower levels, use ranged weapons to avoid the zombie's attacks and chance of disease. At higher levels, zombies can be easily destroyed with melee attacks.";
        public static string ZombieAbilities = "There is a 2% chance per hit of contracting a disease from a zombie";
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
                Summary = GiantSummary,
                Advice = GiantAdvice,
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

        static void LoadText()
        {
            const string csvFilename = "BestiaryModData.csv";

            if (stringTable != null)
                return;

            stringTable = StringTableCSVParser.LoadDictionary(csvFilename);

            if (stringTable.ContainsKey("NoneLabel"))
                NoneLabel = stringTable["NoneLabel"];

            if (stringTable.ContainsKey("AbilitiesLabel"))
                AbilitiesLabel = stringTable["AbilitiesLabel"];

            if (stringTable.ContainsKey("AdviceLabel"))
                AdviceLabel = stringTable["AdviceLabel"];

            if (stringTable.ContainsKey("KillcountLabel"))
                KillcountLabel = stringTable["KillcountLabel"];

            if (stringTable.ContainsKey("LanguageLabel"))
                LanguageLabel = stringTable["LanguageLabel"];

            if (stringTable.ContainsKey("MaterialLabel"))
                MaterialLabel = stringTable["MaterialLabel"];

            if (stringTable.ContainsKey("OverviewLabel"))
                OverviewLabel = stringTable["OverviewLabel"];

            if (stringTable.ContainsKey("SpellsLabel"))
                SpellsLabel = stringTable["SpellsLabel"];

            if (stringTable.ContainsKey("SummaryLabel"))
                SummaryLabel = stringTable["SummaryLabel"];

            if (stringTable.ContainsKey("BestiaryTitle"))
                BestiaryTitle = stringTable["BestiaryTitle"];

            if (stringTable.ContainsKey("SummarySubTitle"))
                SummarySubTitle = stringTable["SummarySubTitle"];

            if (stringTable.ContainsKey("UnlockBestiaryToUseIt"))
                UnlockBestiaryToUseIt = stringTable["UnlockBestiaryToUseIt"];

            if (stringTable.ContainsKey("YouDontHaveBestiary"))
                YouDontHaveBestiary = stringTable["YouDontHaveBestiary"];

            if (stringTable.ContainsKey("YouHaveUnlockedTheBestiary"))
                YouHaveUnlockedTheBestiary = stringTable["YouHaveUnlockedTheBestiary"];

            if (stringTable.ContainsKey("AddedToTheBestiary"))
                AddedToTheBestiary = stringTable["AddedToTheBestiary"];

            if (stringTable.ContainsKey("NoMaterial"))
                NoMaterial = stringTable["NoMaterial"];

            if (stringTable.ContainsKey("MaterialMithril"))
                MaterialMithril = stringTable["MaterialMithril"];

            if (stringTable.ContainsKey("MaterialSilver"))
                MaterialSilver = stringTable["MaterialSilver"];

            if (stringTable.ContainsKey("MaterialDwarven"))
                MaterialDwarven = stringTable["MaterialDwarven"];

            if (stringTable.ContainsKey("MaterialSteel"))
                MaterialSteel = stringTable["MaterialSteel"];

            if (stringTable.ContainsKey("MaterialElven"))
                MaterialElven = stringTable["MaterialElven"];

            if (stringTable.ContainsKey("AnimalsTitle"))
                AnimalsTitle = stringTable["AnimalsTitle"];

            if (stringTable.ContainsKey("AnimalsOverview"))
                AnimalsOverview = stringTable["AnimalsOverview"];

            if (stringTable.ContainsKey("AtronachsTitle"))
                AtronachsTitle = stringTable["AtronachsTitle"];

            if (stringTable.ContainsKey("AtronachsOverview"))
                AtronachsOverview = stringTable["AtronachsOverview"];

            if (stringTable.ContainsKey("DaedraTitle"))
                DaedraTitle = stringTable["DaedraTitle"];

            if (stringTable.ContainsKey("DaedraOverview"))
                DaedraOverview = stringTable["DaedraOverview"];

            if (stringTable.ContainsKey("LycanthropesTitle"))
                LycanthropesTitle = stringTable["LycanthropesTitle"];

            if (stringTable.ContainsKey("LycanthropesOverview"))
                LycanthropesOverview = stringTable["LycanthropesOverview"];

            if (stringTable.ContainsKey("MonstersTitle"))
                MonstersTitle = stringTable["MonstersTitle"];

            if (stringTable.ContainsKey("MonstersOverview"))
                MonstersOverview = stringTable["MonstersOverview"];

            if (stringTable.ContainsKey("OrcsTitle"))
                OrcsTitle = stringTable["OrcsTitle"];

            if (stringTable.ContainsKey("OrcsOverview"))
                OrcsOverview = stringTable["OrcsOverview"];

            if (stringTable.ContainsKey("UndeadTitle"))
                UndeadTitle = stringTable["UndeadTitle"];

            if (stringTable.ContainsKey("UndeadOverview"))
                UndeadOverview = stringTable["UndeadOverview"];

            if (stringTable.ContainsKey("AncientLichSummary"))
                AncientLichSummary = stringTable["AncientLichSummary"];

            if (stringTable.ContainsKey("AncientLichAdvice"))
                AncientLichAdvice = stringTable["AncientLichAdvice"];

            if (stringTable.ContainsKey("CentaurSummary"))
                CentaurSummary = stringTable["CentaurSummary"];

            if (stringTable.ContainsKey("CentaurAdvice"))
                CentaurAdvice = stringTable["CentaurAdvice"];

            if (stringTable.ContainsKey("CentaurAbility"))
                CentaurAbility = stringTable["CentaurAbility"];

            if (stringTable.ContainsKey("DaedraLordSummary"))
                DaedraLordSummary = stringTable["DaedraLordSummary"];

            if (stringTable.ContainsKey("DaedraLordAdvice"))
                DaedraLordAdvice = stringTable["DaedraLordAdvice"];

            if (stringTable.ContainsKey("DaedraLordAbilities"))
                DaedraLordAbilities = stringTable["DaedraLordAbilities"];

            if (stringTable.ContainsKey("DaedraSeducerSummary"))
                DaedraSeducerSummary = stringTable["DaedraSeducerSummary"];

            if (stringTable.ContainsKey("DaedraSeducerAdvice"))
                DaedraSeducerAdvice = stringTable["DaedraSeducerAdvice"];

            if (stringTable.ContainsKey("DaedraSeducerAbilities"))
                DaedraSeducerAbilities = stringTable["DaedraSeducerAbilities"];

            if (stringTable.ContainsKey("DaedrothSummary"))
                DaedrothSummary = stringTable["DaedrothSummary"];

            if (stringTable.ContainsKey("DaedrothAdvice"))
                DaedrothAdvice = stringTable["DaedrothAdvice"];

            if (stringTable.ContainsKey("DaedrothAbilities"))
                DaedrothAbilities = stringTable["DaedrothAbilities"];

            if (stringTable.ContainsKey("DragonlingSummary"))
                DragonlingSummary = stringTable["DragonlingSummary"];

            if (stringTable.ContainsKey("DragonlingAdvice"))
                DragonlingAdvice = stringTable["DragonlingAdvice"];

            if (stringTable.ContainsKey("DragonlingAbilities"))
                DragonlingAbilities = stringTable["DragonlingAbilities"];

            if (stringTable.ContainsKey("DreughSummary"))
                DreughSummary = stringTable["DreughSummary"];

            if (stringTable.ContainsKey("DreughAdvice"))
                DreughAdvice = stringTable["DreughAdvice"];

            if (stringTable.ContainsKey("DreughAbilities"))
                DreughAbilities = stringTable["DreughAbilities"];

            if (stringTable.ContainsKey("FireAtronachSummary"))
                FireAtronachSummary = stringTable["FireAtronachSummary"];

            if (stringTable.ContainsKey("FireAtronachAdvice"))
                FireAtronachAdvice = stringTable["FireAtronachAdvice"];

            if (stringTable.ContainsKey("FireAtronachAbilities"))
                FireAtronachAbilities = stringTable["FireAtronachAbilities"];

            if (stringTable.ContainsKey("FireDaedraSummary"))
                FireDaedraSummary = stringTable["FireDaedraSummary"];

            if (stringTable.ContainsKey("FireDaedraAdvice"))
                FireDaedraAdvice = stringTable["FireDaedraAdvice"];

            if (stringTable.ContainsKey("FireDaedraAbilities"))
                FireDaedraAbilities = stringTable["FireDaedraAbilities"];

            if (stringTable.ContainsKey("FleshAtronachSummary"))
                FleshAtronachSummary = stringTable["FleshAtronachSummary"];

            if (stringTable.ContainsKey("FleshAtronachAdvice"))
                FleshAtronachAdvice = stringTable["FleshAtronachAdvice"];

            if (stringTable.ContainsKey("FleshAtronachAbilities"))
                FleshAtronachAbilities = stringTable["FleshAtronachAbilities"];

            if (stringTable.ContainsKey("GargoyleSummary"))
                GargoyleSummary = stringTable["GargoyleSummary"];

            if (stringTable.ContainsKey("GargoyleAdvice"))
                GargoyleAdvice = stringTable["GargoyleAdvice"];

            if (stringTable.ContainsKey("GargoyleAbilities"))
                GargoyleAbilities = stringTable["GargoyleAbilities"];

            if (stringTable.ContainsKey("GhostSummary"))
                GhostSummary = stringTable["GhostSummary"];

            if (stringTable.ContainsKey("GhostAdvice"))
                GhostAdvice = stringTable["GhostAdvice"];

            if (stringTable.ContainsKey("GiantSummary"))
                GiantSummary = stringTable["GiantSummary"];

            if (stringTable.ContainsKey("GiantAdvice"))
                GiantAdvice = stringTable["GiantAdvice"];

            if (stringTable.ContainsKey("GiantBatSummary"))
                GiantBatSummary = stringTable["GiantBatSummary"];

            if (stringTable.ContainsKey("GiantBatAdvice"))
                GiantBatAdvice = stringTable["GiantBatAdvice"];

            if (stringTable.ContainsKey("GiantBatAbilities"))
                GiantBatAbilities = stringTable["GiantBatAbilities"];

            if (stringTable.ContainsKey("GrizzlyBearSummary"))
                GrizzlyBearSummary = stringTable["GrizzlyBearSummary"];

            if (stringTable.ContainsKey("GrizzlyBearAdvice"))
                GrizzlyBearAdvice = stringTable["GrizzlyBearAdvice"];

            if (stringTable.ContainsKey("HarpySummary"))
                HarpySummary = stringTable["HarpySummary"];

            if (stringTable.ContainsKey("HarpyAdvice"))
                HarpyAdvice = stringTable["HarpyAdvice"];

            if (stringTable.ContainsKey("IceAtronachSummary"))
                IceAtronachSummary = stringTable["IceAtronachSummary"];

            if (stringTable.ContainsKey("IceAtronachAdvice"))
                IceAtronachAdvice = stringTable["IceAtronachAdvice"];

            if (stringTable.ContainsKey("IceAtronachAbilities"))
                IceAtronachAbilities = stringTable["IceAtronachAbilities"];

            if (stringTable.ContainsKey("FrostDaedraSummary"))
                FrostDaedraSummary = stringTable["FrostDaedraSummary"];

            if (stringTable.ContainsKey("FrostDaedraAdvice"))
                FrostDaedraAdvice = stringTable["FrostDaedraAdvice"];

            if (stringTable.ContainsKey("FrostDaedraAbilities"))
                FrostDaedraAbilities = stringTable["FrostDaedraAbilities"];

            if (stringTable.ContainsKey("ImpSummary"))
                ImpSummary = stringTable["ImpSummary"];

            if (stringTable.ContainsKey("ImpAdvice"))
                ImpAdvice = stringTable["ImpAdvice"];

            if (stringTable.ContainsKey("ImpAbilities"))
                ImpAbilities = stringTable["ImpAbilities"];

            if (stringTable.ContainsKey("IronAtronachSummary"))
                IronAtronachSummary = stringTable["IronAtronachSummary"];

            if (stringTable.ContainsKey("IronAtronachAdvice"))
                IronAtronachAdvice = stringTable["IronAtronachAdvice"];

            if (stringTable.ContainsKey("IronAtronachAbilities"))
                IronAtronachAbilities = stringTable["IronAtronachAbilities"];

            if (stringTable.ContainsKey("LamiaSummary"))
                LamiaSummary = stringTable["LamiaSummary"];

            if (stringTable.ContainsKey("LamiaAdvice"))
                LamiaAdvice = stringTable["LamiaAdvice"];

            if (stringTable.ContainsKey("LamiaAbilities"))
                LamiaAbilities = stringTable["LamiaAbilities"];

            if (stringTable.ContainsKey("LichSummary"))
                LichSummary = stringTable["LichSummary"];

            if (stringTable.ContainsKey("LichAdvice"))
                LichAdvice = stringTable["LichAdvice"];

            if (stringTable.ContainsKey("LichAbilities"))
                LichAbilities = stringTable["LichAbilities"];

            if (stringTable.ContainsKey("MummySummary"))
                MummySummary = stringTable["MummySummary"];

            if (stringTable.ContainsKey("MummyAdvice"))
                MummyAdvice = stringTable["MummyAdvice"];

            if (stringTable.ContainsKey("MummyAbilities"))
                MummyAbilities = stringTable["MummyAbilities"];

            if (stringTable.ContainsKey("NymphSummary"))
                NymphSummary = stringTable["NymphSummary"];

            if (stringTable.ContainsKey("NymphAdvice"))
                NymphAdvice = stringTable["NymphAdvice"];

            if (stringTable.ContainsKey("NymphAbilities"))
                NymphAbilities = stringTable["NymphAbilities"];

            if (stringTable.ContainsKey("OrcSummary"))
                OrcSummary = stringTable["OrcSummary"];

            if (stringTable.ContainsKey("OrcAdvice"))
                OrcAdvice = stringTable["OrcAdvice"];

            if (stringTable.ContainsKey("OrcAbilities"))
                OrcAbilities = stringTable["OrcAbilities"];

            if (stringTable.ContainsKey("OrcSergeantSummary"))
                OrcSergeantSummary = stringTable["OrcSergeantSummary"];

            if (stringTable.ContainsKey("OrcSergeantAdvice"))
                OrcSergeantAdvice = stringTable["OrcSergeantAdvice"];

            if (stringTable.ContainsKey("OrcShamanSummary"))
                OrcShamanSummary = stringTable["OrcShamanSummary"];

            if (stringTable.ContainsKey("OrcShamanAdvice"))
                OrcShamanAdvice = stringTable["OrcShamanAdvice"];

            if (stringTable.ContainsKey("OrcWarlordSummary"))
                OrcWarlordSummary = stringTable["OrcWarlordSummary"];

            if (stringTable.ContainsKey("OrcWarlordAdvice"))
                OrcWarlordAdvice = stringTable["OrcWarlordAdvice"];

            if (stringTable.ContainsKey("RatSummary"))
                RatSummary = stringTable["RatSummary"];

            if (stringTable.ContainsKey("RatAdvice"))
                RatAdvice = stringTable["RatAdvice"];

            if (stringTable.ContainsKey("RatAbilities"))
                RatAbilities = stringTable["RatAbilities"];

            if (stringTable.ContainsKey("SabertoothTigerSummary"))
                SabertoothTigerSummary = stringTable["SabertoothTigerSummary"];

            if (stringTable.ContainsKey("SabertoothTigerAdvice"))
                SabertoothTigerAdvice = stringTable["SabertoothTigerAdvice"];

            if (stringTable.ContainsKey("GiantScorpionSummary"))
                GiantScorpionSummary = stringTable["GiantScorpionSummary"];

            if (stringTable.ContainsKey("GiantScorpionAdvice"))
                GiantScorpionAdvice = stringTable["GiantScorpionAdvice"];

            if (stringTable.ContainsKey("GiantScorpionAbilities"))
                GiantScorpionAbilities = stringTable["GiantScorpionAbilities"];

            if (stringTable.ContainsKey("SkeletalWarriorSummary"))
                SkeletalWarriorSummary = stringTable["SkeletalWarriorSummary"];

            if (stringTable.ContainsKey("SkeletalWarriorAdvice"))
                SkeletalWarriorAdvice = stringTable["SkeletalWarriorAdvice"];

            if (stringTable.ContainsKey("SkeletalWarriorAbilities"))
                SkeletalWarriorAbilities = stringTable["SkeletalWarriorAbilities"];

            if (stringTable.ContainsKey("SlaughterfishSummary"))
                SlaughterfishSummary = stringTable["SlaughterfishSummary"];

            if (stringTable.ContainsKey("SlaughterfishAdvice"))
                SlaughterfishAdvice = stringTable["SlaughterfishAdvice"];

            if (stringTable.ContainsKey("SpiderSummary"))
                SpiderSummary = stringTable["SpiderSummary"];

            if (stringTable.ContainsKey("SpiderAdvice"))
                SpiderAdvice = stringTable["SpiderAdvice"];

            if (stringTable.ContainsKey("SpiderAbilities"))
                SpiderAbilities = stringTable["SpiderAbilities"];

            if (stringTable.ContainsKey("SprigganSummary"))
                SprigganSummary = stringTable["SprigganSummary"];

            if (stringTable.ContainsKey("SprigganAdvice"))
                SprigganAdvice = stringTable["SprigganAdvice"];

            if (stringTable.ContainsKey("SprigganAbilities"))
                SprigganAbilities = stringTable["SprigganAbilities"];

            if (stringTable.ContainsKey("VampireSummary"))
                VampireSummary = stringTable["VampireSummary"];

            if (stringTable.ContainsKey("VampireAdvice"))
                VampireAdvice = stringTable["VampireAdvice"];

            if (stringTable.ContainsKey("VampireAbilities"))
                VampireAbilities = stringTable["VampireAbilities"];

            if (stringTable.ContainsKey("VampireAncientSummary"))
                VampireAncientSummary = stringTable["VampireAncientSummary"];

            if (stringTable.ContainsKey("VampireAncientAdvice"))
                VampireAncientAdvice = stringTable["VampireAncientAdvice"];

            if (stringTable.ContainsKey("VampireAncientAbilities"))
                VampireAncientAbilities = stringTable["VampireAncientAbilities"];

            if (stringTable.ContainsKey("WereboarSummary"))
                WereboarSummary = stringTable["WereboarSummary"];

            if (stringTable.ContainsKey("WereboarAdvice"))
                WereboarAdvice = stringTable["WereboarAdvice"];

            if (stringTable.ContainsKey("WereboarAbilities"))
                WereboarAbilities = stringTable["WereboarAbilities"];

            if (stringTable.ContainsKey("WerewolfSummary"))
                WerewolfSummary = stringTable["WerewolfSummary"];

            if (stringTable.ContainsKey("WerewolfAdvice"))
                WerewolfAdvice = stringTable["WerewolfAdvice"];

            if (stringTable.ContainsKey("WerewolfAbilities"))
                WerewolfAbilities = stringTable["WerewolfAbilities"];

            if (stringTable.ContainsKey("WraithSummary"))
                WraithSummary = stringTable["WraithSummary"];

            if (stringTable.ContainsKey("WraithAdvice"))
                WraithAdvice = stringTable["WraithAdvice"];

            if (stringTable.ContainsKey("WraithAbilities"))
                WraithAbilities = stringTable["WraithAbilities"];

            if (stringTable.ContainsKey("ZombieSummary"))
                ZombieSummary = stringTable["ZombieSummary"];

            if (stringTable.ContainsKey("ZombieAdvice"))
                ZombieAdvice = stringTable["ZombieAdvice"];

            if (stringTable.ContainsKey("ZombieAbilities"))
                ZombieAbilities = stringTable["ZombieAbilities"];
        }
    }
}
