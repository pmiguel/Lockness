using LockNess.Comm;
using LockNess.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockNess
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser();
            MessageReceiver mr = new MessageReceiver(ref p);
            mr.Start();
        }
    }
}
