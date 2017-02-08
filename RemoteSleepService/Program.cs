using System.ServiceProcess;

namespace RemoteSleepService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new RemoteSleep()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
