
using UnityEngine;

namespace Script
{
    public static class UtilsExtensions
    {
        public static Vector3 AddZ(this Vector3 vector3, float value)
        {
            vector3.z += value;
            return vector3;
        }
    }
}