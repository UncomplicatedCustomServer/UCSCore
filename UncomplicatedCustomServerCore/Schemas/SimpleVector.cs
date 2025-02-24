using UnityEngine;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal struct SimpleVector(float x, float y, float z, float angle = 0)
    {
        public float x = x;

        public float y = y;

        public float z = z;

        public float angle = angle;

        public SimpleVector(Vector3 vector) : this(vector.x, vector.y, vector.z)
        { }

        public static SimpleVector FromQuaternion(Quaternion quaternion)
        {
            quaternion.ToAngleAxis(out float angle, out Vector3 axis);
            return new(axis.x, axis.y, axis.z, angle);
        }
    }
}
