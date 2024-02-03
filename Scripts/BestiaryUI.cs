using System;
using System.Collections.Generic;
using System.IO;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Utility.AssetInjection;

using UnityEngine;

// ReSharper disable CheckNamespace
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

                Texture2D tempTexture;

                if (!TextureReplacement.TryImportTexture(Archive, 1, 0, out tempTexture))
                {
                    tempTexture = textureReader.GetTexture2D(Archive, 1, 0);
                }

                if (tempTexture.width > tempTexture.height)
                    maxTextureSize = new Vector2(imagePanelMaxSize[0], tempTexture.height);
                else
                    maxTextureSize = new Vector2(tempTexture.width, imagePanelMaxSize[1]);

                attackButton.BackgroundTexture = attackFalseTexture;

                UpdateTextures();
            }

            public bool ReadTexture(int archive, int record, int frame, out Texture2D tex)
            {
                Texture2D externalTexture;
                Texture2D originalTexture = null;

                try
                {
                    originalTexture = textureReader.GetTexture2D(archive, record, frame);
                }
                catch
                {
                    // No origianl texture
                }

                TextureReplacement.TryImportTexture(archive, record, frame, out externalTexture);

                if (externalTexture != null)
                {
                    tex = externalTexture;
                    return true;
                }


                if (originalTexture != null)
                {
                    tex = originalTexture;
                    return true;
                }

                tex = null;
                return false;
            }

            public void UpdateTextures()
            {
                Texture2D pictureTexture = null;

                // It's original
                // use normal way to get frame per record count
                if (Archive <= 511)
                {
                    var filePath = Path.Combine(arena2Path, TextureFile.IndexToFileName(Archive));

                    var framesForRecord = new TextureFile(filePath, FileUsage.Undefined, true).GetFrameCount(Record + AttackModeOffset);

                    if (Frame >= framesForRecord)
                        Frame = 0;
                }
                // Otherwise, check what files are available in the injected assets
                else
                {
                    Texture2D[] texFrames;
                    TextureReplacement.TryImportTexture(Archive, Record + AttackModeOffset, out texFrames);
                    if (Frame >= texFrames.Length)
                        Frame = 0;

                    pictureTexture = texFrames[Frame];
                }

                // Don't read again if we already loaded replacement
                if (pictureTexture == null)
                    ReadTexture(Archive, Record + AttackModeOffset, Frame, out pictureTexture);

                // Let ppl deside if they want to use blurred images
                pictureTexture.filterMode = (FilterMode)DaggerfallUnity.Settings.GUIFilterMode;

                UpdateImagePanel(this);
                ApplyTexture(pictureTexture, this);
                return;
            }

            private void UpdateImagePanel(TextureInfo inputTextureInfo)
            {
                imagePanel.Size = inputTextureInfo.maxTextureSize;
                imagePanel.Position = new Vector2(imagePanelBasePos[0] + ((imagePanelMaxSize[0] - imagePanel.Size[0]) / 2), imagePanelBasePos[1] + ((imagePanelMaxSize[0] - imagePanel.Size[1]) / 2));
            }

            private void ApplyTexture(Texture2D texture, TextureInfo inputTextureInfo)
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
        private readonly List<Vector2> buttonAllPos = new List<Vector2> {
            new Vector2(5, 162),
            new Vector2(48, 162),
            new Vector2(91, 162),
            new Vector2(5, 174),
            new Vector2(48, 174),
            new Vector2(91, 174),
            new Vector2(5, 186),
            new Vector2(48, 186),
            new Vector2(91, 186)
        };
        private readonly Vector2 entryButtonSize = new Vector2(40, 9);
        private readonly Vector2 pageNameSize = new Vector2(52, 10);
        private static readonly Vector2 imagePanelBasePos = new Vector2(18, 51);
        private static readonly Vector2 imagePanelMaxSize = new Vector2(102, 102);

        const float scrollAmount = 24;
        Vector2 pagePanelPositionClassic = new Vector2(148, 40);
        Vector2 pagePanelSizeClassic = new Vector2(159, 138);
        Vector2 pagePanelPositionSDF = new Vector2(143, 40);
        Vector2 pagePanelSizeSDF = new Vector2(169, 138);
        Vector2 pagePanelPosition = Vector2.zero;
        Vector2 pagePanelSize = Vector2.zero;

        static Panel imagePanel;
        private Panel mainPanel;

        List<TextLabel> bookLabels = new List<TextLabel>();
        Panel pagePanel = null;
        Panel monsterNamePanel = null;
        float maxHeight = 0;
        float scrollPosition = 0;

        private TextLabel monsterNameLabel;
        private TextLabel pageNameLabel;
        private TextLabel titleLabel;

        private List<Button> contentButtons = new List<Button>();
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
        private int animationDelay;
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

            if (Input.GetKeyUp(KeyCode.Escape))
                CloseWindow();

            if (BestiaryMain.SettingAnimate && currentTexture != null)
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
            if (IsSetup)
            {
                LayoutBookLabels();
            }
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
                SetActiveTextureForButton(0);
                LoadEntry();
            }
            else
            {
                LoadSummary();
                UpdatePageNameLabel();
            }

        }

        private void ResetButtonTextures()
        {
            foreach (var t in contentButtons)
                t.BackgroundTexture = blankTexture;
        }

        private void LoadButtons()
        {
            if (BestiaryMain.AllText.Pages.Count < currentPageIndex + 1) return;

            for (int i = 0; i < BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count && i < contentButtonTextures.Count; i++)
            {
                contentButtonTextures[i] = DaggerfallUI.GetTextureFromResources(BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[i].ButtonTextureName);
                if (!contentButtonTextures[i])
                    throw new Exception("BestiaryUI: Could not load contentButtonTextures" + (i) + ".");

                contentButtons[i].BackgroundTexture = contentButtonTextures[i];
            }
        }

        private void UpdatePageNameLabel()
        {
            if (BestiaryMain.AllText.Pages.Count < currentPageIndex + 1) return;

            pageNameLabel.Text = BestiaryMain.AllText.Pages[currentPageIndex].Title;
            pageNameLabel.Position = new Vector2(pageNamePos[0] + ((pageNameSize[0] - pageNameLabel.TextWidth) / 2), pageNamePos[1]);
        }
        private void LoadSummary(bool updateTexture = true)
        {
            if (BestiaryMain.AllText.Pages.Count < currentPageIndex + 1) return;

            currentEntryIndex = 69;

            var imageIndexToLoad = UnityEngine.Random.Range(0, BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count);
            if (updateTexture && currentTexture == null || BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[imageIndexToLoad].TextureArchive != currentTexture.Archive)
                currentTexture = new TextureInfo(BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[imageIndexToLoad].TextureArchive);

            monsterNameLabel.Text = BestiaryMain.AllText.Pages[currentPageIndex].PageSummary.Title;

            bookLabels.Clear();
            bookLabels = BestiaryMain.AllText.Pages[currentPageIndex].PageSummary.TextLabels;
            LayoutBookLabels();
        }
        private void LoadEntry()
        {
            if (currentTexture == null || BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[currentEntryIndex].TextureArchive != currentTexture.Archive)
                currentTexture = new TextureInfo(BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[currentEntryIndex].TextureArchive);

            monsterNameLabel.Text = BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[currentEntryIndex].Title;

            bookLabels.Clear();
            bookLabels = BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[currentEntryIndex].TextLabels;
            LayoutBookLabels();
        }

        private void SetUpUIElements()
        {
            ParentPanel.BackgroundColor = ScreenDimColor;

            // Use smaller margins for SDF book text and wider margins for non-SDF (classic pixel font) text
            if (DaggerfallUnity.Settings.SDFFontRendering)
            {
                pagePanelPosition = pagePanelPositionSDF;
                pagePanelSize = pagePanelSizeSDF;
            }
            else
            {
                pagePanelPosition = pagePanelPositionClassic;
                pagePanelSize = pagePanelSizeClassic;
            }

            mainPanel = DaggerfallUI.AddPanel(NativePanel, AutoSizeModes.None);
            mainPanel.Size = new Vector2(320, 200);
            mainPanel.BackgroundTexture = backgroundTexture;
            mainPanel.VerticalAlignment = VerticalAlignment.Middle;
            mainPanel.HorizontalAlignment = HorizontalAlignment.Center;

            // Setup panel to contain text labels
            pagePanel = DaggerfallUI.AddPanel(mainPanel, AutoSizeModes.None);
            pagePanel.Position = pagePanelPosition;
            pagePanel.Size = pagePanelSize;
            pagePanel.RectRestrictedRenderArea = new Rect(pagePanel.Position, pagePanel.Size);

            // Setup panel to contain header
            monsterNamePanel = DaggerfallUI.AddPanel(mainPanel, AutoSizeModes.None);
            monsterNamePanel.Position = new Vector2(144, 23);
            monsterNamePanel.Size = new Vector2(159, 30);
            monsterNamePanel.RectRestrictedRenderArea = new Rect(monsterNamePanel.Position, monsterNamePanel.Size);

            NativePanel.OnMouseScrollDown += Panel_OnMouseScrollDown;
            NativePanel.OnMouseScrollUp += Panel_OnMouseScrollUp;
            LayoutBookLabels();

            imagePanel = DaggerfallUI.AddPanel(mainPanel, AutoSizeModes.None);
            imagePanel.Size = imagePanelMaxSize;
            imagePanel.Position = imagePanelBasePos;
            imagePanel.BackgroundTextureLayout = BackgroundLayout.ScaleToFit;

            titleLabel = new TextLabel();
            titleLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            titleLabel.Size = new Vector2(52, 22);
            titleLabel.Font = DaggerfallUI.TitleFont;
            titleLabel.Text = BestiaryMain.AllText.BestiaryTitle;
            titleLabel.TextColor = BestiaryMain.SettingHeaderFontColor;
            titleLabel.ShadowColor = BestiaryMain.SettingHeaderFontShadowColor;

            if (DaggerfallUnity.Settings.SDFFontRendering)
                titleLabel.Position = new Vector2(15, 16);
            else
            {
                titleLabel.Position = new Vector2(15, 20);
                titleLabel.TextScale = 0.7f;
            }
            mainPanel.Components.Add(titleLabel);

            monsterNameLabel = new TextLabel();
            monsterNameLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            monsterNameLabel.HorizontalAlignment = HorizontalAlignment.Center;
            monsterNameLabel.Font = DaggerfallUI.TitleFont;
            monsterNameLabel.TextColor = BestiaryMain.SettingHeaderFontColor;
            monsterNameLabel.ShadowColor = BestiaryMain.SettingHeaderFontShadowColor;
            monsterNameLabel.Position = Vector2.zero;
            monsterNameLabel.MaxWidth = (int)monsterNamePanel.Size.x;
            monsterNameLabel.RectRestrictedRenderArea = monsterNamePanel.RectRestrictedRenderArea;
            monsterNameLabel.RestrictedRenderAreaCoordinateType = TextLabel.RestrictedRenderArea_CoordinateType.ParentCoordinates;

            monsterNamePanel.Components.Add(monsterNameLabel);

            if (!DaggerfallUnity.Settings.SDFFontRendering)
            {
                monsterNameLabel.TextScale = 0.80f;
                monsterNameLabel.Font = DaggerfallUI.LargeFont;
            }

            exitButton = new Button();
            exitButton.Position = new Vector2(216, 187);
            exitButton.Size = entryButtonSize;
            exitButton.OnMouseClick += ExitButton_OnMouseClick;
            mainPanel.Components.Add(exitButton);

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

            pageNameLabel = new TextLabel();
            pageNameLabel.Position = pageNamePos;
            pageNameLabel.Size = pageNameSize;
            pageNameLabel.TextColor = BestiaryMain.SettingHeaderFontColor;
            pageNameLabel.ShadowColor = BestiaryMain.SettingHeaderFontShadowColor;

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
            Texture2D flipped = new Texture2D(original.width, original.height, original.format, original.mipmapCount > 1);
            // Let ppl deside if they want to use blurred images
            flipped.filterMode = (FilterMode)DaggerfallUnity.Settings.GUIFilterMode;

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
            // RenderTextureFormat.Default, RenderTextureReadWrite.Linear causing textures to go dark
            // Create a RenderTexture to temporarily render the unreadable texture
            RenderTexture renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);

            // Set the active RenderTexture and draw the unreadable texture onto it
            RenderTexture.active = renderTex;
            Graphics.Blit(source, renderTex);

            // Create a new texture and read the pixels from the RenderTexture to copy the data
            Texture2D readableText = new Texture2D(source.width, source.height, TextureFormat.ARGB32, source.mipmapCount > 1)
            {
                // Need to set here too. Otherwise it takes some default value
                filterMode = (FilterMode)DaggerfallUnity.Settings.GUIFilterMode,
            };
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();

            // Release the temporary RenderTexture
            RenderTexture.active = null;
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
            DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
            CloseWindow();
        }

        protected void SetActiveTextureForButton(short i)
        {
            LoadButtons();
            var activeTexture = DaggerfallUI.GetTextureFromResources(BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries()[i].ButtonTextureName + "_active");
            if (activeTexture != null)
            {
                contentButtons[i].BackgroundTexture = activeTexture;
            }
        }

        private void Panel_OnMouseScrollDown(BaseScreenComponent sender)
        {
            ScrollBook(-scrollAmount);
        }

        private void Panel_OnMouseScrollUp(BaseScreenComponent sender)
        {
            ScrollBook(scrollAmount);
        }

        void ScrollBook(float amount)
        {
            // Stop scrolling at top or bottom of book layout
            if (amount < 0 && scrollPosition - pagePanel.Size.y - amount < -maxHeight)
                return;
            else if (amount > 0 && scrollPosition == 0)
                return;

            // Scroll label and only draw what can be seen
            scrollPosition += amount;
            foreach (TextLabel label in bookLabels)
            {
                label.Position = new Vector2(label.Position.x, label.Position.y + amount);
                label.Enabled = label.Position.y < pagePanel.Size.y && label.Position.y + label.Size.y > 0;
            }
        }

        void LayoutBookLabels()
        {
            maxHeight = 0;
            scrollPosition = 0;
            pagePanel.Components.Clear();
            float x = 0, y = 0;
            foreach (TextLabel label in bookLabels)
            {
                label.Position = new Vector2(x, y);
                label.MaxWidth = (int)pagePanel.Size.x;
                label.RectRestrictedRenderArea = pagePanel.RectRestrictedRenderArea;
                label.RestrictedRenderAreaCoordinateType = TextLabel.RestrictedRenderArea_CoordinateType.ParentCoordinates;
                pagePanel.Components.Add(label);
                y += label.Size.y;
                maxHeight += label.Size.y;
            }

            // Reenable labels after scrollPosition reset
            ScrollBook(0);
        }


        protected void ContentButton0_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 0)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 0)
                {
                    currentEntryIndex = 0;
                    SetActiveTextureForButton(0);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton1_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 1)
                {
                    currentEntryIndex = 1;
                    SetActiveTextureForButton(1);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton2_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 2)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 2)
                {
                    currentEntryIndex = 2;
                    SetActiveTextureForButton(2);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton3_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 3)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 3)
                {
                    currentEntryIndex = 3;
                    SetActiveTextureForButton(3);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton4_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 4)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 4)
                {
                    currentEntryIndex = 4;
                    SetActiveTextureForButton(4);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton5_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 5)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 5)
                {
                    currentEntryIndex = 5;
                    SetActiveTextureForButton(5);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton6_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 6)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 6)
                {
                    currentEntryIndex = 6;
                    SetActiveTextureForButton(6);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton7_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 7)
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                if (currentEntryIndex != 7)
                {
                    currentEntryIndex = 7;
                    SetActiveTextureForButton(7);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void ContentButton8_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages[currentPageIndex].FilterEntries().Count > 8)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                if (currentEntryIndex != 8)
                {
                    currentEntryIndex = 8;
                    SetActiveTextureForButton(8);
                    LoadEntry();
                }
                else
                {
                    LoadPage();
                }
            }
        }
        protected void pageRightButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages.Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                ChangePage(true);
            }
        }

        protected void pageLeftButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if (BestiaryMain.AllText.Pages.Count > 1)
            {
                DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
                ChangePage(false);
            }
        }

        protected void RightRotateButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
            if (currentTexture != null)
                currentTexture.DecreaseRecord();
        }

        protected void LeftRotateButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
            if (currentTexture != null)
                currentTexture.IncreaseRecord();
        }

        protected void AttackButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
            if (currentTexture == null)
            {
                return;
            }

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
    }
}