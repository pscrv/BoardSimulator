namespace Simulator
{
    internal class HourlyWorkLog
    {
        protected bool _canLog;

        internal bool CanLog { get { return _canLog; } }

        internal HourlyWorkLog()
        {
            _canLog = false;
        }
    }

    
    internal class UnfilledLog : HourlyWorkLog
    {
        internal UnfilledLog()
        {
            _canLog = true;
        }
    }


}