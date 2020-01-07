using Zoey.Quartz.Job;
using log4net;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Zoey.Quartz.Server
{
	/// <summary>
	/// The main server logic.
	/// </summary>
	public class QuartzWorker : BackgroundService
	{
		private readonly ILog _logger;
		private readonly IScheduler _scheduler;
		private readonly IJobFactory _jobFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="QuartzWorker"/> class.
		/// </summary>
		public QuartzWorker(IJobFactory jobFactory, IScheduler scheduler)
		{
			_scheduler = scheduler;
			_jobFactory = jobFactory;
			_logger = LogManager.GetLogger(GetType());
		}


		/// <summary>
		/// Starts this instance, delegates to scheduler.
		/// </summary>
		public virtual async Task StartAsync()
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				_logger.Fatal($"Scheduler start failed: {ex.Message}", ex);
				throw;
			}

			_logger.Info("Scheduler started successfully");
		}

		/// <summary>
		/// Stops this instance, delegates to scheduler.
		/// </summary>
		public virtual async Task StopAsync()
		{
			try
			{
				await _scheduler.Shutdown(true);
			}
			catch (Exception ex)
			{
				_logger.Error($"Scheduler stop failed: {ex.Message}", ex);
				throw;
			}

			_logger.Info("Scheduler shutdown complete");
		}

		/// <summary>
		/// Initializes the instance of the <see cref="QuartzWorker"/> class.
		/// </summary>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				var job = JobBuilder.Create<HttpJob>().WithIdentity("httpjob", "group1")
					.SetJobData(new HttpJobDataMap
					{
						ApiUrl = "http://baidu.com",
						RequestType = "get"
					})
					.Build();
				ITrigger trigger = TriggerBuilder.Create()
								   .StartNow()
								   .WithIdentity("httpjob", "group1")
								   .WithDescription("HttpJob")
								   .WithCronSchedule("0/10 * * * * ? ")
								   .Build();
				_scheduler.JobFactory = _jobFactory;
				await _scheduler.ScheduleJob(job, trigger);
				await _scheduler.Start();
			}
			catch (Exception e)
			{
				_logger.Error("Server initialization failed:" + e.Message, e);
				throw;
			}
		}
	}
}
