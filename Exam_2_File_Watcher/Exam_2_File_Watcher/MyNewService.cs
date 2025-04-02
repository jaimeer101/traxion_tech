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
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam_2_File_Watcher
{
    public partial class MyNewService : ServiceBase
    {
        EventLog _eventLog1 = new EventLog();
        private int eventId = 1;
        private string path1 = @"c:\Folder1";
        private string path2 = @"c:\Folder2";
        public MyNewService(string[] args)
        {
            InitializeComponent();

            string eventSourceName = "MySource";
            string logName = "MyNewLog";

            if (args.Length > 0)
            {
                eventSourceName = args[0];
            }

            if (args.Length > 1)
            {
                logName = args[1];
            }

            _eventLog1 = new EventLog();

            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }

            _eventLog1.Source = eventSourceName;
            _eventLog1.Log = logName;
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            _eventLog1.WriteEntry("Checking Folders.");
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path1))
                {
                    _eventLog1.WriteEntry("That Folder1 exists.");
                }
                else
                {
                    DirectoryInfo di1 = Directory.CreateDirectory(path1);
                    _eventLog1.WriteEntry(string.Format("The directory was created successfully at {0}.", Directory.GetCreationTime(path1)));
                }
                // Try to create the directory 1.
                

                if (Directory.Exists(path2))
                {
                    _eventLog1.WriteEntry("That Folder2 exists.");
                }
                else
                {
                    DirectoryInfo di2 = Directory.CreateDirectory(path2);
                    _eventLog1.WriteEntry(string.Format("The directory was created successfully at {0}.", Directory.GetCreationTime(path2)));
                }
          
            }
            catch (Exception e)
            {
                _eventLog1.WriteEntry(string.Format("The process failed: {0}", e.ToString()));
            }
            finally { }
            Timer timer = new Timer
            {
                Interval = 10000 // 60 seconds
            };
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();

            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.

            _eventLog1.WriteEntry("Monitoring the Folder", EventLogEntryType.Information, eventId++);
            //watch(path1);
            if (Directory.GetFiles(path1, "*.*").Length > 0)
            {
                //NO matching *.wma files
                _eventLog1.WriteEntry("Files detected in Folder 1");
                string[] fileList = System.IO.Directory.GetFiles(path1, "*.*");
                foreach (string file in fileList)
                {
                    string fileName = Path.GetFileName(file); ;
                    //moving file
                    //File.Move(fileToMove, moveTo);
                    _eventLog1.WriteEntry("Moving: " + file);
                    System.IO.File.Copy(file, Path.Combine(path2, fileName), true);
                    System.IO.File.Delete(file);
                    _eventLog1.WriteEntry(file + ": Successfull moved");
                }
            }

        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            _eventLog1.WriteEntry(value);
            /* using (var folderWatcher = new FileSystemWatcher(path1))
             {
                 /*folderWatcher.Filter = "*.*";

                 var change = folderWatcher.WaitForChanged(WatcherChangeTypes.Created, 1000 * 60);*/

            /*if (!change.TimedOut)
            {


            }*/
            /*_eventLog1.WriteEntry("File detected: " + change.Name);
            _eventLog1.WriteEntry("Moving to: " + path2);
            File.Move(Path.Combine(path1, change.Name), Path.Combine(path2, change.Name));

        }*/
        }
        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            _eventLog1.WriteEntry("In OnStop.");

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnContinue()
        {
            _eventLog1.WriteEntry("In OnContinue.");
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        /*private void watch(string path)
        {
            
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            watcher.Filter = "*.*";
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.EnableRaisingEvents = true;
        }*/

        
    }
}
