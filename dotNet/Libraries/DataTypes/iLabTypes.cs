using System;
using System.Text;

namespace iLabs.DataTypes
{

    /// <summary>
    /// A triplet used as part of an SQL query.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class Criterion
    {
        /// <summary>
        /// A named attribute
        /// </summary>
        public string attribute;

        /// <summary>
        /// A predicate expressed as a string
        /// </summary>
        public string predicate;

        /// <summary>
        /// A constant value expressed as a string
        /// </summary>
        public string value;

        public Criterion() { }

        public Criterion(string attribute, string predicate, string value)
        {
            this.attribute = attribute;
            this.predicate = predicate;
            this.value = value;
        }

        public string ToSQL()
        {
            StringBuilder buf = new StringBuilder("( ");
            buf.Append(attribute + " ");
            buf.Append(predicate + " ");
            //if (value.Contains(" "))
            //{
            //}
            //else
            //{
            //    buf.Append(value);
            //}
            buf.Append("'" + value + "'");
            buf.Append(") ");
            return buf.ToString();
        }
    }

    /// <summary>
    /// Utility for storing an int value and user readable string for DDL processing
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class IntTag : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        public int id;

        /// <summary>
        /// 
        /// </summary>
        public string tag;

        /// <summary>
        /// 
        /// </summary>
        public IntTag()
        {
        }

        public IntTag(int id, string tag)
        {
            this.id = id;
            this.tag = tag;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.tag.CompareTo(((IntTag)obj).tag);
        }

        #endregion
    }

    /// <summary>
    /// Utility for storing a long value and user readable string for DDL processing
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class LongTag : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        public long id;

        /// <summary>
        /// 
        /// </summary>
        public string tag;

        /// <summary>
        /// 
        /// </summary>
        public LongTag()
        {
        }

        public LongTag(long id, string tag)
        {
            this.id = id;
            this.tag = tag;
        }
        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.tag.CompareTo(((LongTag)obj).tag);
        }

        #endregion
    }

    /// <summary>
    /// Utility for storing a long value and user readable string for DDL processing
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class StringTag : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        public string id;

        /// <summary>
        /// 
        /// </summary>
        public string tag;

        /// <summary>
        /// 
        /// </summary>
        public StringTag()
        {
        }

        public StringTag(string id, string tag)
        {
            this.id = id;
            this.tag = tag;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.tag.CompareTo(((StringTag)obj).tag);
        }

        #endregion
      
    }

}
