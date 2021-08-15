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
        Mod dreamMobs = ModManager.Instance.GetMod("DREAM - MOBS");
        // Settings
        public bool isShowing = false;
        public bool animate;
        public bool oldFont = false;
        public bool classicMode = false;
        int animationUpdateDelay = 45;
        int descriptionLableMaxCharacters;
        int textLabelXOffset;
        int contentOffset;
        int maxTextureHeight = 0;
        int maxTextureWidth = 0;
        int attackModeOffset = 0;

        public string currentEntry = "";
        public string currentPage = "";
        string currentPagePath = "";
        string textureSuffix = "";
        bool reloadTexture = false;
        int[] currentTexture = { 267, 0, 0};

        // Texture names
        const string backgroundTextureName = "base_background";
        const string attackTrueTextureName = "button_attack_true";
        const string attackFalseTextureName = "button_attack_false";
        // Path to pages
        const string pathToClassicPage = "page_classic";
        string[] allPages = {"page_animals", "page_atronachs", "page_daedra"};
        
        string defaultEntry;
        string entryToLoad;
        
        string[,] currentEntries = new string[9, 2];

        Texture2D blankTexture;
        Texture2D leftArrowTexture;
        Texture2D rightArrowTexture;
        Texture2D backgroundTexture;
        Texture2D pictureTexture;
        Texture2D attackTrueTexture;
        Texture2D attackFalseTexture;
        Texture2D contentButtonTexture1;
        Texture2D contentButtonTexture2;
        Texture2D contentButtonTexture3;
        Texture2D contentButtonTexture4;
        Texture2D contentButtonTexture5;
        Texture2D contentButtonTexture6;
        Texture2D contentButtonTexture7;
        Texture2D contentButtonTexture8;
        Texture2D contentButtonTexture9;

        Vector2 backgroundSizeVector;
        Vector2 picturebackgroundPosVector;
        Vector2 picturebackgroundSizeVector;
        Vector2 exitButtonSize;
        Vector2 entryButtonSize;
        Vector2 pageNamePosVector;
        Vector2 pageNameSizeVector;

        #region UI Controls
        Panel mainPanel;
        Panel imagePanel;
        TextLabel pageNameLabel;
        TextLabel titleLable;
        TextLabel monsterNameLabel;
        TextLabel subTitleLable1;
        TextLabel subTitleLable2;
        TextLabel subTitleLable3;
        TextLabel subTitleLable4;
        TextLabel subTitleLable5;
        TextLabel subTitleLable6;
        TextLabel subTitleLable7;
        TextLabel subTitleLable8;
        TextLabel subTitleLable9;
        TextLabel subTitleLable10;
        TextLabel subTitleLable11;
        TextLabel subTitleLable12;
        TextLabel subTitleLable13;
        TextLabel subTitleLable14;

        TextLabel descriptionLable1;
        TextLabel descriptionLable2;
        TextLabel descriptionLable3;
        TextLabel descriptionLable4;
        TextLabel descriptionLable5;
        TextLabel descriptionLable6;
        TextLabel descriptionLable7;
        TextLabel descriptionLable8;
        TextLabel descriptionLable9;
        TextLabel descriptionLable10;
        TextLabel descriptionLable11;
        TextLabel descriptionLable12;
        TextLabel descriptionLable13;
        TextLabel descriptionLable14;

        
        Button rightArrowButton;
        Button leftArrowButton;
        Button rightRotateButton;
        Button leftRotateButton;
        Button attackButton;
        Button exitButton;
        Button contentButton3;
        Button contentButton1;
        Button contentButton2;
        Button contentButton4;
        Button contentButton5;
        Button contentButton6;
        Button contentButton7;
        Button contentButton8;
        Button contentButton9;
        

        #endregion
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

            if (oldFont == false)
            {
                descriptionLableMaxCharacters = 48;
                textLabelXOffset = 0;
            }
            else
            {
                descriptionLableMaxCharacters = 24;
                textLabelXOffset = 16;
            }

            setUpUIElements();

            if (classicMode == true)
            {
                textPath = pathToClassicPage;
            }
            else
            {
                textPath = "page_animals";
                LoadArrowsNStuff();
            }
            
            currentEntries = getcurrentEntriesFromFile(textPath);

            loadPage();

            entryToLoad = currentEntries[0, 0];
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
                if (currentTexture[2] % animationUpdateDelay == 0)
                {
                    if(!TextureReplacement.TryImportTexture(currentTexture[0], currentTexture[1] + attackModeOffset, currentTexture[2] / animationUpdateDelay, out pictureTexture)) pictureTexture = textureReader.GetTexture2D(currentTexture[0], currentTexture[1] + attackModeOffset, currentTexture[2] / animationUpdateDelay);
                    reloadTexture = true;
                }

                if (currentTexture[2] < (animationUpdateDelay * 4) - 1)
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
            blankTexture = DaggerfallUI.GetTextureFromResources("blank");
            attackTrueTexture = DaggerfallUI.GetTextureFromResources(attackTrueTextureName);
            attackFalseTexture = DaggerfallUI.GetTextureFromResources(attackFalseTextureName);
            
            
            bool dreamMOBSFound = dreamMobs != null;
            if (dreamMOBSFound == true)
            {
                textureSuffix = "_hd";
            }
            else
            {
                textureSuffix = "";
            }
            backgroundTexture = DaggerfallUI.GetTextureFromResources(backgroundTextureName + textureSuffix);
            if (!backgroundTexture)
                throw new Exception("BestiaryUI: Could not load background texture.");
        }

        
        void loadPage()
        {   
            defaultEntry = currentEntries[0, 0];
            if(!String.IsNullOrEmpty(currentEntries[0, 0]) && !String.IsNullOrEmpty(currentEntries[0, 1]))
            {
                contentButtonTexture1 = DaggerfallUI.GetTextureFromResources(currentEntries[0, 1]);

                contentButton1 = new Button();
                contentButton1.Position = new Vector2(4, 162);
                contentButton1.Size = entryButtonSize;
                contentButton1.BackgroundTexture = contentButtonTexture1;
                contentButton1.OnMouseClick += ContentButton1_OnMouseClick;
                mainPanel.Components.Add(contentButton1);
            }
            if(!String.IsNullOrEmpty(currentEntries[1, 0]) && !String.IsNullOrEmpty(currentEntries[1, 1]))
            {
                contentButtonTexture2 = DaggerfallUI.GetTextureFromResources(currentEntries[1, 1]);

                contentButton2 = new Button();
                contentButton2.Position = new Vector2(50, 162);
                contentButton2.Size = entryButtonSize;
                contentButton2.BackgroundTexture = contentButtonTexture2;
                contentButton2.OnMouseClick += ContentButton2_OnMouseClick;
                mainPanel.Components.Add(contentButton2);
            }
            if(!String.IsNullOrEmpty(currentEntries[2, 0]) && !String.IsNullOrEmpty(currentEntries[2, 1]))
            {
                contentButtonTexture3 = DaggerfallUI.GetTextureFromResources(currentEntries[2, 1]);

                contentButton3 = new Button();
                contentButton3.Position = new Vector2(95, 162);
                contentButton3.Size = entryButtonSize;
                contentButton3.BackgroundTexture = contentButtonTexture3;
                contentButton3.OnMouseClick += ContentButton3_OnMouseClick;
                mainPanel.Components.Add(contentButton3);
            }
            if(!String.IsNullOrEmpty(currentEntries[3, 0]) && !String.IsNullOrEmpty(currentEntries[3, 1]))
            {
                contentButtonTexture4 = DaggerfallUI.GetTextureFromResources(currentEntries[3, 1]);

                contentButton4 = new Button();
                contentButton4.Position = new Vector2(4, 174);
                contentButton4.Size = entryButtonSize;
                contentButton4.BackgroundTexture = contentButtonTexture4;
                contentButton4.OnMouseClick += ContentButton4_OnMouseClick;
                mainPanel.Components.Add(contentButton4);
            }
            if(!String.IsNullOrEmpty(currentEntries[4, 0]) && !String.IsNullOrEmpty(currentEntries[4, 1]))
            {
                contentButtonTexture5 = DaggerfallUI.GetTextureFromResources(currentEntries[4, 1]);
                contentButton5 = new Button();
                contentButton5.Position = new Vector2(50, 174);
                contentButton5.Size = entryButtonSize;
                contentButton5.BackgroundTexture = contentButtonTexture5;
                contentButton5.OnMouseClick += ContentButton5_OnMouseClick;
                mainPanel.Components.Add(contentButton5);
            }
            if(!String.IsNullOrEmpty(currentEntries[5, 0]) && !String.IsNullOrEmpty(currentEntries[5, 1]))
            {
                contentButtonTexture6 = DaggerfallUI.GetTextureFromResources(currentEntries[5, 1]);
                contentButton6 = new Button();
                contentButton6.Position = new Vector2(95, 174);
                contentButton6.Size = entryButtonSize;
                contentButton6.BackgroundTexture = contentButtonTexture6;
                contentButton6.OnMouseClick += ContentButton6_OnMouseClick;
                mainPanel.Components.Add(contentButton6);
            }
            if(!String.IsNullOrEmpty(currentEntries[6, 0]) && !String.IsNullOrEmpty(currentEntries[6, 1]))
            {
                contentButtonTexture7 = DaggerfallUI.GetTextureFromResources(currentEntries[6, 1]);

                contentButton7 = new Button();
                contentButton7.Position = new Vector2(4, 187);
                contentButton7.Size = entryButtonSize;
                contentButton7.BackgroundTexture = contentButtonTexture7;
                contentButton7.OnMouseClick += ContentButton7_OnMouseClick;
                mainPanel.Components.Add(contentButton7);

            }
            if(!String.IsNullOrEmpty(currentEntries[7, 0]) && !String.IsNullOrEmpty(currentEntries[7, 1]))
            {
                contentButtonTexture8 = DaggerfallUI.GetTextureFromResources(currentEntries[7, 1]);

                contentButton8 = new Button();
                contentButton8.Position = new Vector2(50, 187);
                contentButton8.Size = entryButtonSize;
                contentButton8.BackgroundTexture = contentButtonTexture8;
                contentButton8.OnMouseClick += ContentButton8_OnMouseClick;
                mainPanel.Components.Add(contentButton8);
            }
            if(!String.IsNullOrEmpty(currentEntries[8, 0]) && !String.IsNullOrEmpty(currentEntries[8, 1]))
            {
                contentButtonTexture9 = DaggerfallUI.GetTextureFromResources(currentEntries[8, 1]);

                contentButton9 = new Button();
                contentButton9.Position = new Vector2(95, 187);
                contentButton9.Size = entryButtonSize;
                contentButton9.BackgroundTexture = contentButtonTexture9;
                contentButton9.OnMouseClick += ContentButton9_OnMouseClick;
                mainPanel.Components.Add(contentButton9);
            }
            resetTextLabels();
        }
        void loadContent(string assetPath, bool reset = true)
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
                        var textureArray = result[0].Split(new[] { '*' });

                        if (result[3] == currentEntry)
                        {
                            break;
                        }
                        currentTexture[0] = int.Parse(textureArray[0]);
                        currentTexture[1] = int.Parse(textureArray[1]);
                        currentTexture[2] = 0;

                        DaggerfallWorkshop.Utility.TextureReader textureReader = new DaggerfallWorkshop.Utility.TextureReader(DaggerfallUnity.Arena2Path);
                        tempTexture = textureReader.GetTexture2D(currentTexture[0], currentTexture[1]);

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
                        titleLable.Text = result[2];
                        break;
                    case 3:
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
            subTitleLable1.Text = "";
            subTitleLable2.Text = "";
            subTitleLable3.Text = "";
            subTitleLable4.Text = "";
            subTitleLable5.Text = "";
            subTitleLable6.Text = "";
            subTitleLable7.Text = "";
            subTitleLable8.Text = "";
            subTitleLable9.Text = "";
            subTitleLable10.Text = "";
            subTitleLable11.Text = "";
            subTitleLable12.Text = "";
            subTitleLable13.Text = "";
            subTitleLable14.Text = "";

            descriptionLable1.Text = "";
            descriptionLable2.Text = "";
            descriptionLable3.Text = "";
            descriptionLable4.Text = "";
            descriptionLable5.Text = "";
            descriptionLable6.Text = "";
            descriptionLable7.Text = "";
            descriptionLable8.Text = "";
            descriptionLable9.Text = "";
            descriptionLable10.Text = "";
            descriptionLable11.Text = "";
            descriptionLable12.Text = "";
            descriptionLable13.Text = "";
            descriptionLable14.Text = "";
        }
        void resetLabelTextures()
        {
            if (contentButton1 != null) contentButton1.BackgroundTexture = blankTexture;
            if (contentButton2 != null) contentButton2.BackgroundTexture = blankTexture;
            if (contentButton3 != null) contentButton3.BackgroundTexture = blankTexture;
            if (contentButton4 != null) contentButton4.BackgroundTexture = blankTexture;
            if (contentButton5 != null) contentButton5.BackgroundTexture = blankTexture;
            if (contentButton6 != null) contentButton6.BackgroundTexture = blankTexture;
            if (contentButton7 != null) contentButton7.BackgroundTexture = blankTexture;
            if (contentButton8 != null) contentButton8.BackgroundTexture = blankTexture;
            if (contentButton9 != null) contentButton9.BackgroundTexture = blankTexture;
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


        void setSingleDescriptionLabelText(string text, int number)
        {
            switch (number)
            {
                case 1:
                    descriptionLable1.Text = text;
                    break;
                case 2:
                    descriptionLable2.Text = text;
                    break;
                case 3:
                    descriptionLable3.Text = text;
                    break;
                case 4:
                    descriptionLable4.Text = text;
                    break;
                case 5:
                    descriptionLable5.Text = text;
                    break;
                case 6:
                    descriptionLable6.Text = text;
                    break;
                case 7:
                    descriptionLable7.Text = text;
                    break;
                case 8:
                    descriptionLable8.Text = text;
                    break;
                case 9:
                    descriptionLable9.Text = text;
                    break;
                case 10:
                    descriptionLable10.Text = text;
                    break;
                case 11:
                    descriptionLable11.Text = text;
                    break;
                case 12:
                    descriptionLable12.Text = text;
                    break;
                case 13:
                    descriptionLable13.Text = text;
                    break;
                case 14:
                    descriptionLable14.Text = text;
                    break;
                default:
                    break;
            }
        }

        void setSingleSubTitleabelText(string text, int number)
        {
            switch (number)
            {
                case 1:
                    subTitleLable1.Text = text;
                    break;
                case 2:
                    subTitleLable2.Text = text;
                    break;
                case 3:
                    subTitleLable3.Text = text;
                    break;
                case 4:
                    subTitleLable4.Text = text;
                    break;
                case 5:
                    subTitleLable5.Text = text;
                    break;
                case 6:
                    subTitleLable6.Text = text;
                    break;
                case 7:
                    subTitleLable7.Text = text;
                    break;
                case 8:
                    subTitleLable8.Text = text;
                    break;
                case 9:
                    subTitleLable9.Text = text;
                    break;
                case 10:
                    subTitleLable10.Text = text;
                    break;
                case 11:
                    subTitleLable11.Text = text;
                    break;
                case 12:
                    subTitleLable12.Text = text;
                    break;
                case 13:
                    subTitleLable13.Text = text;
                    break;
                case 14:
                    subTitleLable14.Text = text;
                    break;
                default:
                    break;
            }
        }

        void applyText(string[] inputText)
        {
            int labelNumber = 1;
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
                var multiline_text = SplitLineToMultiline(ReverseWords(textTemp[i][1]), descriptionLableMaxCharacters);
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
            int dispayedEntries = 0;
            for (int i = contentOffset; i < text.Count; i++)
            {
                string[] multiItem = text[i].Split('&');

                if (multiItem.Length > 1)
                {
                    setSingleSubTitleabelText(multiItem[0], labelNumber);
                    setSingleDescriptionLabelText(multiItem[1], labelNumber);

                    displayedEntries++;
                }
                else
                {
                    setSingleDescriptionLabelText(multiItem[0], labelNumber);

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

            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    pageTextTemp.Add(item);
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
   

        void setUpUIElements()
        {
            exitButtonSize = new Vector2(30, 9);
            entryButtonSize = new Vector2(40, 9);

            ParentPanel.BackgroundColor = ScreenDimColor;

            mainPanel = DaggerfallUI.AddPanel(NativePanel, AutoSizeModes.None);
            mainPanel.Size = backgroundSizeVector;
            mainPanel.BackgroundTexture = backgroundTexture;
            mainPanel.HorizontalAlignment = HorizontalAlignment.Center;
            mainPanel.VerticalAlignment     = VerticalAlignment.Middle;
            mainPanel.OnMouseScrollDown     += MainPanel_OnMouseScrollDown;
            mainPanel.OnMouseScrollUp       += MainPanel_OnMouseScrollUp;

            imagePanel = DaggerfallUI.AddPanel(mainPanel, AutoSizeModes.None);
            imagePanel.Size = picturebackgroundSizeVector;
            imagePanel.BackgroundTexture = pictureTexture;
            imagePanel.Position = picturebackgroundPosVector;

            titleLable = new TextLabel();
            if(oldFont == false)    titleLable.Position = new Vector2(15, 16); else titleLable.Position = new Vector2(15, 20);
            
            titleLable.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            titleLable.Size = new Vector2(52, 22);
            titleLable.Name = "title_label";
            titleLable.Font = DaggerfallUI.TitleFont;
            if(oldFont == true) titleLable.TextScale = 0.75f;

            mainPanel.Components.Add(titleLable);

            monsterNameLabel = new TextLabel();
            monsterNameLabel.Position = new Vector2(144, 24);
            monsterNameLabel.Size = new Vector2(40, 14);
            monsterNameLabel.Name = "sub_label_1";
            monsterNameLabel.Font = DaggerfallUI.LargeFont;
            mainPanel.Components.Add(monsterNameLabel);

            subTitleLable1 = new TextLabel();
            subTitleLable1.Position = new Vector2(144, 40);
            subTitleLable1.Size = new Vector2(40, 14);
            subTitleLable1.Name = "sub_label_1";
            mainPanel.Components.Add(subTitleLable1);

            subTitleLable2 = new TextLabel();
            subTitleLable2.Position = new Vector2(144, 50);
            subTitleLable2.Size = new Vector2(40, 14);
            subTitleLable2.Name = "sub_label_2";
            mainPanel.Components.Add(subTitleLable2);

            subTitleLable3 = new TextLabel();
            subTitleLable3.Position = new Vector2(144, 60);
            subTitleLable3.Size = new Vector2(40, 14);
            subTitleLable3.Name = "sub_label_3";
            mainPanel.Components.Add(subTitleLable3);

            subTitleLable4 = new TextLabel();
            subTitleLable4.Position = new Vector2(144, 70);
            subTitleLable4.Size = new Vector2(40, 14);
            subTitleLable4.Name = "sub_label_4";
            mainPanel.Components.Add(subTitleLable4);

            subTitleLable5 = new TextLabel();
            subTitleLable5.Position = new Vector2(144, 80);
            subTitleLable5.Size = new Vector2(40, 14);
            subTitleLable5.Name = "sub_label_5";
            mainPanel.Components.Add(subTitleLable5);

            subTitleLable6 = new TextLabel();
            subTitleLable6.Position = new Vector2(144, 90);
            subTitleLable6.Size = new Vector2(40, 14);
            subTitleLable6.Name = "sub_label_6";
            mainPanel.Components.Add(subTitleLable6);

            subTitleLable7 = new TextLabel();
            subTitleLable7.Position = new Vector2(144, 100);
            subTitleLable7.Size = new Vector2(40, 14);
            subTitleLable7.Name = "sub_label_7";
            mainPanel.Components.Add(subTitleLable7);

            subTitleLable8 = new TextLabel();
            subTitleLable8.Position = new Vector2(144, 110);
            subTitleLable8.Size = new Vector2(40, 14);
            subTitleLable8.Name = "sub_label_8";
            mainPanel.Components.Add(subTitleLable8);

            subTitleLable9 = new TextLabel();
            subTitleLable9.Position = new Vector2(144, 120);
            subTitleLable9.Size = new Vector2(40, 14);
            subTitleLable9.Name = "sub_label_9";
            mainPanel.Components.Add(subTitleLable9);

            subTitleLable10 = new TextLabel();
            subTitleLable10.Position = new Vector2(144, 130);
            subTitleLable10.Size = new Vector2(40, 14);
            subTitleLable10.Name = "sub_label_10";
            mainPanel.Components.Add(subTitleLable10);

            subTitleLable11 = new TextLabel();
            subTitleLable11.Position = new Vector2(144, 140);
            subTitleLable11.Size = new Vector2(40, 14);
            subTitleLable11.Name = "sub_label_11";
            mainPanel.Components.Add(subTitleLable11);

            subTitleLable12 = new TextLabel();
            subTitleLable12.Position = new Vector2(144, 150);
            subTitleLable12.Size = new Vector2(40, 14);
            subTitleLable12.Name = "sub_label_12";
            mainPanel.Components.Add(subTitleLable12);

            subTitleLable13 = new TextLabel();
            subTitleLable13.Position = new Vector2(144, 160);
            subTitleLable13.Size = new Vector2(40, 14);
            subTitleLable13.Name = "sub_label_13";
            mainPanel.Components.Add(subTitleLable13);

            subTitleLable14 = new TextLabel();
            subTitleLable14.Position = new Vector2(144, 170);
            subTitleLable14.Size = new Vector2(40, 14);
            subTitleLable14.Name = "sub_label_14";
            mainPanel.Components.Add(subTitleLable14);


            descriptionLable1 = new TextLabel();
            descriptionLable1.Position = new Vector2(textLabelXOffset + 172, 40);
            descriptionLable1.Size = new Vector2(124, 10);
            descriptionLable1.Name = "title_label_1";
            descriptionLable1.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable1);

            descriptionLable2 = new TextLabel();
            descriptionLable2.Position = new Vector2(textLabelXOffset + 172, 50);
            descriptionLable2.Size = new Vector2(120, 10);
            descriptionLable2.Name = "title_label_2";
            descriptionLable2.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable2);

            descriptionLable3 = new TextLabel();
            descriptionLable3.Position = new Vector2(textLabelXOffset + 172, 60);
            descriptionLable3.Size = new Vector2(120, 10);
            descriptionLable3.Name = "title_label_3";
            descriptionLable3.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable3);

            descriptionLable4 = new TextLabel();
            descriptionLable4.Position = new Vector2(textLabelXOffset + 172, 70);
            descriptionLable4.Size = new Vector2(120, 10);
            descriptionLable4.Name = "title_label_4";
            descriptionLable4.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable4);

            descriptionLable5 = new TextLabel();
            descriptionLable5.Position = new Vector2(textLabelXOffset + 172, 80);
            descriptionLable5.Size = new Vector2(120, 10);
            descriptionLable5.Name = "title_label_5";
            descriptionLable5.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable5);

            descriptionLable6 = new TextLabel();
            descriptionLable6.Position = new Vector2(textLabelXOffset + 172, 90);
            descriptionLable6.Size = new Vector2(120, 10);
            descriptionLable6.Name = "title_label_6";
            descriptionLable6.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable6);

            descriptionLable7 = new TextLabel();
            descriptionLable7.Position = new Vector2(textLabelXOffset + 172, 100);
            descriptionLable7.Size = new Vector2(120, 10);
            descriptionLable7.Name = "title_label_7";
            descriptionLable7.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable7);

            descriptionLable8 = new TextLabel();
            descriptionLable8.Position = new Vector2(textLabelXOffset + 172, 110);
            descriptionLable8.Size = new Vector2(120, 10);
            descriptionLable8.Name = "title_label_8";
            descriptionLable8.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable8);

            descriptionLable9 = new TextLabel();
            descriptionLable9.Position = new Vector2(textLabelXOffset + 172, 120);
            descriptionLable9.Size = new Vector2(120, 10);
            descriptionLable9.Name = "title_label_9";
            descriptionLable9.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable9);

            descriptionLable10 = new TextLabel();
            descriptionLable10.Position = new Vector2(textLabelXOffset + 172, 130);
            descriptionLable10.Size = new Vector2(120, 10);
            descriptionLable10.Name = "title_label_10";
            descriptionLable10.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable10);

            descriptionLable11 = new TextLabel();
            descriptionLable11.Position = new Vector2(textLabelXOffset + 172, 140);
            descriptionLable11.Size = new Vector2(120, 10);
            descriptionLable11.Name = "title_label_11";
            descriptionLable11.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable11);

            descriptionLable12 = new TextLabel();
            descriptionLable12.Position = new Vector2(textLabelXOffset + 172, 150);
            descriptionLable12.Size = new Vector2(120, 10);
            descriptionLable12.Name = "title_label_12";
            descriptionLable12.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable12);

            descriptionLable13 = new TextLabel();
            descriptionLable13.Position = new Vector2(textLabelXOffset + 172, 160);
            descriptionLable13.Size = new Vector2(120, 10);
            descriptionLable13.Name = "title_label_13";
            descriptionLable13.MaxCharacters = 50;
            mainPanel.Components.Add(descriptionLable13);

            descriptionLable14 = new TextLabel();
            descriptionLable14.Position = new Vector2(textLabelXOffset + 172, 170);
            descriptionLable14.Size = new Vector2(120, 10);
            descriptionLable14.Name = "title_label_14";
            descriptionLable14.MaxCharacters = descriptionLableMaxCharacters;
            mainPanel.Components.Add(descriptionLable14);

            exitButton = new Button();
            exitButton.Position = new Vector2(216, 187);
            exitButton.Size = entryButtonSize;
            exitButton.OnMouseClick += ExitButton_OnMouseClick;
            mainPanel.Components.Add(exitButton);
        }
        void LoadArrowsNStuff()
        {
            leftArrowTexture = DaggerfallUI.GetTextureFromResources("button_arrow_left");
            rightArrowTexture = DaggerfallUI.GetTextureFromResources("button_arrow_right");
            
            rightArrowButton = new Button();
            rightArrowButton.Position = new Vector2(98, 25);
            rightArrowButton.Size = new Vector2(10, 10);
            rightArrowButton.BackgroundTexture = rightArrowTexture;
            rightArrowButton.OnMouseClick += RightArrowButton_OnMouseClick;
            mainPanel.Components.Add(rightArrowButton);

            leftArrowButton = new Button();
            leftArrowButton.Position = new Vector2(86, 25);
            leftArrowButton.Size = new Vector2(10, 10);
            leftArrowButton.BackgroundTexture = leftArrowTexture;
            leftArrowButton.OnMouseClick += LeftArrowButton_OnMouseClick;
            mainPanel.Components.Add(leftArrowButton);

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
            pageNameLabel.Position = pageNamePosVector;
            pageNameLabel.Size = pageNameSizeVector;
            if(oldFont == false)
            {
                pageNameLabel.Font = DaggerfallUI.LargeFont;
            }
            else pageNamePosVector[1] = 18;

            pageNameLabel.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            mainPanel.Components.Add(pageNameLabel);
        }
        void changePage(bool right)
        {   
            int currentEntryNum = Array.IndexOf(allPages, currentPagePath);;
            
            if (right == true && currentEntryNum > 0)
            {
                currentEntryNum -= 1;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetLabelTextures();
                loadPage();

                entryToLoad = currentEntries[0, 0];
                loadContent(entryToLoad);
            }
            else if (right == true && currentEntryNum <= 0)
            {
                currentEntryNum = allPages.Length - 1;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetLabelTextures();
                loadPage();
                entryToLoad = currentEntries[0, 0];
                loadContent(entryToLoad);
            }
            else if (right == false && currentEntryNum < (allPages.Length - 1))
            {
                currentEntryNum += 1;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetLabelTextures();
                loadPage();
                entryToLoad = currentEntries[0, 0];
                loadContent(entryToLoad);
            }
            else
            {
                currentEntryNum = 0;

                currentEntries = getcurrentEntriesFromFile(allPages[currentEntryNum]);
                resetLabelTextures();
                loadPage();
                entryToLoad = currentEntries[0, 0];
                loadContent(entryToLoad);
            }
        }
        protected void ExitButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            CloseWindow();
        }

        protected void ContentButton1_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[0, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[0, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton2_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[1, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[1, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton3_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[2, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[2, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton4_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[3, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[3, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton5_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[4, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[4, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton6_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[5, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[5, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton7_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[6, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[6, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton8_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[7, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[7, 0];
                loadContent(entryToLoad);
            }
        }

        protected void ContentButton9_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            if(!String.IsNullOrEmpty(currentEntries[8, 0]))
            {
                DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                entryToLoad = currentEntries[8, 0];
                loadContent(entryToLoad);
            }
        }
        protected void RightArrowButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            changePage(true);
        }
        protected void LeftArrowButton_OnMouseClick(BaseScreenComponent sender, Vector2 position)
        {
            DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
            changePage(false);
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
            loadContent(entryToLoad, false);
        }
        protected void MainPanel_OnMouseScrollUp(BaseScreenComponent sender)
        {
            if (contentOffset > 0)
            {
                contentOffset -= 1;
            }
            loadContent(entryToLoad, false);
        }
    }
}