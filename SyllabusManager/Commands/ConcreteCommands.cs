using System;
using System.Collections.Generic;
using System.Linq;
using SyllabusManager.App.Interfaces;
using SyllabusManager.App.Models;
using SyllabusManager.App.Services;

namespace SyllabusManager.App.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected readonly SyllabusService _service;
        protected readonly User _user;

        public abstract string Description { get; }

        protected BaseCommand(SyllabusService service, User user)
        {
            _service = service;
            _user = user;
        }

        public abstract void Execute();
    }

    public class ListSyllabiCommand : BaseCommand
    {
        public ListSyllabiCommand(SyllabusService service, User user) : base(service, user) { }

        public override string Description => "List Syllabi";

        public override void Execute()
        {
            var list = _service.GetAll();
            Console.WriteLine("\n--- Syllabi List ---");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class CreateSyllabusCommand : BaseCommand
    {
        public CreateSyllabusCommand(SyllabusService service, User user) : base(service, user) { }
        public override string Description => "Create Syllabus";

        public override void Execute()
        {
            Console.Write("Enter Course Code (e.g. CE221): ");
            var code = Console.ReadLine();
            Console.Write("Enter Title: ");
            var title = Console.ReadLine();
            Console.Write("Enter Semester: ");
            var semester = Console.ReadLine();

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

            _service.CreateSyllabus(_user, s);
        }
    }

    public class UpdateSyllabusCommand : BaseCommand
    {
        public UpdateSyllabusCommand(SyllabusService service, User user) : base(service, user) { }
        public override string Description => "Update Syllabus";

        public override void Execute()
        {
            Console.Write("Enter Course Code to update: ");
            var code = Console.ReadLine();
            var existing = _service.GetSyllabus(code);
            if (existing == null)
            {
                Console.WriteLine("Not found.");
                return;
            }

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

            _service.UpdateSyllabus(_user, code, newVer, msg);
        }
    }

    public class DeleteSyllabusCommand : BaseCommand
    {
        public DeleteSyllabusCommand(SyllabusService service, User user) : base(service, user) { }
        public override string Description => "Delete Syllabus";
        public override void Execute()
        {
            Console.Write("Enter Course Code: ");
            var code = Console.ReadLine();
            _service.DeleteSyllabus(_user, code);
        }
    }

    public class ViewSyllabusCommand : BaseCommand
    {
        public ViewSyllabusCommand(SyllabusService service, User user) : base(service, user) { }
        public override string Description => "View Syllabus Details";
        public override void Execute()
        {
            Console.Write("Enter Course Code: ");
            var code = Console.ReadLine();
            var s = _service.GetSyllabus(code);
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
    }

    public class SubscribeCommand : ICommand
    {
        private readonly INotificationService _notify;
        private readonly User _user;
        public string Description => "Subscribe to Notifications";

        public SubscribeCommand(INotificationService notify, User user)
        {
            _notify = notify;
            _user = user;
        }

        public void Execute()
        {
            Console.Write("Enter Course Pattern (e.g. CE, SE, CE221): ");
            var pattern = Console.ReadLine();
            _notify.Subscribe(_user.Id, pattern);
        }
    }

    public class ViewHistoryCommand : ICommand
    {
        private readonly IDataRepository _repo;
        public string Description => "View Version History";

        public ViewHistoryCommand(IDataRepository repo)
        {
            _repo = repo;
        }

        public void Execute()
        {
            Console.Write("Enter Course Code (or leave empty for all): ");
            var code = Console.ReadLine();

            var commits = _repo.Commits;
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
    }

    public class LogoutCommand : ICommand
    {
        private readonly Action _onLogout;
        public string Description => "Logout";

        public LogoutCommand(Action onLogout)
        {
            _onLogout = onLogout;
        }

        public void Execute()
        {
            _onLogout.Invoke();
        }
    }

    public class ExitCommand : ICommand
    {
        public string Description => "Exit";
        public void Execute()
        {
            Environment.Exit(0);
        }
    }
}
