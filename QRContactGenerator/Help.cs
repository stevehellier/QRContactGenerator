using System;

namespace QRContactGenerator
{
    class Help
    {
        public static void DisplayUsage()
        {
            Console.WriteLine($"usage: QRContactGenerator data_file output_folder");
            Console.WriteLine($"\t data file in JSON");
            Console.WriteLine($"\t output folder [optional]");
        }
    }
}
