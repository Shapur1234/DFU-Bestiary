using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DaggerfallWorkshop.Game.Utility.ModSupport;

using UnityEngine;

namespace BestiaryMod
{
    public class AllText
    {
        public AllText(List<string> pagesToLoad)
        {
            BestiaryTitle = "Bestiary";
            AllPages = new List<Page>();

            foreach (var item in pagesToLoad)
            {
                switch (BestiaryMain.entries)
                {
                    case 1:
                        Page pageTemp = new Page(item);
                        if (pageTemp.PageEntries.Count > 0)
                            AllPages.Add(pageTemp);
                        break;
                    default:
                        AllPages.Add(new Page(item));
                        break;
                }
            }
        }
        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging AllText {0}", BestiaryTitle));

            Debug.Log("AllPages:");
            foreach (var item in AllPages)
            {
                item.DebugThis();
            }
        }

        public string BestiaryTitle;
        public List<Page> AllPages;
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
                        PageSummary = new Summary(rawText[i]);
                        break;
                    default:
                        switch (BestiaryMain.entries)
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
            PageSummary.DebugThis();

            // Debug.Log("PageEntries:");
            // foreach (var item in PageEntries)
            // {
            //     item.DebugThis();
            // }
        }

        public string PageName;
        public string PageTitle;
        public Summary PageSummary;
        public List<Entry> PageEntries;
    }
    public class Entry
    {
        public Entry(string assetPath)
        {
            // string assetPathTemp;

            // if (bestiaryMod.HasAsset(assetPath + entrySuffix)
            //     assetPathTemp = assetPath + entrySuffix;
            // else
            //     assetPathTemp = assetPath;

            EntryName = assetPath;
            EntryButtonName = "Entry constructor error";
            EntryTitle = "Entry constructor error";
            TextureRecord = 0;
            EntryText = new List<TextPair>();

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(EntryName).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        TextureRecord = int.Parse(rawText[i]);
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
            Debug.Log(String.Format("EntryButtonName: {0}, EntryTitle: {1}, TextureRecord: {2}", EntryButtonName, EntryTitle, TextureRecord));

            Debug.Log("EntryText:");
            foreach (var item in EntryText)
            {
                item.DebugThis();
            }
        }

        public string EntryName;
        public string EntryButtonName;
        public string EntryTitle;
        public int TextureRecord;
        public List<TextPair> EntryText;
    }
    public class Summary
    {
        public Summary(string assetPath)
        {
            // string assetPathTemp;

            // if (bestiaryMod.HasAsset(assetPath + entrySuffix))
            //     assetPathTemp = assetPath + entrySuffix;
            // else
            //     assetPathTemp = assetPath;

            SummaryName = assetPath;
            SummaryTitle = "Summary constructor error";
            SummaryText = new List<TextPair>();
            TextureRecord = 0;

            List<string> rawText = new List<string>(ModManager.Instance.GetMod("Bestiary").GetAsset<TextAsset>(SummaryName).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

            bool foundKillcount = false;
            for (int i = 0; i < rawText.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        TextureRecord = int.Parse(rawText[i]);
                        break;
                    case 1:
                        SummaryTitle = rawText[i];
                        break;
                    default: // fix killcounts
                        if (rawText[i].Length > 2 && rawText[i].Contains("*"))
                        {
                            var splitResult = rawText[i].Split('*');
                            string temp = splitResult[1];

                            if (!foundKillcount && splitResult[0] == "Killcount:")
                                foundKillcount = true;

                            if (foundKillcount)
                                temp += "^";

                            SummaryText.Add(new TextPair(splitResult[0], temp));
                        }
                        break;
                }
            }
        }

        public void DebugThis()
        {
            Debug.Log(String.Format("Debugging Summary {0}", SummaryName));
            Debug.Log(String.Format("SummaryTitle: {0}, TextureRecord: {1}", SummaryTitle, TextureRecord));

            Debug.Log("SummaryText:");
            foreach (var item in SummaryText)
            {
                item.DebugThis();
            }
        }

        public string SummaryName;
        public string SummaryTitle;
        public int TextureRecord;
        public List<TextPair> SummaryText;
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
        public string TitleText;
        public string BodyText;
    }
}
