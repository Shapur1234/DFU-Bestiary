using UnityEngine;

using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;

// ReSharper disable CheckNamespace
namespace BestiaryMod
{
    internal class BestiaryTextHelpers
    {
        public static TextLabel CreateSubtitle(string text)
        {
            return CreateLabel(DaggerfallUI.DefaultFont, HorizontalAlignment.Left, DaggerfallUI.DaggerfallDefaultTextColor, $"   {text}", 1.4f);
        }

        public static TextLabel CreateText(string text)
        {
            return CreateLabel(DaggerfallUI.DefaultFont, HorizontalAlignment.Left, DaggerfallUI.DaggerfallDefaultTextColor, text, 1.1f);
        }

        public static TextLabel CreateLabel(DaggerfallFont font, HorizontalAlignment alignment, Color color, string text, float scale = 1.0f)
        {
            TextLabel label = new TextLabel
            {
                Font = font,
                HorizontalAlignment = alignment,
                TextColor = color,
                ShadowColor = DaggerfallUI.DaggerfallDefaultShadowColor,
                ShadowPosition = DaggerfallUI.DaggerfallDefaultShadowPos,
                WrapText = true,
                WrapWords = true,
                Text = text,
                TextScale = scale
            };

            if (label.HorizontalAlignment == HorizontalAlignment.Center)
            {
                label.HorizontalTextAlignment = TextLabel.HorizontalTextAlignmentSetting.Center;
            }

            return label;
        }
    }
}
