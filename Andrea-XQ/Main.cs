using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static Andrea.Core.Expander;

// ReSharper disable CommentTypo

// ReSharper disable UnusedMember.Global

#pragma warning disable IDE0060

namespace Andrea.XQ
{
    internal static class Main
    {
        private static readonly XqApi Api = new();
        internal static byte[] Authid = new byte[8];

        [DllExport(ExportName = "XQ_AuthId", CallingConvention = CallingConvention.StdCall)]
        public static void XQ_AuthId(int id, int i)
        {
            AuthId(id, i);
        }

        [DllExport(ExportName = "XQ_AutoId", CallingConvention = CallingConvention.StdCall)]
        public static void XQ_AutoId(int id, int i)
        {
            AuthId(id, i);
        }

        private static void AuthId(int id, int i)
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
        public static string XQ_Create(string frameworkversion)
        {
            return Create();
        }

        private static string Create()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException +=
                    (_, args) => ExceptionReport(args.ExceptionObject as Exception);
                return File.ReadAllText(@"Andrea\Other\Andrea.json");
            }
            catch (Exception ex)
            {
                ExceptionReport(ex);
                return "";
            }
        }

        [DllExport(ExportName = "XQ_DestroyPlugin", CallingConvention = CallingConvention.StdCall)]
        public static int XQ_DestroyPlugin()
        {
            return Destory();
        }

        private static int Destory()
        {
            try
            {
                Deinitialize();
                AppDomain.CreateDomain("Unload").DoCallBack(() => AppDomain.Unload(AppDomain.CurrentDomain));
                return 1;
            }
            catch (Exception ex)
            {
                ExceptionReport(ex);
                return 1;
            }
        }

        [DllExport(ExportName = "XQ_SetUp", CallingConvention = CallingConvention.StdCall)]
        public static int XQ_SetUp()
        {
            return 0;
        }

        [DllExport(ExportName = "XQ_Event", CallingConvention = CallingConvention.StdCall)]
        public static int XQ_Event(string robotQq, int eventType, int extraType, string fromGroup, string fromQq,
            string targetQq, string content, string index, string msgid, string udpmsg, string unix, int p)
        {
            return Event(robotQq, eventType, fromGroup, fromQq, targetQq, content, udpmsg);
        }

        private static int Event(string robotQq, int eventType, string fromGroup, string fromQq,
            string targetQq, string content, string udpmsg)
        {
            try
            {
                long.TryParse(fromQq, out var fromQqInt64);
                long.TryParse(fromGroup, out var fromGroupInt64);

                switch (eventType)
                {
                    case 1: // Friend:
                        if (CheckToShield(false, 0, fromQqInt64, robotQq, content, out var messageArrayFriend))
                            return 1;

                        Process(0, Api, robotQq, 0, fromQqInt64, content, messageArrayFriend);
                        return 1;

                    case 2: // Group:
                        if (CheckToShield(true, fromGroupInt64, fromQqInt64, robotQq, content,
                            out var messageArrayGroup)) return 1;
                        Process(1, Api, robotQq, fromGroupInt64, fromQqInt64, content, messageArrayGroup);
                        return 1;

                    case 4: // GroupTmp:
                        if (CheckToShield(false, fromGroupInt64, fromQqInt64, robotQq, content,
                            out var messageArrayTemp))
                            return 1;

                        Process(2, Api, robotQq, fromGroupInt64, fromQqInt64, content, messageArrayTemp);
                        return 1;

                    case 101: // AddFriend:
                        Xqdll.HandleFriendEvent(Authid, robotQq, fromQq, 10, "");
                        return 1;

                    case 202: // SomeoneBeRemovedFromGroup
                    case 203: // SomeoneBeBannedSpeaking 
                        if (targetQq != robotQq) return 1;

                        AddToSheildList(fromGroupInt64, 1);
                        AddToSheildList(fromQqInt64, 0);
                        if (eventType == 202) DeleteGroup(fromGroupInt64);

                        return 1;

                    case 214: // BeInvitedToGroup:
                        var isAdmin = !SheildCheck(fromGroupInt64) && (fromQqInt64 is 1941232341L or 3368228552L ||
                                                                       XqApi.GetGroupAdminList(robotQq, fromGroupInt64)
                                                                           .Contains(fromQqInt64));
                        var message = isAdmin
                            ? ""
                            : SheildCheck(fromGroupInt64)
                                ? "本群处于屏蔽期。"
                                : "抱歉，您不是群管理员。";

                        Xqdll.HandleGroupEvent(Authid, robotQq, 214, fromQq, fromGroup, udpmsg, isAdmin ? 10 : 20,
                            message);
                       
                        return 1;

                    case 12001: // PluginEnable:
                        Initialize(Api);
                        return 1;

                    case 12002: // PluginClosed:
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