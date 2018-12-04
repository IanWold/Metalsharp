using System;

namespace XmlDocToMarkdown
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths;
            if (args.Length == 2)
            {
                paths = args;
            }
            else
            {
                Console.Write("[input_path] [output_path]: ");
                paths = Console.ReadLine().Split(' ');
            }

            Converter.Convert(paths[0], paths[1]);
        }
    }
}
