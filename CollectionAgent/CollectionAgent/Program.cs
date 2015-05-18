using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CollectionAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // If there are no command-line arguments, then run this as a service
            if (args.Length < 1)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new CollectionAgentService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                // If there are no command-line arguments, then run this as a service
                CollectionAgentService svc = new CollectionAgentService();
                svc.runService(args[0], Convert.ToInt32(args[1]));
            }
        }
    }
}
