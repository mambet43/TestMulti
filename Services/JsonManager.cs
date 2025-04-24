using System.Reflection;
using System.Text.Json;
using TestMulti.Models;

namespace TestMulti.Services
{
    internal static class JsonManager
    {
        public static Quest[] DeserializeFromJson(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = $"TestMulti.Resources.Raw.{fileName}";

            using Stream stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                throw new FileNotFoundException($"Файл {fileName} не найден в ресурсах.");
            }

            using StreamReader reader = new StreamReader(stream);
            var jsonContent = reader.ReadToEnd(); // Используем синхронное чтение

            return JsonSerializer.Deserialize<Quest[]>(jsonContent) ?? Array.Empty<Quest>(); // Добавляем защиту от null
        }
    }
}
