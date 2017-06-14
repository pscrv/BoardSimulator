using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    internal class Timeline : IEnumerable<Hour>
    {
        private int _length;
        private Dictionary<Hour, BoardState> _timeline;

        public Timeline (int length)
        {
            _length = length;
            _timeline = new Dictionary<Hour, BoardState>(length);
        }

        internal void AddBoardState(Hour hour, BoardState boardState)
        {
            _timeline.Add(hour, boardState);
        }

        #region IEnumerable
        public IEnumerator<Hour> GetEnumerator()
        {
            for (int i = 0; i < _length; i++)
            {
                yield return new Hour(i);
            }
        }
                
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
