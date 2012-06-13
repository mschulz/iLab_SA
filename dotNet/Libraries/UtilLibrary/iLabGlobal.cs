/* Copyright (c) 2008 The Massachusetts Institute of Technology. All rights reserved. */
/* $Id$ */

using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.UtilLib
{
    /// <summary>
    /// static class to provide release tags.
    /// </summary>
    public class iLabGlobal
    {
        static private string headUrl = "$HeadURL: https://ilabproject.svn.sourceforge.net/svnroot/ilabproject/trunk/iLab_SA/dotNet/Libraries/UtilLibrary/iLabGlobal.cs $";
        static private string info = "$Id$";
        static private string date = "$Date$";
        static private string revision = "$Revision$";
        static private string iLabRelease = "$ilab:Release$";
	static private string release = "Release 4.0.2a";
        static private string buildDate = "$ilab:BuildDate$";
        /// <summary>
        /// Returns the date and svn revision last set, still not auto setting...
        /// </summary>
        public static string Revision
        {
            get
            {
                return revision + " " + date;
            }
        }
        /// <summary>
        /// returns a release string specified in iLabGlobal
        /// </summary>
        public static string Release
        {
            get
            {
                return release + " ( " + revision.Replace("$", "") + " ) " + date.Replace("$", "");
            }
        }

        /// <summary>
        /// returns the build date of the release.
        /// </summary>
        public static string BuildDate
        {
            get
            {
                return buildDate;
            }
        }
    }
}
