using System;
using System.Collections.Generic;

namespace SyllabusManager.App.Commands
{
    public interface ICommand
    {
        void Execute();
        string Description { get; }
    }

    public class CommandInvoker
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public void Register(string key, ICommand command)
        {
            _commands[key] = command;
        }

        public void ShowMenu()
        {
            Console.WriteLine("\n=== MENU ===");
            foreach (var kvp in _commands)
            {
                Console.WriteLine($"{kvp.Key}. {kvp.Value.Description}");
            }
            Console.Write("Select: ");
        }

        public void ExecuteCommand(string key)
        {
            if (_commands.ContainsKey(key))
            {
                _commands[key].Execute();
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }
}
