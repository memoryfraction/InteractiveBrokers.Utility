using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveBrokers.Utilities
{
    public class AccountSummary
    {
        public int AssignedTimes { get; set; }
        public double Cushion { get; set; }
        public double DayTradesRemaining { get; set; }
        public double LookAheadNextChange { get; set; }
        public double AccruedCash { get; set; }
        public double AvailableFunds { get; set; }
        public double BuyingPower { get; set; }
        public double EquityWithLoadValue { get; set; }
        public double ExcessLiquidity { get; set; }
        public double FullAvailableFunds { get; set; }
        public double FullExcessLiquidity { get; set; }
        public double FullInitMarginReq { get; set; }
        public double FullMaintMarginReq { get; set; }
        public double GrossPositionValue { get; set; }
        public double InitMarginReq { get; set; }
        public double LookAheadAvailableFunds { get; set; }
        public double LookAheadExcessLiquidity { get; set; }
        public double LookAheadInitMarginReq { get; set; }
        public double LookAheadMaintMarginReq { get; set; }
        public double MaintMarginReq { get; set; }
        public double NetLiquidation { get; set; }
        public double SMA { get; set; }
        public double TotalCashValue { get; set; }
    }
}
