using System;
using System.Collections.Generic;
using System.Linq;
using SyllabusManager.App.Models;
using SyllabusManager.App.Interfaces;

namespace SyllabusManager.App.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool CanCreateOrEdit(User user, string courseCode)
        {
            if (user is AdminUser) return true;
            // HEAD OF DEPARTMENT: Can create/edit any course
            if (user is HeadOfDepartmentUser) return true; 

            if (user is InstructorUser instructor)
            {
                if (courseCode.StartsWith(instructor.Department, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanDelete(User user)
        {
            if (user is AdminUser) return true;
            if (user is HeadOfDepartmentUser) return true;
            if (user is InstructorUser) return true;
            return false;
        }
    }

    public class VersionControlService : IVersionControlService
    {
        public Commit CreateCommit(Syllabus oldVer, Syllabus newVer, User author, string message)
        {
            var commit = new Commit
            {
                AuthorId = author.Id,
                AuthorName = author.Name,
                Message = message,
                Snapshot = newVer 
            };

            commit.Diff = ComputeDiff(oldVer, newVer);
            return commit;
        }

        private string ComputeDiff(Syllabus oldVer, Syllabus newVer)
        {
            if (oldVer == null) return "Initial Commit - New Syllabus Created";

            var diffs = new List<string>();

            if (oldVer.Title != newVer.Title)
                diffs.Add($"Title changed: '{oldVer.Title}' -> '{newVer.Title}'");

            if (oldVer.Semester != newVer.Semester)
                diffs.Add($"Semester changed: '{oldVer.Semester}' -> '{newVer.Semester}'");

            var allKeys = oldVer.Content.Keys.Union(newVer.Content.Keys);
            foreach (var key in allKeys)
            {
                var oldVal = oldVer.Content.ContainsKey(key) ? oldVer.Content[key]?.ToString() : "null";
                var newVal = newVer.Content.ContainsKey(key) ? newVer.Content[key]?.ToString() : "null";

                if (oldVal != newVal)
                {
                    diffs.Add($"Content['{key}'] changed: '{oldVal}' -> '{newVal}'");
                }
            }

            if (diffs.Count == 0) return "No changes detected.";
            return string.Join("\n", diffs);
        }
    }
}
