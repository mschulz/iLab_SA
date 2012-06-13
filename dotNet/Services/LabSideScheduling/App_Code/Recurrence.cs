using System;
using System.Collections;
using System.Collections.Generic;

using iLabs.DataTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Core;
using iLabs.UtilLib;

using iLabs.Proxies.USS;

namespace iLabs.Scheduling.LabSide
{


    /// <summary>
    /// a structure which holds recurrence
    /// </summary>
 
    public class Recurrence : ITimeBlock
    {
        public enum RecurrenceType : int
        {
            SingleBlock = 1, Daily, Weekly
        }

        /// <summary>
        /// the ID of the recurrence
        /// </summary>
        public int recurrenceId;
        /// <summary>
        /// the resourceID , the labServer and resource the recurrence is assigned to.
        /// </summary>
        public int resourceId;       
        /// <summary>
        /// the recurrenceType which can be SingleBlock = 1, Daily=2, Weekly=3
        /// </summary>
        public RecurrenceType recurrenceType;
        /// <summary>
        /// the start date of the recurrence, 00:00 local time as UTC
        /// </summary>
        public DateTime startDate;
        /// <summary>
        /// The number of days until midnight from the startDate, a value of one means the entire startDate.
        /// </summary>
        public int numDays;
        /// <summary>
        /// Offset in seconds from O:00 AM, used as the start time for each reccurence
        /// </summary>
        public TimeSpan startOffset;
        /// <summary>
        /// The offset from 0:00 AM of the current date used as the end time, depending on recurrenceType it has different limits and use.
        /// </summary>
        public TimeSpan endOffset;
        /// <summary>
        /// The number of minutes
        /// </summary>
        public int quantum;
      
        /// <summary>
        /// A mask defining the days of the week in the recurrence, see DateUtil DayMask
        /// </summary>
        public byte dayMask;
       

        public DateTime Start
        {
            get
            {
                return startDate.Add(startOffset);
            }
        }

        public DateTime End
        {
            get
            {
                DateTime endDate;
                DateTime end =  startDate;;
                switch ((RecurrenceType)recurrenceType)
                {
                    case RecurrenceType.SingleBlock:
                        end = startDate.AddDays(numDays - 1 ).Add(endOffset);
                        break;
                    case RecurrenceType.Daily: //Daily block
                        endDate = startDate.AddDays(numDays);
                        end = startDate.AddDays(numDays -1).Add(endOffset);
                        while(end > endDate){
                            end = end.AddDays(-1);
                        }
                        break;
                    case RecurrenceType.Weekly:
                        endDate = startDate.AddDays(numDays);
                        DateTime work = startDate.AddDays(numDays -1).Add(endOffset);
                        bool cont = true;
                        while (cont && work >= startDate)
                        {
                            if (DateUtil.CheckDayMask(work.DayOfWeek, dayMask))
                            {
                                if (work.Add(endOffset) <= endDate)
                                {
                                    cont = false;
                                    end = work.Add(endOffset);
                                }
                            }
                            work = work.AddDays(-1);
                        }
                        break;
                    default:
                        break;
                }
                return end;
            }
        }

        public int Duration{
            get{
               return (int) (End-Start).TotalSeconds;
            }
        }

        public bool Intersects(ITimeBlock target)
        {
            return (this.Start < target.End && this.End > target.Start);
        }

        public TimeBlock Intersection(ITimeBlock target)
        {
            if (Intersects(target))
            {
                DateTime start = (Start> target.Start) ? Start : target.Start;
                DateTime end = (End < target.End) ? End : target.End;
                return new TimeBlock(start, end);
            }
            else
                return null;
        }

        public int CompareTo(object that)
        {
            return CompareTo((ITimeBlock)that);
        }

        public int CompareTo(ITimeBlock b)
        {
            int status = 0;
            if (Start > b.Start)
            {
                status = 1;
            }
            else if (Start < b.Start)
            {
                status = -1;
            }
            else if (Duration > b.Duration)
            {
                status = 1;
            }
            else if (Duration < b.Duration)
            {
                status = -1;
            }
            return status;
        }


        public bool HasConflict(Recurrence recur)
        {
            bool status = false;
            switch ( (RecurrenceType) recurrenceType)
            {
                case RecurrenceType.SingleBlock: // Single block always overlaps
                    if (Intersects(recur))
                        status = true;
                    break;
                case RecurrenceType.Daily: //Daily block
                    switch ( (RecurrenceType) recur.recurrenceType)
                    {
                        case RecurrenceType.SingleBlock: // Single block always overlaps
                           if(Intersects(recur))
                                status = true;
                            break;
                        case RecurrenceType.Daily: //Daily block
                        case RecurrenceType.Weekly: // Weekly block
                            if (Intersects(recur))
                            {
                                if((startDate.TimeOfDay.Add(startOffset) 
                                    < recur.startDate.TimeOfDay.Add(recur.endOffset))
                                    && (startDate.TimeOfDay.Add(endOffset)
                                        > recur.startDate.TimeOfDay.Add(recur.startOffset)))
                                {
                                    status = true;
                                }
                            }  
                            break;
                        default:
                            break;
                    }
                    break;
                    case RecurrenceType.Weekly: // Weekly block
                    switch ( (RecurrenceType) recur.recurrenceType)
                    {
                        case RecurrenceType.SingleBlock: // Single block always overlaps
                            if (Intersects(recur))
                            {
                                status = true;
                            }
                            break;
                        case RecurrenceType.Daily: //Daily block
                            if (Intersects(recur))
                            {
                                if ((startDate.TimeOfDay.Add(startOffset)
                                    < recur.startDate.TimeOfDay.Add(recur.endOffset))
                                    && (startDate.TimeOfDay.Add(endOffset)
                                        > recur.startDate.TimeOfDay.Add(recur.startOffset)))
                                {
                                    status = true;
                                }
                            }
                            break;
                        case RecurrenceType.Weekly: // Weekly block, currently not supported
                            if (Intersects(recur))
                            {
                                if ((startDate.TimeOfDay.Add(startOffset)
                                    < recur.startDate.TimeOfDay.Add(recur.endOffset))
                                    && (startDate.TimeOfDay.Add(endOffset)
                                        > recur.startDate.TimeOfDay.Add(recur.startOffset)))
                                {
                                    if((dayMask & recur.dayMask) !=0)
                                        status = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return status;
        }

        public TimeBlock[] GetTimeBlocks()
        {
            List<TimeBlock> list = new List<TimeBlock>();
            TimeBlock chunk = null;
            TimeSpan day = TimeSpan.FromDays(1.0);
            DateTime endDate = startDate.AddDays(numDays);
            switch ((RecurrenceType)recurrenceType)
            {
                case RecurrenceType.SingleBlock:
                    chunk = new TimeBlock(Start,Duration);
                    list.Add(chunk);
                    break;
                case RecurrenceType.Daily:
                    DateTime work = startDate.Add(startOffset);
                    for (int i = 0; i < numDays; i++)
                    {
                        chunk = new TimeBlock(work,(int) (endOffset - startOffset).TotalSeconds);
                        if(chunk.End <= endDate)
                            list.Add(chunk);
                        work = work.AddDays(1.0);
                    }
                    break;
                case RecurrenceType.Weekly:
                   
                    DateTime wwork = startDate;
                    for (int i = 0; i < numDays; i++)
                    {
                        if (DateUtil.CheckDayMask(wwork.DayOfWeek, dayMask))
                        {
                            chunk = new TimeBlock();
                            chunk.startTime = wwork.Add(startOffset);
                            chunk.duration = (int) (endOffset - startOffset).TotalSeconds;
                            if(chunk.End <= endDate)
                                list.Add(chunk);
                        }
                        wwork = wwork.AddDays(1.0);
                    }
                    break;
                default:
                    break;
            }
            return list.ToArray();
        }

        public TimeBlock[] GetTimeBlocks(DateTime start, DateTime end)
        {           
            // Bounds check
            if (start >= End || end <= Start)
            {
                return null;
            }
           
            List<TimeBlock> list = new List<TimeBlock>();
            TimeBlock range = new TimeBlock(start >= Start ? start : Start, end.Ticks <= End.Ticks ? end : End);
            if (recurrenceType == RecurrenceType.SingleBlock)
            {
                
                list.Add(range);
            }
            else
            {
                TimeSpan day = TimeSpan.FromDays(1.0);
               
                int dayOffset = Convert.ToInt32(start.Subtract(startDate).TotalDays);
                int endDay = Convert.ToInt32(end.Subtract(startDate).TotalDays);
                endDay = numDays <= endDay ? numDays : endDay;
                DateTime work = startDate.Add(TimeSpan.FromDays(dayOffset -1));
                DateTime eDate = startDate.Add(TimeSpan.FromDays(endDay +1));
                TimeBlock chunk = null; 

                if (recurrenceType == RecurrenceType.Daily)
                {
                    
                    while (work < eDate)
                    {
                        chunk = range.Intersection(new TimeBlock(work.Add(startOffset),work.Add(endOffset)));
                        if(chunk != null)
                            list.Add(chunk);
                        work = work.Add(day);
                    }
                }
                else if (recurrenceType == RecurrenceType.Weekly)
                {
                    while (work < eDate)
                    {
                        if (DateUtil.CheckDayMask(work.DayOfWeek, dayMask))
                        {
                            chunk = range.Intersection(new TimeBlock(work.Add(startOffset), work.Add(endOffset)));
                            if (chunk != null)
                                list.Add(chunk);
                        }
                        work = work.Add(day);
                    }
                }
            }
            return list.ToArray();
        }
    }

}