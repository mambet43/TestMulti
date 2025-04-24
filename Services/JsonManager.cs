using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Linq;
using TestMulti.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace TestMulti.Services
{
    internal static class JsonManager
    {
        public static Quest[] DeserializeFromJson(string filename)
        {
            if (Preferences.Get(filename, null) == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourcePath = $"TestMulti.Resources.Raw.{filename}";
                using Stream stream = assembly.GetManifestResourceStream(resourcePath);
                if (stream == null)
                {
                    throw new FileNotFoundException($"Файл {filename} не найден в ресурсах.");
                }
                using StreamReader reader = new StreamReader(stream);
                var jsonContent = reader.ReadToEnd(); // Используем синхронное чтение
                return JsonSerializer.Deserialize<Quest[]>(jsonContent) ?? Array.Empty<Quest>(); // Добавляем защиту от null
            }
            else
            {
                return JsonSerializer.Deserialize<Quest[]>(Preferences.Get(filename, null)) ?? Array.Empty<Quest>(); // Добавляем защиту от null
            }
        }

        public static void VaworitesCreate()
        {
            Preferences.Set("vaworites", null);
            Quest[] questsEb = JsonSerializer.Deserialize<Quest[]>(Preferences.Get("eb.json", null)) ?? Array.Empty<Quest>();
            Quest[] questsOt = JsonSerializer.Deserialize<Quest[]>(Preferences.Get("ot.json", null)) ?? Array.Empty<Quest>();
            Quest[] questsVis = JsonSerializer.Deserialize<Quest[]>(Preferences.Get("vis.json", null)) ?? Array.Empty<Quest>();
            var questsConcat = questsEb.Concat(questsOt).Concat(questsVis) ?? Array.Empty<Quest>();
            Quest[] vaworitesQ = new Quest[0];
            foreach (Quest quest in questsConcat)
            {
                if (quest != null && quest.Vaworites)
                {
                    Array.Resize(ref vaworitesQ, vaworitesQ.Length + 1);
                    vaworitesQ[vaworitesQ.Length-1] = quest;
                }
            }
            UserQuestSave(vaworitesQ,"vaworites.json");


        }

        public static void UserQuestSave(Quest[] data, string filename)
        {           
            Preferences.Set(filename, JsonSerializer.Serialize(data));
        }


    }
}
