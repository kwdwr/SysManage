using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using SyllabusManager.App.Models;
using SyllabusManager.App.Interfaces;

namespace SyllabusManager.App.Data
{
    public class JsonDataRepository : IDataRepository
    {
        private readonly string _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private readonly object _lock = new object(); // Thread Safety

        // In-memory cache
        public List<Syllabus> Syllabi { get; private set; } = new List<Syllabus>();
        public List<Commit> Commits { get; private set; } = new List<Commit>();
        public List<Subscription> Subscriptions { get; private set; } = new List<Subscription>();
        public List<User> Users { get; private set; } = new List<User>();
        public List<AuditLog> AuditLogs { get; private set; } = new List<AuditLog>();

        public JsonDataRepository()
        {
            if (!Directory.Exists(_dataDir)) Directory.CreateDirectory(_dataDir);
            LoadData();
        }

        private void LoadData()
        {
            lock (_lock)
            {
                Syllabi = LoadFile<List<Syllabus>>("syllabi.json") ?? new List<Syllabus>();
                Commits = LoadFile<List<Commit>>("commits.json") ?? new List<Commit>();
                Subscriptions = LoadFile<List<Subscription>>("subscriptions.json") ?? new List<Subscription>();
                AuditLogs = LoadFile<List<AuditLog>>("audit.json") ?? new List<AuditLog>();

                // Mock Users if empty
                Users = LoadFile<List<User>>("users.json");
                if (Users == null || !Users.Any())
                {
                    Users = new List<User>
                    {
                        new InstructorUser("1", "Kaya Oguz", "CE") { Password = "kayaoguz123" },
                        new InstructorUser("2", "Kadir Biberoglu", "SE") { Password = "kadir123" },
                        new HeadOfDepartmentUser("3", "Department Head", "CE") { Password = "head123" },
                        new AdminUser("99", "Admin", "ALL") { Password = "admin123" }
                    };
                    SaveFile("users.json", Users); // Persist mock users
                }
            }
        }

        public void SaveSyllabi() { lock(_lock) SaveFile("syllabi.json", Syllabi); }
        public void SaveCommits() { lock(_lock) SaveFile("commits.json", Commits); }
        public void SaveSubscriptions() { lock(_lock) SaveFile("subscriptions.json", Subscriptions); }
        
        public void AddLog(string logEntry)
        {
            lock (_lock)
            {
                var log = new AuditLog("Audit", logEntry);
                AuditLogs.Add(log);
                SaveFile("audit.json", AuditLogs);
            }
        }

        public void SaveUsers()
        {
            lock(_lock) SaveFile("users.json", Users);
        }

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
