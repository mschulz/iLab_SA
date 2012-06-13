using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

using iLabs.DataTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.UtilLib;

namespace iLabs.Scheduling.LabSide
{
    public partial class NewTimeBlockPopUp : System.Web.UI.Page
    {
        // string labServerID;
        int[] credentialSetIDs;
        LssCredentialSet[] credentialSets;
        CultureInfo culture = null;
        int userTZ = 0;
        int localTzOffset = 0;
        StringBuilder buf = null;
        //TimeSpan tzOffset = TimeSpan.MinValue;
        LabSchedulingDB dbManager = new LabSchedulingDB();

        public double LocalTZ
        {
            get
            {
                return localTzOffset/60.0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            localTzOffset = DateUtil.LocalTzOffset;
            userTZ = (int)Session["userTZ"];
            

            txtStartDate.Attributes.Add("OnKeyPress", "return false;");
            txtEndDate.Attributes.Add("OnKeyPress", "return false;");

            if (ddlRecurrence.SelectedIndex != 3)
            {
                lblRecur.Visible = false;
                cbxRecurWeekly.Visible = false;
            }
            else
            {
                lblRecur.Visible = true;
                cbxRecurWeekly.Visible = true;
            }
            if (!IsPostBack)
            {
                ddlStartHour.SelectedIndex = 0;
                ddlEndAM.SelectedIndex = 0;
                ddlEndHour.SelectedIndex = 0;
                ddlStartAM.SelectedIndex = 0;

                credentialSetIDs = dbManager.ListCredentialSetIDs();
                credentialSets = dbManager.GetCredentialSets(credentialSetIDs);
                BuildServerDropDownListBox();

            }
        }

        /* 
             * Builds the Select Group drop down box. 
             * By default, the box gets filled with all the groups in the database
             */
        private void BuildServerDropDownListBox()
        {
            ddlLabServers.Items.Clear();
            try
            {
                IntTag[] lsTags = null;
                if(Session["labServerGuid"] != null)
                    lsTags = dbManager.GetLSResourceTags(Session["labServerGuid"].ToString());
                else
                    lsTags = dbManager.GetLSResourceTags();
                    ddlLabServers.Items.Add(new ListItem(" ---------- select Lab Server Resource ---------- "));
                foreach (IntTag tag in lsTags)
                {
                    ddlLabServers.Items.Add(new ListItem(tag.tag,tag.id.ToString()));
                }
                
            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot list LabServer Resources " + ex.Message + ". " + ex.GetBaseException();
                lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }

        }

        /// <summary>
        /// Parse the user time displayed values and convert to UTC 'user reperesentations', add user offset before
        /// inserting into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            DateTime startDate = DateTime.MinValue;
            TimeSpan startTime = TimeSpan.MinValue;
            int startHours = -1;
            int startMinutes = -1;
            
            DateTime endDate = DateTime.MinValue;
            TimeSpan endTime = TimeSpan.MinValue;
            int endHours = -1;
            int endMinutes = -1;
            int recurType = 0;
            int quan = 0;

            // input error check
            try
            {
                if (ddlLabServers.SelectedIndex <= 0)
                {
                    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage("You must select a Lab Server Resource.");
                    lblErrorMessage.Visible = true;
                    return;
                }
                if (ddlRecurrence.SelectedIndex <= 0)
                {
                    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage("You must select a recurrence type.");
                    lblErrorMessage.Visible = true;
                    return;

                }
                // Local System date forced to UTC type
                if (txtStartDate.Text.Length == 0 || txtStartDate.Text.CompareTo(culture.DateTimeFormat.ShortDatePattern) == 0)
                {
                    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage("You must enter the start date of the recurring time block.");
                    lblErrorMessage.Visible = true;
                    return;
                }
               startDate = DateTime.SpecifyKind(DateTime.Parse(txtStartDate.Text, culture), DateTimeKind.Utc);

               // Local System date forced to UTC type
                if (txtEndDate.Text.Length == 0 || txtEndDate.Text.CompareTo(culture.DateTimeFormat.ShortDatePattern) == 0)
                {
                    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage("You must enter the end date of the recurring time block.");
                    lblErrorMessage.Visible = true;
                    return;
                }
                endDate = DateTime.SpecifyKind(DateTime.Parse(txtEndDate.Text, culture), DateTimeKind.Utc);
                endDate = endDate.Add(TimeSpan.FromDays(1.0));
                if (endDate <= startDate)
                {
                    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage("The start date must be less than or equal to the end date.");
                    lblErrorMessage.Visible = true;
                    return;
                }
               
                startHours = ddlStartHour.SelectedIndex;
                if (ddlStartAM.Text.CompareTo("PM") == 0)
                {
                    startHours += 12;
                } 
                if (txtStartMin.Text.Length > 0)
                    startMinutes = int.Parse(txtStartMin.Text);
                if (startMinutes >= 60 || startMinutes < 0)
                {
                    string msg = "Please input minutes ( 0 - 59 ) in the start time ";
                    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage(msg);
                    lblErrorMessage.Visible = true;
                    return;
                }

                endHours = ddlEndHour.SelectedIndex;
                if (ddlEndAM.Text.CompareTo("PM") == 0)
                {
                    endHours += 12;
                }

                
                if (txtEndMin.Text.Length > 0)
                    endMinutes = int.Parse(txtEndMin.Text);
                if (endMinutes >= 60 || endMinutes < 0)
                {
                    string msg = "Please input minutes ( 0 - 59 ) in the end time ";
                    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage(msg);
                    lblErrorMessage.Visible = true;
                    return;
                }

                startTime = new TimeSpan(startHours, startMinutes, 0);
                endTime = new TimeSpan(endHours, endMinutes, 0);
                if (endHours == 0 && endMinutes == 0)
                    endTime = endTime.Add(TimeSpan.FromHours(24));


                if (startTime >= endTime)
                {
                    // If confirm see JavaScript
                    if(endDate.Subtract(startDate) > TimeSpan.FromDays(1.0)){
                        endTime = endTime.Add(TimeSpan.FromDays(1.0));
                    }
                    if (startTime >= endTime)
                    {
                        lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage("the start time should be earlier than the end time.");
                        lblErrorMessage.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
                return;
            }
            if (txtQuantum.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the Quantum of the Recurrence.");
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                quan = Int32.Parse(txtQuantum.Text);


            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a positive integer in the Quantum text box.");
                lblErrorMessage.Visible = true;
                return;
            }
            if (quan <= 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter an integer value greater than Zero in the Quantum text box.");
                lblErrorMessage.Visible = true;
                return;
            }

            //If all the input error checks are cleared
            //add a new Recurrence
            try
            {
                Recurrence recur = new Recurrence();
                recur.resourceId = int.Parse(ddlLabServers.SelectedValue);
                recur.recurrenceType = (Recurrence.RecurrenceType) int.Parse(ddlRecurrence.SelectedValue);
                TimeSpan numDays = endDate - startDate;
                numDays.Add(TimeSpan.FromDays(1));
                recur.numDays = (int)numDays.TotalDays;
                recur.startDate = startDate.AddMinutes(-localTzOffset);
                recur.startOffset = startTime;
                recur.endOffset = endTime;
                recur.dayMask = 0;
                recur.quantum = quan;
                if(recur.recurrenceType == Recurrence.RecurrenceType.Weekly){
                    foreach (ListItem it in cbxRecurWeekly.Items)
                        {
                            if (it.Selected)
                            {
                                switch (it.Text)
                                {
                                    case "Sunday":
                                        recur.dayMask |= DateUtil.SunBit;
                                        break;
                                    case "Monday":
                                        recur.dayMask |= DateUtil.MonBit;
                                        break;
                                    case "Tuesday":
                                        recur.dayMask |= DateUtil.TuesBit;
                                        break;
                                    case "Wednesday":
                                        recur.dayMask |= DateUtil.WedBit;
                                        break;
                                    case "Thursday":
                                        recur.dayMask |= DateUtil.ThursBit;
                                        break;
                                    case "Friday":
                                        recur.dayMask |= DateUtil.FriBit;
                                        break;
                                    case "Saturday":
                                        recur.dayMask |= DateUtil.SatBit;
                                        break;
                                    default:
                                        break;
                                }
                                //repeatDays.Add(it.Text);
                            }
                    }
                }
                //CheckValid is not written
                //string message = recur.CheckValid();
                //if( message != null){
                //    lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatWarningMessage(message);
                //    lblErrorMessage.Visible = true;
                //    return;
                //}

                //Create UTC copies of the the startTime and EndTime, to check Recurrence
                DateTime uStartTime = recur.Start;
                DateTime uEndTime = recur.End;

                // the database will also throw an exception if the combination of start time,
                // end time,lab server id, resource_id exists, or there are some 
                // Recurrences are included in the recurrence to be added.
                // since combination of them must be unique  and the time slots should 
                // be be overlapped
                // this is just another check to throw a meaningful exception

                // should only get & check recurrences on this resource
                Recurrence[] recurs = dbManager.GetRecurrences(dbManager.ListRecurrenceIDsByResourceID(uStartTime, uEndTime, recur.resourceId));
   
                if (recurs != null && recurs.Length > 0)
                {
                    buf = new StringBuilder();
                    bool conflict = false;
                    foreach (Recurrence r in recurs){
                        if(recur.HasConflict(r))
                        {
                            conflict = true;
                            report(r);
                        }
                    }

                    if (conflict)
                    {
                        lblErrorMessage.Text = Utilities.FormatErrorMessage(buf.ToString());
                        lblErrorMessage.Visible = true;
                        return;
                    }
                }
                //Add recurrence accordingly
                // if no recurrence is selected
                int recurID = dbManager.AddRecurrence(recur);
                Session["newOccurrenceID"] = recurID;

                //No longer creating TimeBlocks
             
                string jScript;
                jScript = "<script language=javascript> window.opener.Form1.hiddenPopupOnNewTB.value='1';";
                jScript += "window.close();</script>";
                Page.RegisterClientScriptBlock("postbackScript", jScript);
                return;
            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot add the time block '" + ddlLabServers.SelectedItem.Text + " " + Session["labServerName"].ToString() + " " + txtStartDate.Text + " " + txtEndDate.Text + "'. " + ex.Message + ". " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = iLabs.UtilLib.Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }
        }

        private void report(Recurrence recur)
        {
            // tell whether the recurrence recur is overlapping with the input recurrence
            buf.Append("Conflict with recurrence ");
            buf.Append(recur.recurrenceId + ": " + recur.recurrenceType + " ");
            buf.Append(DateUtil.ToUserDate(recur.startDate, culture, localTzOffset) + "->");
            buf.Append(DateUtil.ToUserDate(recur.startDate.AddDays(recur.numDays -1), culture, localTzOffset) + " ");
            TimeSpan offset = TimeSpan.FromMinutes(localTzOffset);
            buf.Append(recur.startOffset.Add(offset) + " -- " + recur.endOffset.Add(offset));
            buf.AppendLine("<br />");
        }



    }

}