using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    internal class ChairChooser
    {
        #region fields and properties
        private Member _boardChair;
        private List<Member> _secondaryChairs = new List<Member>();
        private List<int> _secondaryPercentages = new List<int>();
        //private Random _randomSource = new Random();


        private int _totalSecondaryPercentage => _secondaryPercentages.Sum();
        private int _chairPercentage => 100 - _totalSecondaryPercentage;

        #endregion


        #region construction
        internal ChairChooser(Member boardChair)
        {
            _boardChair = boardChair;
        }
        #endregion


        internal void AddSecondaryChair(Member member, int percentage)
        {
            if (_secondaryChairs.Contains(member))
                throw new InvalidOperationException("Member has already been added.");

            if (_totalSecondaryPercentage + percentage > 100)
                throw new InvalidOperationException("Atttempt to make percentages > 100");

            _secondaryChairs.Add(member);
            _secondaryPercentages.Add(percentage);
        }


        internal Member ChooseChair()
        {
            // TODO: consider whether it would be better to implement with
            // arrays rather than lists.

            // random initialised here so as always to produce the same sequence
            Random randomSource = new Random(1);
            int randomPercentage = randomSource.Next(100);

            int index = 0;
            int sum = 0;
            foreach (int percentage in _secondaryPercentages)
            {
                sum += _secondaryPercentages[index];
                if (sum >= randomPercentage)
                    return _secondaryChairs[index];
            }

            return _boardChair;
        }
    }
}
