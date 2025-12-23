using System;
using System.Collections.Generic;
using SyllabusManager.App.Models;

namespace SyllabusManager.App.Services
{
    // LO5: Polymorphism - Interface for notification channels
    public interface INotificationChannel
    {
        void Send(string recipient, string message);
    }

    public class EmailChannel : INotificationChannel
    {
        public void Send(string recipient, string message)
        {
            // Simulated Email
            Console.WriteLine($"[Email] Sent to {recipient}: {message} (simulated)");
        }
    }

    public class SmsChannel : INotificationChannel
    {
        public void Send(string recipient, string message)
        {
            // Simulated SMS
            Console.WriteLine($"[SMS] Sent to {recipient}: {message} (simulated)");
        }
    }

    public class NotificationService
    {
        private readonly List<INotificationChannel> _channels;
        private readonly List<Subscription> _subscriptions;
        private readonly List<User> _users; // Repository of users to find emails/numbers

        public NotificationService(List<User> users, List<Subscription> subscriptions)
        {
            _users = users;
            _subscriptions = subscriptions;
            _channels = new List<INotificationChannel>
            {
                new EmailChannel(),
                new SmsChannel()
            };
        }

        public void Notify(Commit commit, string courseCode)
        {
            Console.WriteLine($"Processing notifications for update on {courseCode}...");

            // Find interested observers
            foreach (var sub in _subscriptions)
            {
                if (courseCode.StartsWith(sub.CoursePattern) || courseCode == sub.CoursePattern)
                {
                    var user = _users.Find(u => u.Id == sub.ObserverId);
                    if (user != null)
                    {
                        string message = $"Course {courseCode} has been updated by {commit.AuthorName}. Message: {commit.Message}";

                        // LO5: Polymorphism in action - iterating over different channel implementations
                        foreach (var channel in _channels)
                        {
                            channel.Send(user.Name, message);
                        }
                    }
                }
            }
        }

        public void Subscribe(string userId, string pattern)
        {
            _subscriptions.Add(new Subscription { ObserverId = userId, CoursePattern = pattern });
            Console.WriteLine($"User {userId} subscribed to {pattern}");
        }
    }
}
