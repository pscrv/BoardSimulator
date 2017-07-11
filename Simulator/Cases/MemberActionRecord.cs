using System;

namespace Simulator
{
    internal class ActionRecord
    {
        internal readonly WorkerRole Role;
        internal Hour Enqueue { get; private set; }
        internal Hour Start { get; private set; }
        internal Hour Finish { get; private set; }


        internal ActionRecord(WorkerRole role)
        {
            Role = role;
        }




        internal void SetEnqueue()
        {
            if (Enqueue != null)
                throw new InvalidOperationException("Enqueue time can only be set once.");
        
            Enqueue = SimulationTime.Current;
        }

        internal void SetStart()
        {
            if (Enqueue == null)
                throw new InvalidOperationException("Cannot set start before setting Enqueue.");

            if (Start != null)
                throw new InvalidOperationException("Start can only be set once.");
            
            Start = SimulationTime.Current;
        }

        internal void SetFinish()
        {
            if (Enqueue == null)
                throw new InvalidOperationException("Cannot set Finish before setting Enqueue.");
            if (Start == null)
                throw new InvalidOperationException("Cannot set Finish before setting Start.");
            if (Finish != null)
                throw new InvalidOperationException("Finish can only be set once.");
            
            Finish = SimulationTime.Current;        
        }
    }
}