using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal class Board
    {
        internal enum ChairType { Technical, Legal }

        #region private fields
        private BoardWorker _chair;
        private ChairType _chairType;
        private List<BoardWorker> _technicalMembers;
        private List<BoardWorker> _legalMembers;
        #endregion


        #region properties
        internal int TotalEnqueuedCount
        {
            get
            {
                int count = 0;
                foreach (BoardWorker worker in Members())
                {
                    count += worker.TotalWorkCount;
                }
                return count;
            }
        }
        #endregion

        #region constructors
        internal Board (BoardWorker ch, ChairType chtype, List<BoardWorker> tech, List<BoardWorker> legal)
        {
            _chair = ch;
            _chairType = chtype;
            _technicalMembers = tech;
            _legalMembers = legal;
        }
        #endregion


        #region private methods
        private IEnumerable<BoardWorker> Members()
        {
            yield return _chair;
            foreach (BoardWorker bm in _technicalMembers)
                yield return bm;
            foreach (BoardWorker bm in _legalMembers)
                yield return bm;
        }
        #endregion


        #region internal methods

        internal void EnqueueCase(Case c, Hour h)
        {
            // TODO: deal properly with c, when it is more than an empty object

            AllocatedCase allocated = _allocate(c);
            allocated.Rapporteur.EnqueueRapporteurWork(new SummonsCase(), h);
        }

        internal HourlyBoardLog DoWork()
        {
            HourlyBoardLog boardLog = new HourlyBoardLog();
            HourlyworkerLog log;
            foreach (BoardWorker member in Members())
            {
                log = member.DoWork();
                boardLog.Add(member, log);
            }
            return boardLog;
        }
        #endregion

        #region private methods
        private AllocatedCase _allocate(Case c)
        {
            //  TODO: make a proper allocation routine
            
            switch (_chairType)
            {
                case ChairType.Technical:
                    return new AllocatedCase(
                        c, 
                        _chair, 
                        _leastBusyMember(_technicalMembers), 
                        _leastBusyMember(_legalMembers)
                        );
                case ChairType.Legal:
                    return new AllocatedCase(
                        c, 
                        _chair, 
                        _leastBusyMember(_technicalMembers), 
                        _leastBusyMember(_technicalMembers)
                        );                    
            }
            
            throw new Exception("ChairType invalid.");
        }

        private BoardWorker _leastBusyMember(List<BoardWorker> workers)
        {
            // TODO:  make this efficient  -> extension method
            int minvalue = workers.Min(x => x.TotalWorkCount);
            return workers.Where(x => x.TotalWorkCount == minvalue).First();
        }
        #endregion
    }
}