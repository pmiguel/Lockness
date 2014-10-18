using LockNess.Invoker.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockNess.Invoker
{
    public class Parser
    {
        private Dictionary<string, ICommand> commands;

        public Parser()
        {
            this.commands = new Dictionary<string, ICommand>();
            LoadCommands();
        }

        public void LoadCommands()
        {
            AddCommand(new LockCmd());
        }

        public void AddCommand(ICommand cmd)
        {
            commands.Add(cmd.Name, cmd);
        }

        public void Parse(string input)
        {
            string[] inputCommands = input.Split(' ');
            string cmd = inputCommands[0];
            if (commands.ContainsKey(cmd))
            {
                if (inputCommands.Length > 1)
                    new Task(() => commands[cmd].Execute(inputCommands)).Start();
                else
                    new Task(() => commands[cmd].Execute()).Start();
            }
            else
                throw new Exception("Command not found.");
        }
    }
}
