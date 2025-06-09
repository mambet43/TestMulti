using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml;
using TestMulti.Constants;
using TestMulti.Models;
using TestMulti.Services;

namespace TestMulti.Services
{
    internal static class JsonManager
    { 
       

        public static async Task EditPreferences(ObservableCollection<Quest> quests)
        {
            
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
                ObservableCollection<Quest> deserializedCollection =  JsonSerializer.Deserialize<ObservableCollection<Quest>>(json);

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



        public static async Task<ObservableCollection<Quest>> DeserializeToList(string filename)
        {
            Debug.WriteLine(FileSystem.Current.AppDataDirectory);
            //Проверяем, существует ли файл в Preferences
            if (Preferences.Get(filename, null) == null)
            {
                // Получаем путь к файлу в AppDataDirectory
                string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, filename);

                // Проверяем существует ли файл
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Файл {filename} не найден в AppDataDirectory.");
                }

                // Читаем содержимое файла
                string jsonContent = File.ReadAllText(filePath);

                // Десериализуем JSON
                ObservableCollection<Quest> quests = JsonSerializer.Deserialize<ObservableCollection<Quest>>(jsonContent) ?? new ObservableCollection<Quest>();
                return quests;
            }
            else
            {
                return JsonSerializer.Deserialize<ObservableCollection<Quest>>(Preferences.Get(filename, null)) ?? new ObservableCollection<Quest>();
            }
        }


    }
}
