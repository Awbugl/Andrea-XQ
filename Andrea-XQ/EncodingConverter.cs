using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using AndreaBot.Core;

namespace AndreaBot.XQBridge
{
    internal static class EncodingConverter
    {
        private static readonly Lazy<Regex> Reg = new(() =>
            new Regex("(&nbsp;|\\[em\\](e[0-9]{1,6})\\[\\/em\\])", RegexOptions.IgnoreCase | RegexOptions.ECMAScript));

        private static readonly Encoding Gb18030 = Encoding.GetEncoding("gb18030");

        internal static IEnumerable<string> SplitToList(this string str)
        {
            return str.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList();
        }

        private static string EmojiToSendString(this string msg)
        {
            var ret = new StringBuilder();
            var last = 0;
            Match rslt;
            while ((rslt = Reg.Value.Match(msg.Substring(last))).Success)
            {
                var m = rslt.Groups;
                ret.Append(msg.Substring(last, m[0].Index));
                if (m[0].Value[0] == '&')
                {
                    ret.Append(" ");
                }
                else
                {
                    var codePoint = int.Parse(m[2].Value.Substring(1));
                    switch (codePoint)
                    {
                        case > 200000:
                            ret.Append(
                                $"[emoji={Encoding.Convert(Encoding.UTF32, Encoding.UTF8, BitConverter.GetBytes(codePoint - 200000)).Aggregate("", (current, i) => current + i.ToString("X2"))}]");
                            break;
                        case >= 100000:
                            ret.Append($"[Face{codePoint - 100000}.gif]");
                            break;
                        default:
                            ret.Append($"[pic=http://qzonestyle.gtimg.cn/qzone/em/{m[2].Value}.gif]");
                            break;
                    }
                }

                last += rslt.Index + rslt.Length;
            }

            ret.Append(msg.Substring(last));
            return ret.ToString();
        }

        internal static string IntPtrToString(this IntPtr intPtr)
        {
            if (intPtr == IntPtr.Zero) return "";
            try
            {
                var length = Marshal.ReadInt32(intPtr);
                if (length <= 0) return "";
                var bin = new byte[length];
                Marshal.Copy(IntPtr.Add(intPtr, 4), bin, 0, length);
                return EmojiToSendString(BytesToString(bin));
            }
            finally
            {
                if (Xqdll.HeapFree(Xqdll.GetProcessHeap(), 0, intPtr) == 0)
                {
                    var err = Marshal.GetLastWin32Error();
                    if (err != 0) External.ExceptionReport(new Win32Exception(err));
                }
            }
        }

        internal static string Utf8ToSendString(this string utf8String)
        {
            return BytesToString(Encoding.Convert(Encoding.UTF8, Gb18030, Encoding.UTF8.GetBytes(utf8String)));
        }

        private static string BytesToString(byte[] bin)
        {
            var length = bin.Length;
            if (length == 1) return Gb18030.GetString(bin);

            var sb = new StringBuilder();
            for (var i = 0; i < length;)
                sb.Append(EncodingGetString(Gb18030, bin, ref i, bin[i] < 0x80 ? 1 : bin[i + 1] > 0x3F ? 2 : 4));

            return sb.ToString();
        }

        private static string EncodingGetString(Encoding encoding, byte[] bin, ref int index, int count)
        {
            index += count;

            return count < 4
                ? encoding.GetString(bin, index - count, count)
                : Encoding.Convert(encoding, Encoding.UTF8,
                    bin.Skip(index - count).Take(4).ToArray()).Aggregate("[emoji=", (current, bi)
                    => current + bi.ToString("X2")) + "]";
        }
    }
}