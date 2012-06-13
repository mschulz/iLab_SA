<%@ WebHandler Language="C#" Class="iLabs.ExpStorage.BlobHandler" %>

using System;
using System.Web;

using iLabs.DataTypes.StorageTypes;
using iLabs.ExpStorage.ESS;

namespace iLabs.ExpStorage
{
    public class BlobHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string mime = "text/plain";
            string blobStr = context.Request.QueryString["bid"];
            string authStr = context.Request.QueryString["key"];
            string passStr = context.Request.QueryString["pid"];

            // check auth
            if (true)
            {
                //retrieve BLOB
                long blobId = -1;
                if (blobStr != null & blobStr.Length > 0)
                {
                    blobId = Convert.ToInt64(blobStr);

                    if (blobId >= 0)
                    {
                        BlobsAPI blobAPI = new BlobsAPI();
                        byte[] buf = null;
                        Blob blob = blobAPI.GetBlob(blobId);
                        if (blob != null)
                        {

                            buf = blobAPI.RetrieveBlobData(blobId);
                            if (buf != null && buf.Length > 0)
                            {
                                if (blob.mimeType != null && blob.mimeType.Length > 0)
                                {
                                    mime = blob.mimeType;
                                }
                                else
                                {
                                    mime = "application/octet-stream";
                                }
                                //set  Response fields
                                context.Response.Expires = 10; // Hard coded; cache on server for 10 minutes
                                context.Response.BufferOutput = true;
                                context.Response.ContentType = mime;
                                context.Response.BinaryWrite(buf);
                            }
                            else
                            {
                                context.Response.ContentType = mime;
                                context.Response.Write("Specified BLOB contained no data!");
                            }
                        }
                        else
                        {
                            context.Response.ContentType = mime;
                            context.Response.Write("Specified BLOB does not exist!");
                        }
                    }
                    else
                    {
                        context.Response.ContentType = mime;
                        context.Response.Write("The requested BLOB ID is invalid!");
                    }
                }
                else
                {
                    context.Response.ContentType = mime;
                    context.Response.Write("No BLOB specified!");
                }
            }
            else
            {
                context.Response.ContentType = mime;
                context.Response.Write("Access Denied!");
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}