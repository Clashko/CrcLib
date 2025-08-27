using CrcLib.Extensions;
using CrcLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

// Setup Dependency Injection and Logging
var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Information);
    })
    .AddCrcService() // From CrcLib.Extensions
    .AddMemoryCrcService() // From CrcLib.Extensions
    .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("CrcLib Demo Application Starting...");

Console.WriteLine("CrcLib Demo Application");
Console.WriteLine("-----------------------");

// --- Part 1: In-Memory CRC Demo ---
Console.WriteLine("\n1. In-Memory CRC Calculation");

var memoryService = serviceProvider.GetRequiredService<IMemoryCrcService>();
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
    logger.LogError(ex, "An error occurred during the in-memory demo.");
    Console.WriteLine($"An error occurred during in-memory demo: {ex.Message}");
}


// --- Part 2: File CRC Demo ---
Console.WriteLine("\n2. File CRC Calculation");

string filePath;
if (args.Length == 0)
{
    Console.WriteLine("   No file path provided, using default 'inputFile.txt'.");
    filePath = "CrcLib.Demo/inputFile.txt";
}
else
{
    filePath = args[0];
}

if (!File.Exists(filePath))
{
    logger.LogError("Input file not found: {FilePath}", filePath);
    Console.WriteLine($"   Error: File not found at '{filePath}'");
    return;
}

var fileService = serviceProvider.GetRequiredService<ICrcService>();

try
{
    Console.WriteLine($"   Calculating CRC for file: {filePath}");

    // --- Calculation with Progress Reporting ---
    Console.WriteLine("\n   --- With Progress Reporting ---");
    var progress = new Progress<double>(p =>
    {
        Console.Write($"\r   Progress: {p:F2}%   ");
    });

    string fileCrcHexWithProgress = await fileService.ComputeCrcHexAsync(filePath, progress);
    Console.WriteLine("\n   CRC (hex):   {0}", fileCrcHexWithProgress);

    // --- Standard Calculation (for comparison) ---
    Console.WriteLine("\n   --- Without Progress Reporting ---");
    string fileCrcHex = await fileService.ComputeCrcHexAsync(filePath);
    Console.WriteLine($"   CRC (hex):   {fileCrcHex}");

    // Verification Demo
    bool isFileValid = await fileService.VerifyCrcAsync(filePath, fileCrcHexWithProgress);
    Console.WriteLine($"\n   Verification check: {isFileValid}");
}

catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during the file demo for file: {FilePath}", filePath);
    Console.WriteLine($"   An error occurred during file demo: {ex.Message}");
}

logger.LogInformation("CrcLib Demo Application Finished.");
