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
            Pages = new List<Page>();

            foreach (var item in pagesToLoad)
            {
                Page pageTemp = new Page(item);
                if (pageTemp.Entries.Count > 0)
                    Pages.Add(pageTemp);
            }
        }
        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging AllText {0}", BestiaryTitle));

            Debug.Log("Pages:");
            foreach (var item in Pages)
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
        public List<Page> Pages { get; }
    }
    public class Page
    {
        public Page(string assetPath)
        {
            Name = assetPath;
            Title = "Page constructor error";
            PageSummary = null;
            Entries = new List<Entry>();

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(Name).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        Title = rawText[i];
                        break;
                    case 1:
                        if (BestiaryMain.SettingEntries != 2)
                            PageSummary = new Summary(rawText[i]);
                        break;
                    default:
                        switch (BestiaryMain.SettingEntries)
                        {
                            case 1:
                                if (BestiaryMain.killCounts.ContainsKey(rawText[i]))
                                    Entries.Add(new Entry(rawText[i]));
                                break;
                            default:
                                Entries.Add(new Entry(rawText[i]));
                                break;
                        }
                        break;
                }
            }
        }

        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging Page {0}", Name));
            Debug.Log(String.Format("Title: {0}", Title));
            if (PageSummary != null)
                PageSummary.DebugThis();

            Debug.Log("Entries:");
            foreach (var item in Entries)
                item.DebugThis();
        }

        public string Name { get; }
        public string Title { get; }
        public Summary PageSummary { get; set; }
        public List<Entry> Entries { get; }
    }
    public class Entry
    {
        public Entry(string assetPath)
        {
            if (ModManager.Instance.GetMod("Unleveled Spells") != null && ModManager.Instance.GetMod("Bestiary").HasAsset(assetPath + "-kabs_unleveled_spells"))
                assetPath = assetPath + "-kabs_unleveled_spells";

            Name = assetPath;
            ButtonTextureName = "Entry constructor error";
            Title = "Entry constructor error";
            TextureArchive = 0;
            Text = new List<TextPair>();

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(Name).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        TextureArchive = int.Parse(rawText[i]);
                        break;
                    case 1:
                        ButtonTextureName = rawText[i];
                        break;
                    case 2:
                        Title = rawText[i];
                        break;
                    default:
                        if (rawText[i].Length > 2 && rawText[i].Contains("*"))
                        {
                            var splitResult = rawText[i].Split('*');
                            Text.Add(new TextPair(splitResult[0], splitResult[1]));
                        }
                        break;
                }
            }
        }

        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging Entry {0}", Name));
            Debug.Log(String.Format("ButtonTextureName: {0}, Title: {1}, TextureArchive: {2}", ButtonTextureName, Title, TextureArchive));

            // Debug.Log("Text:");
            // foreach (var item in Text)
            //     item.DebugThis();
        }

        public string Name { get; }
        public string ButtonTextureName { get; }
        public string Title { get; }
        public int TextureArchive { get; }
        public List<TextPair> Text { get; }
    }
    public class Summary
    {
        public Summary(string assetPath)
        {
            Name = assetPath;
            Title = "Summary constructor error";
            Text = new List<TextPair>();

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(Name).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            bool foundKillcountStart = false;
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        if (DaggerfallUnity.Settings.SDFFontRendering)
                            Title = rawText[i];
                        else
                            Title = rawText[i].Replace(" - ", " "); ;
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
                                    Text.Add(new TextPair(splitResult[0], splitResult[1] + BestiaryMain.killCounts[temp]));
                                else
                                    Text.Add(new TextPair(splitResult[0], splitResult[1] + "0"));
                            }
                            else
                                Text.Add(new TextPair(splitResult[0], splitResult[1]));
                        }
                        break;
                }
            }
        }
        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging Summary {0}", Name));
            Debug.Log(String.Format("Title: {0}, TextureArchive: {1}", Title, TextureArchive));

            Debug.Log("Text:");
            foreach (var item in Text)
                item.DebugThis();
        }
        public string Name { get; }
        public string Title { get; }
        public int TextureArchive { get; }
        public List<TextPair> Text { get; }
    }
    public struct TextPair
    {
        public TextPair(string leftText, string rightText)
        {
            LeftText = leftText;
            RightText = rightText;
        }
        public void DebugThis()
        {
            Debug.Log(String.Format("LeftText: {0}, RightText: {1}", LeftText, RightText));
        }

        public string LeftText { get; set; }
        public string RightText { get; set; }
    }
}
