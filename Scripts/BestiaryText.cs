using System.Collections.Generic;

using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Utility;

using UnityEngine;

// ReSharper disable CheckNamespace
namespace BestiaryMod
{
    public class AllTextClass
    {
        public AllTextClass(List<string> pagesToLoad)
        {
            BestiaryTitle = BestiaryTextDB.BestiaryTitle;
            Pages = new List<Page>();

            foreach (var item in pagesToLoad)
            {
                var pageTemp = BestiaryTextDB.GetPage(item);
                if (pageTemp != null && pageTemp.Entries.Count > 0)
                    Pages.Add(pageTemp);
            }
        }
        public void DebugThis()
        {
            Debug.Log($"Debugging AllText {BestiaryTitle}");

            Debug.Log("Pages:");
            foreach (var item in Pages)
                item.DebugThis();
        }
        public string BestiaryTitle { get; }
        public List<Page> Pages { get; }
    }

    public class Page
    {
        public Page(string title, string summary, params MonsterCareers[] monsters)
        {
            Title = title;
            PageSummary = new Summary(title, summary, monsters);
            Entries = new List<Entry>();

            foreach (MonsterCareers monster in monsters)
            {
                switch (BestiaryMain.SettingEntries)
                {
                    case 1:
                        string mName = TextManager.Instance.GetLocalizedEnemyName((int)monster);
                        if (BestiaryMain.killCounts.ContainsKey(mName))
                        {
                            Entries.Add(BestiaryTextDB.GetEntryByMonster(monster));
                        }
                        break;
                    default:
                        Entries.Add(BestiaryTextDB.GetEntryByMonster(monster));
                        break;
                }
            }
        }
        public Page(string assetPath)
        {
            Name = assetPath;
        }

        public void DebugThis()
        {
            Debug.Log($"Debugging Page {Name}");
            Debug.Log($"Title: {Title}");
            PageSummary?.DebugThis();

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
        private string title;
        private int[] spells;
        private string[] abilities;
        private string language;
        private string material;
        private string advice;
        private string summary;

        public Entry(MonsterCareers monster)
        {
            Id = (int)monster;
            Title = TextManager.Instance.GetLocalizedEnemyName((int)monster);
            TextureArchive = EnemyBasics.Enemies[(int)monster].MaleTexture;
        }

        public void DebugThis()
        {
            Debug.Log($"ButtonTextureName: {ButtonTextureName}, Title: {Title}, TextureArchive: {TextureArchive}");
        }

        public string ButtonTextureName { get; set; }
        public int TextureArchive { get; set; }
        public int Id { get; }
        public string Title
        {
            get
            {

                if (BestiaryTextDB.OverrideTitleTable.ContainsKey(Id))
                    return BestiaryTextDB.OverrideTitleTable[Id];

                return title;
            }
            set
            {
                title = value;
            }
        }
        public string Summary
        {
            get
            {
                if (BestiaryTextDB.OverrideSummaryTable.ContainsKey(Id))
                    return BestiaryTextDB.OverrideSummaryTable[Id];

                return summary;
            }
            set
            {
                summary = value;
            }
        }
        public string Advice
        {
            get
            {
                if (BestiaryTextDB.OverrideAdviceTable.ContainsKey(Id))
                    return BestiaryTextDB.OverrideAdviceTable[Id];

                return advice;
            }
            set
            {
                advice = value;
            }
        }
        public string Material
        {
            get
            {
                if (BestiaryTextDB.OverrideMaterialTable.ContainsKey(Id))
                    return BestiaryTextDB.OverrideMaterialTable[Id];

                return material;
            }
            set
            {
                material = value;
            }
        }
        public string Language
        {
            get
            {
                if (BestiaryTextDB.OverrideLanguageTable.ContainsKey(Id))
                    return BestiaryTextDB.OverrideLanguageTable[Id];

                return language;
            }
            set
            {
                language = value;
            }
        }
        public string[] Abilities
        {
            get
            {
                if (BestiaryTextDB.OverrideAbilitiesTable.ContainsKey(Id))
                    return BestiaryTextDB.OverrideAbilitiesTable[Id];

                return abilities;
            }
            set
            {
                abilities = value;
            }
        }
        public int[] Spells
        {
            get
            {
                if (BestiaryTextDB.OverrideSpellsIdsTable.ContainsKey(Id))
                    return BestiaryTextDB.OverrideSpellsIdsTable[Id];

                return spells;
            }
            set
            {
                spells = value;
            }
        }
        public List<TextPair> Text
        {
            get
            {
                string[] AlternativeSpells;
                List<TextPair> textPairs = new List<TextPair>();
                if (Summary != null)
                    textPairs.Add(new TextPair(BestiaryTextDB.SummaryLabel, Summary));
                else
                    textPairs.Add(new TextPair(BestiaryTextDB.SummaryLabel, BestiaryTextDB.NoneLabel));

                if (Advice != null)
                    textPairs.Add(new TextPair(BestiaryTextDB.AdviceLabel, Advice));
                else
                    textPairs.Add(new TextPair(BestiaryTextDB.AdviceLabel, BestiaryTextDB.NoneLabel));

                if (Material != null)
                    textPairs.Add(new TextPair(BestiaryTextDB.MaterialLabel, Material));
                else
                    textPairs.Add(new TextPair(BestiaryTextDB.MaterialLabel, BestiaryTextDB.NoneLabel));

                if (Language != null)
                    textPairs.Add(new TextPair(BestiaryTextDB.LanguageLabel, Language));
                else
                    textPairs.Add(new TextPair(BestiaryTextDB.LanguageLabel, BestiaryTextDB.NoneLabel));

                if (Abilities != null)
                    for (int i = 0; i < Abilities.Length; i++)
                    {
                        if (i == 0)
                        {
                            textPairs.Add(new TextPair(BestiaryTextDB.AbilitiesLabel, Abilities[i]));
                        }
                        else
                        {
                            textPairs.Add(new TextPair(string.Empty, Abilities[i]));
                        }
                    }
                else
                    textPairs.Add(new TextPair(BestiaryTextDB.AbilitiesLabel, BestiaryTextDB.NoneLabel));

                if (BestiaryTextDB.OverrideSpellsTable.ContainsKey(Id))
                {
                    AlternativeSpells = BestiaryTextDB.OverrideSpellsTable[Id];

                    for (int i = 0; i < AlternativeSpells.Length; i++)
                    {
                        if (i == 0)
                        {
                            textPairs.Add(new TextPair(BestiaryTextDB.SpellsLabel, AlternativeSpells[i]));
                        }
                        else
                        {
                            textPairs.Add(new TextPair(string.Empty, AlternativeSpells[i]));
                        }
                    }
                }
                else if (Spells != null)
                {
                    for (int i = 0; i < Spells.Length; i++)
                    {
                        if (i == 0)
                        {
                            textPairs.Add(new TextPair(BestiaryTextDB.SpellsLabel, TextManager.Instance.GetLocalizedSpellName(Spells[i])));
                        }
                        else
                        {
                            textPairs.Add(new TextPair(string.Empty, TextManager.Instance.GetLocalizedSpellName(Spells[i])));
                        }
                    }
                }
                else
                {
                    textPairs.Add(new TextPair(BestiaryTextDB.SpellsLabel, BestiaryTextDB.NoneLabel));
                }

                return textPairs;
            }
        }
    }

    public class Summary
    {
        public Summary(string title, string summary, MonsterCareers[] monsters)
        {
            Title = BestiaryTextDB.SummarySubTitle + " - " + title;

            Text = new List<TextPair>
            {
                new TextPair(BestiaryTextDB.OverviewLabel, summary),
                new TextPair(string.Empty, string.Empty),
            };

            for (int i = 0; i < monsters.Length; i++)
            {
                uint kills;
                string mName = TextManager.Instance.GetLocalizedEnemyName((int)monsters[i]);

                BestiaryMain.killCounts.TryGetValue(mName, out kills);
                string text = mName + " - " + kills;

                if (i == 0)
                    Text.Add(new TextPair(BestiaryTextDB.KillcountLabel, text));
                else
                    Text.Add(new TextPair(string.Empty, text));
            }
        }
        public Summary(string assetPath)
        {
            Name = assetPath;
        }
        public void DebugThis()
        {
            Debug.Log($"Debugging Summary {Name}");
            Debug.Log($"Title: {Title}, TextureArchive: {TextureArchive}");
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
            Debug.Log($"LeftText: {LeftText}, RightText: {RightText}");
        }

        public string LeftText { get; set; }
        public string RightText { get; set; }
    }
}
