using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Wiltoag.BadApple
{
    internal class Program
    {
        private const int ViewPortHeight = 32;

        private static void Main(string[] args)
        {
            var frames = Enumerable.Range(1, 500)
                .Select(frameNumber =>
                {
                    return new Bitmap($"frames/frame{frameNumber:0000}.jpg");
                });
            var parser = new FramesParser();
            var height = ViewPortHeight;
            var source = new Bitmap($"frames/frame0001.jpg");
            var width = height * 2 * source.Width / source.Height;
            if (Console.WindowWidth < width)
            {
                Console.WriteLine($"Console is not large enough, a width of at least {width} characters is requiered (currently {Console.WindowWidth})");
                return;
            }
            if (Console.WindowHeight < height)
            {
                Console.WriteLine($"Console is not large enough, a height of at least {height} characters is requiered (currently {Console.WindowHeight})");
                return;
            }
            Console.Clear();
            var chrono = new Stopwatch();
            chrono.Start();
            var delta = TimeSpan.FromSeconds(1.0 / 30);
            foreach (var frame in frames)
            {
                var pixels = parser.RenderImage(frame, width, height);
                var builder = new StringBuilder();
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    for (int x = 0; x < pixels.GetLength(0); x++)
                        builder.Append(pixels[x, y]);
                    if (y < pixels.GetLength(1) - 1)
                        builder.AppendLine();
                }
                var elapsed = chrono.Elapsed;
                if (delta > elapsed)
                    Thread.Sleep(delta - elapsed);
                chrono.Restart();
                Console.SetCursorPosition(0, 0);
                Console.Write(builder);
            }
        }
    }
}