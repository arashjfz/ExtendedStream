using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ExtendedStream
{
    public static class StreamExtensions
    {
        public static int Read(this Stream stream, byte[] buffer, int count)
        {
            return stream.Read(buffer, 0, count);
        }
        public static int SafeRead(this Stream stream, byte[] buffer, int offset, int count)
        {
            int result = 0;
            while (result < count)
            {
                int readSize = stream.Read(buffer, offset + result, count - result);
                if (readSize == 0)
                    return result;
                result += readSize;

            }
            return result;
        }
        public static int SafeRead(this Stream stream, byte[] buffer, int count)
        {
            return stream.SafeRead(buffer, 0, count);
        }
        public static Stream Append(this Stream stream, byte[] buffer, int offset, int count)
        {
            return new MergedStream(new MemoryStream(buffer, offset, count), stream);
        }
        public static Stream Append(this Stream stream, Stream header)
        {
            return new MergedStream(header, stream);
        }
        public static Stream Prepend(this Stream stream, byte[] buffer, int offset, int count)
        {
            return new MergedStream(stream, new MemoryStream(buffer, offset, count));
        }
        public static Stream Prepend(this Stream stream, Stream tail)
        {
            return new MergedStream(stream, tail);
        }
        public static Stream SubStream(this Stream stream, long offset, long count, bool seekToOffset = true)
        {
            return new SubStream(stream, offset, count, seekToOffset);
        }
        public static Stream SubStream(this Stream stream, long offset, bool seekToOffset = true)
        {
            return new SubStream(stream, offset, seekToOffset);
        }
        public static Stream ToStream(this IEnumerable<Stream> streams)
        {
            Stream[] arrayStream = streams.ToArray();
            if (arrayStream.Length == 1)
                return arrayStream[0];
            return new MergedStream(arrayStream);
        }
        public static MemoryStream ToMemoryStream(this byte[] buffer)
        {
            return new MemoryStream(buffer);
        }
        public static byte[] ToArray(this Stream stream)
        {
            byte[] result = new byte[stream.Length - stream.Position];
            stream.SafeRead(result, 0, result.Length);
            return result;
        }
        public static string ReadString(this Stream stream)
        {
            return stream.ReadString(Encoding.ASCII);
        }
        public static string ReadString(this Stream stream, Encoding encoding)
        {
            return encoding.GetString(stream.ToArray());
        }
        public static void WriteString(this Stream stream, string value)
        {
            WriteString(stream, value, Encoding.ASCII);
        }
        public static void WriteString(this Stream stream, string value, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }
        public static void BinarySerialize(this Stream stream, object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }
        public static object BinaryDeserialize(this Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }
        public static T BinaryDeserialize<T>(this Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(stream);
        }
        public static Stream Inspect(this Stream stream, IStreamInspector inspector)
        {
            return new StreamInspector(stream, inspector);
        }

        public static void CopyTo(this Stream stream, Stream destination)
        {
            int bytesRead;
            byte[] buffer = new byte[2048];
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, bytesRead);
            }
        }

        public static Stream ThreadSafe(this Stream stream)
        {
            return new ThreadSafeStream(stream);
        }
    }
}
