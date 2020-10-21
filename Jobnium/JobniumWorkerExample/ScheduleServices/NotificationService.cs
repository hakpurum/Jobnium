using Jobnium;
using System;
using System.Globalization;

namespace JobniumWorkerExample.ScheduleServices
{
    public class NotificationService : BaseJob<NotificationService>, IJob
    {
        protected override void Run()
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} Bildirimler gönderildi.");
        }
    }
}