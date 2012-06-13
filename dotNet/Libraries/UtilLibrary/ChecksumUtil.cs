using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Net;

namespace iLabs.UtilLib{

/// <summary>
/// Provides Blob access and checksum utility methods.
/// </summary>
public class ChecksumUtil
{
 
    /// <summary>
    /// Computes the CRC32 checksum of a file using the file path. 
    /// The actual CRC32 algorithm is described in RFC 1952
    /// (GZIP file format specification version 4.3), this is also
    /// the specification for the algorithm used in java.util.zip.
    /// </summary>
    /// <param name="filePath">the physical path of the file on the ESS</param>
    /// <returns>the string form of the calculated CRC32 checksum</returns>
    public static string ComputeCRC32(string filePath)
    {
        CRC_1952 crc = new CRC_1952();

        FileStream fs = null;
        ulong csum = 0L;
        try
        {
            fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[512];
            int byteCount = fs.Read(buf, 0, 512);
            while (byteCount > 0)
            {
               csum =  crc.updateCRC(csum, buf, byteCount);
                byteCount = fs.Read(buf, 0, 512);
            }
            
            fs.Close();
            return csum.ToString("X");
        }
        catch (IOException ex)
        {
            throw new IOException("Exception thrown computing CRC32 checksum using the file path", ex);
        }
    }
    

    /// <summary>
    /// Computes the CRC32 checksum of a file using the data array
    /// The actual CRC32 algorithm is described in RFC 1952
    /// (GZIP file format specification version 4.3), this is also
    /// the specification for the algorithm used in java.util.zip.
    /// </summary>
    /// <param name="data">the array of data</param>
    /// <returns>the string form of the calculated CRC32 checksum</returns>
    public static string ComputeCRC32(byte [] data)
    {
        CRC_1952 crc = new CRC_1952();
        ulong csum = 0L;
        try
        {
            sbyte[] sdata = new sbyte[data.Length];
            for (int i = 0; i < data.Length; i++)
                sdata[i] = (sbyte)data[i];

            csum = crc.updateCRC(csum, data,data.Length);
            return csum.ToString("X");
        }
        catch (IOException ex)
        {
            throw new IOException("Exception thrown computing CRC32 checksum using the data array", ex);
        }
    }

    /// <summary>
    /// Computes the MD5 hash of a file using the file path
    /// </summary>
    /// <param name="filePath">the physical path of the file on the ESS</param>
    /// <returns>the string form of the calculated MD5 hash</returns>
    public static string ComputeMD5(string filePath)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        StringBuilder buff = new StringBuilder();
        FileStream fs = null;

        try
        {
            fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] md5Hash = md5.ComputeHash(fs);

            foreach (byte hashByte in md5Hash)
            {
                buff.Append(String.Format("{0:X1}", hashByte));
            }

            return buff.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("Exception thrown computing MD5 hash using the file path", ex);
        }

        finally
        {
            fs.Close();
        }

    }

    /// <summary>
    /// Computes the MD5 hash of a file using the data array
    /// </summary>
    /// <param name="data">the array of data</param>
    /// <returns>the string form of the calculated MD5 hash</returns>
    public static string ComputeMD5(byte[] data)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        StringBuilder buff = new StringBuilder();
        
        try
        {
            byte[] md5Hash = md5.ComputeHash(data);
            foreach (byte hashByte in md5Hash)
            {
                buff.Append(String.Format("{0:X1}", hashByte));
            }

            return buff.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("Exception thrown computing MD5 hash using the data array", ex);
        }

   }
   
}

}



