using System;
using System.Text.RegularExpressions;
using ET;
using Log = TEngine.Log;

namespace GameLogic
{
   public class TELogger: ILog
    {
        public void Trace(string msg)
        {
            Log.Info(msg);
            //UnityEngine.Debug.Log($"<color=gray><b>[Trace] ► </b></color> - {msg}");
        }

        public void Debug(string msg)
        {
            Log.Debug(msg);
        }

        public void Info(string msg)
        {
            Log.Info(msg);
        }

        public void Warning(string msg)
        {
            Log.Warning(msg);
        }

        public void Error(string msg)
        {
#if UNITY_EDITOR
            msg = Msg2LinkStackMsg(msg);
#endif
            Log.Error(msg);
            //UnityEngine.Debug.LogError($"<color=red><b>[ERROR] ► </b></color> - {msg}");
        }
        
        private static string Msg2LinkStackMsg(string msg)
        {
            msg = Regex.Replace(msg,@"at (.*?) in (.*?\.cs):(\w+)", match =>
            {
                string path = match.Groups[2].Value;
                string line = match.Groups[3].Value;
                return $"{match.Groups[1].Value}\n<a href=\"{path}\" line=\"{line}\">{path}:{line}</a>";
            });
            return msg;
        }

        public void Error(Exception e)
        {
            Log.Fatal(e);
            //UnityEngine.Debug.LogException(new Exception($"<color=red><b>[ERROR] ► </b></color> - {e}"));
        }

        public void Trace(string message, params object[] args)
        {
            Log.Info(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            Log.Warning(message, args);
        }

        public void Info(string message, params object[] args)
        {
            Log.Info(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            Log.Debug(message, args);
        }

        public void Error(string message, params object[] args)
        {
            Log.Error(message, args);
        }
    }
}