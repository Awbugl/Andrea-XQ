using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace LibraryLoader
{
    public static class Loader
    {
        private static Dictionary<string, MethodInfo> _dir;
        private static bool _needreload;

        static Loader()
        {
            Init();
            AppDomain.CurrentDomain.UnhandledException += (_, e) => ExceptionReport(e.ExceptionObject as Exception);
        }

        private static void ExceptionReport(Exception ex)
        {
            var dir = $"{AppContext.BaseDirectory}/Log/{DateTime.Now:yyyy-M-d}/";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            try
            {
                File.AppendAllText($"{dir}LoaderErr.log", $"{DateTime.Now:T}\n{ex}\n\n");
            }
            catch
            {
                Thread.Sleep(2000);
                ExceptionReport(ex);
            }
        }

        private static void Init()
        {
            try
            {
                _dir = new();
                _needreload = false;
                Assembly assm = Assembly.Load(File.ReadAllBytes("Plugin/AndreaBot.XQBridge.dll"));

                var mes = assm.GetType("AndreaBot.XQBridge.Main")
                    .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

                foreach (var methodInfo in mes)
                {
                    _dir.Add(methodInfo.Name, methodInfo);
                }

                File.AppendAllText("LoaderLog.log", DateTime.Now.ToShortTimeString() + "     Loaded.\n");
            }
            catch (Exception e)
            {
                ExceptionReport(e);
            }
        }

        public static string XQ_Create(string frameworkversion)
        {
            if (_needreload) Init();
            try
            {
                return _dir.ContainsKey("Create") ? _dir["Create"].Invoke(null, null) as string : "";
            }
            catch (TargetInvocationException e)
            {
                ExceptionReport(e.InnerException);
                return "";
            }
        }


        public static int XQ_DestroyPlugin()
        {
            try
            {
                return _dir.ContainsKey("Destory") ? (int)_dir["Destory"].Invoke(null, null) : 1;
            }
            catch (TargetInvocationException e)
            {
                ExceptionReport(e.InnerException);
                return 1;
            }
            finally
            {
                _needreload = true;
            }
        }

        public static int XQ_SetUp()
        {
            return 0;
        }

        public static void XQ_AuthId(int id, int i)
        {
            if (_needreload) Init();
            try
            {
                if (!_dir.ContainsKey("AuthId")) Thread.Sleep(10000);

                _dir["AuthId"].Invoke(null, new object[] { id, i });
            }
            catch (TargetInvocationException e)
            {
                ExceptionReport(e.InnerException);
            }
        }

        public static int XQ_Event(string robotQq, int eventType, int extraType, string fromGroup, string fromQq,
            string targetQq, string content, string index, string msgid, string udpmsg, string unix, int p)
        {
            if (_needreload) Init();

            if (eventType == 12002)
                _needreload = true;
            try
            {
                return _dir.ContainsKey("Event")
                    ? (int)_dir["Event"].Invoke(null,
                        new object[] { robotQq, eventType, fromGroup, fromQq, targetQq, content, udpmsg })
                    : 1;
            }
            catch (TargetInvocationException e)
            {
                ExceptionReport(e.InnerException);
                return 1;
            }
        }
    }
}