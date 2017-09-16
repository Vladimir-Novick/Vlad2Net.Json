//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.Diagnostics;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Represents a JavaScript Object Notation Boolean data type. This class 
    /// cannot be inherited.
    /// </summary>
    [Serializable()]    
    [DebuggerDisplay("{_value}")]
    public sealed class JsBoolean : JsTypeSkeleton, IJsBoolean
    {
        #region Private Fields.

        private readonly bool _value;

        #endregion

        #region Public Interface.

        /// <summary>
        /// Defines the Json true string. This field is constant.
        /// </summary>
        public const string TrueString = "true";

        /// <summary>
        /// Defines the Json false string. This field is constant.
        /// </summary>
        public const string FalseString = "false";

        /// <summary>
        /// Defines a true JsBoolean instance. This field is readonly.
        /// </summary>
        public static readonly JsBoolean True = new JsBoolean(true);

        /// <summary>
        /// Defines a false JsBoolean instance. This field is readonly.
        /// </summary>
        public static readonly JsBoolean False = new JsBoolean(false);        

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
        /// Returns a <see cref="System.String"/> representation of this JsBoolean instance.
        /// </summary>
        /// <returns> <see cref="System.String"/> representation of this JsBoolean instance</returns>
        public override string ToString() {

            return this.Value ? JsBoolean.TrueString : JsBoolean.FalseString;
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

            return Equals((JsBoolean)obj);
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// JsBoolean.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(JsBoolean other) {

            return other != null && this.Value == other.Value;
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="Vlad2Net.Json.IJsBoolean"/>.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(IJsBoolean other) {

            return other != null && this.Value == other.Value;
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified bool is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(bool other) {

            return this.Value == other;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode() {

            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Gets the value of this JsBoolean.
        /// </summary>
        public bool Value {

            get { return _value; }
        }

        /// <summary>
        /// Returns a static JsBoolean instance representing <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A static JsBoolean instance representing <paramref name="value"/>.
        /// </returns>
        public static JsBoolean Get(bool value) {

            return value ? JsBoolean.True : JsBoolean.False;
        }

        /// <summary>
        /// Determines if the two <see cref="Vlad2Net.Json.JsBoolean"/>s are
        /// equal.
        /// </summary>
        /// <param name="a">The first JsBoolean.</param>
        /// <param name="b">The second JsBoolean.</param>
        /// <returns>True if the JsBooleans are equal, otherwise; false.</returns>
        public static bool Equals(JsBoolean a, JsBoolean b) {

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
        /// <param name="a">The first JsBoolean.</param>
        /// <param name="b">The second JsBoolean.</param>
        /// <returns>True if the JsBooleans are equal, otherwise; false.</returns>
        public static bool operator ==(JsBoolean a, JsBoolean b) {

            return JsBoolean.Equals(a, b);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="a">The first JsBoolean.</param>
        /// <param name="b">The second JsBoolean.</param>
        /// <returns>True if the JsBooleans are not equal, otherwise; false.</returns>
        public static bool operator !=(JsBoolean a, JsBoolean b) {

            return !JsBoolean.Equals(a, b);
        }

        /// <summary>
        /// Implicit <see cref="System.Boolean"/> conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator JsBoolean(bool value) {

            return JsBoolean.Get(value);
        }

        /// <summary>
        /// Implicit <see cref="Vlad2Net.Json.JsNull"/> conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>This method always returns null.</returns>
        public static implicit operator JsBoolean(JsNull value) {

            return null;
        }

        /// <summary>
        /// Explicit <see cref="Vlad2Net.Json.JsBoolean"/> conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator bool(JsBoolean value) {

            if(value == null)
                throw new ArgumentNullException();

            return value.Value;
        }

        #endregion 

        #region Private Impl.

        /// <summary>
        /// Initialises a new instance of the JsBoolean class and specifies the 
        /// value.
        /// </summary>
        /// <param name="value"></param>
        private JsBoolean(bool value)
            : base(JsTypeCode.Boolean) {

            _value = value;
        }

        #endregion
    }
}
