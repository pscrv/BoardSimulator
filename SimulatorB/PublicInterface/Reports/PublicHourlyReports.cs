using System;
using System.Collections.Generic;

namespace SimulatorB.PublicInterface
{
    public class PublicHourlyReports
    {
        private readonly Dictionary<int, PublicBoardReport> _reports;


        internal PublicHourlyReports(Dictionary<int, PublicBoardReport> reports)
        {
            _reports = reports;
        }
       

        public PublicBoardReport Read(int hour)
        {
            return _reports[hour];
        }
    }
}