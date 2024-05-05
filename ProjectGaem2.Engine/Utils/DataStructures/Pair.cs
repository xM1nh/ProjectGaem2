using System;
using System.Collections.Generic;

namespace ProjectGaem2.Engine.Utils.DataStructures
{
    public struct Pair<T>(T first, T second) : IEquatable<Pair<T>>
        where T : class
    {
        public T First = first;
        public T Second = second;

        public void Clear()
        {
            First = Second = null;
        }

        public bool Equals(Pair<T> other)
        {
            return First == other.First && Second == other.Second;
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(First) * 37
                + EqualityComparer<T>.Default.GetHashCode(Second);
        }

        public override bool Equals(object obj)
        {
            return obj is Pair<T> && Equals((Pair<T>)obj);
        }

        public static bool operator ==(Pair<T> left, Pair<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Pair<T> left, Pair<T> right)
        {
            return !(left == right);
        }
    }
}
