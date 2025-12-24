using System;
using System.Collections.Generic;
using SyllabusManager.App.Models;
using SyllabusManager.App.Interfaces;
using System.Linq;

namespace SyllabusManager.App.Services
{
    public class NotificationService : INotificationService
    {
        private readonly List<INotificationChannel> _channels;
        private readonly IDataRepository _repo;

        public NotificationService(IDataRepository repo)
        {
            _repo = repo;
            _channels = new List<INotificationChannel>();
        }

        public void AddChannel(INotificationChannel channel)
        {
            _channels.Add(channel);
        }

        public void Notify(Commit commit, string courseCode)
        {
            Console.WriteLine($"Processing notifications for update on {courseCode}...");
            
            // Retrieve subscriptions from Repo
            var subscriptions = _repo.Subscriptions;

            foreach (var sub in subscriptions)
            {
                if (courseCode.StartsWith(sub.CoursePattern) || courseCode == sub.CoursePattern)
                {
                    var user = _repo.Users.FirstOrDefault(u => u.Id == sub.ObserverId);
                    if (user != null)
                    {
                        string message = $"Course {courseCode} has been updated by {commit.AuthorName}. Message: {commit.Message}";
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
            _repo.Subscriptions.Add(new Subscription { ObserverId = userId, CoursePattern = pattern });
            _repo.SaveSubscriptions();
            Console.WriteLine($"User {userId} subscribed to {pattern}");
            _repo.AddLog($"User {userId} subscribed to {pattern}");
        }
    }
}
