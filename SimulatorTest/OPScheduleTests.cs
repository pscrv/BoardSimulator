using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Tests
{
    [TestClass()]
    public class OPScheduleTests
    {
        //private OPSchedule _schedule;
        //private Timespan _span;
        //private List < BoardMember > _members;
        //private BoardMember _bm1, _bm2;
        

        //public OPScheduleTests()
        //{
        //    //_span = new Timespan(100);
        //    //_bm1 = new BoardMember();
        //    //_bm2 = new BoardMember();
        //    //_members = new List<BoardMember> { _bm1, _bm2 };         
        //}



        //[TestMethod()]
        //public void CanCreate()
        //{
        //    try
        //    {
        //        _schedule = new OPSchedule(_members, _span);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail("constructor threw an exception with message: " + e.Message);
        //    }
        //}

        //[TestMethod()]
        //public void ScheduleOPTest()
        //{
        //    _schedule = new OPSchedule(_members, _span);

        //    Hour hour1 = new Hour(30);
        //    Hour hour2 = new Hour(40);
        //    bool success;
        //    success = _schedule.TryScheduleOP(hour1, _bm1);
        //    Assert.IsTrue(success);
        //    success = _schedule.TryScheduleOP(hour1, _bm2);
        //    Assert.IsTrue(success);
        //    success = _schedule.TryScheduleOP(hour2, _bm1);
        //    Assert.IsFalse(success);
        //}

        //[TestMethod()]
        //public void HasOPTest()
        //{
        //    //_schedule = new OPSchedule(_members, _span);

        //    //Hour hour1 = new Hour(30);
        //    //Hour hour2 = new Hour(34);
        //    //Hour hour3 = new Hour(28);
        //    //Hour hour4 = new Hour(60);
        //    //_schedule.TryScheduleOP(hour1, _bm1);

        //    //bool busy;

        //    //busy = _schedule.HasOP(hour1, _bm1);
        //    //Assert.IsTrue(busy);
        //    //busy = _schedule.HasOP(hour2, _bm1);
        //    //Assert.IsTrue(busy);
        //    //busy = _schedule.HasOP(hour3, _bm1);
        //    //Assert.IsTrue(busy);  //TODO: change when BoardMembers have real lead times.
        //    //busy = _schedule.HasOP(hour4, _bm1);
        //    //Assert.IsFalse(busy);


        //}
    }
}