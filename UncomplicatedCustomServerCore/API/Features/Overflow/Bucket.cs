using System.Collections.Generic;
using System.Threading.Tasks;

namespace UncomplicatedCustomServerCore.API.Features.Overflow
{
    public static class Bucket
    {
        public readonly static List<string> Elements = [];

        public static void Add(string processName) => Elements.Add(processName);

        public static bool Check(string processName) => Elements.Contains(processName);

        public static bool CanExecute(string processName)
        {
            if (Check(processName))
                return false;

            Add(processName);
            return true;
        }

        public static void Remove(string processName) => Elements.RemoveAll(l => l == processName);

        public static void ChronoRemove(string processName, int time = 2)
        {
            if (Check(processName))
                Task.Run(async delegate
                {
                    await Task.Delay(time * 1000);
                    Remove(processName);
                });
        }
    }
}
