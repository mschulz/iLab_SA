/* $Id$ */
/* $Date$ */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace iLabs.UtilLib
{
    public class Utilities
    {

    
        /// <summary>
        /// Utility to convert an ArrayList of strings to an array.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string[] ArrayListToStringArray(ArrayList list)
        {
            string[] stringArray = new string[list.Count];
            int i = 0;

            foreach (object entry in list)
            {
                stringArray[i] = entry.ToString();
                i++;
            }
            return stringArray;
        }

        /// <summary>
        /// Utility to convert an ArrayList of ints to an array.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int[] ArrayListToIntArray(ArrayList list)
        {
            int[] intArray = new int[list.Count];
            int i = 0;

            foreach (object entry in list)
            {
                intArray[i] = Convert.ToInt32(entry);
                i++;
            }
            return intArray;
        }

        /// <summary>
        /// Utility to convert an ArrayList of longs to an array.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static long[] ArrayListToLongArray(ArrayList list)
        {
            long[] array = new long[list.Count];
            int i = 0;

            foreach (object entry in list)
            {
                array[i] = Convert.ToInt64(entry);
                i++;
            }
            return array;
        }
       
        /// <summary>
        /// A utility to pass an array of ints as a CSV string
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToCSV(int[] values)
        {
            StringBuilder buf = new StringBuilder();
            int j = values.Length - 1;
            for (int i = 0; i < values.Length; i++)
            {
                buf.Append(values[i].ToString());
                if (i < j)
                    buf.Append(",");
            }
            return buf.ToString();
        }

        /// <summary>
        ///  A utility to pass an array of longs as a CSV string
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToCSV(long[] values)
        {
            StringBuilder buf = new StringBuilder();
            int j = values.Length - 1;
            for (int i = 0; i < values.Length; i++)
            {
                buf.Append(values[i].ToString());
                if (i < j)
                    buf.Append(",");
            }
            return buf.ToString();
        }

        public static string ToCSV(Object [] values){
            StringBuilder buf = new StringBuilder();
            int j = values.Length -1;
            for(int i = 0; i< values.Length;i++){
                buf.Append(values[i].ToString());
                if(i <j)
                    buf.Append(",");
            }
            return buf.ToString();
        }

        /// <summary>
        /// Standard formatting for error messages.
        /// </summary>
        /// <param name="msgText">The error message text.</param>
        /// <returns>Error message text surrounded by appropriate div and paragraph tags.</returns>
        public static string FormatErrorMessage(string msgText)
        {
            StringBuilder message = new StringBuilder("<div class=\"errormessage\"><p>"); ;
            message.Append(msgText);
            message.AppendLine("</p></div>");
            return message.ToString();
        }
        /// <summary>
        /// Standard formatting for warning messages.
        /// </summary>
        /// <param name="msgText">The error message text.</param>
        /// <returns>Error message text surrounded by appropriate div and paragraph tags.</returns>
        public static string FormatWarningMessage(string msgText)
        {
            StringBuilder message = new StringBuilder("<div class=\"warningmessage\"><p>"); ;
            message.Append(msgText);
            message.AppendLine("</p></div>");
            return message.ToString();
        }
        /// <summary>
        /// Standard formatting for general confirmation messages.
        /// </summary>
        /// <param name="msgText">The message text.</param>
        /// <returns>Message text surrounded by appropriate div and paragraph tags.</returns>
        public static string FormatConfirmationMessage(string msgText)
        {
            StringBuilder message = new StringBuilder("<div class=\"infomessage\"><p>"); ;
            message.Append(msgText);
            message.AppendLine("</p></div>");
            return message.ToString();
        }

        /// <summary>
        /// Take two strings and returns one of them. 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="ignoreCase"></param>
        /// <returns>If either is Empty the other is returned, if both match the string is returned, if they do not match an exception is thrown.</returns>
        public static string ResolveArguments(string arg1, string arg2, bool ignoreCase)
        {
            string value = null;
            
            if ((arg1 == null || arg1.Length < 1) && (arg2 != null && arg2.Length > 0))
            {
                value = arg2;
            }
            else if ((arg1 != null && arg1.Length > 0) && (arg2 == null || arg2.Length < 1))
            {
                value = arg1;
            }
            else if ((arg1 != null && arg1.Length > 0) && (arg2 != null && arg2.Length > 0))
            {
                int status = -1;
                if (ignoreCase)
                {
                    status = arg1.ToLower().CompareTo(arg2.ToLower());
                }
                else
                {
                    status = arg1.CompareTo(arg2);
                }
                if (status  == 0)
                {
                    value = arg1;
                }
                else
                {
                    throw new ApplicationException("Parameter mismatch in ResolveArguments.");
                }
            }
            return value;
        }

        public static string DumpException(Exception e)
        {
            Exception ex = e;
            int count = 0;
            StringBuilder buf = new StringBuilder();
            while (ex != null)
            {
                buf.Append(count.ToString());
                buf.Append("\t--------- Exception Data ---------\n");
                ex = ParseExceptionInfo(buf, ex);
                count++;
            }
            return buf.ToString();
        }

        public static Exception ParseExceptionInfo(StringBuilder buf, Exception ex)
        {
            buf.Append("Message: {0}");
            buf.Append(ex.Message);
            buf.Append("\n");
            buf.Append("Exception Type: {0}");
            buf.Append(ex.GetType().FullName);
            buf.Append("\n");
            buf.Append("Source: {0}");
            buf.Append(ex.Source);
            buf.Append("\n");
            buf.Append("StackTrace: {0}");
            buf.Append(ex.StackTrace);
            buf.Append("\n");
            buf.Append("TargetSite: {0}");
            buf.Append(ex.TargetSite);
            buf.Append("\n");
            return ex.InnerException;
        }



        /// <summary>
        /// Formats a Uri  ( URL class ) into a fully specified URL path, with out any query parameters.
        /// To be used to export URL's to other services.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ExportUrlPath(Uri url)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(url.Scheme + "://");
            buf.Append(url.Host);
            if (!url.IsDefaultPort)
            {
                buf.Append(":" + url.Port);
            }
            buf.Append(url.AbsolutePath);
            return buf.ToString();
        }

        /// <summary>
        /// Simple Guid generator, wraps Microsoft GUID and formats the string as all uppercase with dashes.
        /// </summary>
        /// <returns></returns>
        public static string MakeGuid()
        {
            return MakeGuid("D");
        }
        /// <summary>
        /// Simple Guid generator, wraps Microsoft GUID and formats the string as all uppercase.
        /// </summary>
        /// <param name="format">"N", "D", "B", or "P", null or empty defaults to D</param>
        /// <returns></returns>
        public static string MakeGuid(string format)
        {
            Guid guid = System.Guid.NewGuid();
            return guid.ToString(format).ToUpper();
        }

        public static bool WildCardMatch(string searchString, string compareString)
        {
            Regex regex = new Regex(searchString, RegexOptions.IgnoreCase);

            return regex.IsMatch(compareString);
        }

        /// <summary>
        /// This method insures that critical values are present in web.config. 
        /// If they are missing, it creates SystemMessages to alert an administrator.
        /// </summary>
        //public static void ValidateWebConfig()
        //{
        //    // Create a message object
        //    SystemMessage msg = new SystemMessage();
        //    msg.messageType = SystemMessage.SYSTEM;
        //    msg.groupID = 0;
        //    msg.labServerID = 0;
        //    msg.lastModified = DateTime.Now;
        //    msg.messageTitle = "Service Broker Configuration Validation";
        //    msg.toBeDisplayed = true;

        //    // Check for Needed Email addresses in web.config

        //    // Create a Regular Expressions object with which to validate the email addresses

        //    // This pattern matches: name.surname@blah.com|||Name Surname <name.surname@blah.com>|||"b. blah"@blah.co.nz
        //    // Non-Matches: name surname@blah.com|||name."surname"@blah.com|||name@bla-.com 
        //    // ^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|"((?=[\x01-\x7f])[^"\\]|\\[\x01-\x7f])*"\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|"((?=[\x01-\x7f])[^"\\]|\\[\x01-\x7f])*")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$

        //    // This is a simpler, address-only pattern:
        //    // ^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$

        //    StringBuilder mailPattern = new StringBuilder();
        //    string quote = "\"";
        //    mailPattern.Append(@"^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|");
        //    mailPattern.Append(quote + @"((?=[\x01-\x7f])[^");
        //    mailPattern.Append(quote + @"\\]|\\[\x01-\x7f])*");
        //    mailPattern.Append(quote + @"\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|");
        //    mailPattern.Append(quote + @"((?=[\x01-\x7f])[^");
        //    mailPattern.Append(quote + @"\\]|\\[\x01-\x7f])*");
        //    mailPattern.Append(quote + @")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$");

        //    Regex r = new Regex(mailPattern.ToString(), RegexOptions.Compiled);
        //    char[] comma = { ',' };
        //    StringBuilder errorMessage;

        //    // bugReportMailAddress
        //    // Split the comma-delimited list of email addresses into a string array, to be checked individually in a loop.
        //    string[] bugReportMailAddresses = ConfigurationManager.AppSettings["bugReportMailAddress"].Split(comma);
        //    for (int i = 0; i < bugReportMailAddresses.Length; i++)
        //    {
        //        if (!r.IsMatch(bugReportMailAddresses[i].Trim()))
        //        {
        //            errorMessage = new StringBuilder();
        //            errorMessage.Append("An Administrator needs to <b>edit web.config and add a value for bugReportMailAddress</b>. This is the address where bug reports are to be sent. ");
        //            errorMessage.Append("<br>This message will also appear if one or more of the email addresses for this entry is invalid.");
        //            errorMessage.Append("<br>Rejected or missing value: <b>" + bugReportMailAddresses[i] + "</b>");
        //            errorMessage.Append("<br>This message may be removed on the Messages page of the Service Broker Administrative interface.");
        //            msg.messageBody = errorMessage.ToString();
        //            msg.messageID = AdministrativeAPI.AdministrativeAPI.AddSystemMessage(msg.messageType, msg.toBeDisplayed, msg.groupID, msg.labServerID, msg.MessageBody, msg.messageTitle);
        //        }
        //    }

        //    // supportMailAddress
        //    // Split the comma-delimited list of email addresses into a string array, to be checked individually in a loop.
        //    string[] supportMailAddresses = ConfigurationManager.AppSettings["supportMailAddress"].Split(comma);
        //    for (int i = 0; i < supportMailAddresses.Length; i++)
        //    {
        //        if (!r.IsMatch(supportMailAddresses[i].Trim()))
        //        {
        //            errorMessage = new StringBuilder();
        //            errorMessage.Append("An Administrator needs to <b>edit web.config and add a value for supportMailAddress</b>. This is the address where help requests are sent. ");
        //            errorMessage.Append("<br>This message will also appear if one or more of the email addresses for this entry is invalid.");
        //            errorMessage.Append("<br>Rejected or missing value: <b>" + supportMailAddresses[i] + "</b>");
        //            errorMessage.Append("<br>This message may be removed on the Messages page of the Service Broker Administrative interface.");
        //            msg.messageBody = errorMessage.ToString();
        //            msg.messageID = AdministrativeAPI.AdministrativeAPI.AddSystemMessage(msg.messageType, msg.toBeDisplayed, msg.groupID, msg.labServerID, msg.MessageBody, msg.messageTitle);
        //        }
        //    }

        //    // registrationMailAddress
        //    // Split the comma-delimited list of email addresses into a string array, to be checked individually in a loop.
        //    string[] registrationMailAddresses = ConfigurationManager.AppSettings["registrationMailAddress"].Split(comma);
        //    for (int i = 0; i < registrationMailAddresses.Length; i++)
        //    {
        //        if (!r.IsMatch(registrationMailAddresses[i].Trim()))
        //        {
        //            errorMessage = new StringBuilder();
        //            errorMessage.Append("An Administrator needs to <b>edit web.config and add a value for registrationMailAddress</b>. This is the default email address where requests for membership in a group are sent. ");
        //            errorMessage.Append("<br>This message will also appear if one or more of the email addresses for this entry is invalid.");
        //            errorMessage.Append("<br>Rejected or missing value: <b>" + registrationMailAddresses[i] + "</b>");
        //            errorMessage.Append("<br>This message may be removed on the Messages page of the Service Broker Administrative interface.");
        //            msg.messageBody = errorMessage.ToString();
        //            msg.messageID = AdministrativeAPI.AdministrativeAPI.AddSystemMessage(msg.messageType, msg.toBeDisplayed, msg.groupID, msg.labServerID, msg.MessageBody, msg.messageTitle);
        //        }
        //    }

        //    //Check to see if the Service Broker GUID is not valid (for example, it is blank)
        //    // There are 3 possible conditions:
        //    // 1. Guid is blank or not valid. This will be trapped in the catch block.
        //    // 2. Guid is valid, but a match is not found in Static_ProcessAgents (the agent==null condition).
        //    //    A new Static_ProcessAgent record will be created.
        //    // 3. Guid in web.config matches Guid in Static_ProcessAgent. (fall-through condition). No action will be taken.
        //    string sbGuid;

        //    try
        //    {
        //        // Create a Regular Expressions object with which to validate the GUID
        //        Regex r2 = new Regex(@".{1,50}", RegexOptions.Compiled);
        //        // this line will crash if there is not a valid guid in the sbGID key in web.config
        //        sbGuid = ConfigurationManager.AppSettings["sbGID"];
        //        if ((sbGuid == null) || (!r2.IsMatch(sbGuid)))
        //        {
        //            throw new Exception("sbGUID not set or too long.");
        //        }
        //        else
        //        {
        //            // check to see if the Service Broker GUID (ID) exists in the Static_ProcessAgents table
        //            // it must be present for ticketing to work
        //            try
        //            {
        //                // initialize the dbManager
        //                DBManager dbManager = new DBManager();

        //                // retrieve the record that represents this service broker
        //                StaticProcessAgentDescriptor[] agents =
        //                    dbManager.RetrieveStaticProcessAgentDescriptorsByType(ProcessAgentTypes.SERVICE_BROKER);
        //                if (agents.Length == 0)
        //                    throw new InvalidOperationException("Cannot find Record that represents Service Broker. Service Broker might not have been initialized correctly.");
        //                else
        //                    // set static Service Broker object
        //                    TicketIssuer.serviceBroker = agents[0];

        //                StaticProcessAgentDescriptor agent = agents[0];
        //                // If there is no Service Broker record in Static_ProcessAgents that has the GUID from
        //                // the sbGID key in web.config, update the Static_ProcessAgents table to reflect the new GUID.
        //                // We know there is no match if the agent object is null
        //                if (agent == null)
        //                {
        //                    // save the old Guid for use in the message, below
        //                    string oldSbGuid = dbManager.RetrieveServiceBrokerID();

        //                    // Update the Guid in the Service Broker record in the Static_ProcessAgents table
        //                    TicketIssuer.UpdateServiceBroker(sbGuid, "", "", "", "", "", "");

        //                    // Send a SystemMessage that says the Service Broker GUID has been changed.
        //                    msg.messageBody = "<b>The Service Broker GUID has been changed in web.config.</b><br> .";
        //                    msg.messageBody += "Old GUID: " + oldSbGuid + "<br>";
        //                    msg.messageBody += "New GUID: " + sbGuid;
        //                    msg.messageID = AdministrativeAPI.AdministrativeAPI.AddSystemMessage(msg.messageType, msg.toBeDisplayed, msg.groupID, msg.labServerID, msg.MessageBody, msg.messageTitle);
        //                }
        //            }
        //            catch (InvalidOperationException)
        //            {
        //                // If this error is thrown, no service broker records exist at all in Static_ProcessAgents
        //                TicketIssuer.CreateServiceBroker(sbGuid, "Web Service URL", "Redirect URL", "Service Broker Description", "Info URL", "Contact Email", "Time Zone");
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        // Create a System Message about the lack of a Service Broker GUID
        //        msg.messageBody = "An Administrator needs to <b>edit web.config and add a valid Service Broker identifying GUID to the sbGID key in appSettings</b>. ";
        //        msg.messageBody += "Failure to do this may cause web service calls to lab servers that require authentication to fail. ";
        //        msg.messageBody += "The GUID must be a string used as a Globally unique identifier, maximum length 50 characters. Several tools exist to generate GUID's.";
        //        msg.messageBody += "In Visual Studio, select Tools->Create Guid. Choose Registry Format, and copy the Guid to the clipboard. ";
        //        msg.messageBody += "Paste the Guid into the sbGID value in web.config, and remove the braces{}. ";
        //        msg.messageBody += "<br>This message may be removed on the Messages page of the Service Broker Administrative interface.";
        //        msg.messageID = AdministrativeAPI.AdministrativeAPI.AddSystemMessage(msg.messageType, msg.toBeDisplayed, msg.groupID, msg.labServerID, msg.MessageBody, msg.messageTitle);
        //    }

        //}

    }


    public class PolicyParser
    {

        public static string GenerateRule(string[] fields, string[] properties)
        {
            string rule = null;
            for (int i = 0; i < fields.Length; i++)
            {
                rule = rule + fields[i] + "," + properties[i] + ",";
            }
            return rule;
        }

        public static string getProperty(string rule, string field)
        {
            string[] parts = rule.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Equals(field))
                {
                    return parts[i + 1];
                }
            }

            return null;
        }
    }

  
}
