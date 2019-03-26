using System;
using UnityEngine;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace DebugModule
{
    public class BaseResDebug : IResDebug
    {
        private const string _title = "<color=yellow>资源管理器Debug信息:</color>\n";
        private StringBuilder _sb = new StringBuilder(_title);
        private List<IResChecker> _checkList { get; set; }

        public virtual void PrintLog(string content, bool record = true)
        {
            content = string.Format("{0} : {1} \n ", DateTime.Now.ToString("HH:mm:ss"), content);
            UnityEngine.Debug.Log(content);

            if (!record) return;
            _sb.Append(content);
        }

        public virtual void SaveLog2Disk(string path)
        {
            if (string.IsNullOrEmpty(path) || !path.EndsWith(".txt") || !File.Exists(path))
            {
                path = Path.Combine(Application.persistentDataPath, "DEBUG_INFOMATION.txt");
            }
            File.WriteAllText(path, _sb.ToString());
        }

        /// <summary>
        /// 设置Check代码
        /// </summary>
        /// <param name="ck"></param>
        public virtual void AddChecker(IResChecker ck)
        {
            if (_checkList == null) _checkList = new List<IResChecker>();
            if (ck == null) return;
            _checkList.Add(ck);
        }

        /// <summary>
        /// 开始校验
        /// </summary>
        public virtual void Check(object obj)
        {
            if (_checkList == null || _checkList.Count == 0) return;

            for (int i = 0; i < _checkList.Count; i++)
            {
                var info = _checkList[i].Check(obj);
                if (string.IsNullOrEmpty(info))
                    continue;

                PrintLog(info);
            }
        }

        /// <summary>
        /// 默认启用 Checker
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckerWorkStatus()
        {
            return true;
        }

        /// <summary>
        /// 当前程序退出执行
        /// </summary>
        public virtual void OnApplicationQuit()
        {
            var content = string.Empty;
            for (int i = 0; i < _checkList.Count; i++)
            {
                content = _checkList[0].OnApplicationQuit();
                if (!string.IsNullOrEmpty(content))
                {
                    PrintLog(content);
                }
            }
        }
    }
}
