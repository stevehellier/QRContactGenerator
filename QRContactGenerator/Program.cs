using QRCoder;
using QRContactGenerator.Models;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace QRContactGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0 || args == null)
            {
                Console.WriteLine("Please specify a json file to create the QR codes");
                return;
            }

            var filename = args[0];
            try
            {
                var people = Data.LoadPeople(filename);

                if (people != null)
                {
                    foreach (var person in people)
                    {
                        Console.Write($"Creating QR vCard for {person.Firstname} {person.Lastname} ({person.Organisation})... ");
                        CreateAndDisplayQRCode(person);
                        Console.WriteLine("done!");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Cannot find file {filename}");
            }
        }

        private static void CreateAndDisplayQRCode(PersonModel person, int pixels = 20)
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

            var image = _qrCodeImage;
            var filename = $"{person.Firstname} {person.Lastname} ({person.Organisation})";
            image.Save($"{filename}.png", ImageFormat.Png);

        }
    }
}
