using CCEditor.Classes;

public static class StaticVariables
{
    public static readonly int[] powersOf2;

    public static readonly Quaternion mayaAxisRotation;

    public static readonly Quaternion inversMayaAxisRotation;

    public static readonly Quaternion rotateY45;

    public static readonly Color transparentBlack;

   // public static readonly Plane keyboardPlaneH;

   // public static readonly Plane keyboardPlaneV;

    public static string[] suportedAIExtensions;

    public const string webPath = "https://thepianoapp.nyc3.digitaloceanspaces.com/ARPianist/";

    public const string dashes = "--------------------------------------";

    public const string binaryFormatWithDot = ".mtb";

    public const string binaryFormatNoDot = "mtb";

    public const long tenMillion = 10000000L;

    public const float tenMillionth = 1E-07f;

    public static readonly bool[] isBlackKey;

    public static byte[] blackToIndex;

    public static readonly byte[] whiteToIndex;

    public static readonly byte[] indexToWhite;

    public static readonly sbyte[] whiteIndexAfterBlackIndex;

    public static float[] whiteUnitSizes;

    public static float[] whiteUnitPercentage;

    public static readonly char[] noteLabels;

    public static readonly string[] fullNoteLabelsFromA;

    public static readonly string[] fullNoteLabelsFromC;

    public static readonly Vector3 keyboardCentre;

    public static float[] whiteCumulDist;

    public static float[] whiteCumulRatio;

    public static float[] spritesYs;

    public static float[] NoteSpriteRotations;

    static StaticVariables()
    {
        powersOf2 = new int[31]
        {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512,
            1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
            1048576, 2097152, 4194304, 8388608, 16777216, 33554432, 67108864, 134217728, 268435456, 536870912,
            1073741824
        };
        mayaAxisRotation = new Quaternion(0f, 0.70710677f, 0f, 0.70710677f);
        inversMayaAxisRotation = new Quaternion(0f, 0.70710677f, 0f, -0.70710677f);
        rotateY45 = new Quaternion(0f, 0.38268343f, 0f, 0.9238795f);
        transparentBlack = new Color(0f, 0f, 0f, 0f);
        //keyboardPlaneH = new Plane(Vector3.down, 0.72f);
        //keyboardPlaneV = new Plane(Vector3.forward, 0.153f);
        suportedAIExtensions = "acc mid midi mp3 mpeg mp4 m4a m4v ogg rtx wav wave".Split(' ');
        isBlackKey = new bool[95]
        {
            false, true, false, false, true, false, true, false, false, true,
            false, true, false, true, false, false, true, false, true, false,
            false, true, false, true, false, true, false, false, true, false,
            true, false, false, true, false, true, false, true, false, false,
            true, false, true, false, false, true, false, true, false, true,
            false, false, true, false, true, false, false, true, false, true,
            false, true, false, false, true, false, true, false, false, true,
            false, true, false, true, false, false, true, false, true, false,
            false, true, false, true, false, true, false, false, true, false,
            true, false, false, true, false
        };
        blackToIndex = new byte[36];
        whiteToIndex = new byte[52];
        indexToWhite = new byte[88];
        whiteIndexAfterBlackIndex = new sbyte[88]
        {
            -1, 2, -1, -1, 4, -1, 5, -1, -1, 7,
            -1, 8, -1, 9, -1, -1, 11, -1, 12, -1,
            -1, 14, -1, 15, -1, 16, -1, -1, 18, -1,
            19, -1, -1, 21, -1, 22, -1, 23, -1, -1,
            25, -1, 26, -1, -1, 28, -1, 29, -1, 30,
            -1, -1, 32, -1, 33, -1, -1, 35, -1, 36,
            -1, 37, -1, -1, 39, -1, 40, -1, -1, 42,
            -1, 43, -1, 44, -1, -1, 46, -1, 47, -1,
            -1, 49, -1, 50, -1, 51, -1, -1
        };
        whiteUnitSizes = new float[52];
        whiteUnitPercentage = new float[52];
        noteLabels = new char[12]
        {
            'A', 'A', 'B', 'C', 'C', 'D', 'D', 'E', 'F', 'F',
            'G', 'G'
        };
        fullNoteLabelsFromA = new string[12]
        {
            "A", "A♯", "B", "C", "C♯", "D", "D♯", "E", "F", "F♯",
            "G", "G♯"
        };
        fullNoteLabelsFromC = new string[12]
        {
            "C", "C♯", "D", "D♯", "E", "F", "F♯", "G", "G♯", "A",
            "A♯", "B"
        };
        keyboardCentre = new Vector3(0f, 0.72f, 0.07f);
        whiteCumulDist = new float[53];
        whiteCumulRatio = new float[53];
        spritesYs = new float[89];
        NoteSpriteRotations = new float[88];
        whiteCumulDist[0] = 0f;
        whiteCumulRatio[0] = 0f;
        spritesYs[88] = -50f;
        byte b = 0;
        byte b2 = 0;
        for (byte b3 = 0; b3 < 88; b3 = (byte)(b3 + 1))
        {
            if (isBlackKey[b3])
            {
                blackToIndex[b] = b3;
                indexToWhite[b3] = 0;
                b = (byte)(b + 1);
            }
            else
            {
                whiteToIndex[b2] = b3;
                indexToWhite[b3] = b2;
                b2 = (byte)(b2 + 1);
            }
        }
        for (int i = 0; i < 52; i++)
        {
            if (i < 2)
            {
                whiteUnitSizes[i] = 1.5f;
            }
            else if (i == 51)
            {
                whiteUnitSizes[i] = 1f;
            }
            else if ((i - 2) % 7 < 3)
            {
                whiteUnitSizes[i] = 1.6666666f;
            }
            else
            {
                whiteUnitSizes[i] = 1.75f;
            }
            whiteUnitPercentage[i] = whiteUnitSizes[i] / 88f;
        }
        for (int j = 1; j < 53; j++)
        {
            whiteCumulDist[j] = whiteCumulDist[j - 1] + whiteUnitSizes[j - 1];
            whiteCumulRatio[j] = whiteCumulDist[j] / 88f;
        }
    }
}
