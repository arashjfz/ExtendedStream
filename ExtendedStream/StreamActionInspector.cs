using System;
using System.IO;

namespace ExtendedStream
{
    public class StreamActionInspector : IStreamInspector
    {
        public Action Closing { get; set; }
        public Action Closed { get; set; }
        public Action Flushing { get; set; }
        public Action Flushed { get; set; }
        public Seeking Seeking { get; set; }
        public Sought Sought { get; set; }
        public SetLength SetLength { get; set; }
        public SetLength SettingLength { get; set; }
        public Reading Reading { get; set; }
        public Read Read { get; set; }
        public Write Writing { get; set; }
        public Write Written { get; set; }
        
        #region Implementation of IStreamInspector

        void IStreamInspector.Flushing()
        {
            Flushing?.Invoke();
        }

        void IStreamInspector.Flushed()
        {
            Flushed?.Invoke();
        }

        void IStreamInspector.Seeking(long offset, SeekOrigin origin)
        {
            Seeking?.Invoke(offset, origin);
        }

        void IStreamInspector.Sought(long offset, SeekOrigin origin, ref long result)
        {
            Sought?.Invoke(offset, origin, ref result);
        }

        void IStreamInspector.SettingLength(long value)
        {
            SettingLength?.Invoke(value);
        }

        void IStreamInspector.SetLength(long value)
        {
            SetLength?.Invoke(value);
        }

        void IStreamInspector.Reading(byte[] buffer, int offset, int count)
        {
            Reading?.Invoke(buffer, offset, count);
        }

        void IStreamInspector.Read(byte[] buffer, int offset, int count, ref int result)
        {
            Read?.Invoke(buffer, offset, count, ref result);
        }

        void IStreamInspector.Writing(byte[] buffer, int offset, int count)
        {
            Writing?.Invoke(buffer, offset, count);
        }

        void IStreamInspector.Written(byte[] buffer, int offset, int count)
        {
            Written?.Invoke(buffer, offset, count);
        }

        void IStreamInspector.Closing()
        {
            Closing?.Invoke();
        }

        void IStreamInspector.Closed()
        {
            Closed?.Invoke();
        }

        #endregion
    }
}