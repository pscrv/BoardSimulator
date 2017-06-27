namespace Simulator
{
    internal class AllocatedCase
    {
        internal readonly Case Case;
        internal readonly BoardWorker Chair;
        internal readonly BoardWorker Rapporteur;
        internal readonly BoardWorker OtherMember;

        public AllocatedCase(Case c, BoardWorker chair, BoardWorker rapporteur, BoardWorker other)
        {
            Case = c;
            Chair = chair;
            Rapporteur = rapporteur;
            OtherMember = other;
        }
    }
}