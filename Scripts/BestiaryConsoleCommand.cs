using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Items;

// ReSharper disable CheckNamespace
namespace BestiaryMod
{
    public static class BestiaryConsoleCommand
    {
        public static string Command = "add_bestiaryitem";
        public static string Description = "Put the Bestiary (item) in players inventory";
        public static string Usage = "add_bestiaryitem";

        public static string Execute(params string[] args)
        {
            if (args.Length > 0)
                return Usage;

            var bestiaryItem = ItemBuilder.CreateItem(ItemGroups.UselessItems2, BestiaryItem.templateIndex);
            GameManager.Instance.PlayerEntity.Items.AddItem(bestiaryItem);
            return "BestiaryItem added";
        }
    }
}