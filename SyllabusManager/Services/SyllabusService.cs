using System;
using System.Collections.Generic;
using System.Linq;
using SyllabusManager.App.Interfaces;
using SyllabusManager.App.Models;

namespace SyllabusManager.App.Services
{
    public class SyllabusService
    {
        private readonly IDataRepository _data;
        private readonly IAuthorizationService _auth;
        private readonly IVersionControlService _vcs;
        private readonly INotificationService _notify;

        public SyllabusService(IDataRepository data, IAuthorizationService auth, IVersionControlService vcs, INotificationService notify)
        {
            _data = data;
            _auth = auth;
            _vcs = vcs;
            _notify = notify;
        }

        public void CreateSyllabus(User user, Syllabus syllabus)
        {
            if (!_auth.CanCreateOrEdit(user, syllabus.CourseCode))
            {
                Console.WriteLine("Access Denied: You cannot create this syllabus.");
                _data.AddLog($"Access Denied: User {user.Name} tried to create {syllabus.CourseCode}");
                return;
            }

            if (_data.Syllabi.Any(s => s.CourseCode == syllabus.CourseCode))
            {
                Console.WriteLine("Error: Syllabus already exists.");
                return;
            }

            _data.Syllabi.Add(syllabus);

            var commit = _vcs.CreateCommit(null, syllabus, user, "Initial creation");
            _data.Commits.Add(commit);

            _data.SaveSyllabi();
            _data.SaveCommits();

            Console.WriteLine($"Syllabus {syllabus.CourseCode} created successfully.");
            _data.AddLog($"Created Syllabus: {syllabus.CourseCode} by {user.Name}");
            
            _notify.Notify(commit, syllabus.CourseCode);
        }

        public void UpdateSyllabus(User user, string courseCode, Syllabus newVersion, string commitMessage)
        {
            var existing = _data.Syllabi.FirstOrDefault(s => s.CourseCode == courseCode);
            if (existing == null)
            {
                Console.WriteLine("Error: Syllabus not found.");
                return;
            }

            if (!_auth.CanCreateOrEdit(user, courseCode))
            {
                Console.WriteLine("Access Denied: You cannot edit this syllabus.");
                _data.AddLog($"Access Denied: User {user.Name} tried to edit {courseCode}");
                return;
            }

            var commit = _vcs.CreateCommit(existing, newVersion, user, commitMessage);

            existing.Title = newVersion.Title;
            existing.Semester = newVersion.Semester;
            existing.Content = newVersion.Content;

            _data.Commits.Add(commit);
            _data.SaveSyllabi();
            _data.SaveCommits();

            Console.WriteLine($"Syllabus {courseCode} updated successfully.");
            _data.AddLog($"Updated Syllabus: {courseCode} by {user.Name}");
            
            _notify.Notify(commit, courseCode);
        }

        public void DeleteSyllabus(User user, string courseCode)
        {
            if (!_auth.CanDelete(user))
            {
                Console.WriteLine("Access Denied: You cannot delete syllabi.");
                _data.AddLog($"Access Denied: User {user.Name} tried to delete {courseCode}");
                return;
            }

            var existing = _data.Syllabi.FirstOrDefault(s => s.CourseCode == courseCode);
            if (existing != null)
            {
                _data.Syllabi.Remove(existing);
                _data.SaveSyllabi();
                Console.WriteLine($"Syllabus {courseCode} deleted.");
                _data.AddLog($"Deleted Syllabus: {courseCode} by {user.Name}");
            }
        }

        public Syllabus GetSyllabus(string courseCode)
        {
            return _data.Syllabi.FirstOrDefault(s => s.CourseCode == courseCode);
        }

        public List<Syllabus> GetAll()
        {
            return _data.Syllabi;
        }
    }
}
