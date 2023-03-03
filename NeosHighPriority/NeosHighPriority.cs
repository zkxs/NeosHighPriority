using NeosModLoader;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NeosHighPriority
{
    public class NeosHighPriority : NeosMod
    {
        internal const string VERSION = "1.0.2";

        public override string Name => "NeosHighPriority";
        public override string Author => "runtime";
        public override string Version => VERSION;
        public override string Link => "https://github.com/zkxs/NeosHighPriority";

        const ProcessPriorityClass TARGET_PRIORITY = ProcessPriorityClass.High;

        public override void OnEngineInit()
        {
            Debug("Getting current process...");
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                Debug("Running ProcessPriorityClass check...");
                if (Enum.IsDefined(typeof(ProcessPriorityClass), TARGET_PRIORITY))
                {
                    Debug("Backing up old priority...");
                    ProcessPriorityClass oldPriority = currentProcess.PriorityClass;
                    Debug("About to set priority to High");
                    try
                    {
                        currentProcess.PriorityClass = TARGET_PRIORITY;
                        Msg($"Changed process priority from {oldPriority} to {currentProcess.PriorityClass}");
                    }
                    catch (Win32Exception e)
                    {
                        Error($"Unable to change process priority from {oldPriority} to {TARGET_PRIORITY}: {e}");
                    }
                }
                else
                {
                    Warn("High priority is not valid!? Enumerating valid ProcessPriorityClass values:");
                    foreach (int value in Enum.GetValues(typeof(ProcessPriorityClass)))
                    {
                        string name = Enum.GetName(typeof(ProcessPriorityClass), value);
                        Warn($"{value} = {name}");
                    }
                }
            }
        }
    }
}
