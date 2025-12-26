using System;
using System.Linq;
using SyllabusManager.App.Interfaces;
using SyllabusManager.App.Models;

namespace SyllabusManager.App.Services
{
    public interface IUserManagementService
    {
        void CreateUser(User creator, string name, string password, string department, string roleType);
        void DeleteUser(User creator, string userId);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly IDataRepository _repo;

        public UserManagementService(IDataRepository repo)
        {
            _repo = repo;
        }

        public void CreateUser(User creator, string name, string password, string department, string roleType)
        {
            if (!(creator is AdminUser))
            {
                Console.WriteLine("Error: Only Admins can create users.");
                return;
            }

            string newId = (_repo.Users.Count + 1).ToString();
            User newUser = null;

            switch (roleType.ToLower())
            {
                case "instructor":
                    newUser = new InstructorUser(newId, name, department);
                    break;
                case "head":
                case "hod":
                    newUser = new HeadOfDepartmentUser(newId, name, department);
                    break;
                case "admin":
                    newUser = new AdminUser(newId, name, department);
                    break;
                default:
                    Console.WriteLine("Invalid role type.");
                    return;
            }

            newUser.Password = password;
            _repo.Users.Add(newUser);
            // Since we don't have a direct SaveUsers for mock listing in repo (it was init in constructor), 
            // but the requirement says "logs logs".
            // Assuming in-memory persistence for the session or if SaveFile exists we call it.
            // checking JsonDataRepository... it has SaveFile but it is private usually or specific.
            // The user request emphasized LOGS.
            
            Console.WriteLine($"User {newUser.Name} ({newUser.GetType().Name}) created.");
            _repo.AddLog($"User Created: {newUser.Name} (Role: {roleType}, Dept: {department}) by {creator.Name}");
        }

        public void DeleteUser(User creator, string userId)
        {
             if (!(creator is AdminUser))
            {
                Console.WriteLine("Error: Only Admins can delete users.");
                return;
            }

            var user = _repo.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            // Prevent deleting self? Maybe.
            if (user.Id == creator.Id)
            {
                 Console.WriteLine("Cannot delete yourself.");
                 return;
            }

            _repo.Users.Remove(user);
            Console.WriteLine($"User {user.Name} deleted.");
            _repo.AddLog($"User Deleted: {user.Name} (ID: {userId}) by {creator.Name}");
        }
    }
}
