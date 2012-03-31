using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using ImageResizer.Components;

namespace WFImageResizer
{
    class Program
    {
        private static Options options;

        static void Main(string[] args)
        {
            options = new Options();

            if (ParseArgs(args))
            {
                var inArguments = new Dictionary<string, object>();
                inArguments.Add("Options", options);

                Workflow1 workflow = new Workflow1();
                WorkflowInvoker.Invoke(workflow, inArguments);
            }
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static bool ParseArgs(string[] args)
        {
            int intResult;
            bool boolResult;

            foreach (var arg in args)
            {
                string[] argParts = arg.Split('=');
                if (argParts.Length != 2)
                    return false;

                switch (argParts[0].ToLower())
                {
                    case "/source":
                        options.SourceDirectory = argParts[1];
                        break;
                    case "/target":
                        options.TargetDirectory = argParts[1];
                        break;
                    case "/width":
                        if (!int.TryParse(argParts[1], out intResult))
                        {
                            Console.WriteLine("Width must be numeric!");
                            return false;
                        }
                        options.Width = int.Parse(argParts[1]);
                        break;
                    case "/height":
                        if (!int.TryParse(argParts[1], out intResult))
                        {
                            Console.WriteLine("Height must be numeric!");
                            return false;
                        }
                        options.Height = int.Parse(argParts[1]);
                        break;
                    case "/autorotate":
                        if (!bool.TryParse(argParts[1], out boolResult))
                        {
                            Console.WriteLine("AutoRotate must be either 'True' or 'False'!");
                            return false;
                        }
                        options.AutoRotate = bool.Parse(argParts[1]);
                        break;
                    case "/parallelize":
                        if (!bool.TryParse(argParts[1], out boolResult))
                        {
                            Console.WriteLine("Parallelize must be either 'True' or 'False'!");
                            return false;
                        }
                        options.Parallelize = bool.Parse(argParts[1]);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(options.SourceDirectory) || string.IsNullOrWhiteSpace(options.TargetDirectory))
            {
                Console.WriteLine("USAGE: WFImageResizer /Source:DIR /Target:DIR [/AutoRotate:TRUE|FALSE] [/Width:SIZE] [/Height:SIZE] [/Parallelize:TRUE|FALSE]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine();
                Console.WriteLine("/AutoRotate:[TRUE|FALSE]\tAutomatically attempts to rotate images to the correct orientation.");
                Console.WriteLine("/Width:[SIZE]\t\t\tSets the width (in pixels) of the target image.");
                Console.WriteLine("/Height:[SIZE]\t\t\tSets the height (in pixels) of the target image.");
                Console.WriteLine("/Parallelize:[TRUE|FALSE]\tParallelizes the resizing operation to increase performance.");
                Console.WriteLine();
                return false;
            }

            if (!Directory.Exists(options.SourceDirectory))
            {
                Console.WriteLine("The source Directory '{0}' does not exist!", options.SourceDirectory);
                return false;
            }

            if (!Directory.Exists(options.SourceDirectory))
            {
                Console.WriteLine("The Target Directory '{0}' does not exist!", options.TargetDirectory);
                return false;
            }

            return true;
        }
    }
}
