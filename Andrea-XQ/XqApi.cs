using System.Linq;

using Andrea.Interface;

namespace Andrea.XQ
{
    internal class XqApi : IAndreaApi
    {
        public int SendFriendMessage(string robotqq, long qq, string messages)
        {
            Xqdll.SendMsg(Main.Authid, robotqq, 1, "", qq.ToString(), messages.Utf8ToSendString(), 0);
            return 1;
        }

        public int SendGroupMessage(string robotqq, long group, string messages)
        {
            Xqdll.SendMsg(Main.Authid, robotqq, 2, group.ToString(), "", messages.Utf8ToSendString(), 0);
            return 1;
        }

        public int SendTempMessage(string robotqq, long qq, long group, string messages)
        {
            Xqdll.SendMsg(Main.Authid, robotqq, 4, group.ToString(), qq.ToString(), messages.Utf8ToSendString(), 0);
            return 1;
        }

        public int ExitGroup(string robotqq, long group)
        {
            Xqdll.QuitGroup(Main.Authid, robotqq, group.ToString());
            return 1;
        }

        public string GetQqNick(string robotqq, long qq) => Xqdll.GetNick(Main.Authid, robotqq, qq.ToString()).IntPtrToString();

        public bool GetGroupPermission(string robotqq, long adminqq, long group)
        {
            return GetGroupAdminList(robotqq, group).Contains(adminqq);
        }

        internal static long[] GetGroupAdminList(string robotqq, long group)
        {
            return Xqdll.GetGroupAdmin(Main.Authid, robotqq, group.ToString()).IntPtrToString().SplitToList()
                .Select(long.Parse).ToArray();
        }
    }
}