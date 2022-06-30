using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wiltoag.BadApple
{
    internal class FramesParser
    {
        private const byte Threshold = 50;

        public char[,] RenderImage(Bitmap image, int width, int height)
        {
            width /= 2; // one pixel will use two characters
            var pixels = new byte[image.Width * image.Height * 3];
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            var data = image.LockBits(rect, ImageLockMode.ReadWrite,
                                         image.PixelFormat);

            var pixelsPerWidth = image.Width / width;
            var pixelsPerHeight = image.Height / height;

            var ptr = data.Scan0;
            Marshal.Copy(ptr, pixels, 0, pixels.Length);
            image.UnlockBits(data);
            var characters = new char[width * 2, height];
            for (int row = 0; row < height; ++row)
            {
                for (int column = 0; column < width; ++column)
                {
                    Characters.PixelPosition topleft, topright, botleft, botright;

                    {
                        // top left aggregation
                        var positionOffset = (image.Width * row * pixelsPerHeight + column * pixelsPerWidth) * 3;
                        var channelsSum = 0L;
                        var pixelCount = 0;
                        for (int x = 0; x < pixelsPerWidth / 2; ++x)
                        {
                            for (int y = 0; y < pixelsPerHeight / 2; ++y)
                            {
                                var pixelOffset = positionOffset + (image.Width * y + x) * 3;
                                channelsSum += pixels[pixelOffset];
                                channelsSum += pixels[pixelOffset + 1];
                                channelsSum += pixels[pixelOffset + 2];
                                ++pixelCount;
                            }
                        }
                        var average = channelsSum / 3d / pixelCount;
                        topleft = average > Threshold
                            ? Characters.PixelPosition.TopLeft
                            : Characters.PixelPosition.None;
                    }

                    {
                        // top right aggregation
                        var positionOffset = (image.Width * row * pixelsPerHeight + column * pixelsPerWidth) * 3;
                        var channelsSum = 0L;
                        var pixelCount = 0;
                        for (int x = pixelsPerWidth / 2; x < pixelsPerWidth; ++x)
                        {
                            for (int y = 0; y < pixelsPerHeight / 2; ++y)
                            {
                                var pixelOffset = positionOffset + (image.Width * y + x) * 3;
                                channelsSum += pixels[pixelOffset];
                                channelsSum += pixels[pixelOffset + 1];
                                channelsSum += pixels[pixelOffset + 2];
                                ++pixelCount;
                            }
                        }
                        var average = channelsSum / 3d / pixelCount;
                        topright = average > Threshold
                            ? Characters.PixelPosition.TopRight
                            : Characters.PixelPosition.None;
                    }
                    {
                        // bot left aggregation
                        var positionOffset = (image.Width * row * pixelsPerHeight + column * pixelsPerWidth) * 3;
                        var channelsSum = 0L;
                        var pixelCount = 0;
                        for (int x = 0; x < pixelsPerWidth / 2; ++x)
                        {
                            for (int y = pixelsPerHeight / 2; y < pixelsPerHeight; ++y)
                            {
                                var pixelOffset = positionOffset + (image.Width * y + x) * 3;
                                channelsSum += pixels[pixelOffset];
                                channelsSum += pixels[pixelOffset + 1];
                                channelsSum += pixels[pixelOffset + 2];
                                ++pixelCount;
                            }
                        }
                        var average = channelsSum / 3d / pixelCount;
                        botleft = average > Threshold
                            ? Characters.PixelPosition.BotLeft
                            : Characters.PixelPosition.None;
                    }

                    {
                        // bot right aggregation
                        var positionOffset = (image.Width * row * pixelsPerHeight + column * pixelsPerWidth) * 3;
                        var channelsSum = 0L;
                        var pixelCount = 0;
                        for (int x = pixelsPerWidth / 2; x < pixelsPerWidth; ++x)
                        {
                            for (int y = pixelsPerHeight / 2; y < pixelsPerHeight; ++y)
                            {
                                var pixelOffset = positionOffset + (image.Width * y + x) * 3;
                                channelsSum += pixels[pixelOffset];
                                channelsSum += pixels[pixelOffset + 1];
                                channelsSum += pixels[pixelOffset + 2];
                                ++pixelCount;
                            }
                        }
                        var average = channelsSum / 3d / pixelCount;
                        botright = average > Threshold
                            ? Characters.PixelPosition.BotRight
                            : Characters.PixelPosition.None;
                    }

                    var result = Characters.GetValue(topleft | topright | botleft | botright);
                    characters[column * 2, row] = result[0];
                    characters[column * 2 + 1, row] = result[1];
                }
            }
            return characters;
        }
    }
}