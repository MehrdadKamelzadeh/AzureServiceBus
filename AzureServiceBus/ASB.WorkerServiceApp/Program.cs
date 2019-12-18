using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ASB.WorkerServiceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(configure =>                                 
            {
                configure.Service<SpectrumWorkerService>(service =>                               
                {
                    service.ConstructUsing(name => new SpectrumWorkerService());                
                    service.WhenStarted(tc => tc.Start());                         
                    service.WhenStopped(tc => tc.Stop());                          
                });
                configure.RunAsLocalSystem();                                      

                configure.SetDescription("Sample Topshelf Host");                   
                configure.SetDisplayName("Stuff");                                  
                configure.SetServiceName("Stuff");                                  
            });                                                             

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  
            Environment.ExitCode = exitCode;
        }
    }
}
