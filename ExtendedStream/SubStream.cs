using System;
using System.IO;

namespace ExtendedStream
{
    public class SubStream : Stream
    {
        #region Fields
        private readonly Stream _baseStream;
        private readonly long _offset;
        private readonly long _length; 
        #endregion

        #region Constructors
        public SubStream(Stream baseStream, long offset, bool seekToOffset = true)
        {
            _baseStream = baseStream;
            _offset = offset;
            _length = -1;
            try
            {
                _length = _baseStream.Length - offset;
            }
            catch (Exception)
            {
                // ignored
            }
            if (seekToOffset)
                baseStream.Seek(offset, SeekOrigin.Begin);

        }
        public SubStream(Stream baseStream, long offset, long length, bool seekToOffset = true)
        {
            if (!baseStream.CanRead)
                throw new Exception("Source Must be readable.");
            _baseStream = baseStream;
            _offset = offset;
            _length = length;
            if (seekToOffset)
                baseStream.Seek(offset, SeekOrigin.Begin);
        } 
        #endregion

        #region Overrides of Stream

        public override void Flush()
        {
            lock (this)
            {
                _baseStream.Flush(); 
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (this)
            {
                if (!_baseStream.CanSeek)
                    throw new StreamIsNotSeekableException();
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        return _baseStream.Seek(offset + _offset, SeekOrigin.Begin) - _offset;
                    case SeekOrigin.Current:
                        return _baseStream.Seek(offset, SeekOrigin.Current) - _offset;
                    case SeekOrigin.End:
                        if (_length == -1)
                            return _baseStream.Seek(offset , SeekOrigin.End) - _offset;
                        return _baseStream.Seek(_offset + _length- offset, SeekOrigin.Begin) - _offset;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(origin));
                }
            }
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (this)
            {
                if (_length == -1)
                    return _baseStream.Read(buffer, offset, count);
                if (count > _length - Position)
                    count = (int)(_length - Position);
                return _baseStream.Read(buffer, offset, count); 
            }
            
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => true;

        public override bool CanSeek
        {
            get
            {
                lock (this)
                {
                    return _baseStream.CanSeek; 
                }
            }
        }

        public override bool CanWrite => false;

        public override long Length
        {
            get
            {
                lock (this)
                {
                    return _length == -1 ? _baseStream.Length - _offset : _length; 
                }
            }
        }

        public override long Position
        {
            get
            {
                if(_length==-1)
                    throw new Exception("Position Is Not Supported in unlimited  Stream Length.");
                lock (this)
                {
                    return _baseStream.Position-_offset;
                }
            }
            set => Seek(value, SeekOrigin.Begin);
        }
        public override void Close()
        {
            base.Close();
            lock (this)
            {
                _baseStream.Close();
            }
        }
        #endregion
    }
}
