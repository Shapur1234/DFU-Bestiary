using System.Collections.Generic;
using System.Linq;
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
                RegisterPage(item);

            foreach (var PageTitle in BestiaryTextDB.ExtraPagesSummary.Keys)
            {
                var summary = BestiaryTextDB.ExtraPagesSummary[PageTitle];
                var entries = BestiaryTextDB.ExtraPagesEntries[PageTitle];
                var page = new Page(PageTitle, summary, entries);
                if (page != null && page.FilterEntries().Count > 0)
                    Pages.Add(page);
            }
        }

        public void RegisterPage(string pageID)
        {
            var pageTemp = BestiaryTextDB.GetPage(pageID);
            if (pageTemp != null && pageTemp.FilterEntries().Count > 0)
                Pages.Add(pageTemp);
        }

        public string BestiaryTitle { get; }
        public List<Page> Pages { get; }
    }

    public class Page
    {
        private IReadOnlyCollection<Entry> _entries;

        public Page(string title, string summary, IEnumerable<Entry> monsters)
        {
            Title = title;
            Entries = monsters.ToArray();
            var names = Entries.Select(monster => monster.Title);
            PageSummary = new Summary(title, summary, names);
        }

        public string Title { get; }
        public Summary PageSummary { get; set; }
        public IReadOnlyCollection<Entry> Entries { get { return _entries; } set => _entries = value; }

        public IReadOnlyList<Entry> FilterEntries()
        {
            if (BestiaryMain.SettingEntries == 1)
                return _entries.Where(item => BestiaryMain.killCounts.ContainsKey(item.Title)).ToArray();
            else
                return _entries.ToArray();
        }
    }

    public class Entry
    {
        private string _title;
        private int[] _spells;
        private string[] _abilities;
        private string _language;
        private string _material;
        private string _advice;
        private string _summary;

        public Entry(string monsterId)
        {
            Id = monsterId;
        }

        public Entry(MonsterCareers monster)
        {
            Id = monster.ToString();
            Title = TextManager.Instance.GetLocalizedEnemyName((int)monster);
            TextureArchive = EnemyBasics.Enemies[(int)monster].MaleTexture;
        }

        public string ButtonTextureName { get; set; }
        public int TextureArchive { get; set; }
        public string Id { get; }
        public string Title
        {
            get
            {

                if (BestiaryTextDB.OverrideTitleTable.ContainsKey(Id))
                {
                    return BestiaryTextDB.OverrideTitleTable[Id];
                }

                return _title;
            }
            set
            {
                _title = value;
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

                return _summary;
            }
            set
            {
                _summary = value;
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

                return _advice;
            }
            set
            {
                _advice = value;
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

                return _material;
            }
            set
            {
                _material = value;
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

                return _language;
            }
            set
            {
                _language = value;
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

                return _abilities;
            }
            set
            {
                _abilities = value;
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

                return _spells;
            }
            set
            {
                _spells = value;
            }
        }
        public string[] NamedSpells
        {
            get
            {
                if (BestiaryTextDB.OverrideSpellsTable.ContainsKey(Id))
                {
                    return BestiaryTextDB.OverrideSpellsTable[Id];
                }

                return new string[0] { };
            }
            set
            {
                BestiaryTextDB.OverrideSpellsTable.Add(Id, value);
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
        private readonly string _summary;
        private readonly IReadOnlyList<MonsterCareers> _monsterCareers;
        private readonly IReadOnlyList<string> _monsterNames;

        public Summary(string title, string summary, IReadOnlyList<MonsterCareers> monsterCareers)
        {
            Title = BestiaryTextDB.SummarySubTitle + (DaggerfallUnity.Settings.SDFFontRendering ? " - " : " ") + title;
            _summary = summary;
            _monsterCareers = monsterCareers;
            _monsterNames = new string[0] { };
        }

        public Summary(string title, string summary, IEnumerable<string> monsterNames)
        {
            Title = BestiaryTextDB.SummarySubTitle + (DaggerfallUnity.Settings.SDFFontRendering ? " - " : " ") + title;
            _summary = summary;
            _monsterCareers = new MonsterCareers[0] { };
            _monsterNames = monsterNames.ToArray();
        }
        public string Title { get; }
        public List<TextLabel> TextLabels
        {
            get
            {
                var result = new List<TextLabel>
                {
                    BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.OverviewLabel),
                    BestiaryTextHelpers.CreateText(_summary),
                    BestiaryTextHelpers.CreateText(string.Empty)
                };

                if (_monsterCareers.Count > 0 || _monsterNames.Count > 0)
                {
                    result.Add(BestiaryTextHelpers.CreateSubtitle(BestiaryTextDB.KillcountLabel));
                }

                for (int i = 0; i < _monsterCareers.Count; i++)
                {
                    string mName = TextManager.Instance.GetLocalizedEnemyName((int)_monsterCareers[i]);

                    uint kills;
                    BestiaryMain.killCounts.TryGetValue(mName, out kills);

                    result.Add(BestiaryTextHelpers.CreateText($"- {mName} - {kills}"));
                }

                for (int i = 0; i < _monsterNames.Count; i++)
                {
                    uint kills;
                    BestiaryMain.killCounts.TryGetValue(_monsterNames[i], out kills);

                    result.Add(BestiaryTextHelpers.CreateText($"- {_monsterNames[i]} - {kills}"));
                }


                return result;
            }
        }
    }
}
