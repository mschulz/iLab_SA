using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace iLabs.DataTypes.SchedulingTypes
{

    public interface ITimeBlock : IComparable
    {
         /// <summary>
        /// the start time of the time block
        /// </summary>
        DateTime Start
        {
            get;
        }
         /// <summary>
        /// the end time of the time block
        /// </summary>
        DateTime End
        {
            get;
        }
        /// <summary>
        /// The duration of the entire span in seconds
        /// </summary>
        int Duration
        {
            get;
        }
        bool Intersects(ITimeBlock target);
        TimeBlock Intersection(ITimeBlock target);
        
    }

    
    /// <summary>
    /// a structure which holds time block
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class TimeBlock : ITimeBlock
    {

        /// <summary>
        /// the start time of the time block
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// The length of the TimeBlock in seconds
        /// </summary>
        public int duration;

        public static TimeBlock[] MakeArray(ITimeBlock[] list){
            if(list != null && list.Length >0){
                List<TimeBlock> tbList = new List<TimeBlock>();
                foreach(ITimeBlock tb in list){
                    tbList.Add(new TimeBlock(tb.Start,tb.Duration));
                }
                return tbList.ToArray();
            }
            else return null;
        }


        public static TimeBlock[] Concatenate(ITimeBlock[] listIn)
        {
            if (listIn != null && listIn.Length > 0)
            {
                TimeBlock[] list = TimeBlock.MakeArray(listIn);
             
                TimeBlock wrk = null;
                bool cont = true;
                while (cont)
                {
                    cont = false;
                    bool[] status = new bool[list.Length];
                    List<TimeBlock> returnList = new List<TimeBlock>();
                    for (int i = 0; i < list.Length; i++)
                    {
                        if (!status[i])
                        {
                            status[i] = true;
                            wrk = list[i];
                            for (int j = i + 1; j < list.Length; j++)
                            {
                                if (!status[j])
                                {
                                    if (TimeBlock.HasUnion(wrk, list[j]))
                                    {
                                        wrk = TimeBlock.Union(wrk,list[j]);
                                        status[j] = true;
                                        cont = true;
                                    }
                                }
                            }
                            returnList.Add(wrk);
                        }
                    }
                    list = returnList.ToArray();
                }
                return list;
            }
            else return null;
        }

        public static TimeBlock[] Remaining(ITimeBlock[] range, ITimeBlock[] remove)
        {
            List<TimeBlock> rangeList = null;
            List<TimeBlock> removeBlocks = new List<TimeBlock>();
            List<TimeBlock> removeList = new List<TimeBlock>();
            List<TimeBlock> addList = new List<TimeBlock>();
            int minTime = 0;
            if (range != null && range.Length > 0)
            {
                rangeList = new List<TimeBlock>();
                rangeList.AddRange(TimeBlock.Concatenate(range));
                if (remove != null && remove.Length > 0)
                {
                    removeList.AddRange(TimeBlock.MakeArray(remove));
                    TimeBlock tmp;
                    int code = 0;
                    int hit = 0;
                    TimeBlock intersection = null;
                    TimeBlock previous = null;
                    TimeBlock remainder = null;

                    foreach (TimeBlock remTB in removeList)
                    {
                        
                        foreach (TimeBlock rangeTB in rangeList)
                        {
                            intersection = null;
                            previous = null;
                            remainder = null;
                            if (rangeTB.Intersects(remTB))
                            {
                               
                                code = rangeTB.Split(remTB, ref intersection, ref previous, ref remainder);
                                if (previous != null)
                                {
                                    addList.Add(previous);
                                }
                                if (remainder != null)
                                {
                                    addList.Add(remainder);
                                }
                            }
                            else
                            {
                                addList.Add(rangeTB);
                            }
                        }
                        rangeList.Clear();
                        rangeList.AddRange(addList);
                        addList.Clear();
                    }
                }
            }
            if (rangeList != null)
            {
                return rangeList.ToArray();
            }
            else return null;
        }
            
     

        public static TimeBlock[] Remaining_1(ITimeBlock[] range, ITimeBlock[] remove)
        {
            List<TimeBlock> rangeList = null;
            int minTime = 0;
            if (range != null && range.Length > 0)
            {
                rangeList = new List<TimeBlock>();
                rangeList.AddRange(TimeBlock.MakeArray(range));
                if (remove != null && remove.Length > 0)
                {

                    List<TimeBlock> removeList = new List<TimeBlock>();
                    removeList.AddRange(TimeBlock.MakeArray(remove));
                    TimeBlock tmp;
                    int code = 0;
                    int hit = 0;
                    List<TimeBlock> addBlocks = new List<TimeBlock>();
                    List<TimeBlock> removeBlocks = new List<TimeBlock>();
                    foreach (TimeBlock remTB in removeList)
                    {
                        if (removeBlocks.Count > 0)
                        {
                            foreach (TimeBlock rt in removeBlocks)
                            {
                                rangeList.Remove(rt);
                            }
                            removeBlocks.Clear();
                        }
                        if (addBlocks.Count > 0)
                        {
                            rangeList.AddRange(addBlocks);
                            addBlocks.Clear();
                        }
                        TimeBlock intersection = null;
                        TimeBlock previous = null;
                        TimeBlock remainder = null;
                        foreach (TimeBlock tb in rangeList)
                        {
                            if (tb.duration > minTime)
                            {
                                if (remTB.Intersects(tb))
                                {
                                    removeBlocks.Add(tb);
                                    code = tb.Split(remTB, ref intersection, ref previous, ref remainder);
                                    if ((code & 1) == 1)
                                    {
                                        if (previous.duration >= minTime)
                                        {
                                            addBlocks.Add(previous);
                                        }
                                    }
                                    if ((code & 4) == 4)
                                    {
                                        if (remainder.duration >= minTime)
                                        {
                                            addBlocks.Add(remainder);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                removeBlocks.Add(tb);
                            }
                        }
                    }
                }
               
            }
            if (rangeList != null)
            {
                return rangeList.ToArray();
            }
            else return null;

        }

        public static bool HasUnion(ITimeBlock a, ITimeBlock b)
        {
            return (a.Start <= b.End && a.End >= b.Start);
        }

        public static TimeBlock Union(ITimeBlock a, ITimeBlock b)
        {
            TimeBlock tb = null;
            if(TimeBlock.HasUnion(a,b))
                tb = new TimeBlock(a.Start <= b.Start ? a.Start : b.Start,a.End >= b.End ?a.End:b.End);
            return tb;
        }

        public int CompareTo(object that)
        {
            return CompareTo((ITimeBlock) that);
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

        public TimeBlock() { }

        public TimeBlock(DateTime start, DateTime end)
        {
            startTime = start;
            duration = (int) (end - start).TotalSeconds;
        }

        public TimeBlock(DateTime start, int duration)
        {
            startTime = start;
            this.duration = duration;
        }

        public int Duration
        {
            get
            {
                return duration;
            }
        }

        /// <summary>
        /// the start time of the time block
        /// </summary>
        public DateTime Start
        {
            get
            {
                return startTime;
            }
        }
        /// <summary>
        /// the end time of the time block
        /// </summary>
        public DateTime End{
            get{
                return startTime.AddSeconds(duration);
            }
        }
        public bool Contains(ITimeBlock target)
        {
            return ((this.startTime <= target.Start) && (this.End >= target.End));
        }

        public bool Intersects(ITimeBlock target)
        {
            return (startTime < target.End && this.End > target.Start);
        }

        public TimeBlock Intersection(ITimeBlock target)
        {
            if (Intersects(target))
            {
                DateTime start = (startTime > target.Start)? startTime : target.Start;
                DateTime end =  (End < target.End) ?End : target.End;
                return new TimeBlock(start,end);
            }
            else
                return null;
        }

        

        public int Split(ITimeBlock subBlock, ref TimeBlock intersection, ref TimeBlock previous, ref TimeBlock remaining)
        {
            int status = 0;
            intersection = null;
            previous = null;
            remaining = null;
            if (Intersects(subBlock))
            {
                intersection = Intersection(subBlock);
                if (startTime < intersection.Start)
                {
                    previous = new TimeBlock(startTime, intersection.Start);
                    status |= 1;
                }
                else if (intersection.Start > subBlock.Start)
                {
                    previous = new TimeBlock(subBlock.Start, intersection.Start);
                    status |= 2;
                }
                if (End > intersection.End)
                {
                    remaining = new TimeBlock(intersection.End, End);
                    status |= 4;
                }
                else if (subBlock.End > intersection.End)
                {
                    remaining = new TimeBlock(intersection.End, subBlock.End);
                    status |= 8;
                }
            }
            return status;
        }
    }

    /// <remarks/>
   
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class TimePeriod : TimeBlock
    {
        public static TimePeriod[] MakeArray(ITimeBlock[] list, int quantum)
        {
            if (list != null && list.Length > 0)
            {
                List<TimePeriod> tbList = new List<TimePeriod>();
                foreach (ITimeBlock tb in list)
                {
                    tbList.Add(new TimePeriod(tb.Start, tb.Duration,quantum));
                }
                return tbList.ToArray();
            }
            else return null;
        }
   
        /// <summary>
        /// Suggested quantum in minutes, used to schedule a reccurring permission.
        /// </summary>
        public int quantum = 5; // in Minutes

        public TimePeriod() { }

        public TimePeriod(DateTime start, DateTime end) : base(start,end)
        {       
        }

        public TimePeriod(DateTime start, int duration) : base(start,duration)
        {
        }
        public TimePeriod(DateTime start, DateTime end, int quantum) : base(start, end)
        {
            this.quantum = quantum;
        }

        public TimePeriod(DateTime start, int duration, int quantum): base(start, duration)
        {
            this.quantum = quantum;
        }


        public static ArrayList SortByTime(ArrayList timePeriods)
        {
            for (int i = timePeriods.Count; --i >= 0; )
            {
                bool flipped = false;
                for (int j = 0; j < i; j++)
                {

                    if (((TimePeriod)timePeriods[j]).startTime > ((TimePeriod)timePeriods[j + 1]).startTime)
                    {
                        TimePeriod T = (TimePeriod)timePeriods[j];
                        timePeriods[j] = timePeriods[j + 1];
                        timePeriods[j + 1] = T;
                        flipped = true;
                    }
                }

                if (!flipped)
                {
                    return timePeriods;
                }
            }
            return timePeriods;
        }

        public DateTime endTime
        {
            get
            {
                return startTime.AddSeconds(duration);
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]

    public class Reservation : TimeBlock
    {
        private string userNameField;

        public Reservation() { }

        public Reservation(DateTime start, DateTime end) : base(start,end)
        {       
        }

        public Reservation(DateTime start, int duration) : base(start,duration)
        {
        }
       
        /// <remarks/>
        public string userName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }


    }


}
