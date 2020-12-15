using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo

namespace Andrea_XQ
{
    public class Xqapi
    {
        public static byte[] AuthId;
        
        public void SetAuthId(int id, int addr)
        {
            AuthId = BitConverter.GetBytes(id).Concat(BitConverter.GetBytes(addr)).ToArray();
        }

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="group">群号</param>
        /// <param name="msg">消息</param>
        public void SendGroupMessage(string robot, string group, string msg)
        {
            Xqdll.SendMsgEX(AuthId, robot, 2, group, "", msg, 0, false);
        }

        /// <summary>
        /// 获取好友列表-http模式
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetFriendList(string qq)
        {
            return IntPtrToString(Xqdll.GetFriendList(AuthId, qq));
        }

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetGroupList(string qq)
        {
            return IntPtrToString(Xqdll.GetGroupList(AuthId, qq));
        }

        /// <summary>
        /// 获取机器人在线账号列表
        /// </summary>
        /// <returns></returns>
        public static string GetOnLineList()
        {
            return IntPtrToString(Xqdll.GetOnLineList(AuthId));
        }

        /// <summary>
        /// 获取机器人账号是否在线
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool Getbotisonline(string qq)
        {
            return Xqdll.Getbotisonline(AuthId, qq);
        }

        /// <summary>
        /// 获取群员列表
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetGroupMemberList(string qq, string group)
        {
            return IntPtrToString(Xqdll.GetGroupMemberList(AuthId, qq, group));
        }

        /// <summary>
        /// 获取群成员名片
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetGroupCard(string robotQq, string group, string qq)
        {
            return IntPtrToString(Xqdll.GetGroupCard(AuthId, robotQq, group, qq));
        }

        /// <summary>
        /// 获取群管理⚪列表
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetGroupAdmin(string robotQq, string group)
        {
            return IntPtrToString(Xqdll.GetGroupAdmin(AuthId, robotQq, group));
        }

        /// <summary>
        /// 获取群通知
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetNotice(string robotQq, string group)
        {
            return IntPtrToString(Xqdll.GetNotice(AuthId, robotQq, group));
        }

        /// <summary>
        /// 获取群成员禁言状态
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool IsShutUp(string robotQq, string group, string qq)
        {
            return Xqdll.IsShutUp(AuthId, robotQq, group, qq);
        }

        /// <summary>
        /// 是否是好友
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool IfFriend(string robotQq, string qq)
        {
            return Xqdll.IfFriend(AuthId, robotQq, qq);
        }

        /// <summary>
        /// 取得QQ群页面操作用参数P_skey
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetGroupPsKey(string robotQq)
        {
            return IntPtrToString(Xqdll.GetGroupPsKey(AuthId, robotQq));
        }

        /// <summary>
        /// 取得QQ空间页面操作用参数P_skey
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetZonePsKey(string robotQq)
        {
            return IntPtrToString(Xqdll.GetZonePsKey(AuthId, robotQq));
        }

        /// <summary>
        /// 取得机器人网页操作用的Cookies
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetCookies(string robotQq)
        {
            return IntPtrToString(Xqdll.GetCookies(AuthId, robotQq));
        }

        /// <summary>
        /// 获取赞数量
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static int GetObjVote(string robotQq, string qq)
        {
            return Xqdll.GetObjVote(AuthId, robotQq, qq);
        }

        /// <summary>
        /// 取插件是否启用
        /// </summary>
        /// <returns></returns>
        public static bool IsEnable()
        {
            return Xqdll.IsEnable(AuthId);
        }

        /// <summary>
        /// 取所有QQ列表
        /// </summary>
        /// <returns></returns>
        public static string GetQqList()
        {
            return IntPtrToString(Xqdll.GetQQList(AuthId));
        }

        /// <summary>
        /// 取QQ昵称
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetNick(string robotQq, string qq)
        {
            return IntPtrToString(Xqdll.GetNick(AuthId, robotQq, qq));
        }

        /// <summary>
        /// 取好友备注姓名
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetFriendsRemark(string robotQq, string qq)
        {
            return IntPtrToString(Xqdll.GetFriendsRemark(AuthId, robotQq, qq));
        }

        /// <summary>
        /// 取短Clientkey
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetClientkey(string robotQq)
        {
            return IntPtrToString(Xqdll.GetClientkey(AuthId, robotQq));
        }

        /// <summary>
        /// 取得机器人网页操作用的长Clientkey
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetLongClientkey(string robotQq)
        {
            return IntPtrToString(Xqdll.GetLongClientkey(AuthId, robotQq));
        }

        /// <summary>
        /// 取得腾讯课堂页面操作用参数P_skey
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetClassRoomPsKey(string robotQq)
        {
            return IntPtrToString(Xqdll.GetClassRoomPsKey(AuthId, robotQq));
        }

        /// <summary>
        /// 取得QQ举报页面操作用参数P_skey
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetRepPsKey(string robotQq)
        {
            return IntPtrToString(Xqdll.GetRepPsKey(AuthId, robotQq));
        }

        /// <summary>
        /// 取得财付通页面操作用参数P_skey
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetTenPayPsKey(string robotQq)
        {
            return IntPtrToString(Xqdll.GetTenPayPsKey(AuthId, robotQq));
        }

        /// <summary>
        /// 取bkn
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetBkn(string robotQq)
        {
            return IntPtrToString(Xqdll.GetBkn(AuthId, robotQq));
        }

        /// <summary>
        /// 封包模式获取群号列表(最多可以取得999)
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetGroupList_B(string robotQq)
        {
            return IntPtrToString(Xqdll.GetGroupList_B(AuthId, robotQq));
        }

        /// <summary>
        /// 封包模式取好友列表(与封包模式取群列表同源)
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetFriendList_B(string robotQq)
        {
            return IntPtrToString(Xqdll.GetFriendList_B(AuthId, robotQq));
        }

        /// <summary>
        /// 取登录二维码base64
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQrcode(string key)
        {
            return IntPtrToString(Xqdll.GetQrcode(AuthId, key));
        }

        /// <summary>
        /// 检查登录二维码状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int CheckQrcode(string key)
        {
            return Xqdll.CheckQrcode(AuthId, key);
        }

        /// <summary>
        /// 取指定的群名称
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetGroupName(string robotQq, string group)
        {
            return IntPtrToString(Xqdll.GetGroupName(AuthId, robotQq, group));
        }

        /// <summary>
        /// 取群人数上线与当前人数 换行符分隔
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetGroupMemberNum(string robotQq, string group)
        {
            return IntPtrToString(Xqdll.GetGroupMemberNum(AuthId, robotQq, group));
        }

        /// <summary>
        /// 取群等级
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static int GetGroupLv(string robotQq, string group)
        {
            return Xqdll.GetGroupLv(AuthId, robotQq, group);
        }

        /// <summary>
        /// 取群成员列表
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetGroupMemberList_B(string robotQq, string group)
        {
            return IntPtrToString(Xqdll.GetGroupMemberList_B(AuthId, robotQq, group));
        }

        /// <summary>
        /// 封包模式取群成员列表返回重组后的json文本
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string GetGroupMemberList_C(string robotQq, string group)
        {
            return IntPtrToString(Xqdll.GetGroupMemberList_C(AuthId, robotQq, group));
        }

        /// <summary>
        /// 检查指定QQ是否在线
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool IsOnline(string robotQq, string qq)
        {
            return Xqdll.IsOnline(AuthId, robotQq, qq);
        }

        /// <summary>
        /// 取机器人账号在线信息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string GetRInf(string robotQq)
        {
            return IntPtrToString(Xqdll.GetRInf(AuthId, robotQq));
        }

        /// <summary>
        /// 查询指定群是否允许匿名消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool GetAnon(string robotQq, string group)
        {
            return Xqdll.GetAnon(AuthId, robotQq, group);
        }

        /// <summary>
        /// 通过图片GUID获取图片下载链接
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="imageType"></param>
        /// <param name="group"></param>
        /// <param name="imageGuid"></param>
        /// <returns></returns>
        public static string GetPicLink(string robotQq, int imageType, string group, string imageGuid)
        {
            return IntPtrToString(Xqdll.GetPicLink(AuthId, robotQq, imageType, group, imageGuid));
        }

        /// <summary>
        /// 获取指定QQ个人资料的年龄
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static int GetAge(string robotQq, string qq)
        {
            return Xqdll.GetAge(AuthId, robotQq, qq);
        }

        /// <summary>
        /// 获取QQ个人资料的性别
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static int GetGender(string robotQq, string qq)
        {
            return Xqdll.GetGender(AuthId, robotQq, qq);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="messageType">信息类型</param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="message"></param>
        /// <param name="bubbleId">气泡ID</param>
        public static void SendMsg(string robotQq, int messageType, string group, string qq, string message, int bubbleId)
        {
            Xqdll.SendMsgEX(AuthId, robotQq, messageType, group, qq, message, bubbleId, false);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="messageType"></param>
        /// <param name="groupOrQq"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string UpLoadPic(string robotQq, int messageType, string groupOrQq, byte[] message)
        {
            return IntPtrToString(Xqdll.UpLoadPic(AuthId, robotQq, messageType, groupOrQq, message));
        }

        /// <summary>
        /// 群禁言
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="time"></param>
        public static void ShutUp(string robotQq, string group, string qq, int time)
        {
            Xqdll.ShutUP(AuthId, robotQq, group, qq, time);
        }

        /// <summary>
        ///  修改群成员昵称
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool SetGroupCard(string robotQq, string group, string qq, string card)
        {
            return Xqdll.SetGroupCard(AuthId, robotQq, group, qq, card);
        }

        /// <summary>
        /// 群删除成员
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="allow"></param>
        public static void KickGroupMbr(string robotQq, string group, string qq, bool allow)
        {
            Xqdll.KickGroupMBR(AuthId, robotQq, group, qq, allow);
        }

        /// <summary>
        /// 修改QQ在线状态
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="onLineType"></param>
        /// <param name="message"></param>
        public static void SetRInf(string robotQq, string onLineType, string message)
        {
            Xqdll.SetRInf(AuthId, robotQq, onLineType, message);
        }

        /// <summary>
        /// 发布群公告
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="messageTitle"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool PbGroupNotic(string robotQq, string group, string messageTitle, string message)
        {
            return Xqdll.PBGroupNotic(AuthId, robotQq, group, messageTitle, message);
        }

        /// <summary>
        /// 撤回群消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="messageNumber"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static string WithdrawMsg(string robotQq, string group, string messageNumber, string messageId)
        {
            return IntPtrToString(Xqdll.WithdrawMsg(AuthId, robotQq, group, messageNumber, messageId));
        }

        /// <summary>
        /// 输出日志 (在框架中显示)
        /// </summary>
        /// <param name="message"></param>
        public static void OutPutLog(string message)
        {
            Xqdll.OutPutLog(AuthId, message);
        }

        /// <summary>
        /// 提取图片文字
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="imageMessage"></param>
        /// <returns></returns>
        public static string OcrPic(string robotQq, byte[] imageMessage)
        {
            return IntPtrToString(Xqdll.OcrPic(AuthId, robotQq, imageMessage));
        }

        /// <summary>
        /// 主动加群
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="message"></param>
        public static void JoinGroup(string robotQq, string group, string message)
        {
            Xqdll.JoinGroup(AuthId, robotQq, group, message);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string UpVote(string robotQq, string qq)
        {
            return IntPtrToString(Xqdll.UpVote(AuthId, robotQq, qq));
        }

        /// <summary>
        /// 通过列表或群临时通道点赞
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string UpVote_temp(string robotQq, string qq)
        {
            return IntPtrToString(Xqdll.UpVote_temp(AuthId, robotQq, qq));
        }

        /// <summary>
        /// 置好友添加请求
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        public static void HandleFriendEvent(string robotQq, string qq, int messageType, string message)
        {
            Xqdll.HandleFriendEvent(AuthId, robotQq, qq, messageType, message);
        }

        /// <summary>
        /// 置群请求
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="requestType"></param>
        /// <param name="qq"></param>
        /// <param name="group"></param>
        /// <param name="seq"></param>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        public static void HandleGroupEvent(string robotQq, int requestType, string qq, string group, string seq, int messageType, string message)
        {
            Xqdll.HandleGroupEvent(AuthId, robotQq, requestType, qq, group, seq, messageType, message);
        }

        /// <summary>
        /// 向框架添加一个QQ
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="automatic"></param>
        /// <returns></returns>
        public static string AddQq(string account, string password, bool automatic)
        {
            return IntPtrToString(Xqdll.AddQQ(AuthId, account, password, automatic));
        }

        /// <summary>
        /// 登录指定QQ
        /// </summary>
        /// <param name="qq"></param>
        public static void LoginQq(string qq)
        {
            Xqdll.LoginQQ(AuthId, qq);
        }

        /// <summary>
        /// 离线指定QQ
        /// </summary>
        /// <param name="qq"></param>
        public static void OffLineQq(string qq)
        {
            Xqdll.OffLineQQ(AuthId, qq);
        }

        /// <summary>
        /// 删除指定QQ
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string DelQq(string qq)
        {
            return IntPtrToString(Xqdll.DelQQ(AuthId, qq));
        }

        /// <summary>
        /// 删除指定好友
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool DelFriend(string robotQq, string qq)
        {
            return Xqdll.DelFriend(AuthId, robotQq, qq);
        }

        /// <summary>
        /// 修改好友备注名称
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <param name="message"></param>
        public static void SetFriendsRemark(string robotQq, string qq, string message)
        {
            Xqdll.SetFriendsRemark(AuthId, robotQq, qq, message);
        }

        /// <summary>
        /// 邀请好友加入群
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        public static void InviteGroup(string robotQq, string group, string qq)
        {
            Xqdll.InviteGroup(AuthId, robotQq, group, qq);
        }

        /// <summary>
        /// 邀请群成员加入群
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="groupY"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool InviteGroupMember(string robotQq, string group, string groupY, string qq)
        {
            return Xqdll.InviteGroupMember(AuthId, robotQq, group, groupY, qq);
        }

        /// <summary>
        /// 创建群 组包模式
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string CreateDisGroup(string robotQq)
        {
            return IntPtrToString(Xqdll.CreateDisGroup(AuthId, robotQq));
        }

        /// <summary>
        ///  创建群 群官网Http模式
        /// </summary>
        /// <param name="robotQq"></param>
        /// <returns></returns>
        public static string CreateGroup(string robotQq)
        {
            return IntPtrToString(Xqdll.CreateGroup(AuthId, robotQq));
        }

        /// <summary>
        /// 退出群
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        public static void QuitGroup(string robotQq, string group)
        {
            Xqdll.QuitGroup(AuthId, robotQq, group);
        }

        /// <summary>
        /// 屏蔽或接收某群消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="messageType"></param>
        public static void SetShieldedGroup(string robotQq, string group, bool messageType)
        {
            Xqdll.SetShieldedGroup(AuthId, robotQq, group, messageType);
        }

        /// <summary>
        /// 多功能删除好友 可删除陌生人或者删除为单项好友
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <param name="messageType"></param>
        public static void DelFriend_A(string robotQq, string qq, int messageType)
        {
            Xqdll.DelFriend_A(AuthId, robotQq, qq, messageType);
        }

        /// <summary>
        /// 设置机器人被添加好友时的验证方式
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="messageType"></param>
        public static void Setcation(string robotQq, int messageType)
        {
            Xqdll.Setcation(AuthId, robotQq, messageType);
        }

        /// <summary>
        /// 设置机器人被添加好友时的问题与答案
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="problem"></param>
        /// <param name="answer"></param>
        public static void Setcation_problem_A(string robotQq, string problem, string answer)
        {
            Xqdll.Setcation_problem_A(AuthId, robotQq, problem, answer);
        }

        /// <summary>
        /// 设置机器人被添加好友时的三个可选问题
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="problem1"></param>
        /// <param name="problem2"></param>
        /// <param name="problem3"></param>
        public static void Setcation_problem_B(string robotQq, string problem1, string problem2, string problem3)
        {
            Xqdll.Setcation_problem_B(AuthId, robotQq, problem1, problem2, problem3);
        }

        /// <summary>
        /// 主动添加好友
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <param name="message"></param>
        /// <param name="xxlay"></param>
        /// <returns></returns>
        public static bool AddFriend(string robotQq, string qq, string message, int xxlay)
        {
            return Xqdll.AddFriend(AuthId, robotQq, qq, message, xxlay);
        }

        /// <summary>
        /// 发送json结构消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="sendType"></param>
        /// <param name="messageType"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="jsonMessage"></param>
        public static void SendJson(string robotQq, int sendType, int messageType, string group, string qq, string jsonMessage)
        {
            Xqdll.SendJSON(AuthId, robotQq, sendType, messageType, group, qq, jsonMessage);
        }

        /// <summary>
        /// 发送xml结构消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="sendType"></param>
        /// <param name="messageType"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="xmlMessage"></param>
        public static void SendXml(string robotQq, int sendType, int messageType, string group, string qq, string xmlMessage)
        {
            Xqdll.SendXML(AuthId, robotQq, sendType, messageType, group, qq, xmlMessage);
        }

        /// <summary>
        /// 上传silk语音文件
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="sendType"></param>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string UpLoadVoice(string robotQq, int sendType, string group, byte[] message)
        {
            return IntPtrToString(Xqdll.UpLoadVoice(AuthId, robotQq, sendType, group, message));
        }

        /// <summary>
        /// 发送普通消息支持群匿名方式
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="messageType"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="message"></param>
        /// <param name="bubbleId"></param>
        /// <param name="anonymous"></param>
        /// <returns></returns>
        public static string SendMsgEx(string robotQq, int messageType, string group, string qq, string message, int bubbleId, bool anonymous)
        {
            return IntPtrToString(Xqdll.SendMsgEX(AuthId, robotQq, messageType, group, qq, message, bubbleId, anonymous));
        }

        /// <summary>
        /// 通过语音GUID获取语音文件下载连接
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetVoiLink(string robotQq, string message)
        {
            return IntPtrToString(Xqdll.GetVoiLink(AuthId, robotQq, message));
        }

        /// <summary>
        /// 开关群匿名功能
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="kg"></param>
        /// <returns></returns>
        public static bool SetAnon(string robotQq, string group, bool kg)
        {
            return Xqdll.SetAnon(AuthId, robotQq, group, kg);
        }

        /// <summary>
        /// 修改机器人自身头像
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SetHeadPic(string robotQq, byte[] message)
        {
            return Xqdll.SetHeadPic(AuthId, robotQq, message);
        }

        /// <summary>
        /// 语音GUID转换为文本内容
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="ckdx"></param>
        /// <param name="cklx"></param>
        /// <param name="yyGuid"></param>
        /// <returns></returns>
        public static string VoiToText(string robotQq, string ckdx, int cklx, string yyGuid)
        {
            return IntPtrToString(Xqdll.VoiToText(AuthId, robotQq, ckdx, cklx, yyGuid));
        }

        /// <summary>
        /// 群签到
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="group"></param>
        /// <param name="address"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SignIn(string robotQq, string group, string address, string message)
        {
            return Xqdll.SignIn(AuthId, robotQq, group, address, message);
        }

        /// <summary>
        /// 向好友发送窗口抖动消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool ShakeWindow(string robotQq, string qq)
        {
            return Xqdll.ShakeWindow(AuthId, robotQq, qq);
        }

        /// <summary>
        /// 同步发送消息 有返回值可以用来撤回机器人自己发送的消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="messageType"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="message"></param>
        /// <param name="bubbleId"></param>
        /// <param name="anonymous"></param>
        /// <param name="jsonMessage"></param>
        /// <returns></returns>
        public static string SendMsgEX_V2(string robotQq, int messageType, string group, string qq, string message, int bubbleId, bool anonymous, string jsonMessage)
        {
            return IntPtrToString(Xqdll.SendMsgEX_V2(AuthId, robotQq, messageType, group, qq, message, bubbleId, anonymous, jsonMessage));
        }

        /// <summary>
        /// 撤回群消息或者私聊消息
        /// </summary>
        /// <param name="robotQq"></param>
        /// <param name="withdrawType"></param>
        /// <param name="group"></param>
        /// <param name="qq"></param>
        /// <param name="messageNumber"></param>
        /// <param name="messageId"></param>
        /// <param name="messageTime"></param>
        /// <returns></returns>
        public static string WithdrawMsgEx(string robotQq, int withdrawType, string group, string qq, string messageNumber, string messageId, string messageTime)
        {
            return IntPtrToString(Xqdll.WithdrawMsgEX(AuthId, robotQq, withdrawType, group, qq, messageNumber, messageId, messageTime));
        }

        /// <summary>
        /// 标记函数执行流程 debug时使用 每个函数内只需要调用一次
        /// </summary>
        /// <param name="message"></param>
        public static void DbgName(string message)
        {
            Xqdll.DbgName(AuthId, message);
        }

        /// <summary>
        /// 函数内标记附加信息 函数内可多次调用
        /// </summary>
        /// <param name="message"></param>
        public static void Mark(string message)
        {
            Xqdll.Mark(AuthId, message);
        }

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
                return "错误:" + ex.Message;
            }
        }

        /// <summary>
        /// 获取图片ImageGuid
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<string> ImageGuid(string message)
        {
            try
            {
                if (!message.Contains("[pic=")) return null;
                var str = Regex.Matches(message, @"([pic])(.)+?(?=\])");
                return (from Match item in str select $"[{item.Value}]").ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 图片转byte[]
        /// </summary>
        /// <param name="url">网路地址</param>
        /// <returns></returns>
        public static byte[] HttpImageByte(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                byte[] retult;
                using (WebResponse webResponse = request.GetResponse())
                {
                    int length = (int)webResponse.ContentLength;
                    if (!(webResponse is HttpWebResponse response)) return null;
                    Stream stream = response.GetResponseStream();

                    //读取到内存
                    MemoryStream stmMemory = new MemoryStream();
                    byte[] buffer1 = new byte[length];
                    int i;
                    while (stream != null && (i = stream.Read(buffer1, 0, buffer1.Length)) > 0)
                    {
                        stmMemory.Write(buffer1, 0, i);
                    }
                    byte[] arraryByte = stmMemory.ToArray();
                    retult = arraryByte;
                    stmMemory.Close();
                }
                return retult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 图片转byte[]
        /// </summary>
        /// <param name="url">本地路径</param>
        /// <returns></returns>
        public static byte[] FileImageByte(string url)
        {
            try
            {
                using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read))
                {
                    byte[] arraryByte;
                    using (BinaryReader binaryWriter = new BinaryReader(fs))
                    {
                        arraryByte = binaryWriter.ReadBytes((int)fs.Length);
                        binaryWriter.Close();
                    }
                    fs.Close();
                    return arraryByte;
                }
            }
            catch
            {
                return null;
            }
        }

        public static string GetVer()
        {
            return IntPtrToString(Xqdll.GetVer());
        }
    }
}