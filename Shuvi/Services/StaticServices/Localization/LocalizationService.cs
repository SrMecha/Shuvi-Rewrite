﻿using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Shuvi.Services.StaticServices.Localization
{
    public static class LocalizationService
    {
        private static Dictionary<string, LocalizationLanguagePart> _langs = new();

        public static void Init()
        {
#if DEBUG
            var directory = new DirectoryInfo(Path.GetFullPath("../../../../", AppDomain.CurrentDomain.BaseDirectory) + $"/Language");
#else
            var directory = new DirectoryInfo(Path.GetFullPath("../../", AppDomain.CurrentDomain.BaseDirectory) + $"/Language");
#endif
            foreach (var file in directory.GetFiles("*.csv"))
            {
                InitFile(file.Name, file.Name[..^4]);
            }
        }
        private static void InitFile(string fileName, string partName)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";"
            };
#if DEBUG
            using (var reader = new StreamReader(Path.GetFullPath("../../../../", AppDomain.CurrentDomain.BaseDirectory) + $"/Language/{fileName}"))
#else
            using (var reader = new StreamReader(Path.GetFullPath("../../", AppDomain.CurrentDomain.BaseDirectory) + $"/Language/{fileName}"))
#endif
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
                        fileResult[i - 1].TryAdd(key, value![..].Replace("\\n", "\n"));
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
