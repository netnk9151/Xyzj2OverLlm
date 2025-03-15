using TMPro;
using UnityEngine;

namespace EnglishPatch;

public class TextMetadata : MonoBehaviour
{
    public float OriginalX;
    public float OriginalY;
    public float OriginalWidth;
    public float OriginalHeight;

    public TextAlignmentOptions OriginalAlignment;
    public TextOverflowModes OriginalOverflowMode;

    public bool OriginalAllowWordWrap;
    public bool OriginalAllowAutoSizing;

    public float AdjustX;
    public float AdjustY;
    public float AdjustWidth;
    public float AdjustHeight;
}