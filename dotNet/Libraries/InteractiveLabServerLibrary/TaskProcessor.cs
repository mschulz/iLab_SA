using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

using iLabs.Ticketing;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.ESS;
using iLabs.Proxies.ISB;
using iLabs.UtilLib;


namespace iLabs.LabServer.Interactive
{
	/// <summary>
	/// Tread that processes short in memory handles for current tasks, needs to be loaded 
    /// from database and to update database records when status changes.
	/// </summary>
	public class TaskProcessor
	{
        private static TaskProcessor theInstance = null;

        public static TaskProcessor Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new TaskProcessor();
                }
                return theInstance;
            }
        }

        private int waitTime = 10000;
        private int count = 0;
        private bool go = true;
        private List<LabTask> tasks;
        private Dictionary<long, DataSourceManager> dataManagers = null;
        
		public TaskProcessor()
		{
           Logger.WriteLine("TaskProcessor created");
            tasks = new List<LabTask>();
            dataManagers = new Dictionary<long, DataSourceManager>();
           
			//
			// TODO: Add constructor logic here
			
		}

        public TaskProcessor(int delay)
        : this() {
            waitTime = delay;
        }

        public int WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }

        public void Run()
        {
            while (go)
            {
                ProcessTasks();
                Thread.Sleep(waitTime);
            }
        }
        public void Start()
        {
            lock (this)
            {
                go = true;
            } 
            Run();
        }

        public void Stop()
        {
            lock (this)
            {
                go = false;
            }
        }

        public void Add(LabTask task)
        {
            lock (tasks)
            {
                tasks.Add(task);
            }
        }
        public void Remove(LabTask task)
        {
            lock (tasks)
            {
                tasks.Remove(task);
            }
        }
        public LabTask GetTask(long experimentID)
        {
            LabTask task = null;
            lock (tasks)
            {
                foreach (LabTask t in tasks)
                {
                    if (t.experimentID == experimentID)
                    {
                        task = t;
                        break;
                    }
                }
            }
            return task;
        }


        public List<LabTask> GetTasks(int appID)
        {
            List<LabTask> active = new List<LabTask>();
            lock (tasks)
            {
                foreach (LabTask t in tasks)
                {
                    if (t.labAppID == appID)
                    {
                        active.Add(t);
                    }
                }
            }
            return active;
        }

        public void AddDataManager(long taskID, DataSourceManager mgr)
        {
            lock (dataManagers)
            {
                dataManagers.Add(taskID, mgr);
            }
        }

        public DataSourceManager GetDataManager(long taskID)
        {
            DataSourceManager dataManager = null;
            try
            {
                dataManager = dataManagers[taskID];
            }
            catch (KeyNotFoundException e) { }
            return dataManager;
        }

        public bool RemoveDataManager(long taskID)
        {
            lock (dataManagers)
            {
                return dataManagers.Remove(taskID);
            }
        }


        public void ProcessTasks()
        {
            List<LabTask> toBeRemoved = null;
            count++;
            if (count >= 10000)
            {
               Logger.WriteLine("ProcessTasks");
                count = 0;
            }

            long ticksNow = DateTime.UtcNow.Ticks;
            lock (tasks)
            {
                foreach (LabTask task in tasks)
                {

                    if (ticksNow > task.endTime.Ticks )
                    { // task has expired
                        try
                        {
                           Logger.WriteLine("Found expired task: " + task.taskID);
                            
                            if (toBeRemoved == null)
                                toBeRemoved = new List<LabTask>();
                            toBeRemoved.Add(task);

                        }

                        catch (Exception e1)
                        {
                           Logger.WriteLine("ProcessTasks Expired: exception:" + e1.Message + e1.StackTrace);
                        }

                    }
                    else
                    {
                        try
                        {
                            if (task.Status == LabTask.eStatus.Running)
                            {
                                task.HeartBeat();
                            }
                        }
                        catch (Exception e2)
                        {
                           Logger.WriteLine("ProcessTasks Status: " + e2.Message);
                        }
                    }
                }
                if (toBeRemoved != null)
                {
                    foreach (LabTask t in toBeRemoved)
                    {
                        t.Expire();
                        tasks.Remove(t);
                    }
                }
            }// End tasks lock
        }
	}

    public class TaskHandler
    {
        TaskProcessor taskProc;
        bool go = true;

        public TaskHandler(TaskProcessor task)
        {
            taskProc = task;
            Thread tt = new Thread(new ThreadStart(Run));
            tt.Name = "TaskProcessingTread";
            tt.Start();
        }
        private void Run()
        {
            while (go)
            {
                taskProc.ProcessTasks();
                Thread.Sleep(10000);
            }
        }
    }
}
