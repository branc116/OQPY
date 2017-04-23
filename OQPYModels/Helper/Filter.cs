using System;
using System.Collections.Generic;
using System.Text;

namespace OQPYModels.Helper
{
    public enum Operators{
        plus,
        minus,
        times,
        devide,
        and,
        or,
        eq,
        notEq,
        not,
        gt,
        ge,
        lt,
        le,
        isNull,
        Unique,
        Or,
        Like,
    }

    public class Filter
    {
        public string Operand1;
        public Operators Operator;
        public string Operand2;
        public IEnumerable<Filter> AndFileters;
        public IEnumerable<Filter> OrFilters;
    }
}
