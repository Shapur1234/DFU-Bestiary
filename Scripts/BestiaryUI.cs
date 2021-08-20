using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Utility.AssetInjection;

using UnityEngine;

namespace BestiaryMod
{
    class BestiaryUI : DaggerfallPopupWindow
    {
        Mod mod = ModManager.Instance.GetMod("Bestiary");

        public bool isShowing;
        bool animate;
        bool oldFont;
        bool classicMode;
        int animationUpdateDelay;

        int descriptionLabelMaxCharacters;
        int textLabelXOffset;
        int contentOffset;
        int maxTextureHeight;
        int maxTextureWidth;
        int attackModeOffset;
        int defaultRotation;

        public string currentEntry;
        public string currentPage;
        string currentSummary;
        string currentPagePath;
        bool reloadTexture;
        int[] currentTexture = { 267, 0, 0};

        const string backgroundTextureName = "base_background";
        const string rightArrowTextureName = "button_arrow_right";
        const string leftArrowTextureName = "button_arrow_left";
        const string attackTrueTextureName = "button_attack_true";
        const string attackFalseTextureName = "button_attack_false";
        const string blankTextureName = "blank";

        const string pathToClassicPage = "page_classic";
        string[] allPages = {"page_animals", "page_atronachs", "page_daedra", "page_lycanthropes", "page_monsters1", "page_monsters2", "page_orcs", "page_undead"};
        List<Vector2> buttonAllPos = new List<Vector2> {new Vector2(4, 162), new Vector2(50, 162), new Vector2(95, 162), new Vector2(4, 174), new Vector2(50, 174), new Vector2(95, 174), new Vector2(4, 187), new Vector2(50, 187), new Vector2(95, 187)};
        
        string defaultEntry;
        string entryToLoad;
        
        string[,] currentEntries = new string[9, 2];

        List<Texture2D> contentButtonTextures = new List<Texture2D>();
        Texture2D blankTexture;
        Texture2D leftArrowTexture;
        Texture2D rightArrowTexture;
        Texture2D backgroundTexture;
        Texture2D pictureTexture;
        Texture2D attackTrueTexture;
        Texture2D attackFalseTexture;

        Vector2 backgroundSizeVector;
        Vector2 picturebackgroundPosVector;
        Vector2 picturebackgroundSizeVector;
        Vector2 exitButtonSize;
        Vector2 entryButtonSize;
        Vector2 pageNamePosVector;
        Vector2 pageNameSizeVector;

        Panel mainPanel;
        Panel imagePanel;
        
        List<TextLabel> subtitleLabels = new List<TextLabel>();
        List<TextLabel> descriptionLabels = new List<TextLabel>();
        TextLabel pageNameLabel;
        TextLabel titleLabel;
        TextLabel monsterNameLabel;

        List<Button> contentButtons = new List<Button>();
        Button pageRightButton;
        Button pageLeftButton;
        Button rightRotateButton;
        Button leftRotateButton;
        Button attackButton;
        Button exitButton;
        Button summaryButton;

        public BestiaryUI(IUserInterfaceManager uiManager)
            : base(uiManager)
        {
            pauseWhileOpened = true;
            AllowCancel = false;
        }

        protected override void Setup()
        {
            base.Setup();
            LoadTextures();
            
            ModSettings settings = mod.GetSettings();
            
            string textPath = "";
            backgroundSizeVector = new Vector2(320, 200);
            picturebackgroundSizeVector = new Vector2(102, 102);
            picturebackgroundPosVector = new Vector2(18, 51);
            pageNamePosVector = new Vector2(71, 14);
            pageNameSizeVector = new Vector2(52, 10);

            classicMode = settings.GetBool("General", "ClassicMode");
            oldFont = !DaggerfallUnity.Settings.SDFFontRendering;
            animate = settings.GetBool("General", "EnableAnimations");
            animationUpdateDelay = settings.GetValue<int>("General", "DelayBetweenAnimationFrames");

            defaultRotation = settings.GetValue<int>("General", "DefaultMobOrientation");

            if (oldFont == false)
            {
                descriptionLabelMaxCharacters = 48;
                textLabelXOffset = 30;
            }
            else
            {
                descriptionLabelMaxCharacters = 24;
                textLabelXOffset = 46;
            }

            

            if (classicMode == true)
            {
                textPath = pathToClassicPage;
            }
            else
            {
                textPath = "page_animals";
            }
            currentEntries = getcurrentEntriesFromFile(textPath);

            setUpUIElements();
            loadPage();

            entryToLoad = currentSummary;
            loadContent(entryToLoad);
        }

        public override void Update()
        {
            base.Update();

            DaggerfallWorkshop.Utility.TextureReader textureReader = new DaggerfallWorkshop.Utility.TextureReader(DaggerfallUnity.Arena2Path);

            int newWidth = 0;
            int newHeight = 0;
            int newPosX = 0;
            int newPosY = 0;
            float temp = 0;
            
            if (Input.GetKeyUp(exitKey))
                CloseWindow();

            if (animate == true)
            {
                if (currentTexture[0] == 284 && currentTexture[2] > (animationUpdateDelay * 3) - 1) currentTexture[2] = 0;
                
                if (currentTexture[2] % animationUpdateDelay == 0)
                {
                    if(!TextureReplacement.TryImportTexture(currentTexture[0], currentTexture[1] + attackModeOffset, currentTexture[2] / animationUpdateDelay, out pictureTexture)) pictureTexture = textureReader.GetTexture2D(currentTexture[0], currentTexture[1] + attackModeOffset, currentTexture[2] / animationUpdateDelay);
                    reloadTexture = true;
                }

                if(currentTexture[0] == 255 && attackModeOffset == 0)
                {
                    if (currentTexture[2] < (animationUpdateDelay * 8) - 1)
                    {
                        currentTexture[2]++;
                    }
                    else
                    {
                        currentTexture[2] = 0;
                    }
                }
                else if(currentTexture[0] == 255 && attackModeOffset == 5)
                {
                    if (currentTexture[2] < (animationUpdateDelay * 6) - 1)
                    {
                        currentTexture[2]++;
                    }
                    else
                    {
                        currentTexture[2] = 0;
                    }
                }
                else
                {
                    if (currentTexture[2] < (animationUpdateDelay * 4) - 1)
                    {
                        currentTexture[2]++;
                    }
                    else
                    {
                        currentTexture[2] = 0;
                    }
                }
            }
            else
            {
                if(!TextureReplacement.TryImportTexture(currentTexture[0], currentTexture[1] + attackModeOffset, 0, out pictureTexture)) pictureTexture = textureReader.GetTexture2D(currentTexture[0], currentTexture[1] + attackModeOffset);
            }
            if(reloadTexture == true)
            {
                if (pictureTexture.height > pictureTexture.width)
                {
                    temp = picturebackgroundSizeVector[0] / pictureTexture.height;
                    newWidth = (int)Math.Round(temp * pictureTexture.width);
                    newHeight = (int)Math.Round(temp * pictureTexture.height);
                    if(newHeight > maxTextureHeight && attackModeOffset == 0)
                    {
                        double temp2 = (double)maxTextureHeight / newHeight;
                        newWidth = (int)Math.Round(temp2 * newWidth);
                        newHeight = maxTextureHeight;
                    }
                }
                else
                {
                    temp = picturebackgroundSizeVector[1] / pictureTexture.width;
                    newWidth = (int)Math.Round(temp * pictureTexture.width);
                    newHeight = (int)Math.Round(temp * pictureTexture.height);
                    if(newWidth > maxTextureWidth && attackModeOffset == 0)
                    {
                        double temp2 = (double)maxTextureWidth / newWidth;
                        newHeight = (int)Math.Round(temp2 * newHeight);
                        newWidth = maxTextureWidth;
                    }
                }
                newPosX = (int)picturebackgroundPosVector[0] + (((int)picturebackgroundSizeVector[0] - newWidth) / 2);
                newPosY = (int)picturebackgroundPosVector[1] + (((int)picturebackgroundSizeVector[1] - newHeight) / 2);
                imagePanel.Size = new Vector2(newWidth, newHeight);
                imagePanel.Position = new Vector2(newPosX, newPosY);

                reloadTexture = false;
                pictureTexture.filterMode = FilterMode.Point;
                imagePanel.BackgroundTexture = pictureTexture;
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

        void LoadTextures()
        {
            blankTexture = DaggerfallUI.GetTextureFromResources(blankTextureName);
            backgroundTexture = DaggerfallUI.GetTextureFromResources(backgroundTextureName);

            if (!blankTexture)
                throw new Exception("BestiaryUI: Could not load blankTexture.");
            if (!backgroundTexture)
                throw new Exception("BestiaryUI: Could not load backgroundTexture.");
            
            if(!classicMode)
            {
                leftArrowTexture = DaggerfallUI.GetTextureFromResources(leftArrowTextureName);
                rightArrowTexture = DaggerfallUI.GetTextureFromResources(rightArrowTextureName);
                attackTrueTexture = DaggerfallUI.GetTextureFromResources(attackTrueTextureName);
                attackFalseTexture = DaggerfallUI.GetTextureFromResources(attackFalseTextureName);

                if (!leftArrowTexture)
                    throw new Exception("BestiaryUI: Could not load leftArrowTexture.");
                if (!rightArrowTexture)
                    throw new Exception("BestiaryUI: Could not load rightArrowTexture.");
                if (!attackTrueTexture)
                    throw new Exception("BestiaryUI: Could not load attackTrueTexture.");
                if (!attackFalseTexture)
                    throw new Exception("BestiaryUI: Could not load attackFalseTexture.");
            }
            for (int i = 0; i < 9; i++)
            {
                contentButtonTextures.Add(DaggerfallUI.GetTextureFromResources(blankTextureName));
            }
        }

        
        void loadPage()
        {  
            defaultEntry = currentSummary;

            for (int i = 0; i < contentButtonTextures.Count; i++)
            {
                if (!String.IsNullOrEmpty(currentEntries[i, 1]))
                {
                    contentButtonTextures[i] = DaggerfallUI.GetTextureFromResources(currentEntries[i, 1]);
                    if(!contentButtonTextures[i])
                        throw new Exception("BestiaryUI: Could not load contentButtonTextures" + (i) + ".");

                    contentButtons[i].BackgroundTexture = contentButtonTextures[i];
                }
            }
            resetTextLabels();
        }
        void loadContent(string assetPath, bool summary = false, bool reset = true)
        {
            Texture2D tempTexture;
            
            resetTextLabels();
            attackModeOffset = 0;
            attackButton.BackgroundTexture = attackFalseTexture;

            if (reset == true)
            {
                contentOffset = 0;
            }
            
            if (classicMode == false)
            {
                pageNameLabel.Text = currentPage;
                pageNameLabel.Position = new Vector2(pageNamePosVector[0] + ((pageNameSizeVector[0] - pageNameLabel.TextWidth) / 2), pageNamePosVector[1]);

                summaryButton.Position = pageNameLabel.Position;
                summaryButton.Size = new Vector2(pageNameLabel.TextWidth, pageNameSizeVector[1]);
            }
            string entryText = "";
            var entryTextTemp = new List<string>(); ;

            TextAsset textAsset = mod.GetAsset<TextAsset>(assetPath);
            entryText = textAsset.text;
            var result = entryText.Split(new[] { '\r', '\n' });
            string[] textToApply = new string[result.Length - 4];

            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item)) entryTextTemp.Add(item);
            }

            result = entryTextTemp.ToArray();

            for (int i = 0; i < result.Length; i++)
            {

                switch (i)
                {
                    case 0:
                        if (result[3] == currentEntry)
                        {
                            break;
                        }
                        currentTexture[0] = int.Parse(result[0]);
                        currentTexture[1] = defaultRotation;
                        currentTexture[2] = 0;

                        DaggerfallWorkshop.Utility.TextureReader textureReader = new DaggerfallWorkshop.Utility.TextureReader(DaggerfallUnity.Arena2Path);
                        tempTexture = textureReader.GetTexture2D(currentTexture[0], 1);

                        float temp = 0;

                        if (tempTexture.height > tempTexture.width)
                        {
                            temp = picturebackgroundSizeVector[0] / tempTexture.height;
                            maxTextureHeight = (int)Math.Round(temp * tempTexture.height);
                            maxTextureWidth = (int)Math.Round(temp * tempTexture.width);
                        }
                        else
                        {
                            temp = picturebackgroundSizeVector[1] / tempTexture.width;
                            maxTextureHeight = (int)Math.Round(temp * tempTexture.height);
                            maxTextureWidth = (int)Math.Round(temp * tempTexture.width);
                        }
                        reloadTexture = true;
                        break;
                    case 1:
                        break;
                    case 2:
                        titleLabel.Text = result[2];
                        break;
                    case 3:
                        if (oldFont)
                        {
                            monsterNameLabel.Text = result[3].Replace(" - ", " ");
                            monsterNameLabel.TextScale = 0.85f;
                        }
                        else
                            monsterNameLabel.Text = result[3];
                        currentEntry = result[3];
                        break;
                    default:
                        if (i < result.Length && !string.IsNullOrEmpty(result[i - 4])) textToApply[i - 4] = result[i];
                        break;
                }
            }
            applyText(textToApply);
        }

        void resetTextLabels()
        {
            for (int i = 0; i < subtitleLabels.Count; i++)
            {
                subtitleLabels[i].Text = "";
                descriptionLabels[i].Text = "";
            }  
        }
        void resetButtonTextures()
        {
            for (int i = 0; i < contentButtons.Count; i++)
            {
                contentButtons[i].BackgroundTexture = blankTexture;
            }
        }
        static string SplitLineToMultiline(string input, int rowLength) // taken from here: https://codereview.stackexchange.com/questions/54697/convert-string-to-multiline-text
        {
            StringBuilder result = new StringBuilder();
            StringBuilder line = new StringBuilder();

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

        static string ReverseWords(string sentence)
        {
            string[] words = sentence.Split(' ');
            Array.Reverse(words);
            return string.Join(" ", words);
        }

        void applyText(string[] inputText)
        {
            int labelNumber = 0;
            int displayedEntries = 0;
            var inputTextCleared = new List<string>();
            List<List<string>> textTemp = new List<List<string>>();
            List<string> text = new List<string>();

            foreach (var item in inputText)
            {
                if(!string.IsNullOrEmpty(item)) inputTextCleared.Add(item);
            }

            foreach (var item in inputTextCleared)
            {
                var splitResult = item.Split(new[] { '*' });
                textTemp.Add(new List<string> {splitResult[0], splitResult[1]});
            }

            for (int i = 0; i < textTemp.Count; i++)
            {
                bool first = true;
                var temp = new List<string>();
                var multiline_text = SplitLineToMultiline(ReverseWords(textTemp[i][1]), descriptionLabelMaxCharacters);
                var singlelineText = new List<string>();
                var singleline_text_temp = multiline_text.Split(new[] { '\r', '\n' });

                foreach (var item in singleline_text_temp)
                {
                    if (!string.IsNullOrEmpty(item)) temp.Add(item);
                }

                singlelineText = temp;

                foreach (var item in singlelineText)
                {
                    if (first == true)
                    {
                        text.Add(textTemp[i][0] + "&" + item);
                        first = false;
                    }
                    else
                    {
                        text.Add(item);
                    } 
                }
            }
            if (text.Count < 15)
            {
                contentOffset = 0;
            }
            else if (text.Count - contentOffset <= 0)
            {
                contentOffset = text.Count - 1;
            }
            for (int i = contentOffset; i < text.Count && labelNumber < 14; i++)
            {
                string[] multiItem = text[i].Split('&');

                if (multiItem.Length > 1)
                {
                    subtitleLabels[labelNumber].Text = multiItem[0];
                    descriptionLabels[labelNumber].Text = multiItem[1];

                    displayedEntries++;
                }
                else
                {
                    descriptionLabels[labelNumber].Text = multiItem[0];

                    displayedEntries++;
                }

                labelNumber++;
            }
        }

        string[,] getcurrentEntriesFromFile(string path)
        {
            currentPagePath = path;
            TextAsset textAssetPage;
            TextAsset textAssetEntry;
            
            string entryText = "";
            string entry2Text = "";
            string[,] resultEntries = new string[9, 2];
            var pageTextTemp = new List<string>(); ;

            textAssetPage = mod.GetAsset<TextAsset>(path);
            entryText = textAssetPage.text;
            var result = entryText.Split(new[] { '\r', '\n' });

            currentSummary = result[2];
            for(int i = 0; i < result.Length; i++)
            {
                if (!string.IsNullOrEmpty(result[i]) && i != 1 && i != 2)
                {
                    pageTextTemp.Add(result[i]);
                }
            }
            result = pageTextTemp.ToArray();

            for (int i = 0; i < result.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        currentPage = result[0];
                        break;
                    default:
                        if (!string.IsNullOrEmpty(result[i]))
                        {
                            resultEntries[i - 1, 0] = result[i];
                            
                            textAssetEntry = mod.GetAsset<TextAsset>(resultEntries[i - 1, 0]);
                            entry2Text = textAssetEntry.text;   
                            var result0 = entry2Text.Split(new[] { '\r', '\n' });
                            resultEntries[i - 1, 1] = result0[2];
                        }
                        else
                        {
                            resultEntries[i - 1, 0] = "";
                            resultEntries[i - 1, 1] = "";
                        }
                        break;
                }
            }
            return resultEntries;
        }
        // Texture2D FlipTexture(Texture2D original)
        // {
        //     Texture2D flipped = new Texture2D(original.width, original.height);
        //     flipped.filterMode = FilterMode.Point;


        //     int xN = original.width;
        //     int yN = original.height;

        //     for(int i = 0; i < xN; i++)
        //     {
        //         for(int j = 0; j < yN; j++)
        //         {
        //             flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
        //         }
        //     }
        //     flipped.Apply();
        //     return flipped;
        // }

        void setUpUIElements()
        {
            exitButtonSize = new Vector2(30, 9);
            entryButtonSize = new Vector2(40, 9);

            ParentPanel.BackgroundColor = ScreenDimColor;

            mainPanel = DaggerfallUI.AddPanel(NativePanel, AutoSizeModes.None);
            mainPanel.Size = backgroundSizeVector;
            mainPanel.BackgroundTexture = backgroundTexture;
            mainPanel.HorizontalAlignment = HorizontalAlignment.Center;
            mainPanel.VerticalAlignment = VerticalAlignment.Middle;
            mainPanel.OnMouseScrollDown += MainPanel_OnMouseScrollDown;
            mainPanel.OnMouseScrollUp += MainPanel_OnMouseScrollUp;

            imagePanel = DaggerfallUI.AddPanel(mainPanel, AutoSizeModes.None);
            imagePanel.Size = picturebackgroundSizeVector;
            imagePanel.BackgroundTexture = pictureTexture;
            imagePanel.Position = picturebackgroundPosVector;

            titleLabel = new TextLabel();
            if(oldFont == false)    titleLabel.Position = new Vector2(15, 16); else titleLabel.Position = new Vector2(15, 20);
            
            titleLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            titleLabel.Size = new Vector2(52, 22);
            titleLabel.Name = "title_label";
            titleLabel.Font = DaggerfallUI.TitleFont;
            if(oldFont == true) titleLabel.TextScale = 0.7f;

            mainPanel.Components.Add(titleLabel);

            monsterNameLabel = new TextLabel();
            monsterNameLabel.Position = new Vector2(144, 24);
            monsterNameLabel.Size = new Vector2(40, 14);
            monsterNameLabel.Name = "sub_label_1";
            monsterNameLabel.Font = DaggerfallUI.LargeFont;
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
                summaryButton.Position = pageNamePosVector;
                summaryButton.Size = pageNameSizeVector;
                summaryButton.OnMouseClick += summaryButton_OnMouseClick;
                mainPanel.Components.Add(summaryButton);

                pageNameLabel = new TextLabel();
                pageNameLabel.Position = pageNamePosVector;
                pageNameLabel.Size = pageNameSizeVector;
                if(oldFont == false)
                {
                    pageNameLabel.Font = DaggerfallUI.LargeFont;
                }
                else pageNamePosVector[1] = 18;

                pageNameLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
                mainPanel.Components.Add(pageNameLabel);

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
            }
        }

        void changePage(bool right)
        {   
            int currentEntryNum = Array.IndexOf(allPages, currentPagePath);
            
            if (right == true && currentEntryNum > 0)
            {
                currentEntryNum -= 1;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetButtonTextures();
                loadPage();

                entryToLoad = currentSummary;
                loadContent(entryToLoad, true, true);
            }
            else if (right == true && currentEntryNum <= 0)
            {
                currentEntryNum = allPages.Length - 1;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetButtonTextures();
                loadPage();

                entryToLoad = currentSummary;
                loadContent(entryToLoad, true, true);
            }
            else if (right == false && currentEntryNum < (allPages.Length - 1))
            {
                currentEntryNum += 1;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetButtonTextures();
                loadPage();

                entryToLoad = currentSummary;
                loadContent(entryToLoad, true, true);
            }
            else
            {
                currentEntryNum = 0;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetButtonTextures();
                loadPage();

                entryToLoad = currentSummary;
                loadContent(entryToLoad, true, true);
            }
        }
        protected void ExitButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            CloseWindow();
        }

        protected void ContentButton0_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[0, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[0, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton1_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[1, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[1, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton2_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[2, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[2, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton3_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[3, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[3, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton4_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[4, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[4, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton5_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[5, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[5, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton6_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[6, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[6, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton7_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[7, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[7, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton8_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[8, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[8, 0];
                loadContent(entryToLoad);
            }
        }
        protected void pageRightButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            changePage(false);
        }
        protected void pageLeftButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            changePage(true);
        }
        
        protected void RightRotateButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            reloadTexture = true;
            
            currentTexture[1]--;

            if(currentTexture[1] < 0) currentTexture[1] = 4;
        }
        protected void LeftRotateButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            reloadTexture = true;
            currentTexture[1]++;

            if(currentTexture[1] > 4) currentTexture[1] = 0;
        }
        protected void AttackButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            if(attackModeOffset == 0)
            {
                attackModeOffset = 5;
                attackButton.BackgroundTexture = attackTrueTexture;
            }
            else
            {
                attackModeOffset = 0;
                attackButton.BackgroundTexture = attackFalseTexture;
            }
        }
        protected void MainPanel_OnMouseScrollDown(BaseScreenComponent sender)
        {
            contentOffset += 1;
            loadContent(entryToLoad, false, false);
        }
        protected void MainPanel_OnMouseScrollUp(BaseScreenComponent sender)
        {
            if (contentOffset > 0)
            {
                contentOffset -= 1;
            }
            loadContent(entryToLoad, false, false);
        }

        protected void summaryButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentSummary))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentSummary;
                loadContent(entryToLoad, true);
            }
        }
    }
}