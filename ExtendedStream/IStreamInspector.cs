using System.IO;

namespace ExtendedStream
{
    public interface IStreamInspector
    {
        void Flushing();
        void Flushed();
        void Seeking(long offset, SeekOrigin origin);
        void Sought(long offset, SeekOrigin origin, ref long result);
        void SettingLength(long value);
        void SetLength(long value);
        void Reading(byte[] buffer, int offset, int count);
        void Read(byte[] buffer, int offset, int count, ref int result);
        void Writing(byte[] buffer, int offset, int count);
        void Written(byte[] buffer, int offset, int count);
        void Closing();
        void Closed();
    }
}