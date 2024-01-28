using Unity.VisualScripting;

namespace Utilities
{
    public static class StringExtensions
    {
        public static string Left(this string str) =>
            $"<align=left>{str}</align>";
        
        public static string Right(this string str) =>
            $"<align=right>{str}</align>";
        
        public static string ZeroHeight(this string str) =>
            $"<line-height=0em>{str}";
        
        public static string LineHeight(this string str) =>
            $"<line-height=1em>{str}";

        public static string SpanTo(this string left, string right) =>
            left.Left() + "\n".ZeroHeight() + right.Right() + "".LineHeight();
        
        public static string LeftRight(string left, string right) => 
            $"{left.Left().ZeroHeight()}\n{right.Right().LineHeight()}";

        public static string Colour(this string str, UnityEngine.Color color) =>
            $"<color=#{color.ToHexString()}>{str}</color>";
    }
}