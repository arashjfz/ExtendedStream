using System.IO;

namespace ExtendedStream
{
    public delegate void Sought(long offset, SeekOrigin origin, ref long result);
}