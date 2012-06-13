/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

/* Use the following DOCTYPE and meta values for using this control
 <!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
   <meta http-equiv="X-UA-Compatible" content="IE=Edge"/> 
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/xhtml-11"/>
    <style type="text/css">@import url( css/main.css );  @import url( css/scheduling.css ); 
 */

#define noStyle

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iLabs.DataTypes.SchedulingTypes;
using iLabs.UtilLib;

namespace iLabs.Controls.Scheduling
{
   
    public class SchedulingControl : DataBoundControl, IPostBackEventHandler
    {
        int browserMode = 0;
        DateTime startTime;
        DateTime endTime;
        TimeSpan maxTOD;
        TimeSpan minTOD;
        TimeSpan minDuration;
        TimeSpan tzSpan;

        TimeSpan oneDay = TimeSpan.FromDays(1);
        int minHour = 0;
        int maxHour = 24;
        int numCols;
        CultureInfo culture;
        List<TimePeriod> periods = new List<TimePeriod>();
        List<Reservation> reservations = null;

        string scheduleTableClass = "scheduling";
        string hourTableClass = "hours";
        string dayTableClass = "day";
        string voidClass = "void";
        string reservedClass = "reserved";
        string availableClass = "available";
        Unit one = new Unit(1);
        Unit zero = new Unit(0);
       
        #region Properties

        bool bindReservations = false;
        bool hours24 = false;

        int columnWidth = 120;
        int headerHeight = 25;
        int hourHeight = 40;
        int hourWidth = 50; 
        int defaultCellDuration = 30;
        int userTZ;
        Unit scheduleWidth = new Unit("100%");
        
        Color availableColor = ColorTranslator.FromHtml("#55ff55");
        Color scheduledColor = ColorTranslator.FromHtml("#ff0000");
        //Color timeBorderColor = ColorTranslator.FromHtml("#000000");
        Color voidColor = ColorTranslator.FromHtml("#aaaaaa");

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
            }
        }
        public DateTime StartDate
        {
            get
            {
                return startTime.AddMinutes(userTZ).Date;
            }
           
        }
        public DateTime EndDate
        {
            get
            {
                return endTime.AddMinutes(userTZ).Date;
            }
          
        }
        public int UserTZ
        {
            get
            {
                return userTZ;
            }
            set
            {
                userTZ = value;
                tzSpan = TimeSpan.FromMinutes(userTZ);
            }
        }
        public CultureInfo Culture
        {
            get
            {
                return culture;
            }
            set
            {
                culture = value;
            }
        }

        public TimeSpan MaxTOD
        {
            get
            {
                return maxTOD;
            }
            set
            {
                maxTOD = value;
                if (maxTOD >= oneDay)
                {
                    maxHour = 24;
                }
                else
                {
                    if (maxTOD.TotalHours > maxTOD.Hours)
                        maxHour = maxTOD.Hours + 1;
                    else
                        maxHour = maxTOD.Hours;
                }
            }
     
        }
        public TimeSpan MinTOD
        {
            get
            {
                return minTOD;
            }
            set
            {
                minTOD = value;
                minHour = minTOD.Hours;
            }
  
        }

        public int MaxHour
        {
            get
            {
                return maxHour;
            }
        }

        public int MinHour
        {
            get
            {
                return minHour;
            }

        }

        public TimeSpan MinDuration
        {
            get
            {
                return minDuration;
            }
            set
            {
                minDuration = value;
            }

        }
        public int NumDays
        {
            get
            {
                int tmp = Convert.ToInt32((EndDate - StartDate).TotalDays);
                if (tmp < 1){
                    tmp = 1;
                }
                return tmp;
            }
        }

         public bool BindResevations
        {
            get
            {
                return bindReservations;
            }
            set
            {
                bindReservations = value;
            }
        }

        public bool Hours24
        {
            get
            {
                return hours24;
            }
            set
            {
                hours24 = value;
            }
        }

        public int ColumnWidth
        {
            get
            {
                return columnWidth;
            }
            set
            {
                columnWidth = value;
            }
        }
        public int HeaderHeight
        {
            get
            {
                return headerHeight;
            }
            set
            {
                headerHeight = value;
            }
        }
         public int HourHeight
        {
            get
            {
                return hourHeight;
            }
            set
            {
               hourHeight = value;
            }
        }
        public int HourWidth
        {
            get
            {
                return hourWidth;
            }
            set
            {
                hourWidth = value;
            }
        }
        
        public Unit ScheduleWidth
        {
            get
            {
                return scheduleWidth;
            }
            set
            {
                scheduleWidth = value;
            }
        }
        public Color  AvailableColor {
            get {
                return availableColor;
            }
            set{
                availableColor = value;
            }
        }
        public Color ScheduledColor{
            get {
                return scheduledColor;
            }
            set{
                scheduledColor = value;
            }
        }
        //public Color TimeBorderColor{
        //    get {
        //        return timeBorderColor;
        //    }
        //    set{
        //        timeBorderColor = value;
        //    }
        //}
        public Color VoidColor{
            get {
                return voidColor;
            }
            set{
                voidColor = value;
            }
        }

        #endregion
        #region styleSetting

        private string strHeight(int height)
        {
            int h;
            if (browserMode == 0)
            {
                h = height - 1;
            }
            else
            {
                h = height;
            }
            return h.ToString();
        }

        private int calcHeight(int height)
        {
            if(browserMode == 0){
                return height - 1;
            }
            else{
                return height;
            }
        }

        public void setBrowser(string userAgent)
        {
            if(userAgent.StartsWith("Mozilla/5.0"))
            {
                browserMode = 0;
            }
            else if (userAgent.StartsWith("Mozilla/4.0"))// all IE browsers need further checking
            {
                if (userAgent.Contains("MSIE 8.0") || userAgent.Contains("compatible; MSIE 7.0"))
                {
                    browserMode = 0;
                }
                else
                {
                    browserMode = 1;
                }
            }
        }

        public string getDocType()
        {
            string buf = null;
            switch(browserMode){
              case 0:
                buf = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">";
                break;
              default:
                  buf = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\" >";
                break;
            }
            return buf;
        }
        #endregion

        /// <summary>
        /// Event called when the user clicks a reservation in the calendar. It's only called when DoPostBackForEvent is true.
        /// </summary>
        public event ScheduledClickDelegate ScheduledClick;

        /// <summary>
        /// Event called when the user clicks an availible time block in the calendar. It's only called when DoPostBackForFreeTime is true.
        /// </summary>
        public event AvailableClickDelegate AvailableClick;
   
        #region Rendering

        /// <summary>
        /// Renders the component HTML code.
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            Type bType = Page.Request.Browser.TagWriter;
            output.WriteLine();
            output.WriteLine("<!- Scheduling Table -->");
            // <table>
            output.AddAttribute("id", ID);
            output.AddAttribute("class", scheduleTableClass);
            output.AddAttribute("cols",(NumDays + 1).ToString());
            if (browserMode > 0)
            {
                output.AddAttribute("cellpadding", "0px");
                output.AddAttribute("cellspacing", "0px");
            }
         
#if useStyle 
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(BorderColor));
            output.AddStyleAttribute("border-top", "1px");
            output.AddStyleAttribute("border-right", "1px");
            output.AddStyleAttribute("border-left", "1px");
            output.AddStyleAttribute("border-bottom", "1px");     
#endif
            
            //output.AddStyleAttribute("position", "relative");
            output.RenderBeginTag("table");
            
            // <tr> Table contents
            output.AddAttribute("id", "trTableContents");
            output.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
#if useStyle
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
#endif
            output.RenderBeginTag("tr"); //Single row
            int offset = 1;

            // hours cell
            renderHoursTable(output,offset);
            offset += hourWidth;

            output.WriteLine("<!- Day Tables -->");
            for (DateTime day = StartDate; day < EndDate; day = day.AddDays(1))
            {
                renderDay(output,day.AddMinutes(-userTZ),offset);
                offset += columnWidth;
            }
           
            output.RenderEndTag(); // </tr> Table Contents
            output.RenderEndTag();   // </table>
            output.WriteLine("<!- End Scheduling  Table -->");
        }


        private void renderHoursTable(HtmlTextWriter output, int offset)
        {
            output.AddAttribute("id", "tdHourCell");
            //output.AddStyleAttribute("background-color", "blue");
#if useStyle
            //output.AddStyleAttribute("width", hourWidth + "px");
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(BorderColor));
            output.AddStyleAttribute("border", "0px");
            //output.AddStyleAttribute("border-top", "0px");
            //output.AddStyleAttribute("border-right", "1px");
            //output.AddStyleAttribute("border-left", "0px");
            //output.AddStyleAttribute("border-bottom", "1px");
            //output.AddStyleAttribute("position", "absolute");
            //output.AddStyleAttribute("top", zero.ToString());
            //output.AddStyleAttribute("left", offset.ToString() + "px");
#endif
            output.RenderBeginTag("td");
            output.WriteLine();
            output.WriteLine("<!- Hour Table -->");
            output.AddAttribute("class", hourTableClass);
            if (browserMode > 0)
            {
                output.AddAttribute("cellpadding", "0px");
                output.AddAttribute("cellspacing", "0px");
                output.AddStyleAttribute("border-left","1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
           
#if useStyle
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(BorderColor));
            output.AddStyleAttribute("border-top", "0px");
            output.AddStyleAttribute("border-right", "1px");
            output.AddStyleAttribute("border-left", "0px");
            output.AddStyleAttribute("border-bottom", "1px");
            output.AddStyleAttribute("text-align", "right");
#endif
            output.RenderBeginTag("table");
            output.RenderBeginTag("tr");
            output.AddStyleAttribute("height", HeaderHeight * 2 + "px");
            output.AddStyleAttribute("width", hourWidth + "px");
            if (browserMode > 0)
            {
                output.AddStyleAttribute("border-bottom","1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
#if useStyle
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
#endif
            output.RenderBeginTag("th");
            output.Write("&nbsp;");
            output.RenderEndTag(); // end hd
            output.RenderEndTag(); // end tr
          
            
            for (int i = MinHour; i < MaxHour; i++)
            {
                renderHourCell(output, i);
            }

            //output.WriteLine();
            output.RenderEndTag(); // end of table
            output.RenderEndTag(); // </td> End of hours cell
            output.WriteLine();

        }

       

        private int renderHourCell(HtmlTextWriter output, int hour)
        {
            output.Write("<tr>");

            //output.AddStyleAttribute("top", top + "px");
            output.AddStyleAttribute("height", calcHeight(HourHeight) + "px");
            if (browserMode > 0)
            {
                output.AddStyleAttribute("border-bottom","1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
            //output.AddStyleAttribute("background-color", "Yellow");
#if useStyle
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
            output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
#endif
            output.RenderBeginTag("th");

            bool am = (hour / 12) == 0;
            if (!hours24)
            {
                hour = hour % 12;
                if (hour == 0)
                    hour = 12;
            }
            output.Write(hour);
            output.Write("<span style='font-size:10px; '>&nbsp;");
            if (hours24)
            {
                output.Write("00");
            }
            else
            {
                if (am)
                    output.Write("AM");
                else
                    output.Write("PM");
            }
            output.Write("</span>&nbsp;");
            output.RenderEndTag();
            output.WriteLine("</tr>");
            return HourHeight;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="day">UTC corrected TZ start of day</param>
        /// <param name="offset"></param>
        private void renderDay(HtmlTextWriter output, DateTime day, int offset)
        {
            output.WriteLine();
            //output.AddStyleAttribute("position", "absolute");
            //output.AddStyleAttribute("top", "0px");
            //output.AddStyleAttribute("left", offset.ToString() + "px");
            output.RenderBeginTag("td");
            // <table>
            output.AddAttribute("class", dayTableClass);
            if (browserMode > 0)
            {
                output.AddAttribute("cellpadding", "0px");
                output.AddAttribute("cellspacing", "0px");
                output.AddStyleAttribute("border-left", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
#if useStyle
            output.AddStyleAttribute("width", columnWidth + "px");
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
            output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(BorderColor));
            output.AddStyleAttribute("border-top", "0px");
            output.AddStyleAttribute("border-right", "1px");
            output.AddStyleAttribute("border-left", "0px");
            output.AddStyleAttribute("border-bottom", "1px");
#endif
            output.RenderBeginTag("table");

            //Day Header 
            output.Write("<tr>");
            output.AddStyleAttribute("height", headerHeight*2 + "px");
            output.AddStyleAttribute("width", columnWidth + "px");
            if (browserMode > 0)
            {
                output.AddStyleAttribute("border-bottom","1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
#if useStyle
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
            output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
#endif
            output.RenderBeginTag("th");
            output.Write(day.AddMinutes(userTZ).ToString("ddd", culture) + "<br />");
            output.Write(DateUtil.ToUserDate(day,culture,userTZ));
            output.RenderEndTag();
            output.WriteLine("</tr>");
            if (bindReservations)
            {
                renderReservations(output, day, reservations);
            }
            else
            {
                renderTimePeriods(output, day, periods);
            }
            output.RenderEndTag();
            output.RenderEndTag();
        }

        private void renderTimePeriods(HtmlTextWriter output, DateTime date, IEnumerable periods)
        {
          
            TimeBlock validTime = new TimeBlock(date.AddHours((int)minTOD.Hours), date.Add(maxTOD));
            TimeBlock valid;
            DateTime cur = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            if (periods != null)
            {
                IEnumerator enumTP = null;
                try
                {
                    enumTP = periods.GetEnumerator();
                    enumTP.MoveNext();
                    TimePeriod first = (TimePeriod)enumTP.Current;
                    if (first.Start > validTime.Start)
                    {
                        renderVoidTime(output, Convert.ToInt32((first.Start - validTime.Start).TotalSeconds));
                    }
                }
                catch (Exception ex) {
                    throw ex;
                }
                finally
                {
                    enumTP.Reset();
                }

                foreach (TimePeriod tp in periods)
                {
                    if (validTime.Intersects(tp))
                    {
                        valid = validTime.Intersection(tp);
                        cur = valid.Start;
                        end = valid.End;
                        if (tp.quantum == 0)
                        {
                            renderScheduledTime(output, valid);
                        }
                        else
                        {
                            int cellDur = 0;
                            int tDur = valid.Duration;
                            

                            while (cur < end)
                            {
                                cellDur = defaultCellDuration -( cur.TimeOfDay.Minutes % defaultCellDuration);
                                if(cellDur < defaultCellDuration) 
                                    cellDur += defaultCellDuration;
                                if ((end - cur.AddMinutes(cellDur)).TotalMinutes < (defaultCellDuration/2))
                                {
                                    cellDur += Convert.ToInt32((end - cur.AddMinutes(cellDur)).TotalMinutes );
                                }
                                cellDur = (cur.AddMinutes(cellDur) <= end) ? cellDur : (int)(end - cur).TotalMinutes;
                                renderAvailableTime(output, cur, tDur, tp.quantum, cellDur * 60, false);
                                cur = cur.AddMinutes(cellDur);
                                tDur = tDur - (cellDur * 60);

                                //cellDur = (cur.TimeOfDay.Minutes % defaultCellDuration) + defaultCellDuration;
                                //cellDur = (cur.AddMinutes(cellDur) <= end) ? cellDur : (int)(end - cur).TotalMinutes;
                                //renderAvailableTime(output, cur, tDur, tp.quantum, cellDur * 60, false);
                                //cur = cur.AddMinutes(cellDur);
                                //tDur = tDur - (cellDur * 60);
                            }
                        }
                    }
                } // End of foreach period
                if (end < validTime.End)
                {
                    renderScheduledTime(output, new TimePeriod(end,validTime.End));
                }
            }
        }
       
        private int renderVoidTime(HtmlTextWriter output, int duration)
        {
            output.Write("<tr>");
           int height = Convert.ToInt32((hourHeight* duration)/3600.0);
           output.AddAttribute("class", voidClass);
           //output.AddAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, startTime.ToString("s")));
           output.AddAttribute("title", (duration / 60.0).ToString());
           output.AddStyleAttribute("height", calcHeight(height) + "px");
            if (browserMode > 0)
            {
                output.AddStyleAttribute("border-bottom","1px solid " + ColorTranslator.ToHtml(BorderColor));
                output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(voidColor));
            }
#if useStyle
           output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
           output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());

           output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(voidColor));
            //output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
#endif
           output.RenderBeginTag("td");
            output.RenderEndTag();
            output.WriteLine("</tr>");
            return height;
        }

        private int renderScheduledTime(HtmlTextWriter output,  ITimeBlock tb)
        {
            output.Write("<tr>");
           int height = Convert.ToInt32((((hourHeight * tb.Duration)/3600.0)));
           //output.AddAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, startTime.ToString("s")));
#if useStyle
             output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
           output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(scheduledColor));
            output.AddStyleAttribute("cursor", "hand");
            //output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
#endif
           output.AddAttribute("class", reservedClass);
            output.AddStyleAttribute("height", calcHeight(height) + "px");
           if (browserMode > 0)
            {
                output.AddStyleAttribute("border-bottom","1px solid " + ColorTranslator.ToHtml(BorderColor));
                output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(scheduledColor));
            }
            output.AddAttribute("title", tb.Start.AddMinutes(userTZ).TimeOfDay.ToString() + " - " + tb.End.AddMinutes(userTZ).TimeOfDay.ToString());
            output.RenderBeginTag("td");
            //if(height > 24)
            //    output.Write(tb.Start.AddMinutes(userTZ).TimeOfDay );
            //if(height > 48)
            //    output.Write(" - " + tb.End.AddMinutes(userTZ).TimeOfDay);
            output.RenderEndTag();
            output.WriteLine("</tr>");
            return height;
             
        }

        private int renderAvailableTime(HtmlTextWriter output, DateTime startTime, int duration,int quantum, int cellDuration,bool lastCell)
        {
            string str = startTime.ToString("o") + ", " + duration + ", " + quantum;
            output.Write("<tr>");
            int height = Convert.ToInt32((((hourHeight* cellDuration)/3600.0)));
            output.AddAttribute("class", availableClass);
            output.AddAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, str));
            output.AddAttribute("title", startTime.AddMinutes(userTZ).TimeOfDay.ToString());
            output.AddStyleAttribute("height", calcHeight(height) + "px");
            if (browserMode > 0)
            {
                output.AddStyleAttribute("border-bottom","1px solid " + ColorTranslator.ToHtml(BorderColor));
                output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(availableColor));
            }
            
#if useSttyle
            output.AddStyleAttribute(HtmlTextWriterStyle.Padding, zero.ToString());
            output.AddStyleAttribute(HtmlTextWriterStyle.Margin, zero.ToString());
            output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(availableColor));
            output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(availableColor));
            output.AddStyleAttribute("cursor", "hand");
            if(lastCell)
                output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            else
                output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
#endif
            output.RenderBeginTag("td");
            output.Write("&nbsp;");
            //DateTime end = startTime.AddSeconds(duration);
            //output.Write(startTime.AddMinutes(userTZ).TimeOfDay + " - " + end.AddMinutes(userTZ).TimeOfDay);
           
            output.RenderEndTag();
            output.WriteLine("</tr>");
            return height;
        }

        private void renderReservations(HtmlTextWriter output, DateTime day, IEnumerable reservations)
        { 
            int a = 0;
        }
 
        #endregion

        #region Viewstate

        /// <summary>
        /// Loads ViewState.
        /// </summary>
        /// <param name="savedState"></param>
        //protected override void LoadViewState(object savedState)
        //{
        //    if (savedState == null)
        //        return;

        //    object[] vs = (object[])savedState;

        //    if (vs.Length != 2)
        //        throw new ArgumentException("Wrong savedState object.");

        //    if (vs[0] != null)
        //        base.LoadViewState(vs[0]);

        //    if (vs[1] != null)
        //        items = (ArrayList)vs[1];

        //}

        /// <summary>
        /// Saves ViewState.
        /// </summary>
        /// <returns></returns>
        //protected override object SaveViewState()
        //{
        //    object[] vs = new object[2];
        //    vs[0] = base.SaveViewState();
        //    vs[1] = items;

        //    return vs;
        //}

        #endregion

        #region PostBack


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            AvailableClick(this, new AvailableClickEventArgs  (eventArgument));   
        }

        #endregion

        #region Data binding


        protected override void PerformSelect()
        {
            // Call OnDataBinding here if bound to a data source using the
            // DataSource property (instead of a DataSourceID), because the
            // databinding statement is evaluated before the call to GetData.       
            if (!IsBoundUsingDataSourceID)
            {
                this.OnDataBinding(EventArgs.Empty);
            }

            // The GetData method retrieves the DataSourceView object from  
            // the IDataSource associated with the data-bound control.            
            GetData().Select(CreateDataSourceSelectArguments(),
                this.OnDataSourceViewSelectCallback);

            // The PerformDataBinding method has completed.
            RequiresDataBinding = false;
            MarkAsDataBound();

            // Raise the DataBound event.
            OnDataBound(EventArgs.Empty);
        }

        private void OnDataSourceViewSelectCallback(IEnumerable retrievedData)
        {
            // Call OnDataBinding only if it has not already been 
            // called in the PerformSelect method.
            if (IsBoundUsingDataSourceID)
            {
                OnDataBinding(EventArgs.Empty);
            }
            // The PerformDataBinding method binds the data in the  
            // retrievedData collection to elements of the data-bound control.
            PerformDataBinding(retrievedData);
        }

        protected override void PerformDataBinding(IEnumerable schedulingData)
        {
            // don't bind in design mode
            if (DesignMode)
            {
                return;
            }
            base.PerformDataBinding(schedulingData);
            
            MinTOD = TimeSpan.Zero;
            MaxTOD = oneDay;
            // Verify data exists.
            if (schedulingData != null)
            {

                if (NumDays == 1)
                {
                    TimeSpan tmpTS;
                    TimeSpan tmpMinTOD = oneDay;
                    //TimeSpan tmpMaxTOD = TimeSpan.Zero.Subtract(TimeSpan.FromMinutes(userTZ));
                    //tmpMinTOD = startTime.Add(TimeSpan.FromMinutes(userTZ)).TimeOfDay;
                    // tmpMaxTOD = endTime.Add(TimeSpan.FromMinutes(userTZ)).TimeOfDay;
                    //if (tmpMaxTOD.Hours < 24)
                    //{
                    //    if (tmpMaxTOD.TotalHours > maxTOD.Hours)
                    //    {
                    //        tmpMaxTOD = TimeSpan.FromHours(maxTOD.Hours + 1.0);
                    //    }
                    //}



                    //DateTime localEnd = endTime.Subtract(TimeSpan.FromMinutes(userTZ));
                    //localStart.
                    //TimeSpan tmpTS;
                    //TimeSpan oneDay = TimeSpan.FromDays(1);
                    //maxTOD = TimeSpan.Zero.Subtract(TimeSpan.FromMinutes(userTZ));
                    //minTOD = oneDay;

                    //Check the first timeBlock
                    IEnumerator eTB = schedulingData.GetEnumerator();
                    if (eTB.MoveNext())
                    {
                        tmpTS = (( TimeBlock)eTB.Current).Start.AddMinutes(userTZ).TimeOfDay;
                        if (tmpMinTOD > tmpTS)
                        {
                            tmpMinTOD = tmpTS;
                            MinTOD = tmpMinTOD;
                        }

                    }
                //    //Check all TimeBlocks
                //    foreach (ITimeBlock tb in schedulingData)
                //    {
                //        tmpTS = tb.Start.AddMinutes(userTZ).TimeOfDay;
                //        if (tmpMinTOD > tmpTS)
                //            tmpMinTOD = tmpTS;
                //        //tmpTS = tmpTS.Add(TimeSpan.FromSeconds(tb.Duration));
                //        //if (tmpTS.TotalHours < 24)
                //        //{
                //        //    if (tmpMaxTOD < tmpTS)
                //        //    {
                //        //        tmpMaxTOD = tmpTS;
                //        //    }
                //        //}
                //        //else if (tmpTS.TotalHours == 24)
                //        //{
                //        //    tmpMaxTOD = tmpTS;
                //        //}
                //        //else
                //        //{
                //        //    tmpMinTOD = TimeSpan.Zero;
                //        //    tmpMaxTOD = TimeSpan.FromHours(24);
                //        //    break;
                //        //}
                //    }
                //    if (tmpMinTOD != oneDay)
                //    {
                //        MinTOD = tmpMinTOD;
                //    }
                }
                if (bindReservations)
                { // should be Reservations
                    reservations = new List<Reservation>();
                    foreach (Reservation r in schedulingData)
                    {
                        reservations.Add(r);
                    }
                }
                else
                {  // should be TimePeriods
                    TimeBlock range = new TimeBlock(StartTime, EndTime);
                    periods = new List<TimePeriod>();
                    foreach (TimePeriod tp in schedulingData)
                    {
                        if(range.Contains(tp)){
                            periods.Add(tp);
                        }
                        else if (range.Intersects(tp))
                        {
                            TimeBlock tmpTB = range.Intersection(tp);
                            periods.Add(new TimePeriod(tmpTB.Start, tmpTB.Duration, tp.quantum));
                        }
                    }
                    TimeBlock[] scheduledTBs = TimeBlock.Remaining(new TimeBlock[] { range }, periods.ToArray());
                    if (scheduledTBs != null && scheduledTBs.Length > 0)
                    {
                        periods.AddRange(TimePeriod.MakeArray(scheduledTBs, 0));
                    }
                    periods.Sort();
                }
            }
            else
            {
                
                periods = new List<TimePeriod>();
                periods.Add(new TimePeriod(StartTime, EndTime, 0));
            }
        }


        #endregion


    }

    /// <summary>
	/// Delegate for passing an event primary key.
	/// </summary>
	public delegate void ScheduledClickDelegate(object sender, ScheduledClickEventArgs e);

	/// <summary>
	/// Delegate for passing a starting time.
	/// </summary>
	public delegate void AvailableClickDelegate(object sender, AvailableClickEventArgs e);


    public class ScheduledClickEventArgs : EventArgs
    {
        private string value;

        public string Value
        {
            get { return value; }
        }

        public ScheduledClickEventArgs(string value)
        {
            this.value = value;
        }
    }

    public class AvailableClickEventArgs : EventArgs
    {
        private DateTime start;
        private int duration;
        private int quantum;


        public DateTime Start
        {
            get { return start; }
        }

        public int Duration
        {
            get
            {
                return duration;
            }
        }
        public int Quantum
        {
            get
            {
                return quantum;
            }
        }

        public AvailableClickEventArgs(string values)
        {
            string[] data = values.Split(new char[] { ',' });
            start = DateUtil.ParseUtc(data[0]);
           // start.Kind = DateTimeKind.Utc;
            duration = Int32.Parse(data[1]);
            quantum = Int32.Parse(data[2]);
        }
    }
}
