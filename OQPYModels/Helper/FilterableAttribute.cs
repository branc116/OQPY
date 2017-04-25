using System;

namespace OQPYModels.Helper
{
    public class FilterableAttribute: Attribute
    {
        public bool Filter = false;
        public bool IsList = false;
    }
}