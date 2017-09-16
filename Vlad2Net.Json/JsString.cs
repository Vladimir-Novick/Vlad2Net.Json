// JsString.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Represents a JavaScript Object Notation String data type. This class cannot 
    /// be inherited.
    /// </summary>
    [Serializable()]
    [DebuggerDisplay("{_value}")]
    public sealed class JsString : JsTypeSkeleton, IJsString
    {
        #region Private Fields.

        private string _encodedValue;
        private readonly string _value;

        private static readonly char[] QUOTE_CHARS = { '"', '/', '\\', '\b', '\f', '\n', '\r', '\t' };        

        #endregion

        #region Public Interface.

        /// <summary>
        /// Defines an empty JsString. This field is readonly.
        /// </summary>
        public static readonly JsString Empty = new JsString(string.Empty);

        /// <summary>
        /// Initialises a new instance of the JsString class and specifies the
        /// value.
        /// </summary>
        /// <param name="value">The value of the instance.</param>
        public JsString(string value)
            : base(JsTypeCode.String) {

            if(value == null)
                throw new ArgumentNullException("value");

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

            writer.WriteValue(this.EncodedValue);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of this JsString 
        /// instance.
        /// </summary>
        /// <returns> <see cref="System.String"/> representation of this JsString 
        /// instance.</returns>
        public override string ToString() {

            return this.Value;
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

            return Equals((JsString)obj);
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// JsString.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(JsString other) {

            return other != null && Equals(other.Value);
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="Vlad2Net.Json.IJsString"/>.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(IJsString other) {

            return other != null && Equals(other.Value);
        }  

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(string other) {

            return this.Value.Equals(other);
        }

        /// <summary>
        /// Returns a value indicating equality with the specified instance.
        /// </summary>
        /// <param name="other">The JsNumber to compare.</param>
        /// <returns></returns>
        public int CompareTo(JsString other) {

            return other != null ? this.Value.CompareTo(other.Value) : -1;
        }

        /// <summary>
        /// Returns a value indicating equality with the specified
        /// <see cref="Vlad2Net.Json.IJsString"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vlad2Net.Json.IJsString"/> to 
        /// compare.</param>
        /// <returns></returns>
        public int CompareTo(IJsString other) {

            return other != null ? this.Value.CompareTo(other.Value) : -1;
        }

        /// <summary>
        /// Returns a value indicating equality with the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">The String to compare.</param>
        /// <returns></returns>
        public int CompareTo(string other) {

            return this.Value.CompareTo(other);
        }

        /// <summary>
        /// Returns a hash code for this JsString.
        /// </summary>
        /// <returns>A hash code for this JsString.</returns>
        public override int GetHashCode() {

            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Gets the un-encoded value of the this JsString.
        /// </summary>
        public string Value {

            get { return _value; }
        }

        /// <summary>
        /// Gets the encoded value of this JsString.
        /// </summary>
        public string EncodedValue {

            get {
                if(_encodedValue == null)
                    _encodedValue = JsString.Encode(this.Value);
                return _encodedValue;
            }
        }

        /// <summary>
        /// Determines if the two <see cref="Vlad2Net.Json.JsString"/>s are
        /// equal.
        /// </summary>
        /// <param name="a">The first JsString.</param>
        /// <param name="b">The second JsString.</param>
        /// <returns>True if the JsStrings are equal, otherwise; false.</returns>
        public static bool Equals(JsString a, JsString b) {

            object ao = a;
            object bo = b;

            if(ao == bo)
                return true;
            if(ao == null || bo == null)
                return false;

            return a.Equals(b.Value);
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">The first JsString.</param>
        /// <param name="b">The second JsString.</param>
        /// <returns>True if the JsStrings are equal, otherwise; false.</returns>
        public static bool operator ==(JsString a, JsString b) {

            return JsString.Equals(a, b);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="a">The first JsString.</param>
        /// <param name="b">The second JsString.</param>
        /// <returns>True if the JsStrings are not equal, otherwise; false.</returns>
        public static bool operator !=(JsString a, JsString b) {

            return !JsString.Equals(a, b);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static implicit operator JsString(string value) {            

            if(value == null)
                return null;
            if(value.Equals(string.Empty))
                return JsString.Empty;
            return new JsString(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>This method always returns null.</returns>
        public static implicit operator JsString(JsNull value) {

            return null;
        }

        /// <summary>
        /// Explicit conversion operator.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static explicit operator string(JsString value) {

            return value != null ? value.Value : null;
        }        

        /// <summary>
        /// Encodes the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string Encode(string s) {

            if(s == null)
                throw new ArgumentNullException("s");

            if(s.Equals(string.Empty) || !JsString.ShouldEncode(s))
                return string.Concat("\"", s, "\"");

            char ch;
            StringBuilder sb = new StringBuilder(s.Length);

            sb.Append('"');
            for(int i = 0; i < s.Length; ++i) {
                ch = s[i];
                switch(ch) {
                    case '"':
                        sb.Append(@"\""");
                        break;
                    case '/':
                        sb.Append(@"\/");
                        break;
                    case '\\':
                        sb.Append(@"\\");
                        break;
                    case '\b':
                        sb.Append(@"\b");
                        break;
                    case '\f':
                        sb.Append(@"\f");
                        break;
                    case '\n':
                        sb.Append(@"\n");
                        break;
                    case '\r':
                        sb.Append(@"\r");
                        break;
                    case '\t':
                        sb.Append(@"\t");
                        break;
                    default:
                        if(ch > 0x7F)
                            // TODO: MUST add support for UTF-16.
                            sb.AppendFormat(@"\u{0}", ((int)ch).ToString("X4"));
                        else
                            sb.Append(ch);
                        break;
                }
            }
            sb.Append('"');

            return sb.ToString();
        }        

        #endregion

        #region Private Impl.

        private static bool ShouldEncode(string s) {

            for(int i = 0; i < s.Length; ++i) {
                if(s[i] > 0x7F || Array.IndexOf(JsString.QUOTE_CHARS, s[i]) > -1)
                    return true;
            }

            return false;
        }

        #endregion
    }
}
