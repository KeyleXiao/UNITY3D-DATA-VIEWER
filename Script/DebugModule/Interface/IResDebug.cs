
namespace DebugModule
{
    public interface IResDebug
    {
        /// <summary>
        /// 是否启用检查
        /// 正式环境请务必关闭此选项
        /// </summary>
        bool CheckerWorkStatus();

        /// <summary>
        /// Log记录
        /// </summary>
        /// <param name="content"></param>
        /// <param name="record">是否记录到list中</param>
        void PrintLog(string content,bool record = true);

        /// <summary>
        /// 保存到本地
        /// </summary>
        /// <param name="path">默认路径为项目根目录</param>
        void SaveLog2Disk(string path = "");

        /// <summary>
        /// 校验当前数据
        /// </summary>
        void Check(object obj);

        /// <summary>
        /// 添加校验逻辑
        /// </summary>
        /// <param name="ck"></param>
        void AddChecker(IResChecker ck);

        /// <summary>
        /// 当前程序退出的时候调用次函数(需要手动)
        /// </summary>
        void OnApplicationQuit();
    }
}
