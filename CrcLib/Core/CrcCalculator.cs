namespace CrcLib.Core
{
    /// <summary>
    /// Provides the corporate standard CRC-32 calculation logic.
    /// </summary>
    internal static class CrcCalculator
    {
        public static uint ComputeCorporateCrc32(Stream stream)
        {
            return ComputeCorporateCrc32(stream, null);
        }

        public static uint ComputeCorporateCrc32(Stream stream, IProgress<double>? progress)
        {
            long crc32 = 0;

            // This implementation is based on the user-provided corporate standard.
            var l = stream.Seek(0, SeekOrigin.End);
            stream.Seek(0, SeekOrigin.Begin);
            const long d = (100000 * 3 * (2 ^ 20));

            for (long s = 0; s < l; s += d)
            {
                if (l <= d)
                {
                    var r1 = new byte[l];
                    _ = stream.Read(r1, 0, (int)l);
                    ComputeCrc32(out crc32, crc32, r1, r1.Length, 3);
                }
                else if ((s == 0) && (l > d))
                {
                    var r1 = new byte[d];
                    _ = stream.Read(r1, 0, (int)d);
                    ComputeCrc32(out crc32, crc32, r1, r1.Length, 1);
                    Array.Clear(r1, 0, r1.Length);
                }
                else if ((l - d) > s)
                {
                    var r1 = new byte[d];
                    _ = stream.Read(r1, 0, (int)d);
                    ComputeCrc32(out crc32, crc32, r1, r1.Length, 0);
                    Array.Clear(r1, 0, r1.Length);
                }
                else
                {
                    var r1 = new byte[l - s];
                    _ = stream.Read(r1, 0, (int)(l - s));
                    ComputeCrc32(out crc32, crc32, r1, r1.Length, 2);
                    Array.Clear(r1, 0, r1.Length);
                }
                progress?.Report((double)s / l * 100);
            }
            progress?.Report(100.0);
            stream.Seek(0, SeekOrigin.Begin);
            return (uint)crc32;
        }

        private static void ComputeCrc32(out long crc32, long incrc32, byte[] mem, int ln, int fl)
        {
            crc32 = incrc32;
            var i = 0;

            var fcs = (fl & 1) != 0 ? -1 : crc32;

            for (i = 0; i < ln; i++)
            {
                var iii = mem[i];
                int ii;
                for (ii = 0; ii < 8; ii++)
                {
                    if ((fcs & 1) != 0)
                    {
                        fcs >>= 1;
                        if ((iii & 1) != 0)
                            fcs |= 0x80000000;
                        else
                            fcs &= 0x7FFFFFFF;
                        fcs ^= 0xEDB88320;
                    }
                    else
                    {
                        fcs >>= 1;
                        if ((iii & 1) != 0)
                            fcs |= 0x80000000;
                        else
                            fcs &= 0x7FFFFFFF;
                    }
                    iii >>= 1;
                }
            }

            if ((fl & 2) != 0)
            {
                for (i = 0; i < 32; i++)
                {
                    if ((fcs & 1) != 0)
                    {
                        fcs >>= 1;
                        fcs |= 0x7FFFFFFF;
                        fcs ^= 0xEDB88320;
                    }
                    else
                    {
                        fcs >>= 1;
                        fcs &= 0x7FFFFFFF;
                    }
                }
                fcs ^= -1;
            }

            crc32 = fcs;
        }
    }
}