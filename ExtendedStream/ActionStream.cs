using System;
using System.IO;

namespace ExtendedStream
{
    public class ActionStream : Stream
    {
        private readonly StreamActions _streamActions;
        public ActionStream(StreamActions streamActions)
        {
            _streamActions = streamActions;
        }

        #region Overrides of Stream

        public override void Flush()
        {
            if (_streamActions.Flush == null)
                throw new NotSupportedException("Flushing is not supported for this stream");
            _streamActions.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (_streamActions.Seek == null)
                throw new NotSupportedException("Seeking is not supported for this stream");
            return _streamActions.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            if (_streamActions.SetLength == null)
                throw new NotSupportedException("SetLength is not supported for this stream");
            _streamActions.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_streamActions.Read == null)
                throw new NotSupportedException("Read is not supported for this stream");
            return _streamActions.Read(buffer, offset, count); ;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_streamActions.Write == null)
                throw new NotSupportedException("Write is not supported for this stream");
            _streamActions.Write(buffer, offset, count);
        }

        public override bool CanRead => _streamActions.Read != null;

        public override bool CanSeek => _streamActions.Seek != null;

        public override bool CanWrite => _streamActions.Write != null;

        public override long Length
        {
            get
            {
                if (_streamActions.GetLength == null)
                    throw new NotSupportedException("Get Length is not supported for this stream");
                return _streamActions.GetLength();
            }
        }

        public override long Position
        {
            get => Seek(0,SeekOrigin.Current);
            set => Seek(value, SeekOrigin.Begin);
        }

        #endregion
    }

    public class StreamActions
    {
        public StreamActions()
        {
            Flush = () => { };
        }

        public Func<byte[], int, int, int> Read { get; set; }
        public Action<byte[], int, int> Write { get; set; }
        public Func<long> GetLength { get; set; }
        public Action<long> SetLength { get; set; }
        public Func<long, SeekOrigin, long> Seek { get; set; }
        public Action Flush { get; set; }
    }
}
