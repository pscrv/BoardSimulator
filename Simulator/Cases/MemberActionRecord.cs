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




        internal void SetEnqueue(Hour currentHour)
        {
            if (Enqueue != null)
                throw new InvalidOperationException("Enqueue time can only be set once.");
        
            Enqueue = currentHour;
        }

        internal void SetStart(Hour currentHour)
        {
            if (Enqueue == null)
                throw new InvalidOperationException("Cannot set start before setting Enqueue.");

            if (Start != null)
                throw new InvalidOperationException("Start can only be set once." + currentHour);
            
            Start = currentHour;
        }

        internal void SetFinish(Hour currentHour)
        {
            if (Enqueue == null)
                throw new InvalidOperationException("Cannot set Finish before setting Enqueue.");
            if (Start == null)
                throw new InvalidOperationException("Cannot set Finish before setting Start.");
            if (Finish != null)
                throw new InvalidOperationException("Finish can only be set once.");
            
            Finish = currentHour;        
        }
    }
}