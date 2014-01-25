﻿using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    public class Edge
    {
        public readonly Vertex A;
        public readonly Vertex B;

        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }

        public IEnumerable<Vertex> Vertices()
        {
            yield return A;
            yield return B;
        }

        #region Equals, GetHashCode, (==) and (!=) overrides
        public static bool operator ==(Edge a, Edge b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Edge a, Edge b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var edge = obj as Edge;
            if (edge == null)
            {
                return false;
            }

            return Equals(edge);
        }

        public bool Equals(Edge other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }

            return this.A == other.A && this.B == other.B;
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }
        #endregion
    }
}