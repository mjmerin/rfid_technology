using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace Philips_Ultrasound_Patient_Form
{
    /* Compression Class
     * This wrapper class provide support for Compressing data.
     * Compression class uses the Gzip or GNU zip data compression program.
     * It reduces the size of the named files using Lempel-Ziv(LZ77) and
     * Huffman coding.
     */
    class Compression
    {
       // Compress method
       // This function has two parameters. The first one is the data
	   // That is being compressed and the second is for writing the
       // compressed data into output stream.
        public void Compress(byte[] data, Stream output)
        {
            //Initializes the stream needed to store the compressed data
            GZipOutputStream zipStream = new GZipOutputStream(output);

            //Compresses a block of bytes to this stream
            zipStream.Write(data, 0, data.Length);
            
            //Clears all buffers for this stream
            zipStream.Flush();
            
            //Closes and releases any resources associated with the stream
            zipStream.Close();
        }

        // Decompress method
        // This function will decompress the data inside the dataStream
        // and return the decompressed data as an array of bytes
        public byte[] Decompress(byte[] data)
        {
            //Convert the incoming data into a stream
            Stream dataStream = new MemoryStream(data);
            // Initializes a new MemoryStream to store the decompressed data
            MemoryStream decompressData = new MemoryStream();

            // Decompresses the data
            using (GZipInputStream zipStream = new GZipInputStream(dataStream))
            {
                byte[] buffer = new byte[64];
                int bytesRead;

                while (true)
                {
                    bytesRead = zipStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break;
                    decompressData.Write(buffer, 0, bytesRead);
                }

                // Clears all buffers for this stream
                zipStream.Flush();

                // Closes and releases any resources associated with the stream
                zipStream.Close();
            }
            // Close the stream
            decompressData.Close();
            dataStream.Close();
            
            // Returns decompressed data as an array
            return decompressData.ToArray();
        }

    }
}
