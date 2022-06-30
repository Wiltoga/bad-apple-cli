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
            var files = Directory.GetFiles("frames").ToList();
            files.Sort();
            player.Play(files, 30);
        }
    }
}