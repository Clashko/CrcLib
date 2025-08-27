namespace CrcLib.Interfaces
{
    /// <summary>
    /// Defines the contract for a CRC calculation and verification service.
    /// </summary>
    public interface ICrcService
    {
        /// <summary>
        /// Computes the CRC-32 checksum for a specified file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The CRC-32 checksum as a uint.</returns>
        Task<uint> ComputeCrcAsync(string filePath);

        /// <summary>
        /// Computes the CRC-32 checksum for a specified file with progress reporting.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="progress">The progress reporter.</param>
        /// <returns>The CRC-32 checksum as a uint.</returns>
        Task<uint> ComputeCrcAsync(string filePath, IProgress<double>? progress);

        /// <summary>
        /// Computes the CRC-32 checksum for a specified file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The CRC-32 checksum as a lowercase hex string (e.g., "0123abcd").</returns>
        Task<string> ComputeCrcHexAsync(string filePath);

        /// <summary>
        /// Computes the CRC-32 checksum for a specified file with progress reporting.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="progress">The progress reporter.</param>
        /// <returns>The CRC-32 checksum as a lowercase hex string (e.g., "0123abcd").</returns>
        Task<string> ComputeCrcHexAsync(string filePath, IProgress<double> progress);

        /// <summary>
        /// Verifies the CRC-32 checksum of a specified file against an expected value.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expectedCrc">The expected CRC-32 checksum as a uint.</param>
        /// <returns>True if the computed CRC matches the expected CRC; otherwise, false.</returns>
        Task<bool> VerifyCrcAsync(string filePath, uint expectedCrc);

        /// <summary>
        /// Verifies the CRC-32 checksum of a specified file against an expected hex string value.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expectedCrcHex">The expected CRC-32 checksum as a lowercase hex string.</param>
        /// <returns>True if the computed CRC matches the expected CRC; otherwise, false.</returns>
        Task<bool> VerifyCrcAsync(string filePath, string expectedCrcHex);
    }
}
