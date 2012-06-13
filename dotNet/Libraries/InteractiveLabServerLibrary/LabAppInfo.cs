/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;

namespace iLabs.LabServer.Interactive
{
	/// <summary>
	/// Summary description for LabAppInfo.
	/// </summary>
	public class LabAppInfo
	{
		public int appID;
		public int width;
		public int height;
        public int type;
		public long defaultDuration;
		public string path;
		public string application;
        public string appGuid;
		public string appKey;
        public string library;
        public string appURL;
        public string version;
        public string rev;
		public string page;
		public string title;
		public string description;
		public string extraInfo;
		public long allocatedTime;
		public string contact;
		public string comment;
		public string dataSources;
        public string server;
        public int port;
        public bool reentrant = false;

		public LabAppInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
