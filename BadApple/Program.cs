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
            var files = framesGenerator.GetFrames();
            player.Play(files, 30);
        }
    }
}