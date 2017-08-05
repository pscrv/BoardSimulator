using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Simulator;

namespace SimulatorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            int simulationLength = 1000;
            int initialCaseCount = 10;

            ChairType type = ChairType.Technical;
            MemberParameterCollection chair = new MemberParameterCollection(
                new MemberParameters(6, 6, 12),
                new MemberParameters(40, 8, 24),
                new MemberParameters(3, 4, 8));

            List<MemberParameterCollection> technicals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10))
            };

            List<MemberParameterCollection> legals = new List<MemberParameterCollection>
            {
                new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9)),

                new MemberParameterCollection(
                    new MemberParameters(8, 8, 14),
                    new MemberParameters(42, 10, 26),
                    new MemberParameters(5, 6, 10)),

                new MemberParameterCollection(
                    new MemberParameters(9, 9, 15),
                    new MemberParameters(43, 11, 27),
                    new MemberParameters(6, 7, 11))
            };

            BoardParameters boardParameters = new BoardParameters(
                type,
                chair,
                technicals,
                legals);


            Simulation simulation = new Simulation(simulationLength, boardParameters.AsSimulatorBoardParameters, initialCaseCount);
            simulation.Run();

            BoardParametersViewModel boardVM = new BoardParametersViewModel(boardParameters);
            boardVM.PropertyChanged += (s, e) => _miniSim(boardParameters, boardVM);
            boardVM.FinishedCaseCount = simulation.FinishedCases.Count;
            DataContext = boardVM;     
        }

        private void _miniSim(BoardParameters parameters, BoardParametersViewModel boardVM)
        {
            Simulation sim = new Simulation(1000, parameters.AsSimulatorBoardParameters, 100);
            sim.Run();
            boardVM.FinishedCaseCount = sim.FinishedCases.Count;
        }
    }
}
