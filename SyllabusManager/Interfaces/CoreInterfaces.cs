using System.Collections.Generic;
using SyllabusManager.App.Models;

namespace SyllabusManager.App.Interfaces
{
    public interface IDataRepository
    {
        List<Syllabus> Syllabi { get; }
        List<Commit> Commits { get; }
        List<Subscription> Subscriptions { get; }
        List<User> Users { get; }
        void SaveSyllabi();
        void SaveCommits();
        void SaveSubscriptions();
        void AddLog(string logEntry);
    }

    public interface INotificationChannel
    {
        void Send(string recipient, string message);
    }

    public interface INotificationService
    {
        void Notify(Commit commit, string courseCode);
        void Subscribe(string userId, string pattern);
    }

    public interface IAuthorizationService
    {
        bool CanCreateOrEdit(User user, string courseCode);
        bool CanDelete(User user);
    }

    public interface IVersionControlService
    {
        Commit CreateCommit(Syllabus oldVer, Syllabus newVer, User author, string message);
    }
}
