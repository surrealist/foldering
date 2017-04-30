using System;
using System.Collections.Generic;
using System.IO;

namespace foldering
{
  internal class Program
  {
    static void Main(string[] args)
    {
      new Program().Run(args);
    }

    public void Run(string[] args)
    {
      var o = CommandLineOptions.Parse(args);
      if (o == null) return;

      ProcessFiles(o.Source, o.Dest, o.StartIndex, o.Length, o.Test);
    }

    private static void ProcessFiles(string source, string dest,
      int startIndex, int length, bool isTest)
    {
      var items = new SortedDictionary<string, List<string>>();
      int sum = 0;
      int count = 0;
      int skip = 0;
      int expectedLength = startIndex + length;

      if (isTest)
      {
        Console.WriteLine("// TEST MODE //");
        Console.WriteLine();
      }

      Console.WriteLine($"Source:\r\n {source.ToUpper()}");
      foreach (var f in Directory.EnumerateFiles(source))
      {
        string key = "";
        if (f.Length >= expectedLength)
        {
          key = Path.GetFileName(f).Substring(startIndex, length);
        } 

        if (!items.ContainsKey(key))
        {
          items.Add(key, new List<string>());
        }
        items[key].Add(f);
        count++;
      }
      Console.WriteLine($" Found {count:n0} files");
      Console.WriteLine();

      Console.WriteLine($"Destination:\r\n {dest.ToUpper()}");
      foreach (var item in items)
      {
        Console.Write($@" [{item.Key}] ");
        var destFolder = Path.Combine(dest, item.Key);
        if (!Directory.Exists(destFolder))
        {
          if (!isTest)
          {
            Directory.CreateDirectory(destFolder);
          }
          Console.Write("+ ");
        }
        else
        {
          Console.Write("  ");
        }

        count = 0;
        skip = 0;
        int total = item.Value.Count;
        int left = Console.CursorLeft;
        foreach (var file in item.Value)
        {
          var destFile = Path.Combine(destFolder, Path.GetFileName(file));

          if (file == destFile)
          {
            skip++;
            continue;
          }

          try
          {
            if (!isTest)
            {
              File.Move(file, destFile);
            }
            Console.SetCursorPosition(left, Console.CursorTop);
            count++; sum++;
            Console.Write($"{count:n0} of {total:n0} files");
          }
          catch (Exception)
          {
            //
          }
        }
        Console.SetCursorPosition(left, Console.CursorTop);
        Console.WriteLine($"{count:n0} files moved {(skip > 0 ? $"({skip:n0} skipped)" : "")}");
      }

      Console.WriteLine();
      Console.WriteLine($"Total {sum:n0} files moved");
    } 

  }
}