using System;
using System.Collections.Generic;
using System.Text;

namespace MFAE.Jobs.ApplicationForm
{
    public class LookupAttribute : Attribute
    {
        public LookupAttribute()
        {
        }


        public LookupAttribute(string name, bool isEnum = false)
        {
            lookupname = name;
            IsEnum = isEnum;
        }
        public string lookupname { get; }
        public bool IsEnum { get; }
    }
}
