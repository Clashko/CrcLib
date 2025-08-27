using CrcLib.Core;
using CrcLib.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;

namespace CrcLib.Services
{
    /// <summary>
    /// Provides functionality to compute and verify CRC-32 checksums for files.
    /// </summary>
    public class CrcService : ICrcService
    {
        private readonly ILogger<CrcService> _logger;

        public CrcService(ILogger<CrcService>? logger = null)
        {
            _logger = logger ?? NullLogger<CrcService>.Instance;
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(string filePath)
        {
            return ComputeCrcAsync(filePath, null);
        }

        /// <inheritdoc />
        public Task<uint> ComputeCrcAsync(string filePath, IProgress<double>? progress)
        {
            _logger.LogInformation("Starting CRC computation for file: {FilePath}", filePath);

            if (string.IsNullOrEmpty(filePath))
            {
                _logger.LogError("File path is null or empty.");
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                _logger.LogError("File not found at: {FilePath}", filePath);
                throw new FileNotFoundException("File not found.", filePath);
            }

            return Task.Run(() =>
            {
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false))
                    {
                        var crc = CrcCalculator.ComputeCorporateCrc32(fileStream, progress);
                        _logger.LogInformation("Successfully computed CRC for file {FilePath}. CRC: {Crc}", filePath, crc);
                        return crc;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during CRC computation for file: {FilePath}", filePath);
                    throw;
                }
            });
        }

        /// <inheritdoc />
        public Task<string> ComputeCrcHexAsync(string filePath)
        {
            return ComputeCrcHexAsync(filePath, null);
        }

        /// <inheritdoc />
        public async Task<string> ComputeCrcHexAsync(string filePath, IProgress<double>? progress)
        {
            _logger.LogInformation("Starting CRC computation for file (hex output): {FilePath}", filePath);
            uint crc = await ComputeCrcAsync(filePath, progress);
            var hexCrc = crc.ToString("x8");
            _logger.LogInformation("Successfully computed CRC for file {FilePath}. CRC (hex): {CrcHex}", filePath, hexCrc);
            return hexCrc;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(string filePath, uint expectedCrc)
        {
            _logger.LogInformation("Starting verification for file {FilePath} against expected CRC: {ExpectedCrc}", filePath, expectedCrc);
            var computedCrc = await ComputeCrcAsync(filePath);
            bool isValid = computedCrc == expectedCrc;
            _logger.LogInformation("Verification for file {FilePath} completed. Computed: {ComputedCrc}, Expected: {ExpectedCrc}, Valid: {IsValid}", filePath, computedCrc, expectedCrc, isValid);
            return isValid;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyCrcAsync(string filePath, string expectedCrcHex)
        {
            _logger.LogInformation("Starting verification for file {FilePath} against expected hex CRC: {ExpectedCrcHex}", filePath, expectedCrcHex);
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

            var computedCrc = await ComputeCrcAsync(filePath);
            bool isValid = computedCrc == expectedCrc;
            _logger.LogInformation("Verification for file {FilePath} completed. Computed: {ComputedCrc}, Expected: {ExpectedCrc}, Valid: {IsValid}", filePath, computedCrc, expectedCrc, isValid);
            return isValid;
        }
    }
}
