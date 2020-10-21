using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Jobnium.Model;
using Jobnium.Abstract;
using System.Linq;

namespace Jobnium
{
    public abstract class BaseJob<T> : HostedService where T : class
    {
        protected abstract void Run();

        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                var runSecond = 3;
                var jobName = typeof(T).Name;
                try
                {
                    using (var dbContext = new SqLiteDbContext())
                    {
                        var detail = dbContext.JobTable.FirstOrDefault(f => f.JobName == jobName);
                        if (detail != null && detail.Status)
                        {
                            runSecond = detail.RunSecond;

                            var timer = new Stopwatch();
                            try
                            {
                                var addRecord = dbContext.JobLog.Add(new JobLog { JobName = detail.JobName, Status = (int)JobLogStatus.Pending });
                                dbContext.SaveChanges();
                                timer.Start();
                                Run();
                                timer.Stop();

                                var updateDetail = dbContext.JobLog.FirstOrDefault(f => f.Id == addRecord.Entity.Id);
                                if (updateDetail != null)
                                {
                                    updateDetail.EndDate = DateTime.Now;
                                    updateDetail.ElapsedMilliseconds = timer.ElapsedMilliseconds;
                                    updateDetail.Message = JobLogStatus.Completed.ToString();
                                    updateDetail.Status = (int)JobLogStatus.Completed;
                                    dbContext.SaveChanges();

                                    dbContext.Update(updateDetail);
                                }
                            }
                            catch (Exception exception)
                            {
                                dbContext.JobLog.Add(new JobLog { JobName = detail.JobName, Message = exception.StackTrace, ElapsedMilliseconds = timer.ElapsedMilliseconds, Status = (int)JobLogStatus.Error });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error : {ex}");
                }

                await Task.Delay(TimeSpan.FromSeconds(runSecond), cToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var dbContext = new SqLiteDbContext())
                {
                    var list = dbContext.JobLog.Where(f => f.Status == (int)JobLogStatus.Pending).ToList();
                    foreach (var jobLog in list)
                    {
                        jobLog.Status = (int)JobLogStatus.Error;
                        jobLog.EndDate = DateTime.Now;
                        jobLog.Message = "Service Stopped!";
                        jobLog.ElapsedMilliseconds = (long)(jobLog.EndDate - jobLog.StartDate).TotalMilliseconds;

                        dbContext.Update(jobLog);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex}");
            }

            return base.StopAsync(cancellationToken);
        }
    }
}