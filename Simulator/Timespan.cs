using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulator
{
    internal class Timespan : IEnumerable<Hour>
    {
        List<Hour> _span;

        internal Timespan(int length)
        {
            _span = new List<Hour>(length);
            for (int i = 0; i < length; i++)
            {
                _span.Add(new Hour(i));
            }
        }

        public int Length { get { return _span.Count; } }

        public IEnumerator<Hour> GetEnumerator()
        {
            return _span.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}