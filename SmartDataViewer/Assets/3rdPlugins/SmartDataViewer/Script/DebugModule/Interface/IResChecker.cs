

namespace DebugModule
{
    public interface IResChecker
    {
        string Check(object obj);

        string OnApplicationQuit();
    }
}
