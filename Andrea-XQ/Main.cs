using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using Andrea.Model;

using static Andrea.Expander;
// ReSharper disable CommentTypo

// ReSharper disable UnusedMember.Global

#pragma warning disable IDE0060

namespace Andrea_XQ
{
    public static class Main
    {
        private static readonly XqApi Api = new XqApi();
        public static byte[] Authid = new byte[8];

        [DllExport(ExportName = "XQ_AuthId", CallingConvention = CallingConvention.StdCall)]
        public static void XQ_AuthId(int id, int i) => AuthId(id, i);

        [DllExport(ExportName = "XQ_AutoId", CallingConvention = CallingConvention.StdCall)]
        public static void XQ_AutoId(int id, int i) => AuthId(id, i);

        public static void AuthId(int id, int i)
        {
            try
            {
                Authid = BitConverter.GetBytes(id).Concat(BitConverter.GetBytes(i)).ToArray();
                CosturaUtility.Initialize();
                Initialize(Api);
            }
            catch (Exception ex)
            {
                ExceptionReport(ex);
            }
        }

        [DllExport(ExportName = "XQ_Create", CallingConvention = CallingConvention.StdCall)]
        public static string XQ_Create(string frameworkversion) => Create();

        public static string Create()
        {
            return File.ReadAllText(@"Andrea\Other\Andrea.json");
        }

        [DllExport(ExportName = "XQ_SetUp", CallingConvention = CallingConvention.StdCall)]
        public static int XQ_SetUp() => 0;

        [DllExport(ExportName = "XQ_Event", CallingConvention = CallingConvention.StdCall)]
        public static int XQ_Event(string robotQq, int eventType, int extraType, string fromGroup, string fromQq, string targetQq, string content, string index, string msgid, string udpmsg, string unix, int p)
            => Event(robotQq, eventType, extraType, fromGroup, fromQq, targetQq, content, index, msgid, udpmsg, unix, p);

        public static int Event(string robotQq, int eventType, int extraType, string fromGroup, string fromQq, string targetQq, string content, string index, string msgid, string udpmsg, string unix, int p)
        {
            try
            {
                long.TryParse(fromQq, out long fromQqInt64);
                long.TryParse(fromGroup, out long fromGroupInt64);
                switch (eventType)
                {
                    case 1:// Friend:
                        if (CheckToShield(false, 0, robotQq, content, out string[] messageArrayFriend)) return 1;
                        Process(0, Api, robotQq, 0, fromQqInt64, messageArrayFriend);
                        return 1;

                    case 2:// Group:
                        if (CheckToShield(true, fromGroupInt64, robotQq, content, out string[] messageArrayGroup)) return 1;
                        Process(1, Api, robotQq, fromGroupInt64, fromQqInt64, messageArrayGroup);
                        return 1;

                    case 4:// GroupTmp:
                        if (CheckToShield(false, fromGroupInt64, robotQq, content, out string[] messageArrayTemp)) return 1;
                        Process(2, Api, robotQq, fromGroupInt64, fromQqInt64, messageArrayTemp);
                        return 1;

                    case 101:// AddFriend:
                        Xqdll.HandleFriendEvent(Authid, robotQq, fromQq, 10, "");
                        return 1;

                    case 214:// BeInvitedToGroup:
                        AwReport($"QQ {fromQq} 邀请 {robotQq} 入群 {fromGroup}");
                        return 1;

                    case 12001:// PluginEnable:
                        Initialize(Api);
                        return 1;

                    case 12003:// PluginClicked:
                        Deinitialize();
                        return 1;

                    default:
                        return 1;
                }
            }
            catch (Exception ex)
            {
                ExceptionReport(ex);
                return 1;
            }
        }
    }

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

        public bool GetGroupPermission(string robotqq, long qq, long group)
        {
            return IntPtrToString(Xqdll.GetGroupAdmin(Main.Authid, robotqq, group.ToString())).Contains(qq.ToString());
        }

        public string GetQqNick(string robotqq, long qq) => NickToSendString(IntPtrToString(Xqdll.GetNick(Main.Authid, robotqq, qq.ToString())));

        private static string IntPtrToString(IntPtr intPtr)
        {
            try
            {
                Encoding gb18030 = Encoding.GetEncoding("gb18030");
                var length = Marshal.ReadInt32(intPtr);
                if (length <= 0) return "";

                byte[] bin = new byte[length];
                Marshal.Copy(intPtr + 4, bin, 0, length);
                Xqdll.HeapFree(Xqdll.GetProcessHeap(), 0, intPtr);

                StringBuilder sb = new StringBuilder();

                for ( int i = 0;i < length - 1;)
                    sb.Append(EncodingGetString(gb18030, bin, ref i, bin[i] < 0x80 ? 1 : bin[i + 1] > 0x3F ? 2 : 4));

                if (length > 1 && bin[length - 2] > 0x80) return sb.ToString();

                sb.Append(gb18030.GetString(bin, length - 1, 1));

                return sb.ToString();
            }
            catch (Exception ex)
            {
                ExceptionReport(ex);
                return "";
            }
        }

        private static string EncodingGetString(Encoding encoding, byte[] bin, ref int index, int count)
        {
            index += count;

            return count < 4
                ? encoding.GetString(bin, index - count, count)
                : "[emoji=" + Encoding.Convert(encoding, Encoding.UTF8,
                    bin.Skip(index - count).Take(4).ToArray()).Aggregate("", (current, bi)
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