using UnityEngine;
using UnityEditor;

namespace Options
{
    public class NullCheck
    {
        public static bool Some(object obj)
        {
            return obj != null;
        }

        public static bool None(object obj)
        {
            return obj == null;
        }
    }
}