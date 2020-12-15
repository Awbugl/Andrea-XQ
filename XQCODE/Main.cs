using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Andrea.Core.Basic;

using HuajiTech.UnmanagedExports;

using static Andrea.Expander;

// ReSharper disable UnusedMember.Global

#pragma warning disable IDE0060

namespace Andrea_XQ
{
    public static class Main
    {
        public static byte[] AutoId;
        public const string RobotQq = "2967373629";
        private static readonly XqApi Api = new XqApi();

        [DllExport]
        public static void XQ_AutoId(short id, int imAddr) => AutoId = BitConverter.GetBytes(id).Concat(BitConverter.GetBytes(imAddr)).ToArray();

        [DllExport]
        public static string XQ_Create(string _) => "{\"name\":\"Andrea\",\"pver\":\"2.0.7\",\"sver\":3,\"author\":\"littlenine\",\"desc\":\"Andrea with XQ.Net\"}";

        [DllExport]
        public static int XQ_SetUp() => 0;

        [DllExport]
        public static int XQ_Event(string robotQq, int eventType, int extraType, string fromGroup, string fromQq, string targetQq, string content, string index, string msgid, string udpmsg, string unix, int p)
        {
            try
            {
                long fromQqInt64 = Convert.ToInt64(fromQq), fromGroupInt64 = Convert.ToInt64(fromGroup);
                switch (eventType)
                {
                    case 1:// (int)XqEventType.Friend:
                        if (!CacheCompleted || Check(false, content, out string[] messageArrayFriend)) return 1;
                        Process(0, Api, 0, fromQqInt64, messageArrayFriend);
                        break;

                    case 2:// (int)XqEventType.Group:
                        if (CacheCompleted && AddGroup(fromGroupInt64, out string message)) Api.SendGroupMessage(fromGroupInt64, message);
                        if (!CacheCompleted || Check(true, content, out string[] messageArrayGroup)) return 1;
                        Process(1, Api, fromGroupInt64, fromQqInt64, messageArrayGroup);
                        break;

                    case 4:// (int)XqEventType.GroupTmp:
                        if (!CacheCompleted || Check(false, content, out string[] messageArrayTemp)) return 1;
                        Process(2, Api, fromGroupInt64, fromQqInt64, messageArrayTemp);
                        break;

                    case 101:// (int)XqEventType.AddFriend:
                        Xqdll.HandleFriendEvent(AutoId, robotQq, fromQq, 10, "");
                        break;

                    case 214:// (int)XqEventType.BeInvitedToGroup:
                        AwReport($"来自QQ {fromQq} 邀请入群 {fromGroup}");
                        break;

                    case 12001:// (int)XqEventType.PluginEnable:
                        Initialize(Api);
                        break;

                    case 12003:// (int)XqEventType.PluginClicked:
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
        public int SendFriendMessage(long qq, string messages)
        {
            Xqdll.SendMsgEX(Main.AutoId, Main.RobotQq, 1, "", qq.ToString(), messages, 0, false);
            return 1;
        }

        public int SendGroupMessage(long group, string messages)
        {
            Xqdll.SendMsgEX(Main.AutoId, Main.RobotQq, 2, group.ToString(), "", messages, 0, false);
            return 1;
        }

        public int SendTempMessage(long qq, long group, string messages)
        {
            Xqdll.SendMsgEX(Main.AutoId, Main.RobotQq, 2, group.ToString(), qq.ToString(), messages, 0, false);
            return 1;
        }

        public int ExitGroup(long group)
        {
            Xqdll.QuitGroup(Main.AutoId, Main.RobotQq, group.ToString());
            return 1;
        }

        public bool GetGroupPermission(long qq, long group)
        {
            return IntPtrToString(Xqdll.GetGroupAdmin(Main.AutoId, Main.RobotQq, group.ToString())).Contains(qq.ToString());
        }

        public string GetQqNick(long qq) => IntPtrToString(Xqdll.GetNick(Main.AutoId, Main.RobotQq, qq.ToString()));

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