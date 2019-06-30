using System;
using System.IO;
using Xunit;

namespace ExtendedStream.Test
{
    public class ActionStreamTest
    {
        [Fact]
        public void Flush_Not_Supported()
        {
            ActionStream actionStream = new ActionStream(new StreamActions{Flush = null});
            Assert.Throws<NotSupportedException>(() => actionStream.Flush());
        }
        [Fact]
        public void Read_Not_Supported()
        {
            ActionStream actionStream = new ActionStream(new StreamActions());
            Assert.Throws<NotSupportedException>(() =>
            {
                byte[] buffer = new byte[1];
                actionStream.Read(buffer,0,1);
            });
        }
        [Fact]
        public void Write_Not_Supported()
        {
            ActionStream actionStream = new ActionStream(new StreamActions());
            Assert.Throws<NotSupportedException>(() =>
            {
                byte[] buffer = new byte[1];
                actionStream.Write(buffer, 0, 1);
            });
        }
        [Fact]
        public void Seek_Not_Supported()
        {
            ActionStream actionStream = new ActionStream(new StreamActions());
            Assert.Throws<NotSupportedException>(() =>
            {
                actionStream.Seek(1,SeekOrigin.Begin);
            });
        }
        [Fact]
        public void SetLength_Not_Supported()
        {
            ActionStream actionStream = new ActionStream(new StreamActions());
            Assert.Throws<NotSupportedException>(() =>
            {
                actionStream.SetLength(1);
            });
        }
        [Fact]
        public void Position_Not_Supported()
        {
            ActionStream actionStream = new ActionStream(new StreamActions());
            Assert.Throws<NotSupportedException>(() =>
            {
                long actionStreamPosition = actionStream.Position;
            });
        }
        [Fact]
        public void Length_Not_Supported()
        {
            ActionStream actionStream = new ActionStream(new StreamActions());
            Assert.Throws<NotSupportedException>(() =>
            {
                long actionStreamLength = actionStream.Length;
            });
        }
        [Fact]
        public void Flush()
        {
            bool flushed = false;
            ActionStream actionStream = new ActionStream(new StreamActions {Flush = () => flushed = true});

            actionStream.Flush();

            Assert.True(flushed);
        }

        [Fact]
        public void Read_Content()
        {
            ActionStream actionStream = new ActionStream(new StreamActions { Read = (buffer, offset, count) =>
            {
                for (int i = 0; i < count; i++)
                {
                    buffer[offset + i] = (byte) i;
                }
                return count;
            } });
            byte[] readBuffer = new byte[10];
            actionStream.Read(readBuffer,0,readBuffer.Length);

            Assert.Equal(new byte[]{0,1,2,3,4,5,6,7,8,9},readBuffer);
        }
        [Fact]
        public void Read_Length()
        {
            ActionStream actionStream = new ActionStream(new StreamActions
            {
                Read = (buffer, offset, count) =>
                {
                    for (int i = 0; i < count; i++)
                    {
                        buffer[offset + i] = (byte)i;
                    }
                    return count;
                }
            });
            byte[] readBuffer = new byte[10];
            int readLength = actionStream.Read(readBuffer, 0, readBuffer.Length);

            Assert.Equal(readBuffer.Length, readLength);
        }
        [Fact]
        public void Write()
        {
            byte[] expectedBuffer = new byte[10];
            ActionStream actionStream = new ActionStream(new StreamActions
            {
                Write = (buffer, offset, count) =>
                {
                    for (int i = 0; i < count; i++)
                    {
                        expectedBuffer[i] = buffer[offset + i];
                    }
                }
            });
            byte[] writeBuffer = new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            actionStream.Write(writeBuffer, 0, writeBuffer.Length);

            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, expectedBuffer);
        }
        [Fact]
        public void Set_Length()
        {
            long expectedLength = 0;
            ActionStream actionStream = new ActionStream(new StreamActions
            {
                SetLength = length => expectedLength = length
            });
            actionStream.SetLength(10);

            Assert.Equal(10, expectedLength);
        }

        [Theory]
        [InlineData(3,SeekOrigin.Begin)]
        [InlineData(3,SeekOrigin.Current)]
        [InlineData(3,SeekOrigin.End)]
        public void Seek(long testOffset, SeekOrigin testOrigin)
        {
            long expectedOffset = 0;
            SeekOrigin expectedOrigin = SeekOrigin.Begin;
            ActionStream actionStream = new ActionStream(new StreamActions
            {
                Seek = (offset, origin) =>
                {
                    expectedOffset = offset;
                    expectedOrigin = origin;
                    return 0;
                }
            });
            actionStream.Seek(testOffset,testOrigin);

            Assert.Equal(testOffset, expectedOffset);
            Assert.Equal(testOrigin, expectedOrigin);
        }
        [Fact]
        public void Set_Position()
        {
            long expectedOffset = -1;
            SeekOrigin expectedOrigin = SeekOrigin.End;
            ActionStream actionStream = new ActionStream(new StreamActions
            {
                Seek = (offset, origin) =>
                {
                    expectedOrigin = origin;
                    expectedOffset = offset;
                    return offset;
                }
            });

            actionStream.Position = 10;

            Assert.Equal(10, expectedOffset);
            Assert.Equal(SeekOrigin.Begin, expectedOrigin);
        }
        [Fact]
        public void Get_Position()
        {
            long expectedOffset = -1;
            SeekOrigin expectedOrigin = SeekOrigin.End;
            ActionStream actionStream = new ActionStream(new StreamActions
            {
                Seek = (offset, origin) =>
                {
                    expectedOrigin = origin;
                    expectedOffset = offset;
                    return offset+10;
                }
            });

            long currentPosition = actionStream.Position;

            Assert.Equal(0, expectedOffset);
            Assert.Equal(10, currentPosition);
            Assert.Equal(SeekOrigin.Current, expectedOrigin);
        }
    }
}
