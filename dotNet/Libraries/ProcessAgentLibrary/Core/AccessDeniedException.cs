/*
 * Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.Core
{
    public class AccessDeniedException : System.ApplicationException
    {
        /// <summary>
        /// An AccessDenied exception
        /// </summary>
        /// <param name="message"></param>
        public AccessDeniedException(string message)
            : base(message)
        {
        }

        public AccessDeniedException(string message, ApplicationException baseException)
            : base(message, baseException)
        {
        }
    }
}
