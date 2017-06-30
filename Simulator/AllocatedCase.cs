namespace Simulator
{
    internal class AllocatedCase
    {
        private AppealCase _appealCase;
        private WorkerQueue _allocatedBoard;


        internal AppealCase AppealCase { get { return _appealCase; } }


        internal AllocatedCase(AppealCase appealCase, Member chair, Member rapporteur, Member other)
        {
            _appealCase = appealCase;
            _allocatedBoard = new WorkerQueue(chair, rapporteur, other);
        }


        internal Member NextSummonsWorker()
        {
            return _allocatedBoard.DequeueSummonsWorker();
        }
    }
}