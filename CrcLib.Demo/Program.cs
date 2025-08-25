using CrcLib.Services;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

// Note: For a real application, you would use Dependency Injection.
// For this simple demo, we are instantiating services directly.
// See the README.md for DI examples.

Console.WriteLine("CrcLib Demo Application");
Console.WriteLine("-----------------------");

// --- Part 1: In-Memory CRC Demo ---
Console.WriteLine("\n1. In-Memory CRC Calculation");

var memoryService = new MemoryCrcService();
string testString = "Hello, World!";
byte[] testData = Encoding.UTF8.GetBytes(testString);

try
{
    uint crcUint = await memoryService.ComputeCrcAsync(testData);
    string crcHex = await memoryService.ComputeCrcHexAsync(testData);

    Console.WriteLine($"   Test String: \"{testString}\"");
    Console.WriteLine($"   CRC (uint):  {crcUint}");
    Console.WriteLine($"   CRC (hex):   {crcHex}");

    // Verification Demo
    bool isUintValid = await memoryService.VerifyCrcAsync(testData, crcUint);
    bool isHexValid = await memoryService.VerifyCrcAsync(testData, crcHex);
    Console.WriteLine($"   Verification check (uint): {isUintValid}");
    Console.WriteLine($"   Verification check (hex):  {isHexValid}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during in-memory demo: {ex.Message}");
}


// --- Part 2: File CRC Demo ---
Console.WriteLine("\n2. File CRC Calculation");

if (args.Length == 0)
{
    Console.WriteLine("   No file path provided.");
    Console.WriteLine("   Usage: dotnet run --project CrcLib.Demo <path_to_your_file>");
    return;
}

string filePath = args[0];
if (!File.Exists(filePath))
{
    Console.WriteLine($"   Error: File not found at '{filePath}'");
    return;
}

var fileService = new CrcService();

try
{
    Console.WriteLine($"   Calculating CRC for file: {filePath}");

    // Perform calculation
    uint fileCrcUint = await fileService.ComputeCrcAsync(filePath);
    string fileCrcHex = await fileService.ComputeCrcHexAsync(filePath);

    Console.WriteLine($"   CRC (uint):  {fileCrcUint}");
    Console.WriteLine($"   CRC (hex):   {fileCrcHex}");

    // Verification Demo
    bool isFileValid = await fileService.VerifyCrcAsync(filePath, fileCrcUint);
    Console.WriteLine($"   Verification check: {isFileValid}");
}
catch (Exception ex)
{
    Console.WriteLine($"   An error occurred during file demo: {ex.Message}");
}