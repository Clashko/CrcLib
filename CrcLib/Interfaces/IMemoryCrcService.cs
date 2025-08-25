namespace CrcLib.Interfaces
{
    /// <summary>
    /// Defines the contract for a CRC service that operates on in-memory data.
    /// </summary>
    public interface IMemoryCrcService
    {
        /// <summary>
        /// Computes the CRC-32 checksum for a byte array.
        /// </summary>
        /// <param name="data">The byte array to compute the checksum for.</param>
        /// <returns>The CRC-32 checksum as a uint.</returns>
        Task<uint> ComputeCrcAsync(byte[] data);

        /// <summary>
        /// Computes the CRC-32 checksum for a byte array.
        /// </summary>
        /// <param name="data">The byte array to compute the checksum for.</param>
        /// <returns>The CRC-32 checksum as a lowercase hex string (e.g., "0123abcd").</returns>
        Task<string> ComputeCrcHexAsync(byte[] data);

        /// <summary>
        /// Verifies the CRC-32 checksum of a byte array against an expected value.
        /// </summary>
        /// <param name="data">The byte array to verify.</param>
        /// <param name="expectedCrc">The expected CRC-32 checksum as a uint.</param>
        /// <returns>True if the computed CRC matches the expected CRC; otherwise, false.</returns>
        Task<bool> VerifyCrcAsync(byte[] data, uint expectedCrc);

        /// <summary>
        /// Verifies the CRC-32 checksum of a byte array against an expected hex string value.
        /// </summary>
        /// <param name="data">The byte array to verify.</param>
        /// <param name="expectedCrcHex">The expected CRC-32 checksum as a lowercase hex string.</param>
        /// <returns>True if the computed CRC matches the expected CRC; otherwise, false.</returns>
        Task<bool> VerifyCrcAsync(byte[] data, string expectedCrcHex);

        /// <summary>
        /// Computes the CRC-32 checksum for a stream.
        /// </summary>
        /// <param name="stream">The stream to compute the checksum for.</param>
        /// <returns>The CRC-32 checksum as a uint.</returns>
        Task<uint> ComputeCrcAsync(Stream stream);

        /// <summary>
        /// Computes the CRC-32 checksum for a stream.
        /// </summary>
        /// <param name="stream">The stream to compute the checksum for.</param>
        /// <returns>The CRC-32 checksum as a lowercase hex string (e.g., "0123abcd").</returns>
        Task<string> ComputeCrcHexAsync(Stream stream);

        /// <summary>
        /// Verifies the CRC-32 checksum of a stream against an expected value.
        /// </summary>
        /// <param name="stream">The stream to verify.</param>
        /// <param name="expectedCrc">The expected CRC-32 checksum as a uint.</param>
        /// <returns>True if the computed CRC matches the expected CRC; otherwise, false.</returns>
        Task<bool> VerifyCrcAsync(Stream stream, uint expectedCrc);

        /// <summary>
        /// Verifies the CRC-32 checksum of a stream against an expected hex string value.
        /// </summary>
        /// <param name="stream">The stream to verify.</param>
        /// <param name="expectedCrcHex">The expected CRC-32 checksum as a lowercase hex string.</param>
        /// <returns>True if the computed CRC matches the expected CRC; otherwise, false.</returns>
        Task<bool> VerifyCrcAsync(Stream stream, string expectedCrcHex);
    }
}
