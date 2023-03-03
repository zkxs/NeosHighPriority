using NeosModLoader;
using System.Diagnostics;

namespace NeosHighPriority
{
    public class NeosHighPriority : NeosMod
    {
        internal const string VERSION = "1.0.1";

        public override string Name => "NeosHighPriority";
        public override string Author => "runtime";
        public override string Version => VERSION;
        public override string Link => "https://github.com/zkxs/NeosHighPriority";

        public override void OnEngineInit()
        {
            using (Process p = Process.GetCurrentProcess())
            {
                var oldPriority = p.PriorityClass;
                p.PriorityClass = ProcessPriorityClass.High;
                Msg($"Changed process priority from {oldPriority} to {p.PriorityClass}");
            }
        }
    }
}
