using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.UtilLib{

    /// <summary>
    /// Computes CRC32 data checksum.
    /// The actual CRC32 algorithm is described in RFC 1952
    /// (GZIP file format specification version 4.3), this is also
    /// the specuification for the algorithm used in java.util.zip.
    /// streams.
    /// </summary>
    public class CRC_1952
    {
/*
 * 
 * RFC 1952             GZIP File Format Specification             May 1996
 * 
         The following sample code represents a practical implementation of
   the CRC (Cyclic Redundancy Check). (See also ISO 3309 and ITU-T V.42
   for a formal specification.)

   The sample code is in the ANSI C programming language. Non C users
   may find it easier to read with these hints:

      &      Bitwise AND operator.
      ^      Bitwise exclusive-OR operator.
      >>     Bitwise right shift operator. When applied to an
             unsigned quantity, as here, right shift inserts zero
             bit(s) at the left.
      !      Logical NOT operator.
      ++     "n++" increments the variable n.
      0xNNN  0x introduces a hexadecimal (base 16) constant.
             Suffix L indicates a long value (at least 32 bits).
*/
      /* Table of CRCs of all 8-bit messages. */
      static ulong [] crc_table = new ulong[256];

      /* Make the table for a fast CRC. */
      static void make_crc_table()
      {
        ulong c;

        int n, k;
        for (n = 0; n < 256; n++) {
          c = (ulong) n;
          for (k = 0; k < 8; k++) {
            if ((c & 1L) != 0){
              c = 0xedb88320L ^ (c >> 1);
            } else {
              c = c >> 1;
            }
          }
          crc_table[n] = c;
        }
      }

    static CRC_1952(){
        make_crc_table();
    }

    private ulong crcValue = 0L;

        /// <summary>
        /// Returns the current checksum value.
        /// </summary>
    public ulong Checksum
    {
        get { return crcValue & 0xffffffff; }
    }

        /// <summary>
        /// Clears the current checksum.
        /// </summary>
    public void reset()
    {
        crcValue = 0L;
    }

      /*
         Update a running crc with the bytes buf[0..len-1] and return
       the updated crc. The crc should be initialized to zero. Pre- and
       post-conditioning (one's complement) is performed within this
       function so it shouldn't be done by the caller. Usage example:

         ulong crc = 0L;

         while (read_buffer(buffer, length) != EOF) {
           crc = update_crc(crc, buffer, length);
         }
         if (crc != original_crc) error();
      */

        /// <summary>
        /// Update a running crc with the bytes buf[0..len-1] and return
        /// the updated crc. The crc should be initialized to zero. Pre- and
        /// post-conditioning (one's complement) is performed within this
        /// function so it shouldn't be done by the caller.
        /// </summary>
        /// <param name="crc"></param>
        /// <param name="buf"></param>
        /// <param name="len"></param>
        /// <returns></returns>
      public ulong updateCRC(ulong crc, byte [] buf, int len)
      {
        crcValue = crc ^ 0xffffffffL;

        for (int n = 0; n < len; n++)
        {
            crcValue = crc_table[(crcValue ^ buf[n]) & 0xff] ^ (crcValue >> 8);
        }
            return crcValue ^ 0xffffffffL;
        
      }


      /// <summary>
      /// Return the CRC of the bytes buf[0..len-1].
      /// </summary>
      /// <param name="buf"></param>
      /// <param name="len"></param>
      /// <returns></returns>
      public ulong crc(byte [] buf, int len)
      {
        return updateCRC(0L, buf, len);
      }

    }
}
