using CrcLib.Core;
using CrcLib.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;

namespace CrcLib.Services
{
    /// <summary>
    /// Provides functionality to compute and verify CRC-32 checksums for in-memory data.
    /// </summary>
    public class MemoryCrcService : IMemoryCrcService
    {
        private readonly ILogger<MemoryCrcService> _logger;

        public MemoryCrcService(ILogger<MemoryCrcService>? logger = null)
        {
            _logger = logger ?? NullLogger<MemoryCrcService>.Instance;
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(byte[] data)
        {
            return ComputeCrcAsync(data, null);
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(byte[] data, IProgress<double>? progress)
        {
            _logger.LogInformation("Starting CRC computation for byte array of length {Length}", data?.Length ?? 0);
            if (data == null)
            {
                _logger.LogError("Input byte array is null.");
                throw new ArgumentNullException(nameof(data));
            }

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
        public async Task<string> ComputeCrcHexAsync(byte[] data, IProgress<double>? progress)
        {
            _logger.LogInformation("Starting CRC computation for byte array (hex output) of length {Length}", data?.Length ?? 0);
            if (data == null)
            {
                _logger.LogError("Input byte array is null.");
                throw new ArgumentNullException(nameof(data));
            }
            uint crc = await ComputeCrcAsync(data, progress);
            var hexCrc = crc.ToString("x8");
            _logger.LogInformation("Successfully computed CRC for byte array. CRC (hex): {CrcHex}", hexCrc);
            return hexCrc;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(byte[] data, uint expectedCrc)
        {
            _logger.LogInformation("Starting verification for byte array against expected CRC: {ExpectedCrc}", expectedCrc);
            var computedCrc = await ComputeCrcAsync(data);
            bool isValid = computedCrc == expectedCrc;
            _logger.LogInformation("Verification for byte array completed. Computed: {ComputedCrc}, Expected: {ExpectedCrc}, Valid: {IsValid}", computedCrc, expectedCrc, isValid);
            return isValid;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(byte[] data, string expectedCrcHex)
        { 
            _logger.LogInformation("Starting verification for byte array against expected hex CRC: {ExpectedCrcHex}", expectedCrcHex);
            if (string.IsNullOrEmpty(expectedCrcHex))
            {
                _logger.LogError("Expected CRC hex string is null or empty.");
                throw new ArgumentNullException(nameof(expectedCrcHex));
            }

            if (!uint.TryParse(expectedCrcHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint expectedCrc))
            {
                _logger.LogError("Invalid hex string format for expected CRC: {ExpectedCrcHex}", expectedCrcHex);
                throw new ArgumentException("Invalid hex string format.", nameof(expectedCrcHex));
            }

            var computedCrc = await ComputeCrcAsync(data);
            bool isValid = computedCrc == expectedCrc;
            _logger.LogInformation("Verification for byte array completed. Computed: {ComputedCrc}, Expected: {ExpectedCrc}, Valid: {IsValid}", computedCrc, expectedCrc, isValid);
            return isValid;
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(Stream stream)
        {
            return ComputeCrcAsync(stream, null);
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(Stream stream, IProgress<double>? progress)
        {
            _logger.LogInformation("Starting CRC computation for stream.");
            if (stream == null)
            {
                _logger.LogError("Input stream is null.");
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                _logger.LogError("Input stream is not readable.");
                throw new ArgumentException("Stream must be readable.", nameof(stream));
            }

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
            _logger.LogInformation("Starting CRC computation for stream (hex output).");
            uint crc = await ComputeCrcAsync(stream, progress);
            var hexCrc = crc.ToString("x8");
            _logger.LogInformation("Successfully computed CRC for stream. CRC (hex): {CrcHex}", hexCrc);
            return hexCrc;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(Stream stream, uint expectedCrc)
        {
            _logger.LogInformation("Starting verification for stream against expected CRC: {ExpectedCrc}", expectedCrc);
            var computedCrc = await ComputeCrcAsync(stream);
            bool isValid = computedCrc == expectedCrc;
            _logger.LogInformation("Verification for stream completed. Computed: {ComputedCrc}, Expected: {ExpectedCrc}, Valid: {IsValid}", computedCrc, expectedCrc, isValid);
            return isValid;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(Stream stream, string expectedCrcHex)
        {
            _logger.LogInformation("Starting verification for stream against expected hex CRC: {ExpectedCrcHex}", expectedCrcHex);
            if (string.IsNullOrEmpty(expectedCrcHex))
            {
                _logger.LogError("Expected CRC hex string is null or empty.");
                throw new ArgumentNullException(nameof(expectedCrcHex));
            }

            if (!uint.TryParse(expectedCrcHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint expectedCrc))
            {
                _logger.LogError("Invalid hex string format for expected CRC: {ExpectedCrcHex}", expectedCrcHex);
                throw new ArgumentException("Invalid hex string format.", nameof(expectedCrcHex));
            }

            var computedCrc = await ComputeCrcAsync(stream);
            bool isValid = computedCrc == expectedCrc;
            _logger.LogInformation("Verification for stream completed. Computed: {ComputedCrc}, Expected: {ExpectedCrc}, Valid: {IsValid}", computedCrc, expectedCrc, isValid);
            return isValid;
        }
    }
}
