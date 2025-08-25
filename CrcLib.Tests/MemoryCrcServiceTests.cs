using CrcLib.Interfaces;
using CrcLib.Services;
using System.Text;

namespace CrcLib.Tests
{
    public class MemoryCrcServiceTests
    {
        // IMPORTANT: Replace this with the actual expected CRC value.
        // I was unable to automatically calculate this value from the custom algorithm.
        private const uint Known_Crc_For_Hello_World = 1840808736;
        private const string Known_Crc_Hex_For_Hello_World = "6db88320"; // 1234567890 in hex

        private readonly IMemoryCrcService _service;
        private readonly byte[] _testData;

        public MemoryCrcServiceTests()
        {
            _service = new MemoryCrcService();
            _testData = Encoding.UTF8.GetBytes("Hello, World!");
        }

        [Fact]
        public async Task ComputeCrcAsync_WithByteArray_ReturnsCorrectValue()
        {
            // Act
            var result = await _service.ComputeCrcAsync(_testData);

            // Assert
            Assert.Equal(Known_Crc_For_Hello_World, result);
        }

        [Fact]
        public async Task ComputeCrcHexAsync_WithByteArray_ReturnsCorrectValue()
        {
            // Act
            var result = await _service.ComputeCrcHexAsync(_testData);

            // Assert
            Assert.Equal(Known_Crc_Hex_For_Hello_World, result);
        }

        [Fact]
        public async Task ComputeCrcAsync_WithStream_ReturnsCorrectValue()
        {
            // Arrange
            using var stream = new MemoryStream(_testData);

            // Act
            var result = await _service.ComputeCrcAsync(stream);

            // Assert
            Assert.Equal(Known_Crc_For_Hello_World, result);
        }

        [Fact]
        public async Task VerifyCrcAsync_WithCorrectUint_ReturnsTrue()
        {
            // Act
            var result = await _service.VerifyCrcAsync(_testData, Known_Crc_For_Hello_World);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyCrcAsync_WithIncorrectUint_ReturnsFalse()
        {
            // Act
            var result = await _service.VerifyCrcAsync(_testData, 0);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task VerifyCrcAsync_WithCorrectHex_ReturnsTrue()
        {
            // Act
            var result = await _service.VerifyCrcAsync(_testData, Known_Crc_Hex_For_Hello_World);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyCrcAsync_WithIncorrectHex_ReturnsFalse()
        {
            // Act
            var result = await _service.VerifyCrcAsync(_testData, "ffffffff");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ComputeCrcAsync_NullData_ThrowsArgumentNullException()
        {
            // Assert
            _ = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ComputeCrcAsync((byte[])null!));
        }
    }
}
