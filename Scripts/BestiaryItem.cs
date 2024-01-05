using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;

// ReSharper disable CheckNamespace
namespace BestiaryMod
{
    public class BestiaryItem : DaggerfallUnityItem
    {
        public const int templateIndex = 900;
        private const string NAME = "Bestiary";

        public BestiaryItem()
            : base(ItemGroups.UselessItems2, templateIndex)
        {
            shortName = NAME;
        }

        public override string ItemName
        {
            get { return NAME; }
        }

        public override string LongName
        {
            get { return NAME; }
        }

        public override bool UseItem(ItemCollection collection)
        {
            switch (BestiaryMain.SettingMenuUnlock)
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
                    }
                    DaggerfallUI.UIManager.PushWindow(BestiaryMain.bestiaryUIScreen);
                    break;
            }
            return true;
        }
        public override ItemData_v1 GetSaveData()
        {
            var data = base.GetSaveData();
            data.className = typeof(BestiaryItem).ToString();
            return data;
        }
    }
}