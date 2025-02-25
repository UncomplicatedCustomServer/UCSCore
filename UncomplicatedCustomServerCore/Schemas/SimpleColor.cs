using UnityEngine;

namespace UncomplicatedCustomServerCore.Schemas
{
    internal struct SimpleColor(float r, float g, float b, float a)
    {
        public float r = r;

        public float g = g;

        public float b = b;

        public float a = a;

        public SimpleColor(Color color) : this(color.r, color.g, color.b, color.a)
        { }
    }
}
