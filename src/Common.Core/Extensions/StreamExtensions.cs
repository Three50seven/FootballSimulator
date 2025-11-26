using System;
using System.IO;

namespace Common
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Convert Stream to byte array. If MemoryStream, <see cref="MemoryStream.ToArray"/> is returned.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="copyToMemory">Whether or not stream should be copied to <see cref="MemoryStream"/>. If not, Read and Seek operations are performed. False by default.</param>
        /// <returns></returns>
        public static byte[] ToArray(this Stream stream, bool copyToMemory = false)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("Stream cannot be read.");

            MemoryStream ms = stream as MemoryStream;
            if (ms != null)
                return ms.ToArray();

            byte[] bytes;
            if (copyToMemory)
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }
            }
            else
            {
                long pos = stream.CanSeek ? stream.Position : 0L;
                if (pos != 0L)
                    stream.Seek(0, SeekOrigin.Begin);

                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                if (stream.CanSeek)
                    stream.Seek(pos, SeekOrigin.Begin);
            }

            return bytes;
        }
    }
}
