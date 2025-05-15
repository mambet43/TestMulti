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
            //string[] uniqueFileNames = Array.Empty<string>();            
            //foreach (Quest q in quests)
            //{
            //    uniqueFileNames = quests
            //        .Select(q => q.FileName)
            //        .Where(fn => !string.IsNullOrEmpty(fn))
            //        .Distinct()
            //        .ToArray();               
            //}
            //ObservableCollection<Quest>[] QfromPreferences = new ObservableCollection<Quest>[uniqueFileNames.Length];
            //for (int i = 0; i < QfromPreferences.Length; i++)
            //{
            //    for (int j = 0; j < uniqueFileNames.Length; j++) {
            //    QfromPreferences[i] = JsonSerializer.Deserialize<ObservableCollection<Quest>>(Preferences.Get(uniqueFileNames[j], null)) ?? new ObservableCollection<Quest>();

            //}

            // тоже самое в двух варах с лямбдами. ХЗ как это работает дала нейронка
            //// Получаем уникальные имена файлов
            //var uniqueFileNames = quests
            //    .Select(q => q.FileName)
            //    .Where(fn => !string.IsNullOrEmpty(fn))
            //    .Distinct()
            //    .ToArray();

            //// Создаем массив ObservableCollection<Quest>
            //var QfromPreferences = uniqueFileNames
            //    .Select(fn => JsonSerializer.Deserialize<ObservableCollection<Quest>>(Preferences.Get(fn, null)) ?? new ObservableCollection<Quest>())
            //    .ToArray();


            // тоже самое без лябд
            // Получаем уникальные имена файлов
            List<string> uniqueFileNames = new List<string>();

            foreach (Quest q in quests)
            {
                if (!string.IsNullOrEmpty(q.FileName) && !uniqueFileNames.Contains(q.FileName))
                {
                    uniqueFileNames.Add(q.FileName);
                }
            }

            // Создаем коллекцию из преференсов 
            ObservableCollection<ObservableCollection<Quest>> QfromPreferences = new ObservableCollection<ObservableCollection<Quest>>();

            foreach (string fName in uniqueFileNames)
            {
                string json = Preferences.Get(fName, null);
                ObservableCollection<Quest> deserializedCollection = JsonSerializer.Deserialize<ObservableCollection<Quest>>(json);

                QfromPreferences.Add(deserializedCollection ?? new ObservableCollection<Quest>());
            }


            // заменяем полученные в преференсной коллекции.
            foreach (Quest qFromInput in quests)
            {
                foreach (ObservableCollection<Quest> qCollectionFromPreferences in QfromPreferences)
                {
                    for (int i = 0; i < qCollectionFromPreferences.Count; i++)                     
                    {
                        if (qFromInput.FileName == qCollectionFromPreferences[i].FileName && 
                            qFromInput.number == qCollectionFromPreferences[i].number) 
                            qCollectionFromPreferences[i] = qFromInput;                        
                    }
                }            
            }

            // сохраняем обратно измененную версию
            foreach (ObservableCollection<Quest> qCollectionFromPreferencesСhanged in QfromPreferences)
            {
                Preferences.Set(qCollectionFromPreferencesСhanged[0].FileName, JsonSerializer.Serialize(qCollectionFromPreferencesСhanged)); 
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
