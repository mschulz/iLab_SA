using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.LabServer.Interactive
{
    public abstract class LabDataSource : IDisposable

    {
        public const int AUTOUPDATE = 0x1;
        public const int BUFFERED = 0x2;

        public const int READ = 0x8;
        public const int READ_AUTOUPDATE = READ | AUTOUPDATE;
        public const int READ_BUFFERED = READ | BUFFERED;
        public const int READ_BUFFERED_AUTOUPDATE = READ | BUFFERED | AUTOUPDATE;

        public const int WRITE = 0x10;
        public const int WRITE_AUTOUPDATE = WRITE | AUTOUPDATE;

        public const int READ_WRITE_AUTOUPDATE = READ | WRITE | AUTOUPDATE;
        public const int READ_WRITE_BUFFERED_AUTOUPDATE = READ |WRITE | BUFFERED | AUTOUPDATE;
       
        protected DataSourceManager manager;
        protected string recordType = "xml";
        protected int accessMode = LabDataSource.READ;

        public string Type
        {
            get
            {
                return recordType;
            }
            set
            {
                recordType = value;
            }
        }
        public DataSourceManager DataManager
        {
            get
            {
                return manager;
            }
            set
            {
                manager = value;
            }
        }
        public abstract void Connect(string url, int mode);
        public abstract void Disconnect();
        public abstract void Dispose();
        public abstract void Update();
        public abstract bool Write(object data, int timeout);
    }
}
