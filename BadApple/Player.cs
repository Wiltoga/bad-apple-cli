using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiltoag.BadApple
{
    internal class Player
    {
        public Player()
        {
            Parser = new FramesParser();
        }

        private FramesParser Parser { get; }

        public void Play(IEnumerable<string> frameFiles, int fps)
        {
            var frameChronometer = new Stopwatch();
            var fpsChronometer = new Stopwatch();
            var delta = TimeSpan.FromSeconds(1.0 / fps);
            var fpsCounter = 0;

            fpsChronometer.Start();
            frameChronometer.Start();

            int lastFps = 0;

            // size of the viewport, used to clear the buffer in case the console is resized
            var lastSize = (0, 0);
            foreach (var file in frameFiles)
            {
                frameChronometer.Restart();
                using var frame = new Bitmap(file);
                // we try to keep the ratio of the image
                var height = Math.Min(Console.WindowHeight, Console.WindowWidth * frame.Height / 2 / frame.Width);
                var width = height * 2 * frame.Width / frame.Height;

                // every second we update the fps counter
                if (fpsChronometer.Elapsed >= TimeSpan.FromSeconds(1))
                {
                    fpsChronometer.Restart();
                    lastFps = fpsCounter;
                    fpsCounter = 0;
                }
                var pixels = Parser.RenderImage(frame, width, height);

                // we build the console buffer to display every frame
                // possible improvement of performances : only display characters that changed ?
                var builder = new StringBuilder();
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    for (int x = 0; x < pixels.GetLength(0); x++)
                        builder.Append(pixels[x, y]);
                    if (y < pixels.GetLength(1) - 1)
                        builder.AppendLine();
                }
                Console.Title = $"bad-apple {width}x{height}@{lastFps}fps";
                // we clear to avoid residues from the console buffer
                var currentSize = (width, height);
                if (lastSize != currentSize)
                {
                    lastSize = currentSize;
                    Console.Clear();
                }
                Console.SetCursorPosition(0, 0);
                Console.Write(builder);
                ++fpsCounter;
                while (frameChronometer.Elapsed < delta) ;
            }
        }
    }
}