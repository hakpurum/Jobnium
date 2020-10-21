using Jobnium;
using System;
using System.Globalization;
using System.Threading;

namespace JobniumWorkerExample.ScheduleServices
{
    public class MailService : BaseJob<MailService>, IJob
    {
        protected override void Run()
        {
            Thread.Sleep(new Random().Next(350, 10000));//Deneme amaçlı bekletilmiştir.

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} Mailler gönderildi.");
        }
    }
}