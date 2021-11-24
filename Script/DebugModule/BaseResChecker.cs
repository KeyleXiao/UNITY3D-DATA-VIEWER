namespace DebugModule
{
    public abstract class BaseResChecker<T> : IResChecker
    {
        public virtual string Check(object obj)
        {
            if (!(obj is T))
            {
                return string.Empty;
            }

            return CheckCondition((T)obj);
        }

        /// <summary>
        /// 校验当前类型条件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract string CheckCondition(T obj);

        public virtual string OnApplicationQuit() { return string.Empty; }
    }
}
