using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class Engine
    {
        private Timeline _timeline;
        private Board _board;

        public Engine (int timelinelength)
        {
            _timeline = new Timeline(timelinelength);
            _board = new Board();
        }


        public void Run()
        {
            BoardState boardState = new NullboardState();

            foreach (Hour hour in _timeline)
            {
                boardState = _board.DoOneHourOfWork(boardState);
                _timeline.AddBoardState(hour, boardState);
            }
        }
    }
}
