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

        static DaggerfallUnityItem book = null;

        public BestiaryItem()
            : base(ItemGroups.UselessItems2, templateIndex)
        {
            shortName = "Bestiary";
        }

        public override string ItemName
        {
            get { return shortName; }
        }

        public override bool UseItem(ItemCollection collection)
        {
            book = this;

            if (BestiaryMain.menuUnlock == 1)
            {
                if (!BestiaryMain.unlockedBestiary)
                {
                    BestiaryMain.unlockedBestiary = true;
                    BestiaryMain.DisplayMessage("You study the contents of the book closely. You have unlocked the Bestiary.");
                }
                else
                    BestiaryMain.DisplayMessage("You already know all this book has to offer.");
            }
            else
                BestiaryMain.unlockedBestiary = true;


            return true;
        }
    }
}