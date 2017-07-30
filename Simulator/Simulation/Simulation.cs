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
        #endregion


        #region construction
        internal Simulation(int lengthInHours, BoardParameters boardParameters, IEnumerable<AppealCase> initialCases)
        {
            _timeSpan = new SimulationTimeSpan(new Hour(0), new Hour(lengthInHours));
            _log = new SimulationLog();


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
                legals               
                );

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