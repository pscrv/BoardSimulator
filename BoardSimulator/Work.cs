using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardSimulator
{
    class Work
    {
        #region private data fields
        protected TechnicalMember _rapporteur;
        protected uint _workHours;
        #endregion

        #region public access
        internal TechnicalMember Rapporteur { get { return _rapporteur; } }
        internal uint WorkHours { get { return _workHours; } }
        #endregion

        #region constructors
        public Work(TechnicalMember m)
        {
            _rapporteur = m;
            _workHours = 0;
        }

        protected Work (TechnicalMember m, uint workhours)
        {
            _rapporteur = m;
            _workHours = workhours;
        }
        #endregion

        #region public methods
        internal void Reset()
        {
            _workHours = 0;
        }

        internal void DoWork()
        {
            _workHours++;
        }

        internal virtual Work Copy()
        {
            return new Work(this._rapporteur, this._workHours);
        }
        #endregion
    }

    class Summons : Work
    {
        public Summons(TechnicalMember m)
            : base(m)
        { }

        protected Summons(TechnicalMember m, uint workhours)
            : base (m, workhours)
        { }

        internal new Summons Copy()
        {
            return new Summons(this._rapporteur, this._workHours);
        }

    }

    class Decision : Work
    {
        public Decision(TechnicalMember m)
            : base (m)
        { }

        protected Decision(TechnicalMember m, uint workhours)
            :base(m, workhours)
        { }

        internal new Decision Copy()
        {
            return new Decision(this._rapporteur, this._workHours);
        }
    }

}
