using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LockNess.Invoker.Commands
{
    public class LockCmd : ICommand
    {
        [DllImport("User32.dll")]
        static extern Boolean LockWorkStation();

        public string Name
        {
            get { return "lock"; }
        }

        public void Execute()
        {
            Console.WriteLine("[i]\tLocking workstation...");
            LockWorkStation();
        }

        public void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
