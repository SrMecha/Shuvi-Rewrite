using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Shuvi.Services.StaticServices.Localization
{
    public static class LocalizationService
    {
        private static Dictionary<string, LocalizationLanguagePart> _langs = new();

        public static void Init()
        {
            InitFile("status.csv", "status");
        }
        private static void InitFile(string fileName, string partName)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, 
                Delimiter = ";"
            };
            using (var reader = new StreamReader(Path.GetFullPath("../../../", AppDomain.CurrentDomain.BaseDirectory) + $"/Language/{fileName}"))
            using (var csv = new CsvReader(reader, config))
            {
                string key;
                string? value;
                var fileResult = new List<Dictionary<string, string>>();

                while (csv.Read())
                {
                    key = csv.GetField(0)!;
                    for (int i = 1; csv.TryGetField(i, out value); i++)
                    {
                        if (fileResult.Count < i)
                            fileResult.Add(new());
                        fileResult[i-1].Add(key, value!);
                    }  
                }
                _langs.Add(partName, new(fileResult));
            }
        }
        public static LocalizationLanguagePart Get(string part)
        {
            return _langs.GetValueOrDefault(part, new());
        }
    }
}
