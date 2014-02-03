﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Models
{
    public class VectorField<T> : IEnumerable<Vector>
    {
        public readonly Func<T, int> IndexOf;
        public readonly Vector[] Values;

        public Vector this[int i] { get { return Values[i]; } }
        public Vector this[T t] { get { return Values[IndexOf(t)]; } }

        public int Count { get { return Values.Length; } }

        public VectorField(Func<T, int> indexOf, Vector[] values)
        {
            IndexOf = indexOf;
            Values = values;
        }

        #region Operators
        public static VectorField<T> operator +(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] + b.Values[i];
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }

        public static VectorField<T> operator -(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = a.Values[i] - b.Values[i];
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }

        public static VectorField<T> operator *(double c, VectorField<T> a)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = c * a.Values[i];
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }
        #endregion

        public static VectorField<T> CrossProduct(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new Vector[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = Vector.CrossProduct(a.Values[i], b.Values[i]);
            }
            return new VectorField<T>(a.IndexOf, newValues);
        }

        public static ScalarField<T> ScalarProduct(VectorField<T> a, VectorField<T> b)
        {
            var newValues = new double[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                newValues[i] = Vector.ScalarProduct(a.Values[i], b.Values[i]);
            }
            return new ScalarField<T>(a.IndexOf, newValues);
        }

        public IEnumerator<Vector> GetEnumerator()
        {
            return Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
