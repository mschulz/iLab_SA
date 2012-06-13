/* $Id$ */

using System;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;

using iLabs.UtilLib;
using iLabs.DataTypes.StorageTypes;

namespace iLabs.ExpStorage
{
    public class DownloadAPI
    {
        static Hashtable downloadClients = new Hashtable();
        
        public static void DownloadDataInBackground(long blobID, string blobUrl)
        {
            
            System.Threading.AutoResetEvent waiter = new System.Threading.AutoResetEvent(false);

            BlobWebClient client = new BlobWebClient();
            client.BlobID = blobID;
            client.BlobUrl = blobUrl;
            //client.BlobChecksum = blobChecksum;
            //client.BlobChecksumAlgorithm = blobChecksumAlgorithm;

            Uri uri = new Uri(blobUrl);

            client.DownloadDataCompleted += new System.Net.DownloadDataCompletedEventHandler(client_DownloadDataCompleted);
            client.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            downloadClients.Add(blobID, client);

            client.DownloadDataAsync(uri, waiter);

            //waiter.WaitOne();
        }

        static void RetryDownload(BlobWebClient client)
        {
            if (client.ErrorCount > 2)
            {
                downloadClients.Remove(client.BlobID);
                throw new System.Net.WebException("BlobID: " + client.BlobID + " at URL: " + client.BlobUrl
                    + " Download Error errorCount = " + client.ErrorCount);
            }
            else
            {
                Uri uri = new Uri(client.BlobUrl);
                System.Threading.AutoResetEvent waiter = new System.Threading.AutoResetEvent(false);
                client.DownloadDataAsync(uri, waiter);
            }
        }

        static void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            System.Threading.AutoResetEvent waiter = (System.Threading.AutoResetEvent)e.UserState;

            try
            {
                long blobID = ((BlobWebClient)sender).BlobID;
                if (((BlobWebClient)sender).BlobStatus != (int) Blob.eStatus.DOWNLOADING)
                {
                    BlobsAPI blobsAPI = new BlobsAPI();
                    //blobsAPI.SetBlobStatus(blobID, (int)Blob.eStatus.DOWNLOADING);
                }

            }
            finally
            {
                waiter.Set();
            }
        }

        static void client_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            
            System.Threading.AutoResetEvent waiter = (System.Threading.AutoResetEvent)e.UserState;

            string downloadChecksum = null;
            BlobsAPI blobsAPI = new BlobsAPI();
            try
            {
                // Get the content type
                string contentType = ((BlobWebClient)sender).ResponseHeaders["Content-Type"];
                
                // If the request was not canceled and did not throw any exception or errors
                if (!e.Cancelled && e.Error == null)
                {
                    byte[] data = (byte[])e.Result;

                    if (String.Compare(((BlobWebClient)sender).BlobChecksumAlgorithm, "crc32", true) == 0)
                        downloadChecksum = ChecksumUtil.ComputeCRC32(data);
                    else if (String.Compare(((BlobWebClient)sender).BlobChecksumAlgorithm, "md5", true) == 0)
                        downloadChecksum = ChecksumUtil.ComputeMD5(data);

                    if (String.Compare(downloadChecksum, ((BlobWebClient)sender).BlobChecksum, true) != 0)
                    {
                        // download complete, but corrupt
                        // We should retry
                        blobsAPI.SetBlobStatus(((BlobWebClient)sender).BlobID, (int)Blob.eStatus.CORRUPT);
                        ((BlobWebClient)sender).ErrorCount++;
                        RetryDownload((BlobWebClient)sender);
                    }
                    else
                    {
                        //download complete and successful
                        blobsAPI.StoreBlobData(((BlobWebClient)sender).BlobID, contentType, data);
                        downloadClients.Remove(((BlobWebClient)sender).BlobID);      
                    }

                }

                else if (e.Cancelled)
                {
                    //download cancelled
                    //blobsAPI.SetBlobStatus(((BlobWebClient)sender).BlobID, (int)Blob.eStatus.CANCELLED);
                    downloadClients.Remove(((BlobWebClient)sender).BlobID);
                }

                else if (!e.Cancelled && e.Error != null)
                {
                    //downloadHashTable[blobID] = BlobsAPI.eBlobStatus.FAILED;  //download failed
                    //blobsAPI.SetBlobStatus(((BlobWebClient)sender).BlobID, (int)Blob.eStatus.FAILED);
                    ((BlobWebClient)sender).ErrorCount++;
                    //RetryDownload((BlobWebClient)sender);
                }
            }

            catch (System.Net.WebException webEx)
            {
                blobsAPI.SetBlobStatus(((BlobWebClient)sender).BlobID, (int)Blob.eStatus.FAILED);
                ((BlobWebClient)sender).ErrorCount++;
                RetryDownload((BlobWebClient)sender);
                throw webEx;
            }

            catch (Exception ex)
            {
                //blobsAPI.SetBlobStatus(((BlobWebClient)sender).BlobID, (int)Blob.eStatus.FAILED);
                ((BlobWebClient)sender).ErrorCount++;
                //RetryDownload((BlobWebClient)sender);
                throw new Exception("Exception thrown uploading binary data to ESS", ex);
            }

            finally
            {
                // Let the main application thread resume.
                waiter.Set();
            }
        }

        /// <summary>
        /// Inner Class that the actual downloading. Created upon request to store data.
        /// </summary>
        protected class BlobWebClient : System.Net.WebClient
        {
            /// <summary>
            /// 
            /// </summary>
            private long blobID;

            /// <summary>
            /// 
            /// </summary>
            private int blobStatus = (int)Blob.eStatus.REQUESTED;

            /// <summary>
            /// 
            /// </summary>
            private string blobChecksum;

            /// <summary>
            /// 
            /// </summary>
            private string blobChecksumAlgorithm;

            /// <summary>
            /// 
            /// </summary>
            private string blobUrl;

            /// <summary>
            /// 
            /// </summary>
            private int errorCount = 0;

            /// <summary>
            /// 
            /// </summary>
            public long BlobID
            {
                get { return blobID; }
                set { blobID = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string BlobChecksum
            {
                get { return blobChecksum; }
                set { blobChecksum = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string BlobUrl
            {
                get { return blobUrl; }
                set { blobUrl = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string BlobChecksumAlgorithm
            {
                get { return blobChecksumAlgorithm; }
                set { blobChecksumAlgorithm = value; }
            }

            public int BlobStatus
            {
                get { return blobStatus; }
                set { blobStatus = value; }
            }
            public int ErrorCount
            {
                get { return errorCount; }
                set { errorCount = value; }
            }
            
        }
    }
}

