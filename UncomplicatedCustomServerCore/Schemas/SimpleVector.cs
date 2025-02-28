using Newtonsoft.Json;
using UnityEngine;

namespace UncomplicatedCustomServerCore.Schemas
{
#pragma warning disable IDE0290 // Supress warning for using directives

    internal struct SimpleVector
    {
        public float x;

        public float y;

        public float z;

        public float angle;

        [JsonConstructor]
        public SimpleVector(float x, float y, float z, float angle = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.angle = angle;
        }

        public SimpleVector(Vector3 vector) : this(vector.x, vector.y, vector.z)
        { }

        public static SimpleVector FromQuaternion(Quaternion quaternion)
        {
            quaternion.ToAngleAxis(out float angle, out Vector3 axis);
            return new(axis.x, axis.y, axis.z, angle);
        }

        public Vector3 ToVector3() => new(x, y, z);

        public Quaternion ToQuaternion() => Quaternion.AngleAxis(angle, new Vector3(x, y, z));

        public readonly override string ToString() => $"SVector({x},{y},{z},{angle})";
    }
}
