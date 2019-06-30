using System.IO;
using Xunit;

namespace ExtendedStream.Test
{
    public class SubStreamTest
    {
        [Fact]
        public void Length_Constructor_Length_Not_Determined()
        {
            MemoryStream stream = new MemoryStream(new byte[100]);

            SubStream subStream = new SubStream(stream,25);

            Assert.Equal(75,subStream.Length);
        }
        [Fact]
        public void Length_Constructor_Length_Determined()
        {
            MemoryStream stream = new MemoryStream(new byte[100]);

            SubStream subStream = new SubStream(stream, 25,20);

            Assert.Equal(20, subStream.Length);
        }

        [Fact]
        public void Content()
        {
            MemoryStream stream = new MemoryStream(new byte[]{1,2,3,4,5,6,7,8,9,10});

            SubStream subStream = new SubStream(stream, 5);
            byte[] result = subStream.ToArray();

            Assert.Equal(new byte[] {  6, 7, 8, 9, 10 },result);
        }
        [Theory]
        [InlineData(3, SeekOrigin.Begin, 3)]
        [InlineData(3, SeekOrigin.Current, 3)]
        [InlineData(1, SeekOrigin.End, 4)]
        public void Seek_Position(long offset, SeekOrigin origin, long expectedPosition)
        {
            MemoryStream stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            SubStream subStream = new SubStream(stream, 5);
            subStream.Seek(offset, origin);

            Assert.Equal(expectedPosition, subStream.Position);
        }
        [Theory]
        [InlineData(3, SeekOrigin.Begin, 3)]
        [InlineData(3, SeekOrigin.Current, 3)]
        [InlineData(1, SeekOrigin.End, 4)]
        public void Seek_Result(long offset, SeekOrigin origin, long expectedPosition)
        {
            MemoryStream stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            SubStream subStream = new SubStream(stream, 5);
            long result = subStream.Seek(offset, origin);

            Assert.Equal(expectedPosition, result);
        }
        [Fact]
        public void One_Byte_Read_Result()
        {
            ActionStream stream = new ActionStream(new StreamActions
            {
                Read = (buffer, offset, count) => 
                {
                    buffer[offset] = 32;
                    return 1;
                },
                Seek = (l, origin) => 0,
                GetLength = () => 10
            });

            SubStream subStream = new SubStream(stream, 5);
            byte[] readBuffer = new byte[10];
            int readCount = subStream.Read(readBuffer,0,10);

            Assert.Equal(1,readCount);
        }
    }
}
