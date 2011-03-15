using System;
using System.IO;

namespace NetSourceProjectBuilder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:\n");
                Console.WriteLine("Mike.Net40ProjectBuilder.exe <base directory> <target directory>\n");
                Console.WriteLine("base directory:   the directory that you downloaded the NET4 source to.");
                Console.WriteLine("target directory: the directory to build the project files in.");
                return;
            }

            var baseDirectory = args[0];
            var targetDirectory = args[1];

            if (!Directory.Exists(baseDirectory))
            {
                Console.WriteLine("Base directory does not exist: '{0}'", baseDirectory);
            }
            if (!Directory.Exists(targetDirectory))
            {
                Console.WriteLine("Target directory does not exist: '{0}'", targetDirectory);
            }

            var projectBuilder = new ProjectBuilder();
            projectBuilder.BuildProject(baseDirectory, targetDirectory);
        }
    }
}