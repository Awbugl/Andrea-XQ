using System;
using System.Runtime.InteropServices;

namespace Andrea.XQ
{
    internal static class Xqdll
    {
        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetGroupAdmin")]
        internal static extern IntPtr GetGroupAdmin(byte[] autoid, string robotQq, string group);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_HandleFriendEvent")]
        internal static extern void HandleFriendEvent(byte[] autoid, string robotQq, string qq, int messageType,
            string message);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_GetNick")]
        internal static extern IntPtr GetNick(byte[] autoid, string robotQq, string qq);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_HandleGroupEvent")]
        internal static extern void HandleGroupEvent(byte[] autoid, string robotQq, int requestType, string qq,
            string group, string seq, int messageType, string message);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_QuitGroup")]
        internal static extern void QuitGroup(byte[] autoid, string robotQq, string group);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_SendMsg")]
        internal static extern void SendMsg(byte[] autoid, string robotQq, int messageType, string group, string qq,
            string message, int bubbleId);

        [DllImport("xqapi.dll", CharSet = CharSet.Ansi, EntryPoint = "S3_Api_SendXML")]
        internal static extern void SendXML(byte[] autoid, string robotQq, int sendType, int messageType, string group,
            string qq, string xmlMessage);

        [DllImport("kernel32.dll", EntryPoint = "GetProcessHeap")]
        internal static extern int GetProcessHeap();

        [DllImport("kernel32.dll", EntryPoint = "HeapFree")]
        internal static extern int HeapFree(int hheap, int dwflags, IntPtr lpmeem);
    }
}