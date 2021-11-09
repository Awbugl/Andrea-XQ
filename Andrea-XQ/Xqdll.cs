using System;
using System.Runtime.InteropServices;

namespace AndreaBot.XQ
{
    internal static class Xqdll
    {
        private const string DllName = "xqapi.dll";

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetGroupAdmin")]
        internal static extern IntPtr GetGroupAdmin(byte[] autoid, string robotQq, string group);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_HandleFriendEvent")]
        internal static extern void HandleFriendEvent(byte[] autoid, string robotQq, string qq, int messageType,
                                                      string message);
        
        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_Getbotisonline")]
        internal static extern bool GetBotIsOnline(byte[] autoid, string qq);
        
        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetNick")]
        internal static extern IntPtr GetNick(byte[] autoid, string robotQq, string qq);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_HandleGroupEvent")]
        internal static extern void HandleGroupEvent(byte[] autoid, string robotQq, int requestType, string qq,
                                                     string group, string seq, int messageType, string message);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_QuitGroup")]
        internal static extern void QuitGroup(byte[] autoid, string robotQq, string group);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_SendMsg")]
        internal static extern void SendMsg(byte[] autoid, string robotQq, int messageType, string group, string qq,
                                            string message, int bubbleId);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_SendXML")]
        internal static extern void SendXML(byte[] autoid, string robotQq, int sendType, int messageType, string group,
                                            string qq, string xmlMessage);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_LoginQQ")]
        public static extern void LoginQQ(byte[] autoid, string robotQq);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_OffLineQQ")]
        public static extern void OffLineQQ(byte[] autoid, string robotQq);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetQQList")]
        public static extern IntPtr GetQQList(byte[] autoid);

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetOnLineList")]
        public static extern IntPtr GetOnLineList(byte[] autoid);

        [DllImport("kernel32.dll", EntryPoint = "GetProcessHeap")]
        internal static extern int GetProcessHeap();

        [DllImport("kernel32.dll", EntryPoint = "HeapFree")]
        internal static extern int HeapFree(int hheap, int dwflags, IntPtr lpmeem);
    }
}
