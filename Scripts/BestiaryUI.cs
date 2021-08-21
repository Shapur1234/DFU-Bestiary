using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility.AssetInjection;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.UserInterface;

using UnityEngine;

namespace BestiaryMod
{
    class BestiaryUI : DaggerfallPopupWindow
    {
        Mod mod = ModManager.Instance.GetMod("Bestiary");

        public bool isShowing;
        bool reloadTexture;
        bool oldFont;
        bool classicMode;
        bool animate;

        int[] currentTexture = { 267, 0, 0};
        int textLabelXOffset;
        int maxTextureWidth;
        int maxTextureHeight;
        int descriptionLabelMaxCharacters;
        int defaultRotation;
        int contentOffset;
        int attackModeOffset;
        int animationUpdateDelay;
        
        public string currentEntry;
        public string currentPage;
        string currentPagePath;
        string currentSummary;
        string defaultEntry;
        string entryToLoad;

        const string rightArrowTextureName = "button_arrow_right";
        const string leftArrowTextureName = "button_arrow_left";
        const string blankTextureName = "blank";
        const string backgroundTextureName = "base_background";
        const string attackTrueTextureName = "button_attack_true";
        const string attackFalseTextureName = "button_attack_false";

        const string pathToClassicPage = "page_classic";
        string[] allPages = {"page_animals", "page_atronachs", "page_daedra", "page_lycanthropes", "page_monsters1", "page_monsters2", "page_orcs", "page_undead"};
        string[,] currentEntries = new string[9, 2];

        
        List<Texture2D> contentButtonTextures = new List<Texture2D>();
        Texture2D attackFalseTexture;
        Texture2D attackTrueTexture;
        Texture2D backgroundTexture;
        Texture2D blankTexture;
        Texture2D leftArrowTexture;
        Texture2D pictureTexture;
        Texture2D rightArrowTexture;

        List<Vector2> buttonAllPos = new List<Vector2> {new Vector2(4, 162), new Vector2(50, 162), new Vector2(95, 162), new Vector2(4, 174), new Vector2(50, 174), new Vector2(95, 174), new Vector2(4, 187), new Vector2(50, 187), new Vector2(95, 187)};
        Vector2 backgroundSizeVector;
        Vector2 entryButtonSize;
        Vector2 exitButtonSize;
        Vector2 pageNamePosVector;
        Vector2 pageNameSizeVector;
        Vector2 picturebackgroundPosVector;
        Vector2 picturebackgroundSizeVector;

        Panel imagePanel;
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
            LoadTextures();
            
            ModSettings settings = mod.GetSettings();
            
            string textPath = "";
            picturebackgroundSizeVector = new Vector2(102, 102);
            picturebackgroundPosVector = new Vector2(18, 51);
            pageNameSizeVector = new Vector2(52, 10);
            pageNamePosVector = new Vector2(71, 14);
            backgroundSizeVector = new Vector2(320, 200);

            animate = settings.GetBool("General", "EnableAnimations");
            animationUpdateDelay = settings.GetValue<int>("General", "DelayBetweenAnimationFrames");
            classicMode = settings.GetBool("General", "ClassicMode");
            defaultRotation = settings.GetValue<int>("General", "DefaultMobOrientation");
            oldFont = !DaggerfallUnity.Settings.SDFFontRendering;

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
                textPath = pathToClassicPage;
            else
                textPath = "page_animals";
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
            int newPosY = 0;
            int newPosX = 0;
            int newHeight = 0;
            float temp = 0;
            
            if (Input.GetKeyUp(exitKey))
                CloseWindow();

            if (animate == true)
            {
                if (currentTexture[0] == 284 && currentTexture[2] > (animationUpdateDelay * 3) - 1) 
                    currentTexture[2] = 0;
                
                if (currentTexture[2] % animationUpdateDelay == 0)
                {
                    if (currentTexture[1] < 4)
                    {
                        if(!TextureReplacement.TryImportTexture(currentTexture[0], currentTexture[1] + attackModeOffset, currentTexture[2] / animationUpdateDelay, TextureMap.Albedo, true, out pictureTexture))
                            pictureTexture = textureReader.GetTexture2D(currentTexture[0], currentTexture[1] + attackModeOffset, currentTexture[2] / animationUpdateDelay);
                    }
                    else
                    {
                        if(!TextureReplacement.TryImportTexture(currentTexture[0], 4 - (currentTexture[1] - 4) + attackModeOffset, currentTexture[2] / animationUpdateDelay, TextureMap.Albedo, true, out pictureTexture))
                            pictureTexture = textureReader.GetTexture2D(currentTexture[0], 4 - (currentTexture[1] - 4) + attackModeOffset, currentTexture[2] / animationUpdateDelay);
                    }
                    
                    reloadTexture = true;
                }

                if(currentTexture[0] == 255 && attackModeOffset == 0)
                {
                    if (currentTexture[2] < (animationUpdateDelay * 8) - 1)
                        currentTexture[2]++;
                    else
                        currentTexture[2] = 0;
                }
                else if(currentTexture[0] == 255 && attackModeOffset == 5)
                {
                    if (currentTexture[2] < (animationUpdateDelay * 6) - 1)
                        currentTexture[2]++;
                    else
                        currentTexture[2] = 0;
                }
                else
                {
                    if (currentTexture[2] < (animationUpdateDelay * 4) - 1)
                        currentTexture[2]++;
                    else
                        currentTexture[2] = 0;
                }
            }
            else
            {
                if (currentTexture[1] < 4)
                {
                    if(!TextureReplacement.TryImportTexture(currentTexture[0], currentTexture[1] + attackModeOffset - 4, 0, out pictureTexture)) 
                        pictureTexture = textureReader.GetTexture2D(currentTexture[0], currentTexture[1] + attackModeOffset - 4);
                }
                else
                {
                    if(!TextureReplacement.TryImportTexture(currentTexture[0], 4 - (currentTexture[1] - 4) + attackModeOffset, 0, out pictureTexture)) 
                        pictureTexture = textureReader.GetTexture2D(currentTexture[0], 4 - (currentTexture[1] - 4) + attackModeOffset);
                }
                
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
                        float temp2 = (float)maxTextureHeight / newHeight;
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
                        float temp2 = (float)maxTextureWidth / newWidth;
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

                if (currentTexture[1] < 4)
                    imagePanel.BackgroundTexture = pictureTexture;
                else
                    imagePanel.BackgroundTexture = FlipTexture(pictureTexture);
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
            backgroundTexture = DaggerfallUI.GetTextureFromResources(backgroundTextureName);
            blankTexture = DaggerfallUI.GetTextureFromResources(blankTextureName);

            if (!backgroundTexture)
                throw new Exception("BestiaryUI: Could not load backgroundTexture.");
            if (!blankTexture)
                throw new Exception("BestiaryUI: Could not load blankTexture.");
            
            if(!classicMode)
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
                contentOffset = 0;
            
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
                if (!string.IsNullOrEmpty(item)) 
                    entryTextTemp.Add(item);
            }

            result = entryTextTemp.ToArray();

            for (int i = 0; i < result.Length; i++)
            {

                switch (i)
                {
                    case 0:
                        if (result[3] == currentEntry)
                            break;
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
                        if (i < result.Length && !string.IsNullOrEmpty(result[i - 4])) 
                            textToApply[i - 4] = result[i];
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
                contentButtons[i].BackgroundTexture = blankTexture;
        }
        static string SplitLineToMultiline(string input, int rowLength) // taken from here: https://codereview.stackexchange.com/questions/54697/convert-string-to-multiline-text
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

        static string ReverseWords(string sentence)
        {
            string[] words = sentence.Split(' ');
            Array.Reverse(words);
            return string.Join(" ", words);
        }

        void applyText(string[] inputText)
        {
            List<List<string>> textTemp = new List<List<string>>();
            List<string> text = new List<string>();
            int displayedEntries = 0;
            int labelNumber = 0;
            var inputTextCleared = new List<string>();

            foreach (var item in inputText)
                if(!string.IsNullOrEmpty(item)) 
                    inputTextCleared.Add(item);

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
                    if (!string.IsNullOrEmpty(item)) 
                        temp.Add(item);

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
                contentOffset = 0;
            else if (text.Count - contentOffset <= 0)
                contentOffset = text.Count - 1;
            
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
            
            var pageTextTemp = new List<string>(); ;
            string[,] resultEntries = new string[9, 2];

            textAssetPage = mod.GetAsset<TextAsset>(path);
            var resultAssetPage = textAssetPage.text.Split(new[] { '\r', '\n' });

            currentSummary = resultAssetPage[2];
            for(int i = 0; i < resultAssetPage.Length; i++)
            {
                if (!string.IsNullOrEmpty(resultAssetPage[i]) && i != 1 && i != 2)
                    pageTextTemp.Add(resultAssetPage[i]);
            }
            resultAssetPage = pageTextTemp.ToArray();

            for (int i = 0; i < resultAssetPage.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        currentPage = resultAssetPage[0];
                        break;
                    default:
                        if (!string.IsNullOrEmpty(resultAssetPage[i]))
                        {
                            resultEntries[i - 1, 0] = resultAssetPage[i];
                            
                            textAssetEntry = mod.GetAsset<TextAsset>(resultEntries[i - 1, 0]);
                            var resultAssetEntry = textAssetEntry.text.Split(new[] { '\r', '\n' });
                            resultEntries[i - 1, 1] = resultAssetEntry[2];
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

            if(oldFont == false)
                titleLabel.Position = new Vector2(15, 16); 
            else 
                titleLabel.Position = new Vector2(15, 20);

            titleLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            titleLabel.Size = new Vector2(52, 22);
            titleLabel.Font = DaggerfallUI.TitleFont;
            
            if(oldFont == true)
                titleLabel.TextScale = 0.7f;
            
            mainPanel.Components.Add(titleLabel);

            monsterNameLabel = new TextLabel();
            monsterNameLabel.Position = new Vector2(144, 24);
            monsterNameLabel.Size = new Vector2(40, 14);
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
                    pageNameLabel.Font = DaggerfallUI.LargeFont;
                else 
                    pageNamePosVector[1] = 18;

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

        Texture2D FlipTexture(Texture2D original) //https://girlscancode.wordpress.com/2015/03/02/unity3d-flipping-a-texture/
        {
            Texture2D flipped = new Texture2D(original.width, original.height);
            flipped.filterMode = FilterMode.Point;


            int xN = original.width;
            int yN = original.height;

            for(int i = 0; i < xN; i++)
            {
                for(int j = 0; j < yN; j++)
                {
                    flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
                }
            }
            flipped.Apply();
            return flipped;
        }
        
        Texture2D DuplicateTexture(Texture2D source) //From here: https://stackoverflow.com/questions/44733841/how-to-make-texture2d-readable-via-script
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

            if(currentTexture[1] < 0) 
                currentTexture[1] = 7;
        }
        protected void LeftRotateButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            reloadTexture = true;
            currentTexture[1]++;

            if(currentTexture[1] > 7) 
                currentTexture[1] = 0;
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
                contentOffset -= 1;
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