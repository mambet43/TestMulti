using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Linq;
using TestMulti.Models;
using Microsoft.Maui.Controls;
using System;
using TestMulti.Services;
using System.Collections.Generic;
using TestMulti.Constants;
using System.Collections;
using System.Buffers;
using System.Xml;
using System.Collections.ObjectModel;

namespace TestMulti.Services
{
    internal static class JsonManager
    { 
        public static void EditPreferences(Quest quest)
        {
            string fileName;
            var key = AppConstants.THEMES.FirstOrDefault(x => x.Value == quest.Theme).Key;
            if (key != null)
            {
                fileName = key;
                ObservableCollection<Quest> quests = JsonSerializer.Deserialize<ObservableCollection<Quest>>(Preferences.Get(fileName, null)) ?? new ObservableCollection<Quest>();
                quests[int.Parse(quest.number) - 1] = quest;
                Preferences.Set(fileName, JsonSerializer.Serialize(quests));
            }
        }


        public static void EditPreferences(ObservableCollection<Quest> quests)
        {
            string fileName;
            var key = AppConstants.THEMES.FirstOrDefault(x => x.Value == quests[0].Theme).Key;
            if (key!=null) 
            { 
                fileName = key;
                Preferences.Set(fileName, JsonSerializer.Serialize(quests));
            }

            
        }

        

        public static async Task  <ObservableCollection<Quest>> DeserializeToList(string filename)
        {
            //Проверяем, существует ли файл в Preferences
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
                ObservableCollection<Quest> quests = JsonSerializer.Deserialize<ObservableCollection<Quest>>(jsonContent) ?? new ObservableCollection<Quest>();  // считали из файла ресурсов
                string jsonString = JsonSerializer.Serialize(quests);
                Preferences.Set(filename, jsonString);
                return quests;
            }
            else
            {                
                if (filename == "vaworites.json") return await VaworitesCreate();
                return JsonSerializer.Deserialize<ObservableCollection<Quest>>(Preferences.Get(filename, null)) ?? new ObservableCollection<Quest>();
            }
        }

        public static async Task<ObservableCollection<Quest>> VaworitesCreate()
        {
            List<Quest> q = new List<Quest>();
            ObservableCollection<Quest> myObservableCollection = new ObservableCollection<Quest>();
            foreach (string key in AppConstants.THEMES.Keys)
            {
                q.AddRange(await DeserializeToList(key)); // Добавляем все элементы из списка
            }            
            q = q.Where(q => q.vaworites).ToList(); // Фильтруем и возвращаем список
            foreach (var quest in q)
            {
                myObservableCollection.Add(quest);
            }
            return myObservableCollection;
        }






    }
}
