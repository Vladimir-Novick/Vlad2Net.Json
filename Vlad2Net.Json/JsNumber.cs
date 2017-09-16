// JsNumber.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.Diagnostics;
using System.Globalization;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Represents a JavaScript Object Notation Number data type. This class cannot 
    /// be inherited.
    /// </summary>
    [Serializable()]
    [DebuggerDisplay("{_value}")]
    public sealed class JsNumber : JsTypeSkeleton, IJsNumber
    {
        #region Private Fields.

        private double _value;

        #endregion

        #region Public Interface.

        /// <summary>
        /// Defines the smallest Json number. This field is readonly.
        /// </summary>
        public static readonly JsNumber MinValue = new JsNumber(double.MinValue);

        /// <summary>
        /// Defines a Json number with a value of zero. This field is readonly.
        /// </summary>
        public static readonly JsNumber Zero = new JsNumber(0D);

        /// <summary>
        /// Defines the lasrgest Json number. This field is readonly.
        /// </summary>
        public static readonly JsNumber MaxValue = new JsNumber(double.MaxValue);        

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        public JsNumber(byte value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        [CLSCompliant(false)]
        public JsNumber(sbyte value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        public JsNumber(short value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        [CLSCompliant(false)]
        public JsNumber(ushort value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        public JsNumber(int value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        [CLSCompliant(false)]
        public JsNumber(uint value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        public JsNumber(long value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        [CLSCompliant(false)]
        public JsNumber(ulong value)
            : this((double)value) {
        }

        /// <summary>
        /// Initialises a new instance of the JsNumber class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value">The value of the new instance.</param>
        public JsNumber(double value)
            : base(JsTypeCode.Number) {

            if(double.IsInfinity(value) || double.IsNegativeInfinity(value) || 
                double.IsNaN(value))
                throw new ArgumentOutOfRangeException("value");

            _value = value;
        }
        /// <summary>
        /// Writes the contents of this Json type using the specified
        /// <see cref="Vlad2Net.Json.IJsWriter"/>.
        /// </summary>
        /// <param name="writer">The Json writer.</param>
        public override void Write(IJsWriter writer) {

            if(writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteValue(ToString());
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of this JsNumber instance.
        /// </summary>
        /// <returns> <see cref="System.String"/> representation of this JsNumber instance</returns>
        public override string ToString() {

            return this.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of this JsNumber instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The culture specific format provider.</param>
        /// <returns> <see cref="System.String"/> representation of this JsNumber instance</returns>
        public string ToString(string format, IFormatProvider formatProvider) {

            return this.Value.ToString(format, formatProvider);
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="System.Object"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the specified object is equal to this instance, otherwise;
        /// false.</returns>
        public override bool Equals(object obj) {

            if(obj == null)
                return false;
            if(obj.GetType() != GetType())
                return false;

            return Equals((JsNumber)obj);
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// JsNumber.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(JsNumber other) {

            return other != null && this.Value == other.Value;
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="Vlad2Net.Json.IJsNumber"/>.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(IJsNumber other) {

            return other != null && this.Value == other.Value;
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="System.Double"/>.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified double is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(double other) {

            return this.Value == other;
        }

        /// <summary>
        /// Returns a value indicating equality with the specified instance.
        /// </summary>
        /// <param name="other">The JsNumber to compare.</param>
        /// <returns></returns>
        public int CompareTo(JsNumber other) {

            return other != null ? this.Value.CompareTo(other.Value) : -1;
        }

        /// <summary>
        /// Returns a value indicating equality with the specified instance.
        /// </summary>
        /// <param name="other">The <see cref="Vlad2Net.Json.IJsNumber"/> to compare.</param>
        /// <returns></returns>
        public int CompareTo(IJsNumber other) {

            return other != null ? this.Value.CompareTo(other.Value) : -1;
        }

        /// <summary>
        /// Returns a value indicating equality with the specified <see cref="System.Double"/>.
        /// </summary>
        /// <param name="other">The Double to compare.</param>
        /// <returns></returns>
        public int CompareTo(double other) {

            return this.Value.CompareTo(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode() {

            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Gets the value of this JsNumber.
        /// </summary>
        public double Value {

            get { return _value; }
        }

        /// <summary>
        /// Determines if the two <see cref="Vlad2Net.Json.JsNumber"/>s are
        /// equal.
        /// </summary>
        /// <param name="a">The first JsNumber.</param>
        /// <param name="b">The second JsNumber.</param>
        /// <returns>True if the JsNumbers are equal, otherwise; false.</returns>
        public static bool Equals(JsNumber a, JsNumber b) {

            object ao = a;
            object bo = b;

            if(ao == bo)
                return true;
            if(ao == null || bo == null)
                return false;

            return a.Value == b.Value;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">The first JsNumber.</param>
        /// <param name="b">The second JsNumber.</param>
        /// <returns>True if the JsNumbers are equal, otherwise; false.</returns>
        public static bool operator ==(JsNumber a, JsNumber b) {

            return JsNumber.Equals(a, b);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="a">The first JsNumber.</param>
        /// <param name="b">The second JsNumber.</param>
        /// <returns>True if the JsNumbers are not equal, otherwise; false.</returns>
        public static bool operator !=(JsNumber a, JsNumber b) {

            return !JsNumber.Equals(a, b);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator JsNumber(byte value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static implicit operator JsNumber(sbyte value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator JsNumber(short value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static implicit operator JsNumber(ushort value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator JsNumber(int value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static implicit operator JsNumber(uint value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator JsNumber(long value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static implicit operator JsNumber(ulong value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implict conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator JsNumber(double value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>This method always returns null.</returns>
        public static implicit operator JsNumber(JsNull value) {

            return null;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator double(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator byte(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (byte)value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static explicit operator sbyte(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (sbyte)value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator short(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (short)value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static explicit operator ushort(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (ushort)value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator int(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (int)value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static explicit operator uint(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (uint)value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator long(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (long)value.Value;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static explicit operator ulong(JsNumber value) {

            if(value == null)
                throw new ArgumentNullException();

            return (ulong)value.Value;
        }

        #endregion 
    }
}
