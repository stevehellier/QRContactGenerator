using QRCoder;
using QRContactGenerator.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace QRContactGenerator
{
    class Program
    {
        private const string _errorNoFileSpecified = "Please specify a JSON file to create the QR codes";

        static void Main(string[] args)
        {

            if (args.Length <= 0 || args == null)
            {
                DisplayUsage();
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
                        SaveQRCodeImage(CreateQRCode(person), path, personInfo);
                        Console.WriteLine("done!");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Cannot find file {filename}");
            }
        }

        private static Image CreateQRCode(PersonModel person, int pixels = 20)
        {
            StringBuilder payload = new();
            payload.AppendLine($"BEGIN:VCARD");
            payload.AppendLine($"VERSION:3.0");
            payload.AppendLine($"N:{person.Lastname};{person.Firstname};;{person.Title};");
            payload.AppendLine($"FN:{person.Title} {person.Firstname} {person.Lastname}");
            payload.AppendLine($"ORG:{person.Organisation}");
            payload.AppendLine($"TITLE:{person.JobTitle}");
            payload.AppendLine($"TEL;type=WORK;type=CELL;type=VOICE:{person.MobilePhone}");
            payload.AppendLine($"TEL;type=WORK;type=PREF;type=VOICE:{person.WorkPhone}");
            payload.AppendLine($"ADR;type=WORK:;;{person.HouseNumber} {person.Street};{person.City};{person.County};{person.PostCode};{person.Country}");
            payload.AppendLine($"URL:{person.Website}");
            payload.AppendLine($"EMAIL;type=WORK:{person.Email}");
            payload.AppendLine($"REV:{DateTime.Now:s}");
            payload.AppendLine($"END:VCARD");

            QRCodeGenerator qrCodeGenerator = new();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(payload.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(qrCodeData);

            var _qrCodeImage = qrCode.GetGraphic(pixels);

            return _qrCodeImage;
        }
        private static void SaveQRCodeImage(Image image, string path, string filename)
        {
            filename = Path.Combine(path, filename);
            image.Save($"{filename}.png", ImageFormat.Png);
        }

        private static void DisplayUsage()
        {
            Console.WriteLine($"usage: QRContactGenerator data_file output_folder");
            Console.WriteLine($"\t data file in JSON");
            Console.WriteLine($"\t output folder [optional]");
        }
    }
}
