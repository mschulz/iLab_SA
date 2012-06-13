using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;

namespace iLabs.UtilLib
{
    /// <summary>
    /// A collection of date utilities mostly used to convert to and from UTC and User time.
    /// </summary>
    public class DateUtil{

        // Uses DayOfTheWeek order
        public const byte NoDays = 0;
        public const byte SunBit = 0x01;
        public const byte MonBit = 0x02;
        public const byte TuesBit = 0x04;
        public const byte WedBit = 0x08;
        public const byte ThursBit = 0x10;
        public const byte FriBit = 0x20;
        public const byte SatBit = 0x40;   
        public const byte AllDays = 0x7f;
        
        //public const DateTimeStyles LocalToUTC = DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal;

        // Create an Invariant Culture for general formating 
        private static IFormatProvider ivCulture = new CultureInfo("");


        public static bool CheckDayMask(DayOfWeek day, int mask)
        {
            bool status = false;
            switch (day)
            {
                case DayOfWeek.Sunday:
                    if ((mask & SunBit) > 0)
                        status = true;
                    break;
                case DayOfWeek.Monday:
                    if ((mask & MonBit) > 0)
                        status = true;
                    break;
                case DayOfWeek.Tuesday:
                    if ((mask & TuesBit) > 0)
                        status = true;
                    break;
                case DayOfWeek.Wednesday:
                    if ((mask & WedBit) > 0)
                        status = true;
                    break;
                case DayOfWeek.Thursday:
                    if ((mask & ThursBit) > 0)
                        status = true;
                    break;
                case DayOfWeek.Friday:
                    if ((mask & FriBit) > 0)
                        status = true;
                    break;
                case DayOfWeek.Saturday:
                    if ((mask & SatBit) > 0)
                        status = true;
                    break;
                default:
                    break;
            }
            return status;
        }

      

        /// <summary>
        /// Returns the local UTC timezone offset in minutes
        /// </summary>
        public static int LocalTzOffset
        {
            get
            {
                TimeZone localTZ = TimeZone.CurrentTimeZone;
                TimeSpan tzLocalOffset = localTZ.GetUtcOffset(DateTime.Now);
                return Convert.ToInt32(tzLocalOffset.TotalHours * 60);
            }
        }

        /// <summary>
        /// Parses a local format DateTime String and converts it to a UTC DaterTime object.
        /// </summary>
        /// <param name="dateStr">The User based localized dat string</param>
        /// <param name="culture">CultureInfo used to provide DateTime formats, usually derived from the browser.</param>
        /// <param name="tz">Difference in minutes from GMT, based on the user's browser concept of time. See JavaScript.</param>
        /// <returns></returns>
        public static DateTime ParseUserToUtc(string dateStr, CultureInfo culture, int tz)
        {
            DateTime value = DateTime.Parse(dateStr, culture, DateTimeStyles.AllowTrailingWhite);
            value = SpecifyUTC(value);
            value = value.AddMinutes(-tz);
            return value;
        }
        /// <summary>
        /// Parses a UTC generic format('o') date string and creates a DateTime with UTC specified.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime ParseUtc(string dateStr)
        {
            DateTime value = DateTime.Parse(dateStr, ivCulture , DateTimeStyles.RoundtripKind);
            value = value.ToUniversalTime();
            value = SpecifyUTC(value);
            return value;
        }

        /// <summary>
        /// Outputs a 'o' formated UTC string
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToUtcString(DateTime dt)
        {
            return dt.ToString("o");
        }

        /// <summary>
        /// Assumes a UTC time and returns a culture specific string for specified timezone
        /// </summary>
        /// <param name="dt">DataTime object normally, UTC based but checked for local type</param>
        /// <param name="culture">CultureInfo used to format the resulting string</param>
        /// <param name="tz">Difference in minutes from GMT, derived from Browser concept of tinme . See JavaScript</param>
        /// <returns></returns>
        public static string ToUserTime(DateTime dt, CultureInfo culture, int tz)
        {
            DateTime value;
            if (dt.Kind != DateTimeKind.Utc)
                value = dt.ToUniversalTime();
            else value = dt;
            value = value.AddMinutes(tz);
            return value.ToString(culture);
        }

        public static string ToUserTime(DateTime dt, CultureInfo culture, int tz, string format)
        {
            DateTime value;
            if (dt.Kind != DateTimeKind.Utc)
                value = dt.ToUniversalTime();
            else value = dt;
            value = value.AddMinutes(tz);
            return value.ToString(format, culture);
        }

        /// <summary>
        /// Assumes a UTC time and returns a culture specific string for specified timezone
        /// </summary>
        /// <param name="dt">DataTime object normally, UTC based but checked for local type</param>
        /// <param name="culture">CultureInfo used to format the resulting string</param>
        /// <param name="tz">Difference in minutes from GMT, derived from Browser concept of tinme . See JavaScript</param>
        /// <returns></returns>
        public static string ToUserDate(DateTime dt, CultureInfo culture, int tz)
        {
            DateTime value;
            if (dt.Kind != DateTimeKind.Utc)
                value = dt.ToUniversalTime();
            else value = dt;
            value = value.AddMinutes(tz);
            return value.Date.ToString(culture.DateTimeFormat.ShortDatePattern);
        }

        /// <summary>
        /// Forces the internal DateTime type to UTC, does not convert the value.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SpecifyUTC(DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }
        /// <summary>
        /// Parses the string returns the cultureInfo for the first vaild culture in the string, on an error it returns 'en-us'.
        /// </summary>
        /// <param name="languageStr"></param>
        /// <returns></returns>
        public static CultureInfo ParseCulture(string languageStr){
            CultureInfo culture = null;
            string type = null;
            string[] langs = null;
            try
            {
                if(languageStr.Contains(",")){
                    langs = languageStr.Split(',');
                    if(langs[0] != null && (langs[0].Length > 0) && !langs[0].Contains(";"))
                    type = langs[0];
                }
                else if(languageStr.Contains(";")){
                    langs = languageStr.Split(';');
                    if(langs[0] != null && (langs[0].Length > 0))
                    type = langs[0];
                }
                else{
                    type = languageStr;
                }
                culture = new CultureInfo(type);
                if (culture.IsNeutralCulture)
                {
                    if (type.Length == 2)
                    {
                        string langStr = type.ToLower() + "-" + type.ToUpper();
                        culture = new CultureInfo(langStr);

                    }
                    else
                    {
                        culture = CultureInfo.CreateSpecificCulture(type);
                    }
                    if (culture.IsNeutralCulture)
                    {
                        throw new Exception("Unable to parse language string: " + languageStr);
                    }
                }
            }
            catch(Exception e)
            {
                Logger.WriteLine("Error parsing LanguageString: " + languageStr + "Exception: " + e.Message);
                culture = new CultureInfo("en-us");
            }
            return culture;
        }

        public static string DateTime24(CultureInfo culture)
        {
             string temp = culture.DateTimeFormat.ShortDatePattern;
             if (!temp.Contains("MM"))
                 temp = temp.Replace("M", "MM");
             if (!temp.Contains("dd"))
                 temp = temp.Replace("d", "dd");
            return temp + " HH" + culture.DateTimeFormat.TimeSeparator + "mm";
        }

        /// <summary>
        /// Checks that the current time is after the startTime and within the duration in seconds. If duration is -1 true if now is greater than start. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static bool IsValidTimeSpan(DateTime start, long duration)
        {
            bool status = false;
            DateTime timeNow = DateTime.UtcNow;
            DateTime startTime = start.ToUniversalTime();
            if (duration == -1L)
            {
                status = timeNow >= startTime;
            }
            else
            {
                DateTime endTime = startTime.AddTicks(duration * TimeSpan.TicksPerSecond);
                status = ((timeNow >= startTime) && (timeNow <= endTime));
            }
            return status;
        }

        /// <summary>
        /// Returns the number of seconds remaing before the end of the time span.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="secondsDuration"></param>
        /// <returns></returns>
        public static long SecondsRemaining(DateTime start, long secondsDuration)
        {
            DateTime startTime = start.ToUniversalTime();
            long remaining = long.MaxValue;
            if (secondsDuration != -1)
            {
                TimeSpan ts = startTime.AddTicks(secondsDuration * TimeSpan.TicksPerSecond).Subtract(DateTime.UtcNow);
                remaining = ts.Ticks / TimeSpan.TicksPerSecond;
            }
            return remaining;
        }

        public static string ListDays(byte days,CultureInfo culture)
        {
          
            if (days == DateUtil.NoDays)
            {
                return "";
            }
            else if (days == DateUtil.AllDays)
            {
                return "Every day";
            }
            StringBuilder buf = new StringBuilder();
            bool more = false;
            if ((days & DateUtil.SunBit) == DateUtil.SunBit)
            {
                if(more){
                    buf.Append(", ");
                }
                else{
                    more = true;
                }
                buf.Append(culture.DateTimeFormat.GetShortestDayName(DayOfWeek.Sunday));
            }
            if ((days & DateUtil.MonBit) == DateUtil.MonBit)
            {
                if(more){
                    buf.Append(", ");
                }
                else{
                    more = true;
                }
                buf.Append(culture.DateTimeFormat.GetShortestDayName(DayOfWeek.Monday));
            }
            if ((days & DateUtil.TuesBit) == DateUtil.TuesBit)
            {
                if(more){
                    buf.Append(", ");
                }
                else{
                    more = true;
                }
                buf.Append(culture.DateTimeFormat.GetShortestDayName(DayOfWeek.Tuesday));
            }
            if ((days & DateUtil.WedBit) == DateUtil.WedBit)
            {
                if(more){
                    buf.Append(", ");
                }
                else{
                    more = true;
                }
                buf.Append(culture.DateTimeFormat.GetShortestDayName(DayOfWeek.Wednesday));
            }
            if ((days & DateUtil.ThursBit) == DateUtil.ThursBit)
            {
                if(more){
                    buf.Append(", ");
                }
                else{
                    more = true;
                }
                buf.Append(culture.DateTimeFormat.GetShortestDayName(DayOfWeek.Thursday));
            }
            if ((days & DateUtil.FriBit) == DateUtil.FriBit)
            {
                if(more){
                    buf.Append(", ");
                }
                else{
                    more = true;
                }
                buf.Append(culture.DateTimeFormat.GetShortestDayName(DayOfWeek.Friday));
            }
            if ((days & DateUtil.SatBit) == DateUtil.SatBit)
            {
                if(more){
                    buf.Append(", ");
                }
                else{
                    more = true;
                }
                buf.Append(culture.DateTimeFormat.GetShortestDayName(DayOfWeek.Saturday));
            }
            return buf.ToString();
        }

        public static string TimeSpanTrunc(TimeSpan time){
            StringBuilder buf = new StringBuilder();
            if(time < TimeSpan.Zero)
                buf.Append("-");
            if(time.Days != 0){
                buf.Append(time.Days);
                buf.Append(".");
            }
            buf.Append(time.Hours.ToString("D2"));
            buf.Append(":");
            buf.Append(time.Minutes.ToString("D2"));

           return buf.ToString();
        }

    }
}
