using System.IO;

namespace ExtendedStream
{
    public class ThreadSafeStream : Stream
    {
        private readonly Stream _baseStream;

        public ThreadSafeStream(Stream baseStream)
        {
            _baseStream = baseStream;
        }

        #region Overrides of Stream

        public override void Flush()
        {
            lock (this)
                _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (this)
                return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (this)
                return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            lock (this)
                _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (this)
                _baseStream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get
            {
                lock (this)
                    return _baseStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                lock (this)
                    return _baseStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                lock (this)
                    return _baseStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                lock (this)
                    return _baseStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                lock (this)
                    return _baseStream.Position;
            }
            set
            {
                lock (this)
                    _baseStream.Position = value;
            }
        }

        #endregion
    }
}
