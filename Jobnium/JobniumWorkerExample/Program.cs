using Jobnium;
using Microsoft.Extensions.Hosting;

namespace JobniumWorkerExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddJobs("JobniumWorkerExample", @"D:\Store\TFS\AkpurumTfs\Jobnium\Jobnium\JobniumWorkerExample\JobsDb.db");
                });
    }
}
