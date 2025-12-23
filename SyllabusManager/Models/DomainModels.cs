using System;
using System.Collections.Generic;

namespace SyllabusManager.App.Models
{
    public class Syllabus
    {
        public string CourseCode { get; set; } // e.g. "CE221"
        public string Title { get; set; }
        public string Semester { get; set; }
        // We use a dictionary to allow flexible JSON fields as requested
        public Dictionary<string, object> Content { get; set; } = new Dictionary<string, object>();

        public override string ToString()
        {
            return $"{CourseCode} - {Title} ({Semester})";
        }
    }

    public class Commit
    {
        public string CommitId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } // "Why"
        public string Diff { get; set; } // "What" changed
        public Syllabus Snapshot { get; set; } // The state at this commit

        public Commit()
        {
            CommitId = Guid.NewGuid().ToString().Substring(0, 8);
            Timestamp = DateTime.Now;
        }
    }

    public class Subscription
    {
        public string ObserverId { get; set; }
        public string CoursePattern { get; set; } // e.g., "CE", "SE" or specific "CE221"
    }
}
