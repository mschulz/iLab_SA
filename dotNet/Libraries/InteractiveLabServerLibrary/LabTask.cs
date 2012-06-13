using System;
using System.Text;

using iLabs.UtilLib;

namespace iLabs.LabServer.Interactive
{
	/// <summary>
	/// Summary description for LabTask.
	/// </summary>
	public class LabTask
	{
		public long taskID; // Assigned by database, Primary Key
        public long couponID;
        public long experimentID = -1;
		public int labAppID; // Reference to LabApplication
        protected eStatus status;
		public string groupName; // SB group
        public string issuerGUID;
		public DateTime startTime; // in UTC
		public DateTime endTime; //  == -1 never automaticly ends
		public string data; // any XML encoded data
        public string storage; // information about ESS and data sources ( XML )

        public enum eStatus { NotFound = -1, Unknown = 0, Scheduled=2, Pending =4, Waiting=8, Running = 16, Completed=32, Aborted=64, Expired=128, Closed=256 };

        public static string constructSessionPayload(LabAppInfo appInfo, DateTime start, long duration,
            long taskID, string returnTarget, string user, string statusVI)
        {
            return constructSessionPayload(appInfo.appID,appInfo.title,appInfo.application,appInfo.rev,appInfo.appURL,appInfo.width,appInfo.height,
                start, duration, taskID, returnTarget, user, statusVI);
        }

        public static string constructSessionPayload(int id, string title, string application, string revision, string appUrl,
            int width, int height, DateTime start, long duration, long taskID, string returnTarget, string user, string statusVI)
        {
            StringBuilder buf = new StringBuilder("<payload>");
            buf.Append("<appId>" + id + "</appId>");
            buf.Append("<title>" + id + "</title>");
            buf.Append("<application>" + application + "</application>");
            buf.Append("<revision>" + revision + "</revision>");
            buf.Append("<taskId>" + taskID + "</taskId>");
            buf.Append("<width>" + width + "</width>");
            buf.Append("<height>" + height + "</height>");
            buf.Append("<startTime>" + DateUtil.ToUtcString(start) + "</startTime>");
            buf.Append("<duration>" + duration + "</duration>");
            if (appUrl != null)
                buf.Append("<appUrl>" + appUrl + "</appUrl>");
            if (user != null)
                buf.Append("<user>" + user + "</user>");
            if (statusVI != null)
                buf.Append("<status>" + statusVI + "</status>");
            buf.Append("</payload>");
            return buf.ToString();
        }

        public static string constructTaskXml(long id, string viname, string revision, string statusvi, string essURL)
        {
            StringBuilder buf = new StringBuilder("<task>");
            buf.Append("<appId>" + id + "</appId>");
            buf.Append("<application>" + viname + "</application>");
            buf.Append("<revision>" + revision + "</revision>");
            if (statusvi != null)
                buf.Append("<status>" + statusvi + "</status>");
            if ((essURL != null) && (essURL.Length > 0))
                buf.Append("<essUrl>" + essURL + "</essUrl>");
            buf.Append("</task>");
            return buf.ToString();
        }

		public LabTask()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public virtual eStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public virtual eStatus Close()
        {
            return status;
        }

        public virtual eStatus Close(eStatus status)
        {
            return status;
        }
       
        public virtual eStatus Expire(){
            return status;
        }

        public virtual eStatus HeartBeat()
        {
            return status;
        }


	}
}
