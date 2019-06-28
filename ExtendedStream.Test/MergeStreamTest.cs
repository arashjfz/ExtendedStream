using System;
using System.IO;
using System.Linq;
using Xunit;

namespace ExtendedStream.Test
{
    public class MergeStreamTest
    {
        [Fact]
        public void Length()
        {
            MemoryStream firstStream = new MemoryStream(new byte[20]);
            MemoryStream secondStream = new MemoryStream(new byte[30]);

            MergedStream mergedStream = new MergedStream(firstStream, secondStream);

            Assert.Equal(50, mergedStream.Length);
        }
        [Fact]
        public void Content()
        {
            MemoryStream firstStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            MemoryStream secondStream = new MemoryStream(new byte[] { 6, 7, 8, 9 });

            MergedStream mergedStream = new MergedStream(firstStream, secondStream);
            byte[] result = mergedStream.ToArray();

            Assert.Equal(new byte[]{1,2,3,4,5,6,7,8,9},result);
        }
        [Theory]
        [InlineData(3,SeekOrigin.Begin,3)]
        [InlineData(3,SeekOrigin.Current,3)]
        [InlineData(3,SeekOrigin.End,6)]
        public void Seek_Position(long offset, SeekOrigin origin,long expectedPosition)
        {
            MemoryStream firstStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            MemoryStream secondStream = new MemoryStream(new byte[] { 6, 7, 8, 9 });

            MergedStream mergedStream = new MergedStream(firstStream, secondStream);
            mergedStream.Seek(offset, origin);

            Assert.Equal(expectedPosition,mergedStream.Position);
        }
        [Theory]
        [InlineData(3, SeekOrigin.Begin, 3)]
        [InlineData(3, SeekOrigin.Current, 3)]
        [InlineData(3, SeekOrigin.End, 6)]
        public void Seek_Result(long offset, SeekOrigin origin, long expectedPosition)
        {
            MemoryStream firstStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            MemoryStream secondStream = new MemoryStream(new byte[] { 6, 7, 8, 9 });

            MergedStream mergedStream = new MergedStream(firstStream, secondStream);
            long result = mergedStream.Seek(offset, origin);

            Assert.Equal(expectedPosition, result);
        }

    }
}
