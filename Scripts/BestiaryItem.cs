using System.Collections;
using System.Collections.Generic;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterfaceWindows;

using UnityEngine;

namespace BestiaryMod
{
    public class BestiaryItem : DaggerfallUnityItem
    {
        public const int templateIndex = 900;
        internal const string NAME = "Bestiary";

        public BestiaryItem() : base(ItemGroups.UselessItems2, templateIndex)
        {
            shortName = "Bestiary";
        }

        public override string ItemName
        {
            get { return shortName; }
        }

        public override bool UseItem(ItemCollection collection)
        {
            switch (BestiaryMain.MenuUnlock)
            {
                case 0:
                    DaggerfallUI.UIManager.PushWindow(BestiaryMain.bestiaryUIScreen);
                    break;
                case 1:
                    DaggerfallUI.UIManager.PushWindow(BestiaryMain.bestiaryUIScreen);
                    break;
                case 2:
                    if (!BestiaryMain.UnlockedBestiary)
                    {
                        BestiaryMain.UnlockedBestiary = true;
                        BestiaryMain.DisplayMessage("You study the contents of the book closely. You have unlocked the Bestiary.");
                        DaggerfallUI.UIManager.PushWindow(BestiaryMain.bestiaryUIScreen);
                    }
                    else
                        DaggerfallUI.UIManager.PushWindow(BestiaryMain.bestiaryUIScreen);
                    break;
            }
            return true;
        }
        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(BestiaryItem).ToString();
            return data;
        }

    }
}