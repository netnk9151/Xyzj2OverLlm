﻿using System.Text;
using System.Text.RegularExpressions;

namespace Translate.Utility;

/// <summary>
/// Replace things we know cause issues with the LLM with straight tokens which it seems to handle ok. 
/// </summary>
public class StringTokenReplacer
{
    private static readonly Regex PlaceholderRegex = new(@"(\{[^{}]+\})", RegexOptions.Compiled);
    private static readonly Regex CoordinateRegex = new(@"\(-?\d+,-?\d+\)", RegexOptions.Compiled);
    private static readonly Regex NumericValueRegex = new(@"(?<![{<]|color=|<[^>]*)(?:[+-]?(?:\d+\.\d*|\.\d+|\d+))(?![}>])", RegexOptions.Compiled);
    private static readonly Regex ColorStartRegex = new(@"<color=[^>]+>", RegexOptions.Compiled);
    private static readonly Regex KeyPressRegex = new(@"<\w+\s+>", RegexOptions.Compiled);
    private static readonly Regex TokenRegex;
    private static readonly Regex EmojiRegex;

    public static string[] otherTokens = ["{}"];
    public static string[] EmojiItems = [
        "[发现宝箱]",
        "[石化]",
        "[开心]",
        "[不知所措]",
        "[疑问]",
        "[担忧]",
        "[生气]",
        "[哭泣]",
        "[惊讶]",
        "[发怒]",
        "[抓狂]",
        "[委屈]",
    ];

    private Dictionary<int, string> placeholderMap = new();
    private Dictionary<string, string> colorMap = new();


    // Use Static constructor to make sure the regexes are only compiled once (otherwise very slow)
    static StringTokenReplacer()
    {
        var tokenPattern = string.Join("|", otherTokens.Select(Regex.Escape));
        TokenRegex = new Regex(tokenPattern, RegexOptions.Compiled);

        var emojiPattern = string.Join("|", EmojiItems.Select(Regex.Escape));
        EmojiRegex = new Regex(emojiPattern, RegexOptions.Compiled);
    }

    public string Replace(string input)
    {
        int index = 0;
        int colorIndex = 0;
        placeholderMap.Clear();
        colorMap.Clear();
        var result = new StringBuilder(input);

        result.Replace(PlaceholderRegex, match =>
        {
            placeholderMap.Add(index, match.Value);
            return $"{{{index++}}}";
        });

        result.Replace(CoordinateRegex, match =>
        {
            placeholderMap.Add(index, match.Value);
            return $"{{{index++}}}";
        });

        result.Replace(ColorStartRegex, match =>
        {
            string replacement = $"<color={colorIndex++}>";
            colorMap.Add(replacement, match.Value);
            return replacement;
        });

        result.Replace(KeyPressRegex, match =>
        {
            placeholderMap.Add(index, match.Value.Replace(" ", ""));
            return $"{{{index++}}}";
        });

        result.Replace(NumericValueRegex, match =>
        {
            placeholderMap.Add(index, match.Value);
            return $"{{{index++}}}";
        });      

        result.Replace(TokenRegex, match =>
        {
            placeholderMap.Add(index, match.Value);
            return $"{{{index++}}}";
        });

        result.Replace(EmojiRegex, match =>
        {
            placeholderMap.Add(index, match.Value);
            return $"{{{index++}}}";
        });

        return result.ToString();
    }

    public string Restore(string input)
    {
        var result = new StringBuilder(input);

        result.Replace(PlaceholderRegex, match =>
        {
            if (int.TryParse(match.Value.Trim('{', '}'), out int index)
                && placeholderMap.TryGetValue(index, out string? original))
            {
                return original;
            }
            return match.Value;
        });

        foreach (var color in colorMap)
        {
            result.Replace(color.Key, color.Value);
        }

        return result.ToString();
    }

    public static string CleanTranslatedForApplyRules(string input)
    {
        return EmojiRegex.Replace(input, "");
    }
}