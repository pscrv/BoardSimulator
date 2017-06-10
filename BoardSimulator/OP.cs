using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardSimulator
{
    class OP
    {
        #region private data fields
        private Chair _chair;
        private TechnicalMember _rapporteur;
        private uint _startDay;
        private uint _endDay;
        private uint _startHour;
        private uint _endHour;
        #endregion

        #region public access
        public TechnicalMember Rapporteur { get { return _rapporteur; } }
        internal uint StartDay { get { return _startDay; } }
        public uint EndDay { get { return _endDay; } }
        #endregion

        #region constructors
        public OP(Chair ch, TechnicalMember rapporteur, uint startDay, uint duration)
        {
            _chair = ch;
            _rapporteur = rapporteur;
            _startDay = startDay;
            _endDay = startDay + duration / Board.__HoursPerDay;

            _startHour = startDay * Board.__HoursPerDay;
            _endHour = _startHour + duration - 1;
        }
        #endregion


        internal bool IsRunning(uint _hour)
        {
            return (_hour >= _startHour && _hour <= _endHour);
        }

        internal bool IsOver(uint _hour)
        {
            return (_hour >= _endHour);
        }

        internal bool ChairIsBusy(uint _hour)
        {
            return (_hour >= _startHour - _chair.OPPreparationHours && _hour <= _endHour);
        }

        internal bool RapporteurIsbusy(uint _hour)
        {
            return (_hour >= _startHour - _rapporteur.OPPreparationHours && _hour <= _endHour);
        }
    }
}
