using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Utility.ModSupport;

using UnityEngine;

namespace BestiaryMod
{
    public class AllTextClass
    {
        public AllTextClass(List<string> pagesToLoad)
        {
            BestiaryTitle = "Bestiary";
            AllPages = new List<Page>();

            foreach (var item in pagesToLoad)
            {
                Page pageTemp = new Page(item);
                if (pageTemp.PageEntries.Count > 0)
                    AllPages.Add(pageTemp);
            }
        }
        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging AllText {0}", BestiaryTitle));

            Debug.Log("AllPages:");
            foreach (var item in AllPages)
                item.DebugThis();
        }
        public static string EntryTitleToEntryName(string input)
        {
            switch (input)
            {
                case "Giant Bat":
                    return "entry_giant_bat";
                case "Giant Rat":
                    return "entry_rat";
                case "Grizzly Bear":
                    return "entry_grizzly_bear";
                case "Sabertooth Tiger":
                    return "entry_sabertooth_tiger";
                case "Scorpion":
                    return "entry_scorpion";
                case "Slaughterfish":
                    return "entry_slaughterfish";
                case "Spider":
                    return "entry_spider";
                // --------------------------
                case "Centaur":
                    return "entry_centaur";
                case "Dragonling":
                    return "entry_dragonling";
                case "Dreugh":
                    return "entry_dreugh";
                case "Gargoyle":
                    return "entry_gargoyle";
                case "Giant":
                    return "entry_giant";
                // --------------------------
                case "Harpy":
                    return "entry_harpy";
                case "Imp":
                    return "entry_imp";
                case "Lamia":
                    return "entry_lamia";
                case "Nymph":
                    return "entry_nymph";
                case "Spriggan":
                    return "entry_spriggan";
                // --------------------------
                case "Daedra Lord":
                    return "entry_daedra_lord";
                case "Daedra Seducer":
                    return "entry_daedra_seducer";
                case "Daedroth":
                    return "entry_daedroth";
                case "Fire Daedra":
                    return "entry_fire_daedra";
                case "Ice Daedra":
                    return "entry_ice_daedra";
                // --------------------------
                case "Fire Atronach":
                    return "entry_fire_atronach";
                case "Flesh Atronach":
                    return "entry_flesh_atronach";
                case "Ice Atronach":
                    return "entry_ice_atronach";
                case "Iron Atronach":
                    return "entry_iron_atronach";
                // --------------------------
                case "Wereboar":
                    return "entry_wereboar";
                case "Werewolf":
                    return "entry_werewolf";
                // --------------------------
                case "Orc":
                    return "entry_orc";
                case "Orc Sergeant":
                    return "entry_orc_sergeant";
                case "Orc Shaman":
                    return "entry_orc_shaman";
                case "Orc Warlord":
                    return "entry_orc_warlord";
                // --------------------------
                case "Ghost":
                    return "entry_ghost";
                case "Lich":
                    return "entry_lich";
                case "Ancient Lich":
                    return "entry_ancient_lich";
                case "Mummy":
                    return "entry_mummy";
                case "Skeletal Warrior":
                    return "entry_skeletal_warrior";
                case "Vampire":
                    return "entry_vampire";
                case "Vampire Ancient":
                    return "entry_vampire_ancient";
                case "Wraith":
                    return "entry_wraith";
                case "Zombie":
                    return "entry_zombie";
                // --------------------------
                default:
                    return "false";
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
        public string BestiaryTitle { get; }
        public List<Page> AllPages { get; }
    }
    public class Page
    {
        public Page(string assetPath)
        {
            PageName = assetPath;
            PageTitle = "Page constructor error";
            PageSummary = null;
            PageEntries = new List<Entry>();

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(PageName).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        PageTitle = rawText[i];
                        break;
                    case 1:
                        if (BestiaryMain.Entries != 2)
                            PageSummary = new Summary(rawText[i]);
                        break;
                    default:
                        switch (BestiaryMain.Entries)
                        {
                            case 1:
                                if (BestiaryMain.killCounts.ContainsKey(rawText[i]))
                                    PageEntries.Add(new Entry(rawText[i]));
                                break;
                            default:
                                PageEntries.Add(new Entry(rawText[i]));
                                break;
                        }
                        break;
                }
            }
        }

        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging Page {0}", PageName));
            Debug.Log(String.Format("PageTitle: {0}", PageTitle));
            if (PageSummary != null)
                PageSummary.DebugThis();

            Debug.Log("PageEntries:");
            foreach (var item in PageEntries)
                item.DebugThis();
        }

        public string PageName { get; }
        public string PageTitle { get; }
        public Summary PageSummary { get; set; }
        public List<Entry> PageEntries { get; }
    }
    public class Entry
    {
        public Entry(string assetPath)
        {
            if (ModManager.Instance.GetMod("Unleveled Spells") != null && ModManager.Instance.GetMod("Bestiary").HasAsset(assetPath + "-kabs_unleveled_spells"))
                assetPath = assetPath + "-kabs_unleveled_spells";

            EntryName = assetPath;
            EntryButtonName = "Entry constructor error";
            EntryTitle = "Entry constructor error";
            TextureArchive = 0;
            EntryText = new List<TextPair>();

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(EntryName).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        TextureArchive = int.Parse(rawText[i]);
                        break;
                    case 1:
                        EntryButtonName = rawText[i];
                        break;
                    case 2:
                        EntryTitle = rawText[i];
                        break;
                    default:
                        if (rawText[i].Length > 2 && rawText[i].Contains("*"))
                        {
                            var splitResult = rawText[i].Split('*');
                            EntryText.Add(new TextPair(splitResult[0], splitResult[1]));
                        }
                        break;
                }
            }
        }

        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging Entry {0}", EntryName));
            Debug.Log(String.Format("EntryButtonName: {0}, EntryTitle: {1}, TextureArchive: {2}", EntryButtonName, EntryTitle, TextureArchive));

            // Debug.Log("EntryText:");
            // foreach (var item in EntryText)
            //     item.DebugThis();
        }

        public string EntryName { get; }
        public string EntryButtonName { get; }
        public string EntryTitle { get; }
        public int TextureArchive { get; }
        public List<TextPair> EntryText { get; }
    }
    public class Summary
    {
        public Summary(string assetPath)
        {
            SummaryName = assetPath;
            SummaryTitle = "Summary constructor error";
            SummaryText = new List<TextPair>();
            TextureArchive = 0;

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(SummaryName).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            bool foundKillcountStart = false;
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        TextureArchive = int.Parse(rawText[i]);
                        break;
                    case 1:
                        if (DaggerfallUnity.Settings.SDFFontRendering)
                            SummaryTitle = rawText[i];
                        else
                            SummaryTitle = rawText[i].Replace(" - ", " "); ;
                        break;
                    default:
                        if (rawText[i].Length > 2 && rawText[i].Contains("*"))
                        {
                            var splitResult = rawText[i].Split('*');
                            if (!foundKillcountStart && splitResult[0] == "Killcount:")
                                foundKillcountStart = true;

                            if (foundKillcountStart && splitResult[1].Length > 3)
                            {
                                string temp = AllTextClass.EntryTitleToEntryName(splitResult[1].Remove(splitResult[1].Length - 3));

                                if (BestiaryMain.killCounts.ContainsKey(temp))
                                    SummaryText.Add(new TextPair(splitResult[0], splitResult[1] + BestiaryMain.killCounts[temp]));
                                else
                                    SummaryText.Add(new TextPair(splitResult[0], splitResult[1] + "0"));
                            }
                            else
                                SummaryText.Add(new TextPair(splitResult[0], splitResult[1]));
                        }
                        break;
                }
            }
        }
        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging Summary {0}", SummaryName));
            Debug.Log(String.Format("SummaryTitle: {0}, TextureArchive: {1}", SummaryTitle, TextureArchive));

            Debug.Log("SummaryText:");
            foreach (var item in SummaryText)
                item.DebugThis();
        }
        public string SummaryName { get; }
        public string SummaryTitle { get; }
        public int TextureArchive { get; }
        public List<TextPair> SummaryText { get; }
    }
    public struct TextPair
    {
        public TextPair(string titleText, string bodyText)
        {
            TitleText = titleText;
            BodyText = bodyText;
        }
        public void DebugThis()
        {
            Debug.Log(String.Format("TitleText: {0}, BodyText: {1}", TitleText, BodyText));
        }

        public string TitleText { get; set; }
        public string BodyText { get; set; }
    }
}
