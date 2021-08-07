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
                //
            }
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

                File.AppendAllText("LoaderLog.log", DateTime.Now.ToShortTimeString() + "     Loaded.\n");
                _needreload = false;
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
                return _dir["Create"].Invoke(null, null)as string;
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
                return (int)_dir["Destory"].Invoke(null, null);
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
            try
            {
                if (_needreload) Init();
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
                return (int)_dir["Event"].Invoke(null,
                    new object[] { robotQq, eventType, fromGroup, fromQq, targetQq, content, udpmsg });
            }
            catch (TargetInvocationException e)
            {
                ExceptionReport(e.InnerException);
                return 1;
            }
        }
    }
}