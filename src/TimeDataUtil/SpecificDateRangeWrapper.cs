using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.TimeDataUtil
{
    
    /// <summary>
    /// OBSOLETE. kept for the code
    /// </summary>
   public class SpecificDateRangeWrapper
    {        private DateTimeRange range;

        public DateTime? End {
            get { return range.End; }
        }

        public DateTime? Start {
            get { return range.Start; }
        }

        public string ToString(string fString) {
            return range.ToString(fString);
        }

       

        public bool Includes(DateTime time) {
            return range.Includes(time);
        }

        public bool LaterThanStart(DateTime time) {
            return range.LaterEqualThanStart(time);
        }

        public bool EarlierThanEnd(DateTime time) {
            return range.EarlierEqualThanEnd(time);
        }
        
        
        /// <summary>
        /// Note that the end date will actually be the start second of the next date
        /// so there is always an at least 24 hours TimeSpan for a DateTimeRange.
        /// </summary>
        /// <param name="end">the last day that should be included in the range</param>
        /// <param name="start">the first day that should be inlcuded in the range</param>
        /// <remarks>
        /// the timespan of the last 1/100 second of the end date is not included in the range.
        /// It is 1/100 particularly because of the accuracy of datetime data stored in database
        /// </remarks>
        public SpecificDateRangeWrapper(DateTime? start, DateTime? end)
        { 
            if(start != null )
                start = start.Value.Date;
            if(end != null)
                end = end.Value.AddDays(1).Date.AddSeconds(-0.01); 
            range = new DateTimeRange(start, end);
        }


        public override bool Equals(object obj) {
            if (this == obj)
                return true;
            SpecificDateRangeWrapper that = obj as SpecificDateRangeWrapper;
            if (that != null)
                return range.Equals(that.range);
            return false;
        }
        
        public override string ToString(){
            return range.ToString();
        }

        public override int GetHashCode() {
            return range.GetHashCode();
        }
    }
}
