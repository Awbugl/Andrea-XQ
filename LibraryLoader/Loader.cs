using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LibraryLoader
{
    public static class Loader
    {
        private static Dictionary<string, MethodInfo> _dir;
        private static bool _needreload;

        static Loader()
        {
            Init();
        }

        private static void Init()
        {
            try
            {
                _dir = new();
                Assembly assm = Assembly.Load(File.ReadAllBytes("AndreaSourse/XQBridge/AndreaBot.XQBridge.dll"));
                
                var mes = assm.GetType("AndreaBot.XQBridge.Main")
                    .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

                foreach (var methodInfo in mes)
                {
                    _dir.Add(methodInfo.Name, methodInfo);
                }

                _needreload = false;
            }
            catch (Exception e)
            {
                File.WriteAllText("1.txt", e.ToString());
            }
        }

        public static string XQ_Create(string frameworkversion)
        {
            if (_needreload) Init();
            return _dir["Create"].Invoke(null, null) as string;
        }


        public static int XQ_DestroyPlugin()
        {
            try
            {
                _needreload = true;
                return (int)_dir["Destory"].Invoke(null, null);
            }
            finally
            {
                var curr = AppDomain.CurrentDomain;
                var act = new Action(() => AppDomain.Unload(curr));
                AppDomain.CreateDomain("UnloadLoader").DoCallBack(act.Invoke);
            }
        }

        public static int XQ_SetUp()
        {
            return 0;
        }

        public static void XQ_AuthId(int id, int i)
        {
            if (_needreload) Init();
            _dir["AuthId"].Invoke(null, new object[] { id, i });
        }

        public static int XQ_Event(string robotQq, int eventType, int extraType, string fromGroup, string fromQq,
            string targetQq, string content, string index, string msgid, string udpmsg, string unix, int p)
        {
            if (_needreload) Init();

            if (eventType == 12002)
                _needreload = true;

            return (int)_dir["Event"].Invoke(null,
                new object[] { robotQq, eventType, fromGroup, fromQq, targetQq, content, udpmsg });
        }
    }
}