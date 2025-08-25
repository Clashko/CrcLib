# CrcLib

A simple .NET 9 library for computing and verifying a corporate-standard CRC-32 checksum.

This library provides two main services:
*   `ICrcService`: Works directly with file paths. Use this when you want the library to handle file I/O.
*   `IMemoryCrcService`: Works with in-memory data (`byte[]` or `Stream`). Use this when you have already read a file into memory or have data from another source.

## Installation

You can add this library to your project as a project reference. If it were published as a NuGet package, you could install it via the NuGet Package Manager:

```
Install-Package CrcLib
```

## Usage

First, ensure you have the correct `using` statements:

```csharp
using CrcLib.Extensions; // For DI extension methods
using CrcLib.Interfaces; // For the service interfaces
using CrcLib.Services;   // For direct instantiation
```

### File-Based Service (ICrcService)

#### 1. With Dependency Injection

**Step 1: Register the service in `Program.cs`**
```csharp
builder.Services.AddCrcService();
```

**Step 2: Inject and use the service**
```csharp
public class FileIntegrityChecker
{
    private readonly ICrcService _crcService;

    public FileIntegrityChecker(ICrcService crcService)
    {
        _crcService = crcService;
    }

    public async Task CheckFile(string filePath)
    {
        // Get the CRC as a uint
        uint crcUint = await _crcService.ComputeCrcAsync(filePath);
        Console.WriteLine($"File CRC (uint): {crcUint}");

        // Get the CRC as a hex string
        string crcHex = await _crcService.ComputeCrcHexAsync(filePath);
        Console.WriteLine($"File CRC (hex): {crcHex}");

        // Verify the file against the calculated CRC
        bool isUintValid = await _crcService.VerifyCrcAsync(filePath, crcUint);
        bool isHexValid = await _crcService.VerifyCrcAsync(filePath, crcHex);
        Console.WriteLine($"Verification successful (uint): {isUintValid}");
        Console.WriteLine($"Verification successful (hex): {isHexValid}");
    }
}
```

#### 2. Direct Instantiation

```csharp
var crcService = new CrcService();
var filePath = "path/to/your/file.txt";

string crcHex = await crcService.ComputeCrcHexAsync(filePath);
Console.WriteLine($"CRC-32 Checksum: {crcHex}");
```

---

### In-Memory Service (IMemoryCrcService)

#### 1. With Dependency Injection

**Step 1: Register the service in `Program.cs`**
```csharp
builder.Services.AddMemoryCrcService();
```

**Step 2: Inject and use the service**
```csharp
public class DataProcessor
{
    private readonly IMemoryCrcService _memoryCrcService;

    public DataProcessor(IMemoryCrcService memoryCrcService)
    {
        _memoryCrcService = memoryCrcService;
    }

    public async Task<string> ProcessData(byte[] data)
    {
        // Get the CRC as a hex string directly
        string crcHex = await _memoryCrcService.ComputeCrcHexAsync(data);
        return crcHex;
    }
}
```

#### 2. Direct Instantiation

```csharp
var memoryCrcService = new MemoryCrcService();
byte[] data = System.Text.Encoding.UTF8.GetBytes("This is a test string.");

// Get the CRC as a uint
uint crc = await memoryCrcService.ComputeCrcAsync(data);
Console.WriteLine($"CRC-32 for byte array: {crc}");

// Verify it
bool isValid = await memoryCrcService.VerifyCrcAsync(data, crc);
Console.WriteLine($"Verification successful: {isValid}");
```
