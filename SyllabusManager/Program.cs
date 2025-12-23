using System;
using System.Collections.Generic;
using System.Linq;
using SyllabusManager.App.Data;
using SyllabusManager.App.Models;
using SyllabusManager.App.Services;

namespace SyllabusManager.App
{
    class Program
    {
        static DataService _data;
        static SyllabusService _syllabusService;
        static NotificationService _notifyService;
        static User _currentUser;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing Syllabus Manager...");

            // Setup Dependencies
            _data = new DataService();
            var auth = new AuthorizationService();
            var vcs = new VersionControlService();
            _notifyService = new NotificationService(_data.Users, _data.Subscriptions);
            _syllabusService = new SyllabusService(_data, auth, vcs, _notifyService);

            while (true)
            {
                if (_currentUser == null)
                {
                    ShowLogin();
                }
                else
                {
                    ShowMenu();
                }
            }
        }

        static void ShowLogin()
        {
            Console.WriteLine("\n=== LOGIN ===");
            Console.WriteLine("Select a user:");
            for (int i = 0; i < _data.Users.Count; i++)
            {
                var u = _data.Users[i];
                Console.WriteLine($"{i + 1}. {u.Name} ({u.GetRoleDescription()}) [Dept: {u.Department}]");
            }
            Console.Write("Choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _data.Users.Count)
            {
                _currentUser = _data.Users[choice - 1];
                Console.WriteLine($"Logged in as {_currentUser.Name}");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine($"\n=== MENU (User: {_currentUser.Name}) ===");
            Console.WriteLine("1. List Syllabi");
            Console.WriteLine("2. Create Syllabus");
            Console.WriteLine("3. Update Syllabus");
            Console.WriteLine("4. Delete Syllabus");
            Console.WriteLine("5. View Syllabus Details");
            Console.WriteLine("6. View Version History (Git Sim)");
            Console.WriteLine("7. Subscribe to Notifications");
            Console.WriteLine("8. Logout");
            Console.WriteLine("9. Exit");
            Console.Write("Select: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1": ListSyllabi(); break;
                case "2": CreateSyllabus(); break;
                case "3": UpdateSyllabus(); break;
                case "4": DeleteSyllabus(); break;
                case "5": ViewSyllabus(); break;
                case "6": ViewHistory(); break;
                case "7": Subscribe(); break;
                case "8": _currentUser = null; break;
                case "9": Environment.Exit(0); break;
                default: Console.WriteLine("Invalid option."); break;
            }
        }

        static void ListSyllabi()
        {
            var list = _syllabusService.GetAll();
            Console.WriteLine("\n--- Syllabi List ---");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        static void CreateSyllabus()
        {
            Console.Write("Enter Course Code (e.g. CE221): ");
            var code = Console.ReadLine();
            Console.Write("Enter Title: ");
            var title = Console.ReadLine();
            Console.Write("Enter Semester: ");
            var semester = Console.ReadLine();

            // Flexible content
            var content = new Dictionary<string, object>();
            Console.WriteLine("Enter content fields (key=value). Enter 'done' to finish.");
            while (true)
            {
                Console.Write("Key: ");
                var key = Console.ReadLine();
                if (key == "done" || string.IsNullOrWhiteSpace(key)) break;
                Console.Write("Value: ");
                var val = Console.ReadLine();
                content[key] = val;
            }

            var s = new Syllabus
            {
                CourseCode = code,
                Title = title,
                Semester = semester,
                Content = content
            };

            _syllabusService.CreateSyllabus(_currentUser, s);
        }

        static void UpdateSyllabus()
        {
            Console.Write("Enter Course Code to update: ");
            var code = Console.ReadLine();
            var existing = _syllabusService.GetSyllabus(code);
            if (existing == null)
            {
                Console.WriteLine("Not found.");
                return;
            }

            // Clone for 'new version'
            var newVer = new Syllabus
            {
                CourseCode = existing.CourseCode,
                Title = existing.Title,
                Semester = existing.Semester,
                Content = new Dictionary<string, object>(existing.Content)
            };

            Console.WriteLine("Leave fields blank to keep current value.");

            Console.Write($"New Title [{existing.Title}]: ");
            var t = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(t)) newVer.Title = t;

            Console.Write($"New Semester [{existing.Semester}]: ");
            var s = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(s)) newVer.Semester = s;

            Console.WriteLine("Edit Content? (y/n)");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Enter content fields (key=value). Existing keys will he overwritten.");
                while (true)
                {
                    Console.Write("Key: ");
                    var key = Console.ReadLine();
                    if (key == "done" || string.IsNullOrWhiteSpace(key)) break;
                    Console.Write("Value: ");
                    var val = Console.ReadLine();
                    newVer.Content[key] = val;
                }
            }

            Console.Write("Commit Message (Why are you changing this?): ");
            var msg = Console.ReadLine();

            _syllabusService.UpdateSyllabus(_currentUser, code, newVer, msg);
        }

        static void DeleteSyllabus()
        {
            Console.Write("Enter Course Code: ");
            var code = Console.ReadLine();
            _syllabusService.DeleteSyllabus(_currentUser, code);
        }

        static void ViewSyllabus()
        {
            Console.Write("Enter Course Code: ");
            var code = Console.ReadLine();
            var s = _syllabusService.GetSyllabus(code);
            if (s != null)
            {
                Console.WriteLine($"\nCode: {s.CourseCode}");
                Console.WriteLine($"Title: {s.Title}");
                Console.WriteLine($"Semester: {s.Semester}");
                Console.WriteLine("Content:");
                foreach (var kvp in s.Content)
                    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            else
            {
                Console.WriteLine("Not found.");
            }
        }

        static void ViewHistory()
        {
            Console.Write("Enter Course Code (or leave empty for all): ");
            var code = Console.ReadLine();

            // Filter commits
            var commits = _data.Commits;
            if (!string.IsNullOrEmpty(code))
            {
                commits = commits.Where(c => c.Snapshot.CourseCode == code).ToList();
            }

            Console.WriteLine("\n--- COMMIT HISTORY ---");
            foreach (var c in commits)
            {
                Console.WriteLine($"ID: {c.CommitId} | Time: {c.Timestamp} | Author: {c.AuthorName}");
                Console.WriteLine($"Message: {c.Message}");
                Console.WriteLine($"Diff:\n{c.Diff}");
                Console.WriteLine("----------------------");
            }
        }

        static void Subscribe()
        {
            Console.Write("Enter User ID to subscribe for (Current: " + _currentUser.Id + "): ");
            var uid = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(uid)) uid = _currentUser.Id;

            Console.Write("Enter Course Pattern (e.g. CE, SE, CE221): ");
            var pattern = Console.ReadLine();

            _notifyService.Subscribe(uid, pattern);
            _data.SaveSubscriptions();
        }
    }
}
