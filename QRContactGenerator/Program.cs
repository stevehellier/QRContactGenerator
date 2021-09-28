using System;
using System.IO;

namespace QRContactGenerator
{
    class Program
    {
        private const string _errorNoFileSpecified = "Please specify a JSON file to use";
        private const string _errorFileNotFound = "Cannot find file {0}";

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
                        var p = QRCoder.CreateQRCode(person);
                        QRCoder.SaveQRCodeImage(p, path, personInfo);

                        Console.WriteLine("done!");
                    }
                }
                else
                {
                    Console.WriteLine("No data found, unable to create QR Code!");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(_errorFileNotFound, filename);
            }
            catch
            {
                Console.WriteLine("Invalid JSON file");
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Quitting...");
        }
    }
}
