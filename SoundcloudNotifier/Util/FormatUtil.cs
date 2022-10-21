namespace SoundcloudNotifier.Util; 

public static class FormatUtil {
  
    public static string Truncate(string text,int maxLength) {
      if (text.Length <= maxLength) return text;
      return (text.Substring(0, maxLength)) + "...";
    }

    public static string[] DiscordLineToFormattedMultiLine(string lineStr, string? colorSymbol) {
      if (colorSymbol is null)
        colorSymbol = "";
      else
        colorSymbol += "\n";

      int maxLength = 2000;
      var startIndex = 0;
      var lines = lineStr.Split(new string[] { "\r\n" }, StringSplitOptions.None);
      int maxIndex = lines.Length - 1;
      var newGeneratedString = "";
      var listOfLines = new List<string>();
      for (int i = startIndex; i < maxIndex; ++i) {
        var line = lines[i];
        if (newGeneratedString.Length + line.Length + 11 >= maxLength) {
          listOfLines.Add($"```{colorSymbol}{newGeneratedString}```");
          newGeneratedString = "";
        } else {
          newGeneratedString += line + "\n";
        }
      }

      if (newGeneratedString.Length > 0) {
        listOfLines.Add($"```{colorSymbol}\n{newGeneratedString}```");
      }

      if (listOfLines.Count > 0) {
        return listOfLines.ToArray();
      }

      return new string[] { };
    }
}