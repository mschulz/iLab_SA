using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;

namespace iLabs.Core
{
    // ${token}
    public  class iLabParser{
    
        public iLabParser()
        {
        }

        public static string Parse(StringBuilder input, Hashtable properties)
        {
            return Parse(input.ToString(), properties, false);
        }
        public static string Parse(StringBuilder input, Hashtable properties, bool appendDefaults)
        {
            return Parse(input.ToString(), properties, appendDefaults);
        }
        public static string Parse(string input, Hashtable properties)
        {
            return Parse(input, properties, false);
        }
        public static string Parse(string inputStr, Hashtable properties, bool appendDefaults)
        {
            string input = null;
            if (appendDefaults)
                input = addDefaultParameters(inputStr);
            else
                input = inputStr;
            int offset = 0;
            int pos = 0;
            int idx = -2;
            int len = 0;
            if(input == null || input.Length ==0){
                return input;
            }
            StringBuilder output = new StringBuilder();
            // test for token
            idx = input.IndexOf("${");
            if (idx == -1)
            {
                return input;
            }
            while (offset < input.Length)
            {
                idx = input.IndexOf("${",offset);
                if (idx == -1)
                {
                    output.Append(input.Substring(offset));
                    offset = input.Length;
                }
                else
                {
                    output.Append(input.Substring(offset, idx - offset));
                    offset = idx + 2;
                    idx = input.IndexOf('}', offset);
                    if (idx == -1)
                    {
                        throw new ParserException("Closing bracket not found");
                    }
                    string key = input.Substring(offset, idx - offset);
                    object value = properties[key];
                    if (value != null)
                    {
                        output.Append(value);
                        offset = idx + 1;
                    }
                    else
                    {
                        throw new ParserException(key + ": value not found");
                    }
                }
            }
            return output.ToString();
        }

        /// <summary>
        /// Check that the default operation coupon parameters are part of the loader script
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string addDefaultParameters(string str)
        {
            // check that the default auth tokens are added
            string loader = str;
            if (loader.IndexOf("coupon_id=") == -1)
            {
                if (loader.IndexOf("?") == -1)
                    loader += "?";
                else
                    loader += "&";
                loader += "coupon_id=${op:couponId}";
            }
            if (loader.IndexOf("passkey=") == -1)
            {
                if (loader.IndexOf("?") == -1)
                    loader += "?";
                else
                    loader += "&";
                loader += "passkey=${op:passkey}";
            }
            if (loader.IndexOf("issuer_guid=") == -1)
            {
                if (loader.IndexOf("?") == -1)
                    loader += "?";
                else
                    loader += "&";
                loader += "issuer_guid=${op:issuer}";
            }
            return loader;
        }
    }

public class ParserException: System.ApplicationException 
	{
		public ParserException(string message): base(message)
		{
		}
	}
    public class iLabProperties : Hashtable
    {
        public iLabProperties()
        {
        }
       
        public override void Add(object key, object value)
        {
            Add(key.ToString(), value.ToString());
        }
        public void Add(string key, object  value)
        {
            base.Add(key, value.ToString());
        }
        public void Add(string key, string value)
        {
            base.Add(key, value);
        }
        public void Add(string key, ProcessAgent info)
        {
            Add(key + ":agentGuid", info.agentGuid);
            Add(key + ":agentName", info.agentName);
            Add(key + ":codebase", info.codeBaseUrl);
            Add(key + ":serviceUrl", info.webServiceUrl);
        }
        public void Add(string key, ProcessAgentInfo info)
        {
            Add(key + ":agentGuid", info.agentGuid);
            Add(key + ":agentName", info.agentName);
            Add(key + ":codebase", info.codeBaseUrl);
            Add(key + ":serviceUrl", info.webServiceUrl);

        }
        public void Add(string key, Coupon coupon)
        {
            Add(key + ":couponId", coupon.couponId);
            Add(key + ":issuer", coupon.issuerGuid);
            Add(key + ":passkey", coupon.passkey);
        }
    }
}
