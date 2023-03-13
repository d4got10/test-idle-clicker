using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Services
{
    public class SaveLoadService
    {
        public SaveLoadService(string saveDirectory, string extension)
        {
            _saveDirectory = Path.Combine(Application.persistentDataPath, saveDirectory);
            _extension = extension;
        }

        private readonly string _saveDirectory;
        private readonly string _extension;

        public bool Exists(string fileName)
        {
            var path = Path.Combine(_saveDirectory, fileName + "." + _extension);
            return File.Exists(path);
        }

        public void Save<T>(string fileName, T data)
        {
            var path = Path.Combine(_saveDirectory, fileName + "." + _extension);
            try
            {
                Directory.CreateDirectory(_saveDirectory);
                var json = JsonConvert.SerializeObject(data);
                using var stream = new FileStream(path, FileMode.Create);
                using var writer = new BinaryWriter(stream);
                writer.Write(json);
            }
            catch(Exception e)
            {
                Debug.LogError($"Error occured while trying to save data to file: {path}\n{e}");
            }
        }

        public bool TryLoad<T>(string fileName, out T value)
        {
            var path = Path.Combine(_saveDirectory, fileName + "." + _extension);
            value = default;

            if (!File.Exists(path))
            {
                Debug.LogError($"No save file was found for loading: {path}");
                return false;
            }
            try
            {
                using var stream = new FileStream(path, FileMode.Open);
                using var reader = new BinaryReader(stream);
                var json = reader.ReadString();
                var data = JsonConvert.DeserializeObject<T>(json);
                value = data;
                return true;
            }
            catch(Exception e)
            {
                Debug.LogError($"Error occured while trying to load data from file: {path}\n{e}");
            }
            return false;
        }
    }
}