using System.Diagnostics;

internal class Program
{
    private static async Task Main(string[] args)
    {
        List<string> filenames =
            Directory.EnumerateFiles(Directory.GetCurrentDirectory())
            .Select(s => Path.GetFileName(s))
            .ToList();
        var tmp = $".{Guid.NewGuid()}.tmp";

        File.AppendAllLines(tmp, filenames);

        using var process = new Process
        {
            StartInfo =
                {
                    FileName = "pwsh.exe",
                    Arguments = $"-c hx {tmp}",
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    WindowStyle = ProcessWindowStyle.Maximized,
                }
        };

        Console.WriteLine($"Starting! Found {filenames.Count} files to rename.");
        
        var passed = false;
        List<string> newFilenames = new();
        while (!passed)
        {
            process.Start();
            await process.WaitForExitAsync();
            newFilenames = (await File.ReadAllLinesAsync(tmp)).ToList();
            passed = true;
            if (filenames.Count != newFilenames.Count)
            {
                Console.WriteLine("There's a different number of lines than before, I can't decide which lines to take into account. Try again. ");
                File.Delete(tmp);
                File.AppendAllLines(tmp, filenames);
                passed = false;
                continue;
            }
            bool illegal = false;
            foreach (var filename in newFilenames)
            {
                if (string.IsNullOrEmpty(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    Console.WriteLine($"New filename {filename} contains illegal filename characters. Try again.");
                    illegal = true;
                }
                
            }
            if (illegal)
            {
                File.Delete(tmp);
                File.AppendAllLines(tmp, filenames);
                passed = false;
                continue;
            }

        }
        var maxLength = CalculateMaxLength(filenames);
        for (int i = 0; i < filenames.Count; i++)
        {
            File.Move(filenames[i], newFilenames[i]);
            Console.WriteLine($"File {Pad(filenames[i], maxLength)} is now {newFilenames[i]}");
        }
        File.Delete(tmp);
        Console.WriteLine($"Exited!");
    }

    private static string Pad(string s, int length)
    {
        return s.PadRight(length);
    }

    private static int CalculateMaxLength(List<string> strings)
    {
        return strings.Select(s => s.Length).Max();
    }
}