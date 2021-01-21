using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Andrea.Interface;
using Andrea.Model;

namespace Andrea.XQ
{
    internal class XqApi : IAndreaApi
    {

        public int SendFriendMessage(string robotqq, long qq, string messages)
        {
            Xqdll.SendMsg(Main.Authid, robotqq, 1, "", qq.ToString(), messages, 0);
            return 1;
        }

        public int SendGroupMessage(string robotqq, long group, string messages)
        {
            Xqdll.SendMsg(Main.Authid, robotqq, 2, group.ToString(), "", messages, 0);
            return 1;
        }

        public int SendTempMessage(string robotqq, long qq, long group, string messages)
        {
            Xqdll.SendMsg(Main.Authid, robotqq, 4, group.ToString(), qq.ToString(), messages, 0);
            return 1;
        }

        public int ExitGroup(string robotqq, long group)
        {
            Xqdll.QuitGroup(Main.Authid, robotqq, group.ToString());
            return 1;
        }

        public string GetQqNick(string robotqq, long qq) => NickToSendString(IntPtrToString(Xqdll.GetNick(Main.Authid, robotqq, qq.ToString())));

        public bool GetGroupPermission(string robotqq, long adminqq, long group)
        {
            return GetGroupAdminList(robotqq, group).Contains(adminqq);
        }

        internal static long[] GetGroupAdminList(string robotqq, long group)
        {
            return IntPtrToString(Xqdll.GetGroupAdmin(Main.Authid, robotqq, group.ToString()))
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();
        }

        internal static string IntPtrToString(IntPtr intPtr)
        {
            try
            {
                Encoding gb18030 = Encoding.GetEncoding("gb18030");
                var length = Marshal.ReadInt32(intPtr);
                if (length <= 0) return "";

                byte[] bin = new byte[length];
                Marshal.Copy(intPtr + 4, bin, 0, length);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < length - 1;)
                    sb.Append(EncodingGetString(gb18030, bin, ref i, bin[i] < 0x80 ? 1 : bin[i + 1] > 0x3F ? 2 : 4));

                if (length > 1 && bin[length - 2] > 0x80) return sb.ToString();

                sb.Append(gb18030.GetString(bin, length - 1, 1));

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Expander.ExceptionReport(ex);
                return "";
            }
            finally
            {
                if (Xqdll.HeapFree(Xqdll.GetProcessHeap(), 0, intPtr) == 0)
                {
                    int err = Marshal.GetLastWin32Error();
                    if (err != 0) Expander.ExceptionReport(new Win32Exception(err));
                }
            }
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

        private static string NickToSendString(string msg)
        {
            StringBuilder ret = new StringBuilder();
            Regex reg = new Regex("(&nbsp;|\\[em\\](e[0-9]{1,6})\\[\\/em\\])", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            int last = 0;
            Match rslt;
            while ((rslt = reg.Match(msg.Substring(last))).Success)
            {
                var m = rslt.Groups;
                ret.Append(msg.Substring(last, m[0].Index));
                if (m[0].Value[0] == '&') ret.Append(" ");
                else
                {
                    int codePoint = int.Parse(m[2].Value.Substring(1));

                    if (codePoint > 200000)
                        ret.Append($"[emoji={Encoding.Convert(Encoding.UTF32, Encoding.UTF8, BitConverter.GetBytes(codePoint - 200000)).Aggregate("", (current, i) => current + i.ToString("X2"))}]");
                    else if (codePoint >= 100000)
                        ret.Append($"[Face{codePoint - 100000}.gif]");
                    else
                        ret.Append($"[pic=http://qzonestyle.gtimg.cn/qzone/em/{m[2].Value}.gif]");
                }
                last += rslt.Index + rslt.Length;
            }
            ret.Append(msg.Substring(last));
            return ret.ToString();
        }
    }
}