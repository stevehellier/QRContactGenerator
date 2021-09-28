using QRCoder;
using QRContactGenerator.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace QRContactGenerator
{
    class QRCoder
    {
        public static Image CreateQRCode(PersonModel person)
        {

            var payload = GenerateVCard(person);
            return GenerateQRImage(payload);
        }

        public static void SaveQRCodeImage(Image image, string path, string filename)
        {
            filename = Path.Combine(path, filename);
            image.Save($"{filename}.png", ImageFormat.Png);
        }

        private static Image GenerateQRImage(string payload, int pixels = 20)
        {
            QRCodeGenerator qrCodeGenerator = new();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(payload.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(qrCodeData);

            var _qrCodeImage = qrCode.GetGraphic(pixels);

            return _qrCodeImage;
        }

        private static string GenerateVCard(PersonModel person)
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

            return payload.ToString();
        }

    }
}
