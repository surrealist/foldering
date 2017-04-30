using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace foldering
{
  internal class CommandLineOptions
  {
    public string Source { get; set; }
    public string Dest { get; set; }

    public int StartIndex { get; set; }
    public int Length { get; set; }

    public bool Test { get; set; }

    public static CommandLineOptions Parse(string[] args)
    {
      var cmd = new CommandLineApplication(false)
      {
        Name = "foldering",
        FullName = "foldering - move or copy files from a source to multiple target folders based on file name",
        Out = Console.Out,
        Error = Console.Error,
        AllowArgumentSeparator = true
      };

      var source = cmd.Argument("Source", "Source folder that contain the input files (default is current folder)");
      var dest = cmd.Argument("Dest", "Destination folder to put files into several sub-folders (default is current folder)");

      var oS = cmd.Option("-s|--startIndex", "Start index of input file name (default is 0)", CommandOptionType.SingleValue);
      var oL = cmd.Option("-l|--length", "Length of input file name (default is 7)", CommandOptionType.SingleValue);

      var test = cmd.Option("-t|--test", "Enable test mode. Just preview result without moving or copying", CommandOptionType.NoValue);

      var help = cmd.Option("-h|--help", "Display this help text", CommandOptionType.NoValue);

      cmd.Execute(args);

      int startIndex, length;

      if (!oS.HasValue())
      {
        startIndex = 0;
      }
      else
      {
        startIndex = int.Parse(oS.Value());
      }

      if (!oL.HasValue())
      {
        length = 7;
      }
      else
      {
        length = int.Parse(oL.Value());
      }

      if (string.IsNullOrEmpty(source.Value)
        || string.IsNullOrEmpty(dest.Value)
        || help.HasValue())
      {
        cmd.ShowHelp();
        return null;
      }

      cmd.ShowRootCommandFullNameAndVersion();

      return new CommandLineOptions
      {
        Source = source.Value,
        Dest = dest.Value,
        StartIndex = startIndex,
        Length = length,
        Test = test.HasValue()
      };
    }
  }
}