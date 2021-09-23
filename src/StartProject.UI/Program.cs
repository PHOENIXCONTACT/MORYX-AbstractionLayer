using System;
using Moryx.ClientFramework.Kernel;

namespace StartProject.UI
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var heartOfLead = new HeartOfLead(args);
            heartOfLead.AuthorizeEverything();
            heartOfLead.Initialize();
            heartOfLead.Start();
        }
    }
}
