using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using SyllabusManager.App.Models;

namespace SyllabusManager.App.Data
{
    public class DataService
    {
        private readonly string _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        // In-memory cache
        public List<Syllabus> Syllabi { get; private set; } = new List<Syllabus>();
        public List<Commit> Commits { get; private set; } = new List<Commit>();
        public List<Subscription> Subscriptions { get; private set; } = new List<Subscription>();
        public List<User> Users { get; private set; } = new List<User>();

        public DataService()
        {
            if (!Directory.Exists(_dataDir)) Directory.CreateDirectory(_dataDir);
            LoadData();
        }

        private void LoadData()
        {
            Syllabi = LoadFile<List<Syllabus>>("syllabi.json") ?? new List<Syllabus>();
            Commits = LoadFile<List<Commit>>("commits.json") ?? new List<Commit>();
            Subscriptions = LoadFile<List<Subscription>>("subscriptions.json") ?? new List<Subscription>();

            // Mock Users if empty
            Users = LoadFile<List<User>>("users.json");
            if (Users == null || !Users.Any())
            {
                Users = new List<User>
                {
                    new InstructorUser("1", "Kaya Oguz", "CE"),
                    new InstructorUser("2", "John Doe", "SE"),
                    new HeadOfDepartmentUser("3", "Jane Smith", "CE"),
                    new AdminUser("99", "Admin", "ALL")
                };
            }
        }

        public void SaveSyllabi() => SaveFile("syllabi.json", Syllabi);
        public void SaveCommits() => SaveFile("commits.json", Commits);
        public void SaveSubscriptions() => SaveFile("subscriptions.json", Subscriptions);

        private T LoadFile<T>(string fileName)
        {
            string path = Path.Combine(_dataDir, fileName);
            if (!File.Exists(path)) return default;
            try
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }

        private void SaveFile<T>(string fileName, T data)
        {
            string path = Path.Combine(_dataDir, fileName);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
