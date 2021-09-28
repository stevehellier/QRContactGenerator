using System;
using System.IO;

namespace QRContactGenerator
{
    class Program
    {
        private const string _errorNoFileSpecified = "Please specify a JSON file to create the QR codes";

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            if (args.Length <= 0 || args == null)
            {
                Help.DisplayUsage();
                Console.WriteLine(_errorNoFileSpecified);
                return;
            }

            var filename = args[0];
            string path;
            if (args.Length == 1)
            {
                path = Directory.GetCurrentDirectory();
            }
            else
            {
                path = args[1];
            }

            try
            {
                var people = Data.LoadPeople(filename);

                if (people != null)
                {

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (var person in people)
                    {
                        var personInfo = $"{person.Firstname} {person.Lastname} ({person.Organisation})";
                        Console.Write($"Creating QR vCard for {personInfo}... ");
                        QRCoder.SaveQRCodeImage(QRCoder.CreateQRCode(person), path, personInfo);
                        Console.WriteLine("done!");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Cannot find file {filename}");
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Quitting...");
        }
    }
}
