using System.Threading.Tasks;

namespace UncomplicatedCustomServerCore.Extensions
{
    internal static class TaskExtension
    {
        public static T ReSync<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
