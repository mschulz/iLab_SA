using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;

using iLabs.Proxies.ESS;
using iLabs.UtilLib;

namespace iLabs.LabServer.Interactive
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class FileDataSource : LabDataSource
    {
        private bool go = true;
        int count = 0;
        private Coupon opCoupon;
        private long experimentID;
        private string socketUrl;
        FileSystemWatcher theWatcher;

        public FileDataSource()
        {
            // Create a new FileSystemWatcher and set its properties.
            theWatcher = new FileSystemWatcher();
            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            theWatcher.NotifyFilter = NotifyFilters.LastWrite |  NotifyFilters.Size | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        }

        //void OnCreated(object source, FileSystemEventArgs e)
        public void AddFileSystemEventHandler(FileSystemEventHandler onChanged)
        {
            theWatcher.Changed += new FileSystemEventHandler(onChanged);
        }

        //void OnCreated(object source, FileSystemEventArgs e)
        public void AddFileSystemEventHandler(FileSystemEventHandler onChange, WatcherChangeTypes changeMask)
        {
            if((changeMask & WatcherChangeTypes.Changed) == WatcherChangeTypes.Changed)
               theWatcher.Changed += new FileSystemEventHandler(onChange);
            if ((changeMask & WatcherChangeTypes.Created) == WatcherChangeTypes.Created)
                theWatcher.Created += new FileSystemEventHandler(onChange);
            if ((changeMask & WatcherChangeTypes.Deleted) == WatcherChangeTypes.Deleted)
                theWatcher.Deleted += new FileSystemEventHandler(onChange);
            if ((changeMask & WatcherChangeTypes.Renamed) == WatcherChangeTypes.Renamed)
               theWatcher.Renamed += new RenamedEventHandler(onChange);
        }

        public NotifyFilters NotifyFilter{
            get
            {
                return theWatcher.NotifyFilter;
            }
            set
            {
                theWatcher.NotifyFilter = value;
            }
    }

        public string Path
        {
            get
            {
                return theWatcher.Path;
            }
            set
            {
                theWatcher.Path = value;
            }
        }
        public string Filter
        {
            get
            {
                return theWatcher.Filter;
            }
            set
            {
                theWatcher.Filter = value;
            }
        }

        public bool RaiseEvents
        {
            get
            {
                return theWatcher.EnableRaisingEvents;
            }
            set
            {
                theWatcher.EnableRaisingEvents = value;
            }
        }

       
        public void Start()
        {
            lock (this)
            {
                go = true;
                
            }
            theWatcher.EnableRaisingEvents = true;
            
            //Run();
        }

        public void Stop()
        {
            lock (this)
            {
                go = false;
            }
            theWatcher.EnableRaisingEvents = false;
        }

        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        //public void Run()
        //{
        //    while (go)
        //    {
        //        int i = 0;
        //        Thread.Sleep(waitTime);
        //        i++;
        //    }
        //    theWatcher.EnableRaisingEvents = false;
        //    try
        //    {
        //        theThread.Abort();
        //    }
        //    catch (ThreadAbortException tEx)
        //    {
        //        //terminate the thread
        //        ;
        //    }
        //    theThread = null;
        //}

        public override void Connect(string url, int mode)
        {
            Path = url;
            Start();
        }
        public override void Disconnect()
        {
            Stop();
        }
        public override void Dispose()
        {
            Stop();
            theWatcher.Dispose();
        }
        public override void Update()
        {
            string test = "YES";
        }

        public override bool Write(object data, int timeout)
        {
            return false;
        }

        // Define default event handlers.
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.

            DateTime lastWrite = File.GetLastWriteTimeUtc(e.FullPath);
            //FileInfo fInfo = new FileInfo(e.FullPath);
            //long  len = fInfo.Length;
            Console.WriteLine("File Created: " + e.FullPath);


        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                    break;
                case WatcherChangeTypes.Changed:

                    DateTime lastWrite = File.GetLastWriteTimeUtc(e.FullPath);

                    FileInfo fInfo = new FileInfo(e.FullPath);
                    FileStream inFile = fInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);

                    long len = fInfo.Length;
                    Console.WriteLine("File Changed: " + e.FullPath + " size: " + len + " \tDate: " + lastWrite);
                    break;
                case WatcherChangeTypes.Deleted:
                    Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                    break;
            }
        }

        // Define the event handlers.
        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File Deleted: " + e.FullPath + " " + e.ChangeType);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
    }
}
