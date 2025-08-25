using CrcLib.Core;
using CrcLib.Interfaces;
using System.Globalization;

namespace CrcLib.Services
{
    /// <summary>
    /// Provides functionality to compute and verify CRC-32 checksums for files.
    /// </summary>
    public class CrcService : ICrcService
    {
        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            return Task.Run(() =>
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false))
                {
                    return CrcCalculator.ComputeCorporateCrc32(fileStream);
                }
            });
        }

        /// <inheritdoc />
        public async Task<string> ComputeCrcHexAsync(string filePath)
        {
            uint crc = await ComputeCrcAsync(filePath);
            return crc.ToString("x8");
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(string filePath, uint expectedCrc)
        {
            var computedCrc = await ComputeCrcAsync(filePath);
            return computedCrc == expectedCrc;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(string filePath, string expectedCrcHex)
        {
            if (string.IsNullOrEmpty(expectedCrcHex))
                throw new ArgumentNullException(nameof(expectedCrcHex));

            if (!uint.TryParse(expectedCrcHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint expectedCrc))
            {
                throw new ArgumentException("Invalid hex string format.", nameof(expectedCrcHex));
            }

            var computedCrc = await ComputeCrcAsync(filePath);
            return computedCrc == expectedCrc;
        }
    }
}
