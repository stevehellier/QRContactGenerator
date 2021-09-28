using QRContactGenerator.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace QRContactGenerator
{
    public class Data
    {
        public static IList<PersonModel> LoadPeople(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException(filename);
            }
            var json = File.ReadAllText(filename);

            var people = JsonSerializer.Deserialize<IList<PersonModel>>(json);

            return people;

        }
    }
}
