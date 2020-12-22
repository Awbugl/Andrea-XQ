using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Andrea.Core.Basic;

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
            try
            {
                return File.ReadAllText(@"Andrea\Other\Andrea.json");
            }
            catch (Exception ex)
            {
                ExceptionReport(ex);
                return "";
            }
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
                        if (!CacheCompleted || Check(false, content, out string[] messageArrayFriend)) return 1;
                        Process(0, Api, robotQq, 0, fromQqInt64, messageArrayFriend);
                        break;

                    case 2:// Group:
                        AddGroup(fromGroupInt64, robotQq);
                        if (!CacheCompleted || Check(true, content, out string[] messageArrayGroup)) return 1;
                        Process(1, Api, robotQq, fromGroupInt64, fromQqInt64, messageArrayGroup);
                        break;

                    case 4:// GroupTmp:
                        if (!CacheCompleted || Check(false, content, out string[] messageArrayTemp)) return 1;
                        Process(2, Api, robotQq, fromGroupInt64, fromQqInt64, messageArrayTemp);
                        break;

                    case 101:// AddFriend:
                        Xqdll.HandleFriendEvent(Authid, robotQq, fromQq, 10, "");
                        break;

                    case 214:// BeInvitedToGroup:
                        AwReport($"来自QQ {fromQq} 邀请入群 {fromGroup}");
                        break;

                    case 12001:// PluginEnable:
                        break;

                    case 12003:// PluginClicked:
                        Deinitialize();
                        break;
                }
                return 1;
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

        public string GetQqNick(string robotqq, long qq) => IntPtrToString(Xqdll.GetNick(Main.Authid, robotqq, qq.ToString()));

        private static string IntPtrToString(IntPtr intPtr)
        {
            try
            {
                if (Marshal.ReadInt32(intPtr) < 0)
                    return "";
                byte[] bin = new byte[Marshal.ReadInt32(intPtr)];
                Xqdll.RtlMoveMemory(bin, intPtr + 4, Marshal.ReadInt32(intPtr));
                Xqdll.HeapFree1(Xqdll.GetProcessHeap(), 0, intPtr);
                return Encoding.Default.GetString(bin);
            }
            catch (Exception ex)
            {
                ExceptionReport(ex);
                return "";
            }
        }
    }
}