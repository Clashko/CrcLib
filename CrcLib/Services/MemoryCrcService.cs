using CrcLib.Core;
using CrcLib.Interfaces;
using System.Globalization;

namespace CrcLib.Services
{
    /// <summary>
    /// Provides functionality to compute and verify CRC-32 checksums for in-memory data.
    /// </summary>
    public class MemoryCrcService : IMemoryCrcService
    {
        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(byte[] data)
        {
            return ComputeCrcAsync(data, null);
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(byte[] data, IProgress<double>? progress)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return Task.Run(() =>
            {
                using (var memoryStream = new MemoryStream(data))
                {
                    return CrcCalculator.ComputeCorporateCrc32(memoryStream, progress);
                }
            });
        }

        /// <inheritdoc />
        public Task<string> ComputeCrcHexAsync(byte[] data)
        {
            return ComputeCrcHexAsync(data, null);
        }

        /// <inheritdoc />
        public async Task<string> ComputeCrcHexAsync(byte[] data, IProgress<double> progress)
        {
            uint crc = await ComputeCrcAsync(data, progress);
            return crc.ToString("x8");
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(byte[] data, uint expectedCrc)
        {
            var computedCrc = await ComputeCrcAsync(data);
            return computedCrc == expectedCrc;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(byte[] data, string expectedCrcHex)
        {
            if (string.IsNullOrEmpty(expectedCrcHex))
                throw new ArgumentNullException(nameof(expectedCrcHex));

            if (!uint.TryParse(expectedCrcHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint expectedCrc))
            {
                throw new ArgumentException("Invalid hex string format.", nameof(expectedCrcHex));
            }

            var computedCrc = await ComputeCrcAsync(data);
            return computedCrc == expectedCrc;
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(Stream stream)
        {
            return ComputeCrcAsync(stream, null);
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(Stream stream, IProgress<double>? progress)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("Stream must be readable.", nameof(stream));

            return Task.Run(() => CrcCalculator.ComputeCorporateCrc32(stream, progress));
        }

        /// <inheritdoc />
        public Task<string> ComputeCrcHexAsync(Stream stream)
        {
            return ComputeCrcHexAsync(stream, null);
        }

        /// <inheritdoc />
        public async Task<string> ComputeCrcHexAsync(Stream stream, IProgress<double>? progress)
        {
            uint crc = await ComputeCrcAsync(stream, progress);
            return crc.ToString("x8");
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(Stream stream, uint expectedCrc)
        {
            var computedCrc = await ComputeCrcAsync(stream);
            return computedCrc == expectedCrc;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(Stream stream, string expectedCrcHex)
        {
            if (string.IsNullOrEmpty(expectedCrcHex))
                throw new ArgumentNullException(nameof(expectedCrcHex));

            if (!uint.TryParse(expectedCrcHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint expectedCrc))
            {
                throw new ArgumentException("Invalid hex string format.", nameof(expectedCrcHex));
            }

            var computedCrc = await ComputeCrcAsync(stream);
            return computedCrc == expectedCrc;
        }
    }
}