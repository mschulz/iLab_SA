
/*
 * Copyright (c) 2004-2006 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Services;

using iLabs.DataTypes.StorageTypes;
using iLabs.Proxies.ESS;
using iLabs.Proxies.ISB;

namespace iLabs.LabServer.Interactive
{

    /// <summary>
    /// Summary description for DataSourceManager
    /// </summary>
    public class DataSourceManager : IDisposable

    {
        public long taskID = -1L;
        public long experimentID = -1L;
        public ExperimentStorageProxy essProxy;
        private string appKey;
        private ArrayList dataSources;


        public DataSourceManager()
        {
            dataSources = new ArrayList();
        }

        public long ExperimentID
        {
            get
            {
                return experimentID;
            }
            set
            {
                experimentID = value;
            }
        }
        public void AddDataSource(LabDataSource ds)
        {
            ds.DataManager = this;
            dataSources.Add(ds);
        }

        public void CloseDataSources()
        {
            foreach(LabDataSource ds in dataSources)
            {
                ds.Disconnect();
                ds.Dispose();

            }
        }

        public long TaskID
        {
            get
            {
                return taskID;
            }
            set
            {
                taskID = value;
            }
        }

        public  string AppKey
        {
            get
            {
                return appKey;
            }
            set
            {
                appKey = value;
            }
        }

        public void Dispose()
        {
            CloseDataSources();
        }

    }
}

