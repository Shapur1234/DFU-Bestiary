using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using DaggerfallConnect;
using DaggerfallConnect.Arena2;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Utility.AssetInjection;

using UnityEngine;

namespace BestiaryMod
{
    class BestiaryUI : DaggerfallPopupWindow
    {
        struct EntryInfo
        {
            public EntryInfo(string entry, string button)
            {
                Entry = entry;
                Button = button;
            }

            public string Entry;
            public string Button;
        }

        class TextureInfo
        {
            public TextureInfo(int archive)
            {
                Archive = archive;
                Record = defaultRotation;
                Frame = 0;
                Flip = false;

                switch (Archive)
                {
                    case 275:
                        Mirrored = true;
                        break;
                    default:
                        Mirrored = false;
                        break;
                }
                Texture2D tempTexture = textureReader.GetTexture2D(Archive, 1, 0);

                if (tempTexture.width > tempTexture.height)
                    maxTextureSize = new Vector2(imagePanelMaxSize[0], tempTexture.height);
                else
                    maxTextureSize = new Vector2(tempTexture.width, imagePanelMaxSize[1]);

                UpdateTextures();
            }

            public void UpdateTextures()
            {
                Texture2D pictureTexture;

                if (Frame >= (new TextureFile(Path.Combine(arena2Path, string.Format("TEXTURE.{0:000}", Archive)), FileUsage.Undefined, true)).GetFrameCount(Record + attackModeOffset))
                    Frame = 0;

                if (!TextureReplacement.TryImportTexture(Archive, Record + attackModeOffset, Frame, out pictureTexture))
                    pictureTexture = textureReader.GetTexture2D(Archive, Record + attackModeOffset, Frame);
                pictureTexture.filterMode = FilterMode.Point;

                UpdateImagePanel(pictureTexture, this);
                ApplyTexture(pictureTexture, this);
            }

            public void IterateAnimationFrame()
            {
                Frame += 1;
                UpdateTextures();
            }

            public void IncreaseRecord()
            {
                if (rotate8)
                {
                    if (!Flip)
                    {
                        Record++;
                        if (Record >= 5)
                        {
                            Record = 3;
                            Flip = !Flip;
                        }
                    }
                    else
                    {
                        Record--;
                        if (Record < 0)
                        {
                            Record = 1;
                            Flip = !Flip;
                        }
                    }
                }
                else
                {
                    Record++;
                    if (Record > 4)
                        Record = 0;
                }
                UpdateTextures();
            }

            public void DecreaseRecord()
            {
                if (rotate8)
                {
                    if (!Flip)
                    {
                        Record--;
                        if (Record < 0)
                        {
                            Record = 1;
                            Flip = !Flip;
                        }
                    }
                    else
                    {
                        Record++;
                        if (Record >= 5)
                        {
                            Record = 3;
                            Flip = !Flip;
                        }
                    }
                }
                else
                {
                    Record--;
                    if (Record < 0)
                        Record = 4;
                }
                UpdateTextures();
            }

            public int Archive;
            public int Record;
            public int Frame;
            public bool Flip;
            public bool Mirrored;
            public Vector2 maxTextureSize;
        }

        private static DaggerfallWorkshop.Utility.TextureReader textureReader;
        private Mod bestiaryMod = ModManager.Instance.GetMod("Bestiary");
        private Mod kabsUnleveledSpellsMod = ModManager.Instance.GetMod("Unleveled Spells");

        public static bool animate;
        public static bool classicMode;
        public static bool rotate8;

        public bool isShowing;
        public bool kabsUnleveledSpellsModFound;
        public bool oldFont = !DaggerfallUnity.Settings.SDFFontRendering;

        private TextureInfo currentTexture;
        public static int animationUpdateDelay;
        public static int defaultRotation;
        public static int attackModeOffset;
        private int contentOffset;
        private int descriptionLabelMaxCharacters;
        private int textLabelXOffset;
        private int animationDelay = 0;

        readonly List<string> allPagesArchive = new List<string> { "page_animals", "page_atronachs", "page_daedra", "page_lycanthropes", "page_monsters1", "page_monsters2", "page_orcs", "page_undead" };
        private List<string> allPages = new List<string>();
        private List<EntryInfo> allEntries;
        private List<EntryInfo> currentEntries;

        private string currentPage;
        private string currentSummary;
        private string entrySuffix;
        private string entryToLoad;
        private static string arena2Path;
        private const string pathToClassicPage = "page_classic";
        private const string blankTextureName = "blank";
        private const string backgroundTextureName = "base_background";
        private const string attackTrueTextureName = "button_attack_true";
        private const string attackFalseTextureName = "button_attack_false";
        private const string rightArrowTextureName = "button_arrow_right";
        private const string leftArrowTextureName = "button_arrow_left";

        public List<Texture2D> contentButtonTextures = new List<Texture2D>();
        private Texture2D attackFalseTexture;
        private Texture2D attackTrueTexture;
        private Texture2D backgroundTexture;
        private Texture2D blankTexture;
        private Texture2D leftArrowTexture;
        private Texture2D rightArrowTexture;

        private Vector2 pageNamePos = new Vector2(71, 14);
        private readonly List<Vector2> buttonAllPos = new List<Vector2> { new Vector2(4, 162), new Vector2(50, 162), new Vector2(95, 162), new Vector2(4, 174), new Vector2(50, 174), new Vector2(95, 174), new Vector2(4, 187), new Vector2(50, 187), new Vector2(95, 187) };
        private readonly Vector2 entryButtonSize = new Vector2(40, 9);
        private readonly Vector2 pageNameSize = new Vector2(52, 10);
        private static readonly Vector2 imagePanelBasePos = new Vector2(18, 51);
        private static readonly Vector2 imagePanelMaxSize = new Vector2(102, 102);

        static Panel imagePanel;
        Panel mainPanel;
        List<TextLabel> descriptionLabels = new List<TextLabel>();
        List<TextLabel> subtitleLabels = new List<TextLabel>();
        TextLabel monsterNameLabel;
        TextLabel pageNameLabel;
        TextLabel titleLabel;

        List<Button> contentButtons = new List<Button>();
        Button summaryButton;
        Button rightRotateButton;
        Button pageRightButton;
        Button pageLeftButton;
        Button leftRotateButton;
        Button exitButton;
        Button attackButton;

        public BestiaryUI(IUserInterfaceManager uiManager)
            : base(uiManager)
        {
            pauseWhileOpened = true;
            AllowCancel = false;
        }

        protected override void Setup()
        {
            base.Setup();
            string textPath;

            arena2Path = DaggerfallUnity.Arena2Path;
            textureReader = new DaggerfallWorkshop.Utility.TextureReader(arena2Path);
            kabsUnleveledSpellsModFound = kabsUnleveledSpellsMod != null;

            LoadTextures();
            if (!oldFont)
            {
                descriptionLabelMaxCharacters = 48;
                textLabelXOffset = 30;
            }
            else
            {
                descriptionLabelMaxCharacters = 24;
                textLabelXOffset = 46;
            }

            if (BestiaryMain.menuUnlock == 2)
                allPages = GetAvailablePages();
            else
                allPages = allPagesArchive;

            if (classicMode)
                textPath = pathToClassicPage;
            else
                textPath = allPages[0];

            if (kabsUnleveledSpellsModFound)
                entrySuffix = "-kabs_unleveled_spells";
            else
                entrySuffix = "";

            SetUpUIElements();
            currentTexture = new TextureInfo(267);
            currentEntries = GetcurrentEntriesFromFile(textPath);
            allEntries = GetcurrentEntriesFromFile(textPath, true);

            ResetButtonTextures();
            LoadPage();
            LoadContent(currentSummary, true);
        }

        public override void Update()
        {
            base.Update();
            if (Input.GetKeyUp(exitKey))
                CloseWindow();
            if (animate)
            {
                animationDelay += 1;
                if (animationDelay >= animationUpdateDelay)
                {
                    animationDelay = 0;
                    currentTexture.IterateAnimationFrame();
                }
            }
        }

        private static void UpdateImagePanel(Texture2D inputTexture, TextureInfo inputTextureInfo)
        {
            imagePanel.Size = inputTextureInfo.maxTextureSize;
            imagePanel.Position = new Vector2(imagePanelBasePos[0] + ((imagePanelMaxSize[0] - imagePanel.Size[0]) / 2), imagePanelBasePos[1] + ((imagePanelMaxSize[0] - imagePanel.Size[1]) / 2));
        }

        private static void ApplyTexture(Texture2D texture, TextureInfo inputTextureInfo)
        {
            bool doFlip;

            if (rotate8)
            {
                if (inputTextureInfo.Flip && inputTextureInfo.Record != 0)
                    doFlip = true;
                else
                    doFlip = false;

                if (inputTextureInfo.Mirrored)
                    doFlip = !doFlip;
            }
            else
                doFlip = false;

            if (doFlip)
                imagePanel.BackgroundTexture = FlipTexture(DuplicateTexture(texture));
            else
                imagePanel.BackgroundTexture = texture;
        }

        public override void OnPush()
        {
            base.OnPush();
            isShowing = true;
        }

        public override void OnPop()
        {
            base.OnPop();
            isShowing = false;
        }

        private void LoadTextures()
        {
            backgroundTexture = DaggerfallUI.GetTextureFromResources(backgroundTextureName);
            blankTexture = DaggerfallUI.GetTextureFromResources(blankTextureName);

            if (!backgroundTexture)
                throw new Exception("BestiaryUI: Could not load backgroundTexture.");
            if (!blankTexture)
                throw new Exception("BestiaryUI: Could not load blankTexture.");

            if (!classicMode)
            {
                attackFalseTexture = DaggerfallUI.GetTextureFromResources(attackFalseTextureName);
                attackTrueTexture = DaggerfallUI.GetTextureFromResources(attackTrueTextureName);
                leftArrowTexture = DaggerfallUI.GetTextureFromResources(leftArrowTextureName);
                rightArrowTexture = DaggerfallUI.GetTextureFromResources(rightArrowTextureName);

                if (!attackFalseTexture)
                    throw new Exception("BestiaryUI: Could not load attackFalseTexture.");
                if (!attackTrueTexture)
                    throw new Exception("BestiaryUI: Could not load attackTrueTexture.");
                if (!leftArrowTexture)
                    throw new Exception("BestiaryUI: Could not load leftArrowTexture.");
                if (!rightArrowTexture)
                    throw new Exception("BestiaryUI: Could not load rightArrowTexture.");
            }
            for (int i = 0; i < 9; i++)
                contentButtonTextures.Add(DaggerfallUI.GetTextureFromResources(blankTextureName));
        }

        private List<string> GetAvailablePages()
        {
            List<string> output = new List<string>();
            foreach (var item in allPagesArchive)
            {
                List<EntryInfo> tempEntries = GetcurrentEntriesFromFile(item, true);
                for (int i = 0; i < tempEntries.Count; i++)
                {
                    if (BestiaryMain.killCounts.ContainsKey(tempEntries[i].Entry))
                    {
                        output.Add(item);
                        break;
                    }
                }
            }
            return output;
        }

        private void LoadPage()
        {
            for (int i = 0; i < currentEntries.Count && i < contentButtonTextures.Count; i++)
            {
                contentButtonTextures[i] = DaggerfallUI.GetTextureFromResources(currentEntries[i].Button);
                if (!contentButtonTextures[i])
                    throw new Exception("BestiaryUI: Could not load contentButtonTextures" + (i) + ".");

                contentButtons[i].BackgroundTexture = contentButtonTextures[i];
            }
        }

        private void LoadContent(string assetPath, bool reset = true)
        {
            if (reset)
            {
                contentOffset = 0;
                entryToLoad = assetPath;
            }

            List<string> textToApply = new List<string>();
            List<string> result;

            string assetPathTemp;
            bool isSummary = false;
            bool summary2ndTime = false;

            ResetTextLabels();
            if (assetPath[0] == 's')
                isSummary = true;

            if (!classicMode)
            {
                attackModeOffset = 0;
                attackButton.BackgroundTexture = attackFalseTexture;
            }

            if (bestiaryMod.HasAsset(assetPath + entrySuffix))
                assetPathTemp = assetPath + entrySuffix;
            else
                assetPathTemp = assetPath;

            result = new List<string>(bestiaryMod.GetAsset<TextAsset>(assetPathTemp).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

            if (isSummary)
            {
                pageNameLabel.Text = result[1];
                pageNameLabel.Position = new Vector2(pageNamePos[0] + ((pageNameSize[0] - pageNameLabel.TextWidth) / 2), pageNamePos[1]);

                if (summaryButton.Position == pageNameLabel.Position)
                    summary2ndTime = true;

                summaryButton.Position = pageNameLabel.Position;
                summaryButton.Size = new Vector2(pageNameLabel.TextWidth, pageNameSize[1]);
            }

            for (int i = 0; i < result.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        if (reset)
                        {
                            if (isSummary)
                            {
                                int newTextureRecord = int.Parse(new List<string>(bestiaryMod.GetAsset<TextAsset>(currentEntries[0].Entry).text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))[0]);

                                if (!summary2ndTime && currentTexture.Frame != newTextureRecord)
                                    currentTexture = new TextureInfo(newTextureRecord);
                            }
                            else
                            {
                                if (currentTexture.Frame != int.Parse(result[0]))
                                    currentTexture = new TextureInfo(int.Parse(result[0]));
                            }
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        titleLabel.Text = result[2];
                        break;
                    case 3:
                        if (oldFont)
                            monsterNameLabel.Text = result[3].Replace(" - ", " ");
                        else
                            monsterNameLabel.Text = result[3];
                        break;
                    case 4:
                        textToApply.Add(result[i]);
                        if (isSummary)
                            textToApply.Add(" * ");
                        break;
                    default:
                        if (isSummary)
                        {
                            if (i - 5 < allEntries.Count)
                            {
                                string suffix;
                                if (BestiaryMain.killCounts.ContainsKey(allEntries[i - 5].Entry))
                                    suffix = BestiaryMain.killCounts[allEntries[i - 5].Entry].ToString();
                                else
                                    suffix = "0";

                                textToApply.Add(result[i] + suffix);
                            }
                        }
                        else
                        {
                            if (i < result.Count)
                                textToApply.Add(result[i]);
                        }
                        break;
                }
            }
            ApplyText(textToApply);
        }

        private void ResetTextLabels()
        {
            for (int i = 0; i < subtitleLabels.Count; i++)
            {
                subtitleLabels[i].Text = "";
                descriptionLabels[i].Text = "";
            }
        }

        private void ResetButtonTextures()
        {
            for (int i = 0; i < contentButtons.Count; i++)
                contentButtons[i].BackgroundTexture = blankTexture;
        }

        //Taken from here: https://codereview.stackexchange.com/questions/54697/convert-string-to-multiline-text.
        private static string SplitLineToMultiline(string input, int rowLength)
        {
            StringBuilder line = new StringBuilder();
            StringBuilder result = new StringBuilder();

            Stack<string> stack = new Stack<string>(input.Split(' '));

            while (stack.Count > 0)
            {
                var word = stack.Pop();
                if (word.Length > rowLength)
                {
                    string head = word.Substring(0, rowLength);
                    string tail = word.Substring(rowLength);

                    word = head;
                    stack.Push(tail);
                }

                if (line.Length + word.Length > rowLength)
                {
                    result.AppendLine(line.ToString());
                    line.Clear();
                }
                line.Append(word + " ");
            }
            result.Append(line);
            return result.ToString();
        }

        private static string ReverseWords(string sentence)
        {
            string[] words = sentence.Split(' ');
            Array.Reverse(words);
            return string.Join(" ", words);
        }

        private void ApplyText(List<string> inputText)
        {
            List<List<string>> textTemp = new List<List<string>>();
            List<string> text = new List<string>();

            foreach (var item in inputText)
            {
                var splitResult = item.Split(new[] { '*' });
                textTemp.Add((new List<string> { splitResult[0], splitResult[1] }));
            }

            for (int i = 0; i < textTemp.Count; i++)
            {
                bool first = true;
                var singlelineText = SplitLineToMultiline(ReverseWords(textTemp[i][1]), descriptionLabelMaxCharacters).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in singlelineText)
                {
                    if (first)
                    {
                        text.Add(textTemp[i][0] + "&" + item);
                        first = false;
                    }
                    else
                        text.Add(item);
                }
            }

            if (text.Count < 15)
                contentOffset = 0;
            else if (text.Count - contentOffset <= 0)
                contentOffset = text.Count - 1;

            int labelNumber = 0;
            for (int i = contentOffset; i < text.Count && labelNumber < 14; i++)
            {
                string[] multiItem = text[i].Split('&');

                if (multiItem.Length > 1)
                {
                    subtitleLabels[labelNumber].Text = multiItem[0];
                    descriptionLabels[labelNumber].Text = multiItem[1];
                }
                else
                    descriptionLabels[labelNumber].Text = multiItem[0];

                labelNumber++;
            }
        }

        private List<EntryInfo> GetcurrentEntriesFromFile(string path, bool firstLoad = false)
        {
            List<EntryInfo> output = new List<EntryInfo>();

            currentPage = path;

            string[] resultAssetPage = bestiaryMod.GetAsset<TextAsset>(path).text.Split(new[] { '\r', '\n' }).Skip(1).ToArray();
            currentSummary = resultAssetPage[0];

            for (int i = 0; i < resultAssetPage.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        break;
                    default:
                        if (!string.IsNullOrEmpty(resultAssetPage[i])
                            && ((BestiaryMain.menuUnlock != 2 || BestiaryMain.killCounts.ContainsKey(resultAssetPage[i])) || firstLoad))
                        {
                            TextAsset textAssetEntry = bestiaryMod.GetAsset<TextAsset>(resultAssetPage[i]);
                            string[] resultAssetEntry = textAssetEntry.text.Split(new[] { '\r', '\n' });

                            output.Add(new EntryInfo(resultAssetPage[i], resultAssetEntry[1]));
                        }
                        break;
                }
            }
            return output;
        }

        private void SetUpUIElements()
        {
            ParentPanel.BackgroundColor = ScreenDimColor;

            mainPanel = DaggerfallUI.AddPanel(NativePanel, AutoSizeModes.None);
            mainPanel.Size = new Vector2(320, 200);
            mainPanel.BackgroundTexture = backgroundTexture;
            mainPanel.HorizontalAlignment = HorizontalAlignment.Center;
            mainPanel.VerticalAlignment = VerticalAlignment.Middle;
            mainPanel.OnMouseScrollDown += MainPanel_OnMouseScrollDown;
            mainPanel.OnMouseScrollUp += MainPanel_OnMouseScrollUp;

            imagePanel = DaggerfallUI.AddPanel(mainPanel, AutoSizeModes.None);
            imagePanel.Size = imagePanelMaxSize;
            imagePanel.Position = imagePanelBasePos;
            imagePanel.BackgroundTextureLayout = BackgroundLayout.ScaleToFit;

            titleLabel = new TextLabel();
            titleLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            titleLabel.Size = new Vector2(52, 22);
            titleLabel.Font = DaggerfallUI.TitleFont;

            if (oldFont)
            {
                titleLabel.Position = new Vector2(15, 20);
                titleLabel.TextScale = 0.7f;
            }
            else
                titleLabel.Position = new Vector2(15, 16);

            mainPanel.Components.Add(titleLabel);

            monsterNameLabel = new TextLabel();
            monsterNameLabel.Position = new Vector2(144, 24);
            monsterNameLabel.Size = new Vector2(40, 14);
            monsterNameLabel.Font = DaggerfallUI.LargeFont;

            if (oldFont)
                monsterNameLabel.TextScale = 0.85f;

            mainPanel.Components.Add(monsterNameLabel);

            exitButton = new Button();
            exitButton.Position = new Vector2(216, 187);
            exitButton.Size = entryButtonSize;
            exitButton.OnMouseClick += ExitButton_OnMouseClick;
            mainPanel.Components.Add(exitButton);

            int yPos = 40;
            for (int i = 0; i < 14; i++)
            {
                subtitleLabels.Add(new TextLabel());
                subtitleLabels[i].Position = new Vector2(144, yPos);
                subtitleLabels[i].Size = new Vector2(40, 14);
                mainPanel.Components.Add(subtitleLabels[i]);

                descriptionLabels.Add(new TextLabel());
                descriptionLabels[i].Position = new Vector2(144 + textLabelXOffset, yPos);
                descriptionLabels[i].Size = new Vector2(124, 10);
                descriptionLabels[i].MaxCharacters = descriptionLabelMaxCharacters;
                mainPanel.Components.Add(descriptionLabels[i]);

                yPos += 10;
            }

            for (int i = 0; i < 9; i++)
            {
                contentButtons.Add(new Button());
                contentButtons[i].Position = buttonAllPos[i];
                contentButtons[i].Size = entryButtonSize;
                switch (i)
                {
                    case 0:
                        contentButtons[i].OnMouseClick += ContentButton0_OnMouseClick;
                        break;
                    case 1:
                        contentButtons[i].OnMouseClick += ContentButton1_OnMouseClick;
                        break;
                    case 2:
                        contentButtons[i].OnMouseClick += ContentButton2_OnMouseClick;
                        break;
                    case 3:
                        contentButtons[i].OnMouseClick += ContentButton3_OnMouseClick;
                        break;
                    case 4:
                        contentButtons[i].OnMouseClick += ContentButton4_OnMouseClick;
                        break;
                    case 5:
                        contentButtons[i].OnMouseClick += ContentButton5_OnMouseClick;
                        break;
                    case 6:
                        contentButtons[i].OnMouseClick += ContentButton6_OnMouseClick;
                        break;
                    case 7:
                        contentButtons[i].OnMouseClick += ContentButton7_OnMouseClick;
                        break;
                    case 8:
                        contentButtons[i].OnMouseClick += ContentButton8_OnMouseClick;
                        break;
                }
                mainPanel.Components.Add(contentButtons[i]);
            }

            if (!classicMode)
            {
                pageRightButton = new Button();
                pageRightButton.Position = new Vector2(98, 25);
                pageRightButton.Size = new Vector2(10, 10);
                pageRightButton.BackgroundTexture = rightArrowTexture;
                pageRightButton.OnMouseClick += pageRightButton_OnMouseClick;
                mainPanel.Components.Add(pageRightButton);

                pageLeftButton = new Button();
                pageLeftButton.Position = new Vector2(86, 25);
                pageLeftButton.Size = new Vector2(10, 10);
                pageLeftButton.BackgroundTexture = leftArrowTexture;
                pageLeftButton.OnMouseClick += pageLeftButton_OnMouseClick;
                mainPanel.Components.Add(pageLeftButton);

                rightRotateButton = new Button();
                rightRotateButton.Position = new Vector2(116, 145);
                rightRotateButton.Size = new Vector2(10, 10);
                rightRotateButton.BackgroundTexture = rightArrowTexture;
                rightRotateButton.OnMouseClick += RightRotateButton_OnMouseClick;
                mainPanel.Components.Add(rightRotateButton);

                leftRotateButton = new Button();
                leftRotateButton.Position = new Vector2(104, 145);
                leftRotateButton.Size = new Vector2(10, 10);
                leftRotateButton.BackgroundTexture = leftArrowTexture;
                leftRotateButton.OnMouseClick += LeftRotateButton_OnMouseClick;
                mainPanel.Components.Add(leftRotateButton);

                attackButton = new Button();
                attackButton.Position = new Vector2(78, 145);
                attackButton.Size = new Vector2(24, 10);
                attackButton.BackgroundTexture = attackFalseTexture;
                attackButton.OnMouseClick += AttackButton_OnMouseClick;
                mainPanel.Components.Add(attackButton);

                summaryButton = new Button();
                summaryButton.Position = pageNamePos;
                summaryButton.Size = pageNameSize;
                summaryButton.OnMouseClick += summaryButton_OnMouseClick;
                mainPanel.Components.Add(summaryButton);

                pageNameLabel = new TextLabel();
                pageNameLabel.Position = pageNamePos;
                pageNameLabel.Size = pageNameSize;

                if (!oldFont)
                    pageNameLabel.Font = DaggerfallUI.LargeFont;
                else
                    pageNamePos[1] = 18;

                pageNameLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
                mainPanel.Components.Add(pageNameLabel);
            }
        }

        //From here: https://girlscancode.wordpress.com/2015/03/02/unity3d-flipping-a-texture/.
        private static Texture2D FlipTexture(Texture2D original)
        {
            Texture2D flipped = new Texture2D(original.width, original.height);
            flipped.filterMode = FilterMode.Point;


            int xN = original.width;
            int yN = original.height;

            for (int i = 0; i < xN; i++)
            {
                for (int j = 0; j < yN; j++)
                {
                    flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
                }
            }
            flipped.Apply();
            return flipped;
        }

        //From here: https://stackoverflow.com/questions/44733841/how-to-make-texture2d-readable-via-script.
        private static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;

            Texture2D readableText = new Texture2D(source.width, source.height);

            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            return readableText;
        }

        private void ChangePage(bool right)
        {
            int currentEntryNum = allPages.IndexOf(currentPage);

            if (right && currentEntryNum > 0)
            {
                currentEntryNum -= 1;

                currentEntries = GetcurrentEntriesFromFile(allPages[currentEntryNum]);
                ResetButtonTextures();
                LoadPage();
                LoadContent(currentSummary, true);
            }
            else if (right && currentEntryNum <= 0)
            {
                currentEntryNum = allPages.Count - 1;

                currentEntries = GetcurrentEntriesFromFile(allPages[currentEntryNum]);
                ResetButtonTextures();
                LoadPage();
                LoadContent(currentSummary, true);
            }
            else if (!right && currentEntryNum < (allPages.Count - 1))
            {
                currentEntryNum += 1;

                currentEntries = GetcurrentEntriesFromFile(allPages[currentEntryNum]);
                ResetButtonTextures();
                LoadPage();
                LoadContent(currentSummary, true);
            }
            else
            {
                currentEntryNum = 0;

                currentEntries = GetcurrentEntriesFromFile(allPages[currentEntryNum]);
                ResetButtonTextures();
                LoadPage();
                LoadContent(currentSummary, true);
            }
            allEntries = GetcurrentEntriesFromFile(allPages[currentEntryNum], true);
        }
        protected void ExitButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            CloseWindow();
        }

        protected void ContentButton0_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 0)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[0].Entry)
                    LoadContent(currentEntries[0].Entry);
            }
        }

        protected void ContentButton1_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[1].Entry)
                    LoadContent(currentEntries[1].Entry);
            }
        }

        protected void ContentButton2_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 2)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[2].Entry)
                    LoadContent(currentEntries[2].Entry);
            }
        }

        protected void ContentButton3_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 3)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[3].Entry)
                    LoadContent(currentEntries[3].Entry);
            }
        }

        protected void ContentButton4_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 4)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[4].Entry)
                    LoadContent(currentEntries[4].Entry);
            }
        }

        protected void ContentButton5_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 5)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[5].Entry)
                    LoadContent(currentEntries[5].Entry);
            }
        }

        protected void ContentButton6_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 6)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[6].Entry)
                    LoadContent(currentEntries[6].Entry);
            }
        }

        protected void ContentButton7_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 7)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[7].Entry)
                    LoadContent(currentEntries[7].Entry);
            }
        }

        protected void ContentButton8_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (currentEntries.Count > 8)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (entryToLoad != currentEntries[8].Entry)
                    LoadContent(currentEntries[8].Entry);
            }
        }

        protected void pageRightButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (allPages.Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                ChangePage(false);
            }
        }

        protected void pageLeftButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (allPages.Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                ChangePage(true);
            }
        }

        protected void RightRotateButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            currentTexture.DecreaseRecord();
        }

        protected void LeftRotateButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            currentTexture.IncreaseRecord();
        }

        protected void AttackButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            if (attackModeOffset == 0)
            {
                attackModeOffset = 5;
                attackButton.BackgroundTexture = attackTrueTexture;
                currentTexture.UpdateTextures();
            }
            else
            {
                attackModeOffset = 0;
                attackButton.BackgroundTexture = attackFalseTexture;
                currentTexture.UpdateTextures();
            }
        }

        protected void MainPanel_OnMouseScrollDown(BaseScreenComponent sender)
        {
            contentOffset += 1;
            LoadContent(entryToLoad, false);
        }

        protected void MainPanel_OnMouseScrollUp(BaseScreenComponent sender)
        {
            if (contentOffset > 0)
                contentOffset -= 1;
            LoadContent(entryToLoad, false);
        }

        protected void summaryButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (!String.IsNullOrEmpty(currentSummary))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                LoadContent(currentSummary, true);
            }
        }
    }
}