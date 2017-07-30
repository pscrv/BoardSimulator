using System;
using System.Collections.Generic;

namespace Simulator
{
    internal class Simulation
    {
        #region fields and properties
        private SimulationTimeSpan _timeSpan;
        private SimulationLog _log;
        private Board _board;

        private BoardQueue _boardQueue;
        private IncomingCaseQueue _incoming;
        private CirculationQueue _circulation;
        private OPSchedule _opSchedule;

        #endregion


        #region construction
        internal Simulation(int lengthInHours, BoardParameters boardParameters, IEnumerable<AppealCase> initialCases)
        {
            _timeSpan = new SimulationTimeSpan(new Hour(0), new Hour(lengthInHours));
            _log = new SimulationLog();

            _boardQueue = new BoardQueue();
            _incoming = new IncomingCaseQueue();
            _circulation = new CirculationQueue();
            _opSchedule = new OPSchedule(_circulation);


            Member chair = new Member(boardParameters.Chair);

            List<Member> technicals = new List<Member>();
            foreach (MemberParameterCollection parameters in boardParameters.Technicals)
            {
                technicals.Add( new Member(parameters));
            }

            List<Member> legals = new List<Member>();
            foreach (MemberParameterCollection  parameters in boardParameters.Legals)
            {
                legals.Add(new Member(parameters));
            }            

            _board = new Board(
                chair, 
                boardParameters.ChairType, 
                technicals, 
                legals,
                _incoming,
                //_circulation,
                _opSchedule);

            foreach (AppealCase ac in initialCases)
            {
                _board.ProcessNewCase(ac, _timeSpan.Start);
            }
        }
        #endregion


        internal void Run()
        {
            foreach (Hour hour in _timeSpan)
            {
                _board.DoWork(hour);
            }
        }
        
    }
}