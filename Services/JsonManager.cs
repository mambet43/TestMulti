using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Linq;
using TestMulti.Models;
using Microsoft.Maui.Controls;
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
        public static void EditPreferences(Quest quest)
        {
            string fileName = "";
            switch (quest.Theme)
            {
                case "Электробезопасность":  fileName = "eb.json"; break; 
                case "Охрана труда":  fileName = "ot.json"; break; 
                case "Работы на высоте":  fileName = "vis.json"; break; 
            }
            Quest [] quests = JsonSerializer.Deserialize<Quest[]>(Preferences.Get(fileName, null)) ?? Array.Empty<Quest>();
            quests[int.Parse(quest.number) - 1].Vaworites = !quests[int.Parse(quest.number) - 1].Vaworites;
            UserQuestSave(quests, fileName);
        }

        public static void VaworitesCreate()
        {
            Quest[] questsEb = Array.Empty<Quest>();
            Quest[] questsOt = Array.Empty<Quest>();
            Quest[] questsVis = Array.Empty<Quest>();
            Preferences.Set("vaworites", null);
            if (Preferences.ContainsKey("eb.json")) { questsEb = JsonSerializer.Deserialize<Quest[]>(Preferences.Get("eb.json", null)) ?? Array.Empty<Quest>(); }
            if (Preferences.ContainsKey("ot.json")) { questsOt = JsonSerializer.Deserialize<Quest[]>(Preferences.Get("ot.json", null)) ?? Array.Empty<Quest>(); }
            if (Preferences.ContainsKey("vis.json")) { questsVis = JsonSerializer.Deserialize<Quest[]>(Preferences.Get("vis.json", null)) ?? Array.Empty<Quest>(); }
            var questsConcat = questsEb.Concat(questsOt).Concat(questsVis) ?? Array.Empty<Quest>();
            Quest[] vaworitesQ = new Quest[0];
            foreach (Quest quest in questsConcat)
            {
                 if (quest != null && quest.Vaworites)
                 {
                     Array.Resize(ref vaworitesQ, vaworitesQ.Length + 1);
                     vaworitesQ[vaworitesQ.Length - 1] = quest;
                 }
            }
            UserQuestSave(vaworitesQ, "vaworites.json");      

        }

        

        public static void UserQuestSave(Quest[] data, string filename)
        {           
            Preferences.Set(filename, JsonSerializer.Serialize(data));
        }


    }
}
