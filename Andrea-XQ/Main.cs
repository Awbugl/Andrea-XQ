using System;
using System.Linq;
using System.Runtime.InteropServices;
using static AndreaBot.Core.External;

namespace AndreaBot.XQ
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

        private static void AuthId(int id, int i)
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
                return
                    @"{""name"":""AndreaBot"",""pver"":""3.0.0"",""sver"": 3,""author"":""littlenine12"",""desc"":""AndreaBot""}";
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
                var act = new Action(() => AppDomain.Unload(AppDomain.CurrentDomain));
                AppDomain.CreateDomain("Unload").DoCallBack(act.Invoke);
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
                        Process(0, Api, robotQq, 0, fromQqInt64, content);
                        return 1;

                    case 2: // Group:
                        Process(1, Api, robotQq, fromGroupInt64, fromQqInt64, content);
                        return 1;

                    case 4: // GroupTmp:
                        Process(2, Api, robotQq, fromGroupInt64, fromQqInt64, content);
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

                        var result = HandleAddGroupEvent(fromGroupInt64, fromQqInt64, robotQq, out var refuseMsg);

                        Xqdll.HandleGroupEvent(Authid, robotQq, 214, fromQq, fromGroup, udpmsg, result ? 10 : 20,
                            refuseMsg);

                        return 1;

                    case 216: //GroupDissolved
                        DeleteGroup(fromGroupInt64);
                        return 1;

                    case 219: // SomeoneHasBeenInvitedIntoGroup
                        if (targetQq == robotQq)
                        {
                            GroupCountCheck(Api,robotQq,fromGroupInt64);
                        }
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