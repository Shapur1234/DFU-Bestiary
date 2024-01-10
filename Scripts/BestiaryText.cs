using System.Collections.Generic;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Utility;

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
                {
                    Pages.Add(pageTemp);
                }
            }
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

        public string ButtonTextureName { get; set; }
        public int TextureArchive { get; set; }
        public int Id { get; }
        public string Title
        {
            get
            {

                if (BestiaryTextDB.OverrideTitleTable.ContainsKey(Id))
                {
                    return BestiaryTextDB.OverrideTitleTable[Id];
                }

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
                {
                    return BestiaryTextDB.OverrideSummaryTable[Id];
                }

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
                {
                    return BestiaryTextDB.OverrideAdviceTable[Id];
                }

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
                {
                    return BestiaryTextDB.OverrideMaterialTable[Id];
                }

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
                {
                    return BestiaryTextDB.OverrideLanguageTable[Id];
                }

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
                {
                    return BestiaryTextDB.OverrideAbilitiesTable[Id];
                }

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
                {
                    return BestiaryTextDB.OverrideSpellsIdsTable[Id];
                }

                return spells;
            }
            set
            {
                spells = value;
            }
        }

        public List<TextLabel> TextLabels
        {
            get
            {
                string[] AlternativeSpells;
                var result = new List<TextLabel>
                {
                    BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.SummaryLabel)
                };

                if (Summary != null)
                {
                    result.Add(BestiaryTextHelpers.CreateText(Summary));
                }
                else
                {
                    result.Add(BestiaryTextHelpers.CreateText(BestiaryTextDB.NoneLabel));
                }

                result.Add(BestiaryTextHelpers.CreateText(string.Empty));
                result.Add(BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.AdviceLabel));

                if (Advice != null)
                {
                    result.Add(BestiaryTextHelpers.CreateText(Advice));
                }
                else
                {
                    result.Add(BestiaryTextHelpers.CreateText(BestiaryTextDB.NoneLabel));
                }

                result.Add(BestiaryTextHelpers.CreateText(string.Empty));
                result.Add(BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.MaterialLabel));

                if (Material != null)
                {
                    result.Add(BestiaryTextHelpers.CreateText(Material));
                }
                else
                {
                    result.Add(BestiaryTextHelpers.CreateText(BestiaryTextDB.NoneLabel));
                }

                result.Add(BestiaryTextHelpers.CreateText(string.Empty));
                result.Add(BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.LanguageLabel));

                if (Language != null)
                {
                    result.Add(BestiaryTextHelpers.CreateText(Language));
                }
                else
                {
                    result.Add(BestiaryTextHelpers.CreateText(BestiaryTextDB.NoneLabel));
                }

                result.Add(BestiaryTextHelpers.CreateText(string.Empty));
                result.Add(BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.AbilitiesLabel));

                if (Abilities != null)
                {
                    for (int i = 0; i < Abilities.Length; i++)
                    {
                        result.Add(BestiaryTextHelpers.CreateText($"- {Abilities[i]}"));
                    }
                }
                else
                {
                    result.Add(BestiaryTextHelpers.CreateText(BestiaryTextDB.NoneLabel));
                }

                result.Add(BestiaryTextHelpers.CreateText(string.Empty));
                result.Add(BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.SpellsLabel));

                if (BestiaryTextDB.OverrideSpellsTable.ContainsKey(Id))
                {
                    AlternativeSpells = BestiaryTextDB.OverrideSpellsTable[Id];

                    for (int i = 0; i < AlternativeSpells.Length; i++)
                    {
                        result.Add(BestiaryTextHelpers.CreateText($"- {AlternativeSpells[i]}"));
                    }
                }
                else if (Spells != null)
                {
                    for (int i = 0; i < Spells.Length; i++)
                    {
                        result.Add(BestiaryTextHelpers.CreateText($"- {TextManager.Instance.GetLocalizedSpellName(Spells[i])}"));
                    }
                }
                else
                {
                    result.Add(BestiaryTextHelpers.CreateText(BestiaryTextDB.NoneLabel));
                }

                return result;
            }
        }
    }

    public class Summary
    {
        private readonly string summary;
        private readonly MonsterCareers[] monsters;

        public Summary(string title, string summary, MonsterCareers[] monsters)
        {
            Title = BestiaryTextDB.SummarySubTitle + (DaggerfallUnity.Settings.SDFFontRendering ? " - " : " ") + title;
            this.summary = summary;
            this.monsters = monsters;
        }
        public string Title { get; }
        public List<TextLabel> TextLabels
        {
            get
            {
                var result = new List<TextLabel>
                {
                    BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.OverviewLabel),
                    BestiaryTextHelpers.CreateText(summary),
                    BestiaryTextHelpers.CreateText(string.Empty)
                };

                if (monsters.Length > 0)
                {
                    result.Add(BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.KillcountLabel));
                }

                for (int i = 0; i < monsters.Length; i++)
                {
                    string mName = TextManager.Instance.GetLocalizedEnemyName((int)monsters[i]);

                    uint kills;
                    BestiaryMain.killCounts.TryGetValue(mName, out kills);

                    result.Add(BestiaryTextHelpers.CreateText($"- {mName} - {kills}"));
                }


                return result;
            }
        }
    }
}
