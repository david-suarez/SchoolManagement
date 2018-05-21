using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Utilities
{
    public static class ReflectionExtensionMethods
    {
        public static int CalculateHashCode(this object sourceObject, params object[] additionalObjects)
        {
            var num = 17;
            foreach (object additionalObject in additionalObjects)
            {
                num = num * 23 + (additionalObject == null ? 0 : additionalObject.GetHashCode());
            }

            return num;
        }
    }
}
