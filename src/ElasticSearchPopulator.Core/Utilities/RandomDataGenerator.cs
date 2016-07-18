namespace ElasticSearchPopulator.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RandomDataGenerator : IRandomDataGenerator
    {
        private static readonly Random Randomiser = new Random();

        private readonly List<char> alphabet;

        private readonly List<GreatBritishCounty> counties;

        private readonly List<string> streets;

        private readonly List<string> firstNames;
        private readonly List<string> lastNames;

        private readonly List<string> occupations;

        public RandomDataGenerator()
        {
            this.alphabet = "abcdefghijklmnopqrstuvwxyz".ToUpperInvariant().Select(x => x).ToList();

            var rawData = new RawData();

            this.counties = rawData.CountiesWithPostCode();
            this.streets = rawData.PlaceNames();

            this.firstNames = rawData.FirstNames();
            this.lastNames = rawData.LastNames();

            this.occupations =
                "Disabled, Single, Married, Partnership, Carer".Split(
                    new[] { "," },
                    StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        public string RandomAddress()
        {
            var number = Randomiser.Next(0, 150).ToString();
            var street = this.GetRandomString(this.streets);
            var county = this.GetRandomString(this.counties);

            var postCodeDistrict = Randomiser.Next(1, 25);
            var postCodeSector = Randomiser.Next(0, 9);
            var postCodeUnit = string.Join("", Enumerable.Range(0, 2).Select(x => this.GetRandomString(this.alphabet)));

            var finalPostcode = county.PostCodeArea + postCodeDistrict + " " + postCodeSector + postCodeUnit;

            return
                number + Environment.NewLine +
                street + Environment.NewLine +
                county.Name + Environment.NewLine + 
                finalPostcode;
        }

        public string RandomName()
        {
            return this.GetRandomString(this.firstNames) + " " + this.GetRandomString(this.lastNames);
        }

        public string RandomNationalInsuranceNumber()
        {
            var sixDigits =string.Join("", Enumerable.Range(0, 3).Select(x => Randomiser.Next(0, 9).ToString() + Randomiser.Next(0, 9).ToString()));
            var suffix = this.GetRandomString(this.alphabet.Take(4).ToList());

            var exclusionLetters = "D, F, I, Q, U, V".Split(',').Select(x => x.Trim().ToCharArray()[0]);
            var prefixLetters = this.alphabet.Where(x => exclusionLetters.All(y => x != y)).ToList();

            var prefix = string.Join("", Enumerable.Range(0, 2).Select(x => this.GetRandomString(prefixLetters)));

            return prefix + sixDigits + suffix;
        }

        public string DateOfBirth()
        {
            var birth = new DateTime(Randomiser.Next(1965, 1998), Randomiser.Next(1, 12), Randomiser.Next(1, 28));

            return birth.ToShortDateString();
        }

        public string Occupation()
        {
            return this.GetRandomString(this.occupations);
        }

        public string RandomUsername()
        {
            var users = new[] { "callum", "veronica", "kim", "tom", "dave" };

            return this.GetRandomString(users.ToList());
        }

        private T GetRandomString<T>(List<T> list)
        {
            return list[Randomiser.Next(0, list.Count - 1)];
        }
    }
}