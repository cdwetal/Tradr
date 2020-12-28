using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TradrDataSVC
{
    public partial class DataService : ServiceBase
    {
        private EventLog eventLog = null;
        private int eventId = 1;
        private Timer dayTimer = null;
        private Timer hourTimer = null;
        private Timer minuteTimer = null;

        public DataService()
        {
            InitializeComponent();
            eventLog = new EventLog();
            if (!EventLog.SourceExists("TradrDataSVC"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "TradrDataSVC", "TradrDataSVCEventLog");
            }
            eventLog.Source = "TradrDataSVC";
            eventLog.Log = "TradrDataSVCEventLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("TradrDataSVC Started Successfully", EventLogEntryType.Information, eventId++);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);

            dayTimer = new Timer();
            dayTimer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            dayTimer.Elapsed += new ElapsedEventHandler(this.OnDay);
            dayTimer.Start();

            hourTimer = new Timer();
            hourTimer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            hourTimer.Elapsed += new ElapsedEventHandler(this.OnHour);
            hourTimer.Start();

            minuteTimer = new Timer();
            minuteTimer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            minuteTimer.Elapsed += new ElapsedEventHandler(this.OnMinute);
            minuteTimer.Start();
        }

        protected override void OnStop()
        {
        }

        public void OnDay(object sender, ElapsedEventArgs args)
        {
            eventLog.WriteEntry("Running Day Processes", EventLogEntryType.Information, eventId++);
            StringBuilder log = new StringBuilder();
            log.Append("Processing...");
            log.Append("Completed");
            eventLog.WriteEntry(log.ToString(), EventLogEntryType.Information, eventId++);
        }

        public void OnHour(object sender, ElapsedEventArgs args)
        {
            eventLog.WriteEntry("Running Hour Processes", EventLogEntryType.Information, eventId++);
            StringBuilder log = new StringBuilder();
            log.Append("Processing...");
            log.Append("Completed");
            eventLog.WriteEntry(log.ToString(), EventLogEntryType.Information, eventId++);
        }

        public void OnMinute(object sender, ElapsedEventArgs args)
        {
            eventLog.WriteEntry("Running Minute Processes", EventLogEntryType.Information, eventId++);
            StringBuilder log = new StringBuilder();
            log.Append("Processing...");
            log.Append("Completed");
            eventLog.WriteEntry(log.ToString(), EventLogEntryType.Information, eventId++);
        }

        public void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            eventLog.WriteEntry(((Exception)e.ExceptionObject).Message, EventLogEntryType.Error, eventId++);
        }
    }
}
