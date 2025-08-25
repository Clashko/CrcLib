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
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return Task.Run(() =>
            {
                using (var memoryStream = new MemoryStream(data))
                {
                    return CrcCalculator.ComputeCorporateCrc32(memoryStream);
                }
            });
        }

        /// <inheritdoc />
        public async Task<string> ComputeCrcHexAsync(byte[] data)
        {
            uint crc = await ComputeCrcAsync(data);
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
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("Stream must be readable.", nameof(stream));

            return Task.Run(() => CrcCalculator.ComputeCorporateCrc32(stream));
        }

        /// <inheritdoc />
        public async Task<string> ComputeCrcHexAsync(Stream stream)
        {
            uint crc = await ComputeCrcAsync(stream);
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
