using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Simulator;

namespace SimulatorUI
{
    public class BoardParameters
    {
        #region static
        private static ChairType __type = ChairType.Technical;

        private static MemberParameterCollection __chair = new MemberParameterCollection(
            new MemberParameters(6, 6, 12),
            new MemberParameters(40, 8, 24),
            new MemberParameters(3, 4, 8),
            100);
        
        private static MemberParameterCollection __defaultTechnical()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }

        private static MemberParameterCollection __defaultLegal()
        {
            return new MemberParameterCollection(
                    new MemberParameters(7, 7, 13),
                    new MemberParameters(41, 9, 25),
                    new MemberParameters(4, 5, 9));
        }


        private static List<MemberParameterCollection> __technicals = new List<MemberParameterCollection>
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

        private static List<MemberParameterCollection> __legals = new List<MemberParameterCollection>
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


        public static BoardParameters __DefaultParameters()
        {
            return new BoardParameters(
                __type,
                __chair,
                __technicals,
                __legals);
        }        
        #endregion



        public ChairType ChairType;
        public MemberParameterCollection Chair;
        public List<MemberParameterCollection> Technicals;
        public List<MemberParameterCollection> Legals;


        public Simulator.BoardParameters AsSimulatorBoardParameters
        {
            get
            {
                return new Simulator.BoardParameters(
                    ChairType, 
                    Chair.AsSimulatorParameters, 
                    _asSimulatorParameters(Technicals), 
                    _asSimulatorParameters(Legals));
            }
        }



        public BoardParameters(
            ChairType chairType, 
            MemberParameterCollection chair, 
            List<MemberParameterCollection> technicals,
            List<MemberParameterCollection> legals)
        {
            ChairType = chairType;
            Chair = chair;
            Technicals = technicals;
            Legals = legals;
        }



        private List<Simulator.MemberParameterCollection> _asSimulatorParameters(List<MemberParameterCollection> collectionList)
        {
            List<Simulator.MemberParameterCollection> result = new List<Simulator.MemberParameterCollection>();
            foreach (MemberParameterCollection parameterCollection in collectionList)
            {
                result.Add(parameterCollection.AsSimulatorParameters);
            }
            return result;
        }
        
    }
}