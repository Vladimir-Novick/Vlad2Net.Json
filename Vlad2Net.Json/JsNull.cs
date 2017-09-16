// JsNull.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.IO;
using System.Diagnostics;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Represents a Json null value. This class cannot be inherited.
    /// </summary>
    [Serializable()]    
    [DebuggerDisplay("JsNull")]
    public sealed class JsNull : JsTypeSkeleton, IJsNull, IObjectReference
    {
        #region Public Interface.

        /// <summary>
        /// Defines the JsNull string. This field is constant.
        /// </summary>
        public const string NullString = "null";

        /// <summary>
        /// Defines a JsNull instance. This field is readonly.
        /// </summary>
        public static readonly JsNull Null = new JsNull();        

        /// <summary>
        /// Writes the contents of this Json type using the specified
        /// <see cref="Vlad2Net.Json.IJsWriter"/>.
        /// </summary>
        /// <param name="writer">The Json writer.</param>
        public override void Write(IJsWriter writer) {

            if(writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteValue(JsNull.NullString);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of this JsonNull instance.
        /// </summary>
        /// <returns> <see cref="System.String"/> representation of this JsonNull instance.</returns>
        public override string ToString() {

            return JsNull.NullString;
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

            return Equals((JsNull)obj);
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// JsonNull.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(JsNull other) {

            // Should I make a null JsonNull equal to this regardless?
            return other != null;
        }

        /// <summary>
        /// Returns a indicating whether this instance is equal to the specified
        /// <see cref="Vlad2Net.Json.IJsNull"/>.
        /// </summary>
        /// <param name="other">The value to compare.</param>
        /// <returns>True if the specified instance is equal to this instance, otherwise;
        /// false.</returns>
        public bool Equals(IJsNull other) {

            // Should I make a null IJsNull equal to this regardless?
            return other != null;
        }   

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode() {            

            // We do not want to return zero as that would make this equal
            // with a false JsBoolean.
            return 0x3641694F;            
        }             

        /// <summary>
        /// Maps the specified value to the type of the type paramater.
        /// </summary>
        /// <typeparam name="T">The type to map to.</typeparam>
        /// <param name="value">The value to map.</param>
        /// <returns>The mapped value if not logically null, otherwise the default value of 
        /// <typeparamref name="T"/>.</returns>
        public static T Map<T>(IJsType value) where T : IJsType {

            if(value == null || value.JsTypeCode == JsTypeCode.Null)
                return default(T);
            return (T)value;
        }

        #endregion

        #region Explicit Interface.

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        object IObjectReference.GetRealObject(StreamingContext context) {

            return JsNull.Null;
        }

        #endregion

        #region Private Impl.

        /// <summary>
        /// Initialises a new instance of the JsonNull class.
        /// </summary>
        private JsNull()
            : base(JsTypeCode.Null) {
        }

        #endregion
    }
}
