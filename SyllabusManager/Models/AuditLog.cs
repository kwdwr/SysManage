using System;

namespace SyllabusManager.App.Models
{
    public class AuditLog
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }

        public AuditLog(string action, string details)
        {
            Timestamp = DateTime.Now;
            Action = action;
            Details = details;
        }

        public override string ToString()
        {
            return $"[{Timestamp}] {Action}: {Details}";
        }
    }
}
