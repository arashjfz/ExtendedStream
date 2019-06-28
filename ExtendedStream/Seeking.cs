using System.IO;

namespace ExtendedStream
{
    public delegate void Seeking(long offset, SeekOrigin origin);
}