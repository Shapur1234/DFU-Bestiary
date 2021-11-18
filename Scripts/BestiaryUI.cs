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
    public class BestiaryUI : DaggerfallPopupWindow
    {
        private class TextureInfo
        {
            public TextureInfo(int archive)
            {
                Archive = archive;
                Record = BestiaryMain.SettingDefaultRotation;
                Frame = 0;
                AttackModeOffset = 0;
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

                attackButton.BackgroundTexture = attackFalseTexture;

                UpdateTextures();
            }

            public void UpdateTextures()
            {
                Texture2D pictureTexture;

                if (Frame >= (new TextureFile(Path.Combine(arena2Path, string.Format("TEXTURE.{0:000}", Archive)), FileUsage.Undefined, true)).GetFrameCount(Record + AttackModeOffset))
                    Frame = 0;

                if (!TextureReplacement.TryImportTexture(Archive, Record + AttackModeOffset, Frame, out pictureTexture))
                    pictureTexture = textureReader.GetTexture2D(Archive, Record + AttackModeOffset, Frame);
                pictureTexture.filterMode = FilterMode.Point;

                UpdateImagePanel(pictureTexture, this);
                ApplyTexture(pictureTexture, this);

                void UpdateImagePanel(Texture2D inputTexture, TextureInfo inputTextureInfo)
                {
                    imagePanel.Size = inputTextureInfo.maxTextureSize;
                    imagePanel.Position = new Vector2(imagePanelBasePos[0] + ((imagePanelMaxSize[0] - imagePanel.Size[0]) / 2), imagePanelBasePos[1] + ((imagePanelMaxSize[0] - imagePanel.Size[1]) / 2));
                }
                void ApplyTexture(Texture2D texture, TextureInfo inputTextureInfo)
                {
                    bool doFlip;

                    if (BestiaryMain.SettingEnableAllDirectionRotation)
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
            }

            public void IterateAnimationFrame()
            {
                Frame += 1;
                UpdateTextures();
            }

            public void IncreaseRecord()
            {
                if (BestiaryMain.SettingEnableAllDirectionRotation)
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
                if (BestiaryMain.SettingEnableAllDirectionRotation)
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

            public int Archive { get; }
            public int Record { get; set; }
            public int Frame { get; set; }
            public int AttackModeOffset { get; set; }

            public bool Flip { get; set; }
            public bool Mirrored { get; }
            public Vector2 maxTextureSize { get; }
        }

        public bool isShowing;
        #region uiLayoutVars
        private int descriptionLabelMaxCharacters;
        private int textLabelXOffset;
        #endregion

        #region textureNameVars
        private const string blankTextureName = "blank";
        private const string backgroundTextureName = "base_background";
        private const string attackTrueTextureName = "button_attack_true";
        private const string attackFalseTextureName = "button_attack_false";
        private const string rightArrowTextureName = "button_arrow_right";
        private const string leftArrowTextureName = "button_arrow_left";
        #endregion

        #region textureVars
        private List<Texture2D> contentButtonTextures = new List<Texture2D>();
        private static Texture2D attackFalseTexture;
        private Texture2D attackTrueTexture;
        private Texture2D backgroundTexture;
        private Texture2D blankTexture;
        private Texture2D leftArrowTexture;
        private Texture2D rightArrowTexture;
        #endregion

        #region uiVars
        private Vector2 pageNamePos = new Vector2(71, 14);
        private readonly List<Vector2> buttonAllPos = new List<Vector2> { new Vector2(4, 162), new Vector2(50, 162), new Vector2(95, 162), new Vector2(4, 174), new Vector2(50, 174), new Vector2(95, 174), new Vector2(4, 187), new Vector2(50, 187), new Vector2(95, 187) };
        private readonly Vector2 entryButtonSize = new Vector2(40, 9);
        private readonly Vector2 pageNameSize = new Vector2(52, 10);
        private static readonly Vector2 imagePanelBasePos = new Vector2(18, 51);
        private static readonly Vector2 imagePanelMaxSize = new Vector2(102, 102);

        static Panel imagePanel;
        private Panel mainPanel;

        private List<TextLabel> descriptionLabels = new List<TextLabel>();
        private List<TextLabel> subtitleLabels = new List<TextLabel>();
        private TextLabel monsterNameLabel;
        private TextLabel pageNameLabel;
        private TextLabel titleLabel;

        private List<Button> contentButtons = new List<Button>();
        private Button summaryButton;
        private Button rightRotateButton;
        private Button pageRightButton;
        private Button pageLeftButton;
        private Button leftRotateButton;
        private Button exitButton;
        private static Button attackButton;
        #endregion

        private static DaggerfallWorkshop.Utility.TextureReader textureReader;
        private TextureInfo currentTexture = null;
        private static int currentEntryIndex;
        private static int currentPageIndex;
        private static List<TextPair> currentText;
        private int animationDelay;
        private int scrollOffset;
        private static string arena2Path;

        public BestiaryUI(IUserInterfaceManager uiManager)
            : base(uiManager)
        {
            pauseWhileOpened = true;
            AllowCancel = false;
        }

        protected override void Setup()
        {
            base.Setup();

            animationDelay = 0;
            currentEntryIndex = 0;
            currentPageIndex = 0;

            arena2Path = DaggerfallUnity.Arena2Path;
            textureReader = new DaggerfallWorkshop.Utility.TextureReader(arena2Path);

            LoadTextures();
            SetUpUIElements();

            LoadPage();
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(exitKey))
                CloseWindow();

            if (BestiaryMain.SettingAnimate)
            {
                animationDelay += 1;
                if (animationDelay >= BestiaryMain.SettingAnimationUpdateDelay)
                {
                    animationDelay = 0;
                    currentTexture.IterateAnimationFrame();
                }
            }
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
            if (!backgroundTexture)
                throw new Exception("BestiaryUI: Could not load backgroundTexture.");

            blankTexture = DaggerfallUI.GetTextureFromResources(blankTextureName);
            if (!blankTexture)
                throw new Exception("BestiaryUI: Could not load blankTexture.");

            attackFalseTexture = DaggerfallUI.GetTextureFromResources(attackFalseTextureName);
            if (!attackFalseTexture)
                throw new Exception("BestiaryUI: Could not load attackFalseTexture.");

            attackTrueTexture = DaggerfallUI.GetTextureFromResources(attackTrueTextureName);
            if (!attackTrueTexture)
                throw new Exception("BestiaryUI: Could not load attackTrueTexture.");

            leftArrowTexture = DaggerfallUI.GetTextureFromResources(leftArrowTextureName);
            if (!leftArrowTexture)
                throw new Exception("BestiaryUI: Could not load leftArrowTexture.");

            rightArrowTexture = DaggerfallUI.GetTextureFromResources(rightArrowTextureName);
            if (!rightArrowTexture)
                throw new Exception("BestiaryUI: Could not load rightArrowTexture.");

            for (int i = 0; i < 9; i++)
                contentButtonTextures.Add(DaggerfallUI.GetTextureFromResources(blankTextureName));
        }
        private void LoadPage()
        {
            ResetButtonTextures();
            LoadButtons();

            if (BestiaryMain.SettingEntries == 2)
            {
                currentEntryIndex = 0;
                LoadEntry();
            }
            else
            {
                LoadSummary();
                UpdatePageNameLabel();
            }

            void ResetButtonTextures()
            {
                for (int i = 0; i < contentButtons.Count; i++)
                    contentButtons[i].BackgroundTexture = blankTexture;
            }
            void LoadButtons()
            {
                for (int i = 0; i < BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count && i < contentButtonTextures.Count; i++)
                {
                    contentButtonTextures[i] = DaggerfallUI.GetTextureFromResources(BestiaryMain.AllText.Pages[currentPageIndex].Entries[i].ButtonTextureName);
                    if (!contentButtonTextures[i])
                        throw new Exception("BestiaryUI: Could not load contentButtonTextures" + (i) + ".");

                    contentButtons[i].BackgroundTexture = contentButtonTextures[i];
                }
            }
            void UpdatePageNameLabel()
            {
                pageNameLabel.Text = BestiaryMain.AllText.Pages[currentPageIndex].Title;

                pageNameLabel.Position = new Vector2(pageNamePos[0] + ((pageNameSize[0] - pageNameLabel.TextWidth) / 2), pageNamePos[1]);
                summaryButton.Position = pageNameLabel.Position;
                summaryButton.Size = new Vector2(pageNameLabel.TextWidth, pageNameSize[1]);
            }
        }
        private void LoadSummary(bool updateTexture = true)
        {
            scrollOffset = 0;
            currentEntryIndex = 69;

            int imageIndexToLoad = UnityEngine.Random.Range(0, BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count);
            if (updateTexture && currentTexture == null || BestiaryMain.AllText.Pages[currentPageIndex].Entries[imageIndexToLoad].TextureArchive != currentTexture.Archive)
                currentTexture = new TextureInfo(BestiaryMain.AllText.Pages[currentPageIndex].Entries[imageIndexToLoad].TextureArchive);

            monsterNameLabel.Text = BestiaryMain.AllText.Pages[currentPageIndex].PageSummary.Title;
            currentText = ProcessText(BestiaryMain.AllText.Pages[currentPageIndex].PageSummary.Text);
            ApplyText();
        }
        private void LoadEntry()
        {
            scrollOffset = 0;
            if (currentTexture == null || BestiaryMain.AllText.Pages[currentPageIndex].Entries[currentEntryIndex].TextureArchive != currentTexture.Archive)
                currentTexture = new TextureInfo(BestiaryMain.AllText.Pages[currentPageIndex].Entries[currentEntryIndex].TextureArchive);

            monsterNameLabel.Text = BestiaryMain.AllText.Pages[currentPageIndex].Entries[currentEntryIndex].Title;
            currentText = ProcessText(BestiaryMain.AllText.Pages[currentPageIndex].Entries[currentEntryIndex].Text);
            ApplyText();
        }
        private List<TextPair> ProcessText(List<TextPair> inputText)
        {
            List<TextPair> output = new List<TextPair>();

            foreach (var item in inputText)
            {
                string temp = item.LeftText;
                string[] splitText = SplitLineToMultiline(ReverseWords(item.RightText), descriptionLabelMaxCharacters).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var itemm in splitText)
                {
                    output.Add(new TextPair(temp, itemm));
                    temp = "";
                }
            }
            return output;
        }
        private void ApplyText()
        {
            ResetTextLabels();

            if (scrollOffset < 0 || currentText.Count <= subtitleLabels.Count)
                scrollOffset = 0;
            else if (currentText.Count - scrollOffset <= 0)
                scrollOffset = currentText.Count - 1;

            int textCounter = scrollOffset;
            for (int i = 0; textCounter < currentText.Count && i < subtitleLabels.Count && i < descriptionLabels.Count; i++)
            {
                subtitleLabels[i].Text = currentText[textCounter].LeftText;
                descriptionLabels[i].Text = currentText[textCounter].RightText;
                textCounter += 1;
            }

            void ResetTextLabels()
            {
                for (int i = 0; i < subtitleLabels.Count && i < descriptionLabels.Count; i++)
                {
                    subtitleLabels[i].Text = "";
                    descriptionLabels[i].Text = "";
                }
            }
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
        private void SetUpUIElements()
        {
            if (DaggerfallUnity.Settings.SDFFontRendering)
            {
                descriptionLabelMaxCharacters = 48;
                textLabelXOffset = 30;
            }
            else
            {
                descriptionLabelMaxCharacters = 24;
                textLabelXOffset = 46;
            }

            ParentPanel.BackgroundColor = ScreenDimColor;

            mainPanel = DaggerfallUI.AddPanel(NativePanel, AutoSizeModes.None);
            mainPanel.Size = new Vector2(320, 200);
            mainPanel.BackgroundTexture = backgroundTexture;
            mainPanel.VerticalAlignment = VerticalAlignment.Middle;
            mainPanel.HorizontalAlignment = HorizontalAlignment.Center;
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
            titleLabel.Text = BestiaryMain.AllText.BestiaryTitle;

            if (DaggerfallUnity.Settings.SDFFontRendering)
                titleLabel.Position = new Vector2(15, 16);
            else
            {
                titleLabel.Position = new Vector2(15, 20);
                titleLabel.TextScale = 0.7f;
            }
            mainPanel.Components.Add(titleLabel);

            monsterNameLabel = new TextLabel();
            monsterNameLabel.Position = new Vector2(144, 24);
            monsterNameLabel.Size = new Vector2(40, 14);
            monsterNameLabel.Font = DaggerfallUI.LargeFont;

            if (!DaggerfallUnity.Settings.SDFFontRendering)
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

            pageRightButton = new Button();
            pageRightButton.Position = new Vector2(98, 25);
            pageRightButton.Size = new Vector2(10, 10);
            pageRightButton.OnMouseClick += pageRightButton_OnMouseClick;
            mainPanel.Components.Add(pageRightButton);

            pageLeftButton = new Button();
            pageLeftButton.Position = new Vector2(86, 25);
            pageLeftButton.Size = new Vector2(10, 10);
            pageLeftButton.BackgroundTexture = leftArrowTexture;
            pageLeftButton.OnMouseClick += pageLeftButton_OnMouseClick;
            mainPanel.Components.Add(pageLeftButton);

            if (BestiaryMain.AllText.Pages.Count > 1)
            {
                pageRightButton.BackgroundTexture = rightArrowTexture;
                pageLeftButton.BackgroundTexture = leftArrowTexture;
            }
            else
            {
                pageRightButton.BackgroundTexture = blankTexture;
                pageLeftButton.BackgroundTexture = blankTexture;
            }

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

            if (DaggerfallUnity.Settings.SDFFontRendering)
                pageNameLabel.Font = DaggerfallUI.LargeFont;
            else
                pageNamePos[1] = 18;

            pageNameLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            mainPanel.Components.Add(pageNameLabel);

        }

        //From here: https://girlscancode.wordpress.com/2015/03/02/unity3d-flipping-a-texture/.
        private static Texture2D FlipTexture(Texture2D original)
        {
            Texture2D flipped = new Texture2D(original.width, original.height);
            flipped.filterMode = FilterMode.Point;

            int xN = original.width;
            int yN = original.height;

            for (int i = 0; i < xN; i++)
                for (int j = 0; j < yN; j++)
                    flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));

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
            currentEntryIndex = 0;

            if (right)
            {
                currentPageIndex += 1;
                if (currentPageIndex >= BestiaryMain.AllText.Pages.Count)
                    currentPageIndex = 0;
            }
            else
            {
                currentPageIndex -= 1;
                if (currentPageIndex < 0)
                    currentPageIndex = BestiaryMain.AllText.Pages.Count - 1;
            }
            LoadPage();
        }
        protected void ExitButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            CloseWindow();
        }

        protected void ContentButton0_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 0)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 0)
                {
                    currentEntryIndex = 0;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton1_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 1)
                {
                    currentEntryIndex = 1;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton2_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 2)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 2)
                {
                    currentEntryIndex = 2;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton3_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 3)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 3)
                {
                    currentEntryIndex = 3;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton4_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 4)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 4)
                {
                    currentEntryIndex = 4;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton5_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 5)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 5)
                {
                    currentEntryIndex = 5;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton6_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 6)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 6)
                {
                    currentEntryIndex = 6;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton7_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 7)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 7)
                {
                    currentEntryIndex = 7;
                    LoadEntry();
                }
            }
        }
        protected void ContentButton8_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].Entries.Count > 8)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 8)
                {
                    currentEntryIndex = 8;
                    LoadEntry();
                }
            }
        }
        protected void pageRightButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages.Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                ChangePage(false);
            }
        }

        protected void pageLeftButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages.Count > 1)
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
            if (currentTexture.AttackModeOffset == 0)
            {
                currentTexture.AttackModeOffset = 5;
                attackButton.BackgroundTexture = attackTrueTexture;
                currentTexture.UpdateTextures();
            }
            else
            {
                currentTexture.AttackModeOffset = 0;
                attackButton.BackgroundTexture = attackFalseTexture;
                currentTexture.UpdateTextures();
            }
        }

        protected void MainPanel_OnMouseScrollDown(BaseScreenComponent sender)
        {
            scrollOffset += 1;
            ApplyText();
        }

        protected void MainPanel_OnMouseScrollUp(BaseScreenComponent sender)
        {
            scrollOffset -= 1;
            ApplyText();
        }

        protected void summaryButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            if (BestiaryMain.AllText.Pages[currentPageIndex].PageSummary != null && currentEntryIndex != 69)
            {
                currentEntryIndex = 69;
                LoadSummary(false);
            }
        }
    }
}