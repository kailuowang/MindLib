using System;
using NHibernate.Expression;

namespace MindHarbor.TimeDataUtil {
    ///<summary>
    ///</summary>
    public interface IDateTimeRange {
        DateTime? End { get;  }

        DateTime? Start { get; }

        string ToString(string fString);
        bool Includes(DateTime time);
        bool EarlierEqualThanEnd(DateTime? time);
        bool LaterEqualThanStart(DateTime? time);
        ICriterion RangeExpression(string propertyName);
        bool LargerOrEqual(DateRange dr);
    }
}