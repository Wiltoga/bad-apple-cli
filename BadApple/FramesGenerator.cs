using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wiltoag.BadApple
{
    internal class FramesGenerator
    {
        private static readonly string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bad-apple-cache");
        private static readonly string FrameFileFormat = "frame%05d.jpg";

        public IEnumerable<string> GetFrames()

        {
            IEnumerable<string> files;
            if (Directory.Exists(DirectoryPath) &&
                (files = Directory.GetFiles(DirectoryPath)).Any())
            {
                var list = files.ToList();
                list.Sort();
                return list;
            }
            Console.WriteLine("Generating frames...");
            Directory.CreateDirectory(DirectoryPath);
            var output = Path.Combine(DirectoryPath, FrameFileFormat);
            var input = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".", "Bad Apple.mp4");
            var ffmpeg = new Process
            {
                StartInfo = new ProcessStartInfo("ffmpeg", @$"-i ""{input}"" ""{output}""")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            ffmpeg.Start();
            ffmpeg.WaitForExit();
            {
                var list = Directory.GetFiles(DirectoryPath).ToList();
                list.Sort();
                return list;
            }
        }

        public void ResetCache()
        {
            Console.WriteLine("Deleting cache...");
            Directory.Delete(DirectoryPath, true);
        }
    }
}