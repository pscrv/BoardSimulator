using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorUI
{

    public class MemberParameterCollection
    {
        public static MemberParameterCollection DefaultCollection()
        {
            return new MemberParameterCollection(
                new MemberParameters(16, 4, 8),
                new MemberParameters(40, 8, 24),
                new MemberParameters(8, 4, 8));
        }



        public readonly MemberParameters ChairWorkParameters;
        public readonly MemberParameters RapporteurWorkParameters;
        public readonly MemberParameters OtherWorkParameters;



        public Simulator.MemberParameterCollection AsSimulatorParameters
        {
            get
            {
                return new Simulator.MemberParameterCollection(
                    ChairWorkParameters.AsSimulatorParameters,
                    RapporteurWorkParameters.AsSimulatorParameters,
                    OtherWorkParameters.AsSimulatorParameters);
            }
        }



        public MemberParameterCollection(MemberParameters chair, MemberParameters rapporteur, MemberParameters other)
        {
            ChairWorkParameters = chair;
            RapporteurWorkParameters = rapporteur;
            OtherWorkParameters = other;
        }
    }

}
