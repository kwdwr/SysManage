using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SyllabusManager.App.Models;

namespace SyllabusManager.App.Services
{
    public class AuthorizationService
    {
        public bool CanCreateOrEdit(User user, string courseCode)
        {
            if (user is AdminUser) return true; // Admins can do anything

            if (user is InstructorUser instructor)
            {
                // Logic: Instructor can only edit courses matching their department prefix
                // e.g. CE user -> CExxx courses
                // SE user -> SExxx courses
                if (courseCode.StartsWith(instructor.Department, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanDelete(User user)
        {
            // Only admins or maybe Instructors can delete? 
            // The prompt says "ce_user which has the privilege of creating, updating, and deleting syllabi"
            // So Instructors can delete too.
            if (user is AdminUser) return true;
            if (user is InstructorUser) return true;
            return false;
        }
    }

    public class VersionControlService
    {
        public Commit CreateCommit(Syllabus oldVer, Syllabus newVer, User author, string message)
        {
            var commit = new Commit
            {
                AuthorId = author.Id,
                AuthorName = author.Name,
                Message = message,
                Snapshot = newVer // Store the full snapshot of the new version
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

            // Compare Content Dictionary
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
