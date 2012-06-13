using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iLabs.UtilLib;

namespace iLabs.LabServer.Interactive
{
    /// <summary>
    /// Summary description for LabUtils
    /// </summary>
    public class LabUtils
    {
        public LabUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string TimerScript(string utcStr, long duration, int userTZ, CultureInfo culture, string returnURL, int checkEvery)
        {
            return TimerScript(utcStr, duration, userTZ, culture, returnURL, checkEvery, null);
        }

        public static string TimerScript(string utcStr, long duration, int userTZ, CultureInfo culture, string returnURL,int checkEvery, string target)
        {
            DateTime startUTC = DateUtil.ParseUtc(utcStr);
            DateTime end = startUTC.AddSeconds(duration);
            DateTime start = startUTC.AddMinutes(userTZ);
            StringBuilder buf = new StringBuilder();
            int year = start.Year;
            int month = start.Month -1;
            int day = start.Day;
            int hour = start.Hour;
            int minute = start.Minute;
            int second = start.Second;

            buf.Append("<SCRIPT LANGUAGE=\"JavaScript\"><!--\n");
            buf.Append("function display(){\n");
            buf.Append("	rtime=etime-ctime;\n");
            buf.Append("	if (rtime>60)\n");
            buf.Append("		m=parseInt(rtime/60);\n");
            buf.Append("	else\n");
            buf.Append("		m=0;\n");
            buf.Append("	s=parseInt(rtime-m*60);\n");
            buf.Append("	if(s<10)\n");
            buf.Append("		s=\"0\"+s\n");
  
            buf.Append("	window.setTimeout(\"checktime()\"," + checkEvery + ");\n");
            if (target == null || target.Length == 0)
            {
                buf.Append("	window.status=\"Time Remaining :  \"+m+\":\"+s\n");
            }
            else
            {
                buf.Append("	document.");
                buf.Append(target + ".innerText=");
                buf.Append("\"+m+\":\"+s\n");
            }
            buf.Append("    return true\n");
            buf.Append("}\n");
            buf.Append("function redirectSB(){\n");
            buf.Append("	location.href=\"" + returnURL + "\";\n");
            buf.Append("}\n");
            buf.Append("function settimer(year,month,day,hour,minute,second,duration){\n");

            buf.Append("	var time = new Date(year,month,day,hour,minute,second);\n");
            //buf.Append("    var end = new Date(time.getTime() + (duration * 1000));\n");
            buf.Append("	hours= time.getHours();\n");
            buf.Append("	mins= time.getMinutes();\n");
            buf.Append("	secs= time.getSeconds();\n");
            buf.Append("	etime=hours*3600+mins*60+secs;\n");
            buf.Append("	etime+=" + duration + ";\n");
            //buf.Append("	etime =end.getTime();\n");
            buf.Append("    alert(\"Your reservation started at: ");

            buf.Append(DateUtil.ToUserTime(startUTC, culture, userTZ));
            buf.Append("\\nYou have until ");
            buf.Append(DateUtil.ToUserTime(end, culture, userTZ));
            buf.Append(" to run the Lab. ");
            buf.Append("\\nThe remaining time will be displayed in the windows status bar.\"");
            
          
            buf.Append(");\n");
            buf.Append("	checktime();\n");
            buf.Append("}\n");

            buf.Append("function checktime(){\n");
            buf.Append("	var time= new Date();\n");
            buf.Append("	hours= time.getHours();\n");
            buf.Append("	mins= time.getMinutes();\n");
            buf.Append("	secs= time.getSeconds();\n");
            buf.Append("	ctime=hours*3600+mins*60+secs\n");
            buf.Append("	if(ctime>etime)\n");
            buf.Append("		expired();\n");
            buf.Append("	else\n");
            buf.Append("		display();\n");
            buf.Append("}\n");

            buf.Append("function expired()\n");
            buf.Append("{\n");
            buf.Append("	window.status=\"Your reservation has expired!\"\n");
            buf.Append("	alert(\"Your reservation has expired!\");\n");
            //buf.Append("	window.setTimeout(\"redirecSB();\",20000);\n");
            //buf.Append("	location.href=\"" + returnURL + "\";\n");
            buf.Append("}\n");

            buf.Append("settimer(" + year + "," + month + "," + day + ",");
            buf.Append(hour + "," + minute +"," + second + "," + duration + ");\n");

            buf.Append("// --></SCRIPT>\n");
            return buf.ToString();
        }
    }
}
