using NeosModLoader;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace NeosHighPriority
{
    public class NeosHighPriority : NeosMod
    {
        internal const string VERSION = "1.0.3";

        public override string Name => "NeosHighPriority";
        public override string Author => "runtime";
        public override string Version => VERSION;
        public override string Link => "https://github.com/zkxs/NeosHighPriority";

        public override void OnEngineInit()
        {
            Debug("Getting current process...");
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                // attempt to go all the way up
                if (!ChangePriority(currentProcess, ProcessPriorityClass.High, "High"))
                {
                    // fallback attempt at a lower priority
                    ChangePriority(currentProcess, ProcessPriorityClass.AboveNormal, "AboveNormal");
                }
            }
        }

        private bool ChangePriority(Process process, ProcessPriorityClass targetPriority, string targetPriorityName)
        {
            DebugFunc(() => $"Running ProcessPriorityClass.{targetPriorityName} check...");
            if (Enum.IsDefined(typeof(ProcessPriorityClass), targetPriority))
            {
                Debug("Backing up current priority...");
                ProcessPriorityClass oldPriority = process.PriorityClass;
                DebugFunc(() => $"About to set priority to {targetPriorityName}");
                try
                {
                    process.PriorityClass = targetPriority;
                    Msg($"Changed process priority from {oldPriority} to {process.PriorityClass}");
                    return true;
                }
                catch (Win32Exception e)
                {
                    Error($"Unable to change process priority from {oldPriority} to {targetPriority}: {e}");
                    return false;
                }
            }
            else
            {
                Warn($"{targetPriority} = {targetPriorityName} priority is not valid!? Enumerating valid ProcessPriorityClass values:");
                foreach (int value in Enum.GetValues(typeof(ProcessPriorityClass)))
                {
                    string name = Enum.GetName(typeof(ProcessPriorityClass), value);
                    Warn($"{value} = {name}");
                }
                return false;
            }
        }
    }
}
