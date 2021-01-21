using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static Andrea.Expander;
// ReSharper disable CommentTypo

// ReSharper disable UnusedMember.Global

#pragma warning disable IDE0060

namespace Andrea.XQ
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

        [DllExport(ExportName = "XQ_DestroyPlugin", CallingConvention = CallingConvention.StdCall)]
        public static int XQ_DestroyPlugin() => Destory();

        private static int Destory()
        {
            Deinitialize();
            return 0;
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
                        bool isAdmin = robotQq != "2967373629" && XqApi.GetGroupAdminList(robotQq, fromGroupInt64).Contains(fromQqInt64);
                        string message = isAdmin ? "" : robotQq == "2967373629"
                            ? "Andrea暂停加群，请邀请Beatrice或Cadilotta(2708288417)。"
                            : "抱歉，您不是群管理员。";

                        Xqdll.HandleGroupEvent(Authid, robotQq, 214, fromQq, fromGroup, udpmsg, isAdmin ? 10 : 20, message);

                        AwReport($"[BeInvitedToGroupEvent]\nFromQQ : {fromQqInt64}\nRobotQQ : {robotQq}\nFromGroup : {fromGroupInt64}\nState : {(isAdmin ? "Agree" : "Disagree")}");

                        return 1;

                    case 12001:// PluginEnable:
                        Initialize(Api);
                        return 1;

                    case 12002:// PluginClosed:
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
}