using CrcLib.Interfaces;
using CrcLib.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace CrcLib.Tests
{
    public class CrcServiceTests : IDisposable
    {
        // IMPORTANT: This value must be the same as in MemoryCrcServiceTests.cs
        private const uint Known_Crc_For_Hello_World = 1840808736;
        private const string Known_Crc_Hex_For_Hello_World = "6db88320";

        private readonly ICrcService _service;
        private readonly string _tempFilePath;

        public CrcServiceTests()
        {
            _service = new CrcService(new NullLogger<CrcService>());
            
            // Create a temporary file with test data
            _tempFilePath = Path.GetTempFileName();
            File.WriteAllText(_tempFilePath, "Hello, World!");
        }

        public void Dispose()
        {
            // Clean up the temporary file
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        [Fact]
        public async Task ComputeCrcAsync_ReturnsCorrectValue()
        {
            // Act
            var result = await _service.ComputeCrcAsync(_tempFilePath);

            // Assert
            Assert.Equal(Known_Crc_For_Hello_World, result);
        }

        [Fact]
        public async Task ComputeCrcHexAsync_ReturnsCorrectValue()
        {
            // Act
            var result = await _service.ComputeCrcHexAsync(_tempFilePath);

            // Assert
            Assert.Equal(Known_Crc_Hex_For_Hello_World, result, ignoreCase: true);
        }

        [Fact]
        public async Task VerifyCrcAsync_WithCorrectUint_ReturnsTrue()
        {
            // Act
            var result = await _service.VerifyCrcAsync(_tempFilePath, Known_Crc_For_Hello_World);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyCrcAsync_WithCorrectHex_ReturnsTrue()
        {
            // Act
            var result = await _service.VerifyCrcAsync(_tempFilePath, Known_Crc_Hex_For_Hello_World);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ComputeCrcAsync_NonExistentFile_ThrowsFileNotFoundException()
        {
            // Arrange
            var nonExistentFile = "non_existent_file.tmp";

            // Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => _service.ComputeCrcAsync(nonExistentFile));
        }
    }
}
