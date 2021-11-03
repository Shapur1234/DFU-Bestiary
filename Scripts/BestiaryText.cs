using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DaggerfallWorkshop;
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
                                Debug.Log(rawText[i]);
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
            // Adds killcounts to Summary text
            {
                bool foundKillcountStart = false;
                int killcountCounter = 0;
                // for (int ii = 0; ii < PageSummary.SummaryText.Count; ii++)
                // {
                //     if (!foundKillcountStart && PageSummary.SummaryText[ii].TitleText == "Killcount:")
                //         foundKillcountStart = true;

                //     if (foundKillcountStart)
                //     {
                //         if (BestiaryMain.killCounts.ContainsKey(PageEntries[killcountCounter].EntryName))
                //             PageSummary.SummaryText[ii] = new TextPair(PageSummary.SummaryText[ii].TitleText, PageSummary.SummaryText[ii].BodyText + BestiaryMain.killCounts[PageEntries[killcountCounter].EntryName].ToString());
                //         else
                //             PageSummary.SummaryText[ii] = new TextPair(PageSummary.SummaryText[ii].TitleText, PageSummary.SummaryText[ii].BodyText + "0");

                //         killcountCounter++;
                //     }
                // }
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
            {
                item.DebugThis();
            }
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
