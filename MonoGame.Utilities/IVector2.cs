// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Quaternion = Microsoft.Xna.Framework.Quaternion;

namespace MonoGame.Utilities
{
    /// <summary>
    /// Describes a 2D-vector.
    /// </summary>
#if XNADESIGNPROVIDED
    [System.ComponentModel.TypeConverter(typeof(Microsoft.Xna.Framework.Design.Vector2TypeConverter))]
#endif
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct IVector2 : IEquatable<IVector2>
    {
        #region Private Fields

        private static readonly IVector2 zeroVector = new IVector2(0, 0);
        private static readonly IVector2 unitVector = new IVector2(1, 1);
        private static readonly IVector2 unitXVector = new IVector2(1, 0);
        private static readonly IVector2 unitYVector = new IVector2(0, 1);

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="IVector2"/>.
        /// </summary>
        [DataMember]
        public int X;

        /// <summary>
        /// The y coordinate of this <see cref="IVector2"/>.
        /// </summary>
        [DataMember]
        public int Y;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a <see cref="IVector2"/> with components 0, 0.
        /// </summary>
        public static IVector2 Zero
        {
            get { return zeroVector; }
        }

        /// <summary>
        /// Returns a <see cref="IVector2"/> with components 1, 1.
        /// </summary>
        public static IVector2 One
        {
            get { return unitVector; }
        }

        /// <summary>
        /// Returns a <see cref="IVector2"/> with components 1, 0.
        /// </summary>
        public static IVector2 UnitX
        {
            get { return unitXVector; }
        }

        /// <summary>
        /// Returns a <see cref="IVector2"/> with components 0, 1.
        /// </summary>
        public static IVector2 UnitY
        {
            get { return unitYVector; }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    this.X.ToString(), "  ",
                    this.Y.ToString()
                );
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a 2d vector with X and Y from two values.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public IVector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a 2d vector with X and Y set to the same value.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public IVector2(int value)
        {
            this.X = value;
            this.Y = value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts a <see cref="System.Numerics.Vector2"/> to a <see cref="IVector2"/>.
        /// </summary>
        /// <param name="value">The converted value.</param>
        public static implicit operator IVector2(System.Numerics.Vector2 value)
        {
            return new IVector2((int)value.X, (int)value.Y);
        }

        /// <summary>
        /// Inverts values in the specified <see cref="IVector2"/>.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static IVector2 operator -(IVector2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="IVector2"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static IVector2 operator +(IVector2 value1, IVector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Subtracts a <see cref="IVector2"/> from a <see cref="IVector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="IVector2"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static IVector2 operator -(IVector2 value1, IVector2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="IVector2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static IVector2 operator *(IVector2 value1, IVector2 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static IVector2 operator *(IVector2 value, int scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="IVector2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static IVector2 operator *(int scaleFactor, IVector2 value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="IVector2"/> by the components of another <see cref="IVector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="IVector2"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IVector2 operator /(IVector2 value1, IVector2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="IVector2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IVector2 operator /(IVector2 value1, float divider)
        {
            float factor = 1 / divider;
            float newX = value1.X * factor;
            float newY = value1.Y * factor;

            return new IVector2((int)newX, (int)newY);
        }

        /// <summary>
        /// Compares whether two <see cref="IVector2"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="IVector2"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="IVector2"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(IVector2 value1, IVector2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        /// <summary>
        /// Compares whether two <see cref="IVector2"/> instances are not equal.
        /// </summary>
        /// <param name="value1"><see cref="IVector2"/> instance on the left of the not equal sign.</param>
        /// <param name="value2"><see cref="IVector2"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
        public static bool operator !=(IVector2 value1, IVector2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static IVector2 Add(IVector2 value1, IVector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and
        /// <paramref name="value2"/>, storing the result of the
        /// addition in <paramref name="result"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <param name="result">The result of the vector addition.</param>
        public static void Add(ref IVector2 value1, ref IVector2 value2, out IVector2 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static IVector2 Barycentric(IVector2 value1, IVector2 value2, IVector2 value3, float amount1, float amount2)
        {
            return new IVector2(
                (int)MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                (int)MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(ref IVector2 value1, ref IVector2 value2, ref IVector2 value3, float amount1, float amount2, out IVector2 result)
        {
            result.X = (int)MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = (int)MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static IVector2 CatmullRom(IVector2 value1, IVector2 value2, IVector2 value3, IVector2 value4, float amount)
        {
            return new IVector2(
                (int)MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                (int)MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(ref IVector2 value1, ref IVector2 value2, ref IVector2 value3, ref IVector2 value4, float amount, out IVector2 result)
        {
            result.X = (int)MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = (int)MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        }


        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static IVector2 Clamp(IVector2 value1, IVector2 min, IVector2 max)
        {
            return new IVector2(
                MathHelper.Clamp(value1.X, min.X, max.X),
                MathHelper.Clamp(value1.Y, min.Y, max.Y));
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="result">The clamped value as an output parameter.</param>
        public static void Clamp(ref IVector2 value1, ref IVector2 min, ref IVector2 max, out IVector2 result)
        {
            result.X = MathHelper.Clamp(value1.X, min.X, max.X);
            result.Y = MathHelper.Clamp(value1.Y, min.Y, max.Y);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static float Distance(IVector2 value1, IVector2 value2)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return MathF.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref IVector2 value1, ref IVector2 value2, out float result)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = MathF.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static float DistanceSquared(IVector2 value1, IVector2 value2)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared(ref IVector2 value1, ref IVector2 value2, out float result)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Divides the components of a <see cref="IVector2"/> by the components of another <see cref="IVector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Divisor <see cref="IVector2"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static IVector2 Divide(IVector2 value1, IVector2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="IVector2"/> by the components of another <see cref="IVector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Divisor <see cref="IVector2"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(ref IVector2 value1, ref IVector2 value2, out IVector2 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// Divides the components of a <see cref="IVector2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static IVector2 Divide(IVector2 value1, float divider)
        {
            float factor = 1 / divider;
            float newX = value1.X * factor;
            float newY = value1.Y * factor;

            return new IVector2((int)newX, (int)newY);
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static float Dot(IVector2 value1, IVector2 value2)
        {
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref IVector2 value1, ref IVector2 value2, out float result)
        {
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is IVector2)
            {
                return Equals((IVector2)obj);
            }

            return false;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="IVector2"/>.
        /// </summary>
        /// <param name="other">The <see cref="IVector2"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(IVector2 other)
        {
            return (X == other.X) && (Y == other.Y);
        }
        /// <summary>
        /// Gets the hash code of this <see cref="IVector2"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="IVector2"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static IVector2 Hermite(IVector2 value1, IVector2 tangent1, IVector2 value2, IVector2 tangent2, float amount)
        {
            return new IVector2((int)MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount), (int)MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(ref IVector2 value1, ref IVector2 tangent1, ref IVector2 value2, ref IVector2 tangent2, float amount, out IVector2 result)
        {
            result.X = (int)MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = (int)MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        }

        /// <summary>
        /// Returns the length of this <see cref="IVector2"/>.
        /// </summary>
        /// <returns>The length of this <see cref="IVector2"/>.</returns>
        public float Length()
        {
            return MathF.Sqrt((X * X) + (Y * Y));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="IVector2"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="IVector2"/>.</returns>
        public float LengthSquared()
        {
            return (X * X) + (Y * Y);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static IVector2 Lerp(IVector2 value1, IVector2 value2, float amount)
        {
            return new IVector2(
                (int)MathHelper.Lerp(value1.X, value2.X, amount),
                (int)MathHelper.Lerp(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(ref IVector2 value1, ref IVector2 value2, float amount, out IVector2 result)
        {
            result.X = (int)MathHelper.Lerp(value1.X, value2.X, amount);
            result.Y = (int)MathHelper.Lerp(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains linear interpolation of the specified vectors.
        /// Uses <see cref="MathHelper.LerpPrecise"/> on MathHelper for the interpolation.
        /// Less efficient but more precise compared to <see cref="IVector2.Lerp(IVector2, IVector2, float)"/>.
        /// See remarks section of <see cref="MathHelper.LerpPrecise"/> on MathHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static IVector2 LerpPrecise(IVector2 value1, IVector2 value2, float amount)
        {
            return new IVector2(
                (int)MathHelper.LerpPrecise(value1.X, value2.X, amount),
                (int)MathHelper.LerpPrecise(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains linear interpolation of the specified vectors.
        /// Uses <see cref="MathHelper.LerpPrecise"/> on MathHelper for the interpolation.
        /// Less efficient but more precise compared to <see cref="IVector2.Lerp(ref IVector2, ref IVector2, float, out IVector2)"/>.
        /// See remarks section of <see cref="MathHelper.LerpPrecise"/> on MathHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void LerpPrecise(ref IVector2 value1, ref IVector2 value2, float amount, out IVector2 result)
        { 
            result.X = (int)MathHelper.LerpPrecise(value1.X, value2.X, amount);
            result.Y = (int)MathHelper.LerpPrecise(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="IVector2"/> with maximal values from the two vectors.</returns>
        public static IVector2 Max(IVector2 value1, IVector2 value2)
        {
            return new IVector2(value1.X > value2.X ? value1.X : value2.X,
                               value1.Y > value2.Y ? value1.Y : value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="IVector2"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref IVector2 value1, ref IVector2 value2, out IVector2 result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="IVector2"/> with minimal values from the two vectors.</returns>
        public static IVector2 Min(IVector2 value1, IVector2 value2)
        {
            return new IVector2(value1.X < value2.X ? value1.X : value2.X,
                               value1.Y < value2.Y ? value1.Y : value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="IVector2"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref IVector2 value1, ref IVector2 value2, out IVector2 result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Source <see cref="IVector2"/>.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static IVector2 Multiply(IVector2 value1, IVector2 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Source <see cref="IVector2"/>.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref IVector2 value1, ref IVector2 value2, out IVector2 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a multiplication of <see cref="IVector2"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static IVector2 Multiply(IVector2 value1, float scaleFactor)
        {
            float newX = value1.X * scaleFactor;
            float newY = value1.Y * scaleFactor;
            return new IVector2((int)newX, (int)newY);
        }
        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static IVector2 Negate(IVector2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref IVector2 value, out IVector2 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// Turns this <see cref="IVector2"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            float val = 1.0f / MathF.Sqrt((X * X) + (Y * Y));
            X = (int)(X * val);
            Y = (int)(Y * val);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <returns>Unit vector.</returns>
        public static IVector2 Normalize(IVector2 value)
        {
            float val = 1.0f / MathF.Sqrt((value.X * value.X) + (value.Y * value.Y));
            value.X = (int)(value.X * val);
            value.Y = (int)(value.Y * val);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref IVector2 value, out IVector2 result)
        {
            float val = 1.0f / MathF.Sqrt((value.X * value.X) + (value.Y * value.Y));
            result.X = (int)(value.X * val);
            result.Y = (int)(value.Y * val);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="IVector2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static IVector2 Reflect(IVector2 vector, IVector2 normal)
        {
            IVector2 result;
            float val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = (int)(vector.X - (normal.X * val));
            result.Y = (int)(vector.Y - (normal.Y * val));
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="IVector2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect(ref IVector2 vector, ref IVector2 normal, out IVector2 result)
        {
            float val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = (int)(vector.X - (normal.X * val));
            result.Y = (int)(vector.Y - (normal.Y * val));
        }

        /// <summary>
        /// Round the members of this <see cref="IVector2"/> to the nearest integer value.
        /// </summary>
        public void Round()
        {
            X = (int)MathF.Round(X);
            Y = (int)MathF.Round(Y);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <returns>The rounded <see cref="IVector2"/>.</returns>
        public static IVector2 Round(IVector2 value)
        {
            value.X = (int)MathF.Round(value.X);
            value.Y = (int)MathF.Round(value.Y);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <param name="result">The rounded <see cref="IVector2"/>.</param>
        public static void Round(ref IVector2 value, out IVector2 result)
        {
            result.X = (int)MathF.Round(value.X);
            result.Y = (int)MathF.Round(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Source <see cref="IVector2"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static IVector2 SmoothStep(IVector2 value1, IVector2 value2, float amount)
        {
            return new IVector2(
                (int)MathHelper.SmoothStep(value1.X, value2.X, amount),
                (int)MathHelper.SmoothStep(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Source <see cref="IVector2"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(ref IVector2 value1, ref IVector2 value2, float amount, out IVector2 result)
        {
            result.X = (int)MathHelper.SmoothStep(value1.X, value2.X, amount);
            result.Y = (int)MathHelper.SmoothStep(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains subtraction of on <see cref="IVector2"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Source <see cref="IVector2"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static IVector2 Subtract(IVector2 value1, IVector2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains subtraction of on <see cref="IVector2"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="IVector2"/>.</param>
        /// <param name="value2">Source <see cref="IVector2"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref IVector2 value1, ref IVector2 value2, out IVector2 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="IVector2"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="IVector2"/>.</returns>
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + "}";
        }

        /// <summary>
        /// Gets a <see cref="Point"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Point"/> representation for this object.</returns>
        public Point ToPoint()
        {
            return new Point((int) X,(int) Y);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="IVector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed <see cref="IVector2"/>.</returns>
        public static IVector2 Transform(IVector2 position, Matrix matrix)
        {
            return new IVector2(
                (int)((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41), 
                (int)((position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="IVector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed <see cref="IVector2"/> as an output parameter.</param>
        public static void Transform(ref IVector2 position, ref Matrix matrix, out IVector2 result)
        {
            var x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            var y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.X = (int)x;
            result.Y = (int)y;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="IVector2"/>.</returns>
        public static IVector2 Transform(IVector2 value, Quaternion rotation)
        {
            Transform(ref value, ref rotation, out value);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="IVector2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="IVector2"/> as an output parameter.</param>
        public static void Transform(ref IVector2 value, ref Quaternion rotation, out IVector2 result)
        {
            var rot1 = new Microsoft.Xna.Framework.Vector3(rotation.X + rotation.X, rotation.Y + rotation.Y, rotation.Z + rotation.Z);
            var rot2 = new Microsoft.Xna.Framework.Vector3(rotation.X, rotation.X, rotation.W);
            var rot3 = new Microsoft.Xna.Framework.Vector3(1, rotation.Y, rotation.Z);
            var rot4 = rot1*rot2;
            var rot5 = rot1*rot3;

            var v = new IVector2();
            v.X = (int)((double)value.X * (1.0 - (double)rot5.Y - (double)rot5.Z) + (double)value.Y * ((double)rot4.Y - (double)rot4.Z));
            v.Y = (int)((double)value.X * ((double)rot4.Y + (double)rot4.Z) + (double)value.Y * (1.0 - (double)rot4.X - (double)rot5.Z));
            result.X = v.X;
            result.Y = v.Y;
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="IVector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="IVector2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            IVector2[] sourceArray,
            int sourceIndex,
            ref Matrix matrix,
            IVector2[] destinationArray,
            int destinationIndex,
            int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                var position = sourceArray[sourceIndex + x];
                var destination = destinationArray[destinationIndex + x];
                destination.X = (int)((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41);
                destination.Y = (int)((position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="IVector2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="IVector2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform
        (
            IVector2[] sourceArray,
            int sourceIndex,
            ref Quaternion rotation,
            IVector2[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                var position = sourceArray[sourceIndex + x];
                var destination = destinationArray[destinationIndex + x];

                IVector2 v;
                Transform(ref position,ref rotation,out v); 

                destination.X = v.X;
                destination.Y = v.Y;

                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="IVector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            IVector2[] sourceArray,
            ref Matrix matrix,
            IVector2[] destinationArray)
        {
            Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="IVector2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform
        (
            IVector2[] sourceArray,
            ref Quaternion rotation,
            IVector2[] destinationArray
        )
        {
            Transform(sourceArray, 0, ref rotation, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="IVector2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static IVector2 TransformNormal(IVector2 normal, Matrix matrix)
        {
            return new IVector2(
                (int)((normal.X * matrix.M11) + (normal.Y * matrix.M21)),
                (int)((normal.X * matrix.M12) + (normal.Y * matrix.M22)));
        }

        /// <summary>
        /// Creates a new <see cref="IVector2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="IVector2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(ref IVector2 normal, ref Matrix matrix, out IVector2 result)
        {
            var x = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            var y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = (int)x;
            result.Y = (int)y;
        }
        
        /// <summary>
        /// Apply transformation on normals within array of <see cref="IVector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="IVector2"/> should be written.</param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal
        (
            IVector2[] sourceArray,
            int sourceIndex,
            ref Matrix matrix,
            IVector2[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int i = 0; i < length; i++)
            {
                var normal = sourceArray[sourceIndex + i];

                destinationArray[destinationIndex + i] = new IVector2((int)((normal.X * matrix.M11) + (normal.Y * matrix.M21)),
                                                                      (int)((normal.X * matrix.M12) + (normal.Y * matrix.M22)));
            }
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="IVector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal
            (
            IVector2[] sourceArray,
            ref Matrix matrix,
            IVector2[] destinationArray
            )
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination array length is lesser than source array length");

            for (int i = 0; i < sourceArray.Length; i++)
            {
                var normal = sourceArray[i];

                destinationArray[i] = new IVector2((int)((normal.X * matrix.M11) + (normal.Y * matrix.M21)),
                                                   (int)((normal.X * matrix.M12) + (normal.Y * matrix.M22)));
            }
        }

        /// <summary>
        /// Rotates a vector by the specified number of radians
        /// </summary>
        /// <param name="value">The vector to be rotated.</param>
        /// <param name="radians">The amount to rotate the vector.</param>
        /// <returns>A rotated copy of value.</returns>
        /// <remarks>
        /// A positive angle and negative angle
        /// would rotate counterclockwise and clockwise,
        /// respectively
        /// </remarks>
        public static IVector2 Rotate(IVector2 value, float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            return new IVector2(
                (int)(value.X * cos - value.Y * sin), 
                (int)(value.X * sin + value.Y * cos)
            );
        }

        /// <summary>
        /// Rotates a <see cref="IVector2"/> by the specified number of radians
        /// </summary>
        /// <param name="radians">The amount to rotate this <see cref="IVector2"/>.</param>
        /// <remarks>
        /// A positive angle and negative angle
        /// would rotate counterclockwise and clockwise,
        /// respectively
        /// </remarks>
        public void Rotate(float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            float oldx = X;

            X = (int)(X * cos - Y * sin);
            Y = (int)(oldx * sin + Y * cos);
        }

        /// <summary>
        /// Rotates a <see cref="IVector2"/> around another <see cref="IVector2"/> representing a location
        /// </summary>
        /// <param name="value">The <see cref="IVector2"/> to be rotated</param>
        /// <param name="origin">The origin location to be rotated around</param>
        /// <param name="radians">The amount to rotate by in radians</param>
        /// <returns>The rotated <see cref="IVector2"/></returns>
        /// <remarks>
        /// A positive angle and negative angle
        /// would rotate counterclockwise and clockwise,
        /// respectively
        /// </remarks>
        public static IVector2 RotateAround(IVector2 value, IVector2 origin, float radians)
        {
            return Rotate(value - origin, radians) + origin;
        }

        /// <summary>
        /// Rotates a <see cref="IVector2"/> around another <see cref="IVector2"/> representing a location
        /// </summary>
        /// <param name="origin">The origin location to be rotated around</param>
        /// <param name="radians">The amount to rotate by in radians</param>
        /// <remarks>
        /// A positive angle and negative angle
        /// would rotate counterclockwise and clockwise,
        /// respectively
        /// </remarks>
        public void RotateAround(IVector2 origin, float radians)
        {
            this -= origin;
            Rotate(radians);
            this += origin;
        }

        /// <summary>
        /// Deconstruction method for <see cref="IVector2"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }

        /// <summary>
        /// Returns a <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public System.Numerics.Vector2 ToNumerics()
        {
            return new System.Numerics.Vector2(this.X, this.Y);
        }

        #endregion
    }
}