using System;
using SyllabusManager.App.Commands;
using SyllabusManager.App.Factories;
using SyllabusManager.App.Interfaces;
using SyllabusManager.App.Models;

namespace SyllabusManager.App
{
    class Program
    {
        static User _currentUser;
        static IDataRepository _data;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing Syllabus Manager with Design Patterns...");
            
            // Initialization via Factory
            _data = ServiceFactory.GetDataRepository();

            while (true)
            {
                if (_currentUser == null)
                {
                    ShowLogin();
                }
                else
                {
                    ShowMenu();
                }
            }
        }

        static void ShowLogin()
        {
            Console.WriteLine("\n=== LOGIN ===");
            Console.WriteLine("Select a user:");
            for (int i = 0; i < _data.Users.Count; i++)
            {
                var u = _data.Users[i];
                Console.WriteLine($"{i + 1}. {u.Name} [Pass: {u.Password}] ({u.GetRoleDescription()}) [Dept: {u.Department}]");
            }
            Console.Write("Choice (0 to Exit): ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0)
                {
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                }

                if (choice > 0 && choice <= _data.Users.Count)
                {
                    var selectedUser = _data.Users[choice - 1];
                    Console.Write($"Enter Password for {selectedUser.Name}: ");
                    var password = ReadPassword();

                    if (password == selectedUser.Password)
                    {
                        _currentUser = selectedUser;
                        Console.WriteLine($"Logged in as {_currentUser.Name}");
                        _data.AddLog($"Login: {_currentUser.Name}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Password.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        static string ReadPassword()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if(key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                }
            } while (true);
            return pass;
        }

        static void ShowMenu()
        {
            // Prepare Invoker
            var invoker = new CommandInvoker();
            var syllabusService = ServiceFactory.GetSyllabusService();
            var notifyService = ServiceFactory.GetNotificationService();

            invoker.Register("1", new ListSyllabiCommand(syllabusService, _currentUser));
            invoker.Register("2", new CreateSyllabusCommand(syllabusService, _currentUser));
            invoker.Register("3", new UpdateSyllabusCommand(syllabusService, _currentUser));
            invoker.Register("4", new DeleteSyllabusCommand(syllabusService, _currentUser));
            invoker.Register("5", new ViewSyllabusCommand(syllabusService, _currentUser));
            invoker.Register("6", new ViewHistoryCommand(_data));
            invoker.Register("7", new SubscribeCommand(notifyService, _currentUser));
            invoker.Register("8", new RevertCommand(syllabusService, _currentUser));
            invoker.Register("9", new LogoutCommand(() => { 
                Console.WriteLine($"Logging out {_currentUser.Name}...");
                _currentUser = null; 
            }));

            // ADMIN COMMANDS
            if (_currentUser is AdminUser)
            {
                var userService = ServiceFactory.GetUserManagementService();
                invoker.Register("10", new CreateUserCommand(userService, _currentUser));
                invoker.Register("11", new DeleteUserCommand(userService, _currentUser));
            }

            invoker.Register("0", new ExitCommand());

            Console.WriteLine($"\n=== MENU (User: {_currentUser.Name}) ===");
            invoker.ShowMenu();
            var input = Console.ReadLine();
            invoker.ExecuteCommand(input);
        }
    }
}
