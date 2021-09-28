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
            _ = new List<PersonModel>();
            string json;

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException(filename);
            }

            IList<PersonModel> people;

            json = File.ReadAllText(filename);

            people = JsonSerializer.Deserialize<IList<PersonModel>>(json);

            return people;
        }
    }
}
