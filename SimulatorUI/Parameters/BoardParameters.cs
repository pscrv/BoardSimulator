using System;
using System.Collections.Generic;

using Simulator;

namespace SimulatorUI
{
    public class BoardParameters
    {
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