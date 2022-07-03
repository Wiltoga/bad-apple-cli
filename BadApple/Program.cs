using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Wiltoag.BadApple
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var player = new Player();
            var framesGenerator = new FramesGenerator();
            var fps = 30;
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    switch (args[i].ToLowerInvariant())
                    {
                        case "--fps":
                            _ = int.TryParse(args[++i], out fps);
                            break;

                        default:
                            Console.WriteLine(@"
Usage :
bad-apple [options]

Options :
--fps <frames per second>           Overrides the 30 fps limit");
                            return;
                    }
                }
            }

            var files = framesGenerator.GetFrames();
            player.Play(files, fps);
        }
    }
}