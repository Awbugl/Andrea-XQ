using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AndreaBot.Core;

namespace AndreaBot.XQ
{
    internal class XqApi : IAndreaApi
    {
        private static readonly Dictionary<long, List<long>> GroupAdminCache = new();

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

        public int SendGroupXmlMessage(string robotqq, long group, string xmlMessage)
        {
            Xqdll.SendXML(Main.Authid, robotqq, 1, 2, group.ToString(), "", xmlMessage.Utf8ToSendString());
            return 1;
        }

        public int SendFriendXmlMessage(string robotqq, long qq, string xmlMessage)
        {
            Xqdll.SendXML(Main.Authid, robotqq, 1, 1, "", qq.ToString(), xmlMessage.Utf8ToSendString());
            return 1;
        }

        public int SendTempXmlMessage(string robotqq, long qq, long group, string xmlMessage)
        {
            Xqdll.SendXML(Main.Authid, robotqq, 1, 4, group.ToString(), qq.ToString(), xmlMessage.Utf8ToSendString());
            return 1;
        }

        public void ExitGroup(string robotqq, long group) { Xqdll.QuitGroup(Main.Authid, robotqq, group.ToString()); }

        public string GetQqNick(string robotqq, long qq) =>
            Xqdll.GetNick(Main.Authid, robotqq, qq.ToString()).IntPtrToString();

        public bool GetRobotIsOnline(string robotqq) => Xqdll.GetBotIsOnline(Main.Authid, robotqq);

        public void LoginQq(string robotqq)
        {
            Xqdll.LoginQQ(Main.Authid, robotqq);
            Thread.Sleep(1000);
        }

        public bool GetGroupPermission(string robotqq, long adminqq, long group, bool added = true) =>
            GetGroupAdminList(robotqq, group, added).Contains(adminqq);

        public string[] GetOnlineRobotList() =>
            Xqdll.GetOnLineList(Main.Authid).IntPtrToString().SplitToList().ToArray();

        public void ReloginAllRobots()
        {
            foreach (var robot in Xqdll.GetOnLineList(Main.Authid).IntPtrToString().SplitToList())
            {
                Xqdll.OffLineQQ(Main.Authid, robot);
                Thread.Sleep(1000);
            }

            Thread.Sleep(10000);

            foreach (var robot in Xqdll.GetQQList(Main.Authid).IntPtrToString().SplitToList())
            {
                Xqdll.LoginQQ(Main.Authid, robot);
                Thread.Sleep(1000);
            }
        }

        public void ReloginOfflineRobots()
        {
            foreach (var robot in Xqdll.GetQQList(Main.Authid).IntPtrToString().SplitToList()
                                       .Except(Xqdll.GetOnLineList(Main.Authid).IntPtrToString().SplitToList()))
            {
                Xqdll.LoginQQ(Main.Authid, robot);
                Thread.Sleep(1000);
            }
        }

        internal void AdminUpdate(bool isdel, long group, long admin)
        {
            if (GroupAdminCache.ContainsKey(group))
                if (isdel)
                    GroupAdminCache[group].Remove(admin);
                else
                {
                    if (!GroupAdminCache[group].Contains(admin)) GroupAdminCache[group].Add(admin);
                }
        }

        private static List<long> GetGroupAdminList(string robotqq, long group, bool added)
        {
            if (GroupAdminCache.ContainsKey(group) && added) return GroupAdminCache[group];
            var ls = Xqdll.GetGroupAdmin(Main.Authid, robotqq, group.ToString()).IntPtrToString().SplitToList()
                          .Select(long.Parse).ToList();
            if (!GroupAdminCache.ContainsKey(group)) GroupAdminCache.Add(group, ls);
            return ls;
        }
    }
}
