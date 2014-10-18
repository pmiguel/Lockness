using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockNess.Invoker.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void Execute(string[] args);
    }
}
