using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Linq;
using TestMulti.Models;
using Microsoft.Maui.Controls;
using System;
using TestMulti.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestMulti.Services
{
    internal static class JsonManager
    {       

        public static Quest[] DeserializeFromJson(string filename)
        {

            // Проверяем, существует ли файл в Preferences
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
                var jsonContent = reader.ReadToEnd();                
                Quest[] quests = JsonSerializer.Deserialize<Quest[]>(jsonContent) ?? Array.Empty<Quest>(); // считали из файла ресурсов
                string theme = ""; 
                switch (filename) // тему  опредлеяем
                {
                    case "eb.json": theme = "Электробезопасность"; break;
                    case "ot.json": theme = "Охрана труда"; break;
                    case "vis.json": theme = "Работы на высоте"; break;                    
                }
                foreach (var quest in quests)
                {
                    quest.Theme = theme;
                }
                string jsonString = JsonSerializer.Serialize(quests);
                Preferences.Set(filename, jsonString);
                return quests;
            }
            else
            {
                if (filename == "vaworites.json") return VaworitesCreate();
                return JsonSerializer.Deserialize<Quest[]>(Preferences.Get(filename, null)) ?? Array.Empty<Quest>();  
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
            quests[int.Parse(quest.number) - 1] = quest;
            Preferences.Set(fileName, JsonSerializer.Serialize(quests));
        }

        public static Quest [] VaworitesCreate()
        {
            Quest[] questsEb = Array.Empty<Quest>();
            Quest[] questsOt = Array.Empty<Quest>();
            Quest[] questsVis = Array.Empty<Quest>();
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
            return vaworitesQ;

        }

        



    }
}
