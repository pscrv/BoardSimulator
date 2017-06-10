using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardSimulator
{
    class Information
    {
        static public string __Info =
            "This simulation considers only chairs and rapporteurs, who work as follows."
            + System.Environment.NewLine
            + System.Environment.NewLine
            + "Preparation and conduct of oral proceedings have absolute priority. When the time comes, that is what they do."
            + System.Environment.NewLine
            + "Otherwise, each takes a decision, if there is one waiting, or else a summons."
            + System.Environment.NewLine
            + System.Environment.NewLine
            + "Summons: a new case is started and worked on until a summons is produced. When the rapporteur is finished, it is passed to the chair. " 
            + "When the chair is finished, the summons is sent out and oral proceedings are scheduled. "
            + "You can set the average number of hours rapporteurs and chairs spend on this, which should include any back-and-forth passing."
            + System.Environment.NewLine
            + System.Environment.NewLine
            + "Decision: oral proceedings are over, the rapporteur works on the decision, and passes it to the chair. "
            + "When the chair has finished, the decision is issued."
            + System.Environment.NewLine
            + System.Environment.NewLine
            + "Oral proceedings: rapporteurs and chairs spend a number of hours in preparation, including any preparatory meeting. "
            + "You can set those, and also the minimum number of days beween proceedings."
            + System.Environment.NewLine
            + System.Environment.NewLine
            + "You can change various parameters  - click 'Run Simulation' to see the effect. It can take a few seconds to simulate twelve years."
            + System.Environment.NewLine
            + System.Environment.NewLine
            + "You can also see what each member is doing for each hour of the twelve years by clicking 'Show Activities', "
            + "but be patient while all the information is assembled."
            ;
    }
}
