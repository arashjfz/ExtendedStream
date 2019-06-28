using System.IO;

namespace ExtendedStream
{
    public class StreamInspector : Stream
    {
        private readonly Stream _baseStream;
        private readonly IStreamInspector _streamInspector;

        public StreamInspector(Stream baseStream, IStreamInspector streamInspector)
        {
            _baseStream = baseStream;
            _streamInspector = streamInspector;
        }

        #region Overrides of Stream

        public override void Flush()
        {
            _streamInspector.Flushing();
            _baseStream.Flush();
            _streamInspector.Flushed();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            _streamInspector.Seeking(offset, origin);
            long result = _baseStream.Seek(offset, origin);
            _streamInspector.Sought(offset, origin, ref result);
            return result;
        }

        public override void SetLength(long value)
        {
            _streamInspector.SettingLength(value);
            _baseStream.SetLength(value);
            _streamInspector.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            _streamInspector.Reading(buffer, offset, count);
            int result = _baseStream.Read(buffer, offset, count);
            _streamInspector.Read(buffer, offset, count, ref result);
            return result;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _streamInspector.Writing(buffer, offset, count);
            _baseStream.Write(buffer, offset, count);
            _streamInspector.Written(buffer, offset, count);
        }

        public override bool CanRead => _baseStream.CanRead;
        public override bool CanSeek => _baseStream.CanSeek;
        public override bool CanWrite => _baseStream.CanWrite;
        public override long Length => _baseStream.Length;
        public override long Position
        {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        #endregion

        #region Overrides of Stream

        public override void Close()
        {
            Stream baseStream = _baseStream;
            _streamInspector.Closing();
            base.Close();
            baseStream.Close();
            _streamInspector.Closed();
        }

        #endregion
    }
}
