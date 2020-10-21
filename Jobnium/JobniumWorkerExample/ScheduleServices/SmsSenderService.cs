using Jobnium;
using System;
using System.Globalization;

namespace JobniumWorkerExample.ScheduleServices
{
    public class SmsSenderService : BaseJob<SmsSenderService>, IJob
    {
        protected override void Run()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} Smsler gönderildi.");
        }
    }
}