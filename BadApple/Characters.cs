using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiltoag.BadApple
{
    /// <summary>
    /// https://www.w3.org/TR/xml-entity-names/025.html
    /// </summary>
    internal static class Characters
    {
        [Flags]
        public enum PixelPosition
        {
            None = 0,
            TopLeft = 1 << 0,
            TopRight = 1 << 1,
            BotLeft = 1 << 2,
            BotRight = 1 << 3,
            Left = TopLeft | BotLeft,
            Right = TopRight | BotRight,
            Bot = BotLeft | BotRight,
            Top = TopLeft | TopRight,
            Fill = Bot | Top
        }

        public static string GetValue(PixelPosition position)
        {
            return position switch
            {
                PixelPosition.None => "  ",
                PixelPosition.Fill => "██",
                PixelPosition.TopLeft => "▀ ",
                PixelPosition.TopRight => " ▀",
                PixelPosition.BotLeft => "▄ ",
                PixelPosition.BotRight => " ▄",
                PixelPosition.Left => "█ ",
                PixelPosition.Right => " █",
                PixelPosition.Bot => "▄▄",
                PixelPosition.Top => "▀▀",
                PixelPosition.Bot | PixelPosition.TopLeft => "█▄",
                PixelPosition.Bot | PixelPosition.TopRight => "▄█",
                PixelPosition.Top | PixelPosition.BotLeft => "█▀",
                PixelPosition.Top | PixelPosition.BotRight => "▀█",
                PixelPosition.TopLeft | PixelPosition.BotRight => "▀▄",
                PixelPosition.TopRight | PixelPosition.BotLeft => "▄▀",
                _ => "  "
            };
        }
    }
}