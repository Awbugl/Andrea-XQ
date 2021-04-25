using System;
using System.Runtime.InteropServices;

namespace Andrea.XQ
{
    public static class Xqdll
    {
        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetGroupAdmin")]
        public static extern IntPtr GetGroupAdmin(byte[] autoid, string robotQq, string group);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_HandleFriendEvent")]
        public static extern void HandleFriendEvent(byte[] autoid, string robotQq, string qq, int messageType,
            string message);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetNick")]
        public static extern IntPtr GetNick(byte[] autoid, string robotQq, string qq);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_HandleGroupEvent")]
        public static extern void HandleGroupEvent(byte[] autoid, string robotQq, int requestType, string qq,
            string group, string seq, int messageType, string message);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_QuitGroup")]
        public static extern void QuitGroup(byte[] autoid, string robotQq, string group);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_SendMsg")]
        public static extern void SendMsg(byte[] autoid, string robotQq, int messageType, string group, string qq,
            string message, int bubbleId);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_SendXML")]
        public static extern void SendXML(byte[] autoid, string robotQq, int sendType, int messageType, string group,
            string qq, string xmlMessage);

        [DllImport("kernel32.dll", EntryPoint = "GetProcessHeap")]
        public static extern int GetProcessHeap();

        [DllImport("kernel32.dll", EntryPoint = "HeapFree")]
        public static extern int HeapFree(int hheap, int dwflags, IntPtr lpmeem);
    }
}