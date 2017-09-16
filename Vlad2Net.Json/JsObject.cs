// JsObject.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Represents a JavaScript Object Notation Object data type which contains a 
    /// collection of <see cref="Vlad2Net.Json.IJsType"/>s accessed by key.
    /// </summary>
    [Serializable()]
    public class JsObject : Dictionary<string, IJsType>, IJsObject
    {
        #region Protected Interface.

        /// <summary>
        /// Deserialisation constructor.
        /// </summary>
        /// <param name="info">The object containing the data needed to deserialise an instance.</param>
        /// <param name="context">The boejct which specifies the source of the deserialisation.</param>
        protected JsObject(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        #endregion

        #region Public Interface.

        /// <summary>
        /// Inialises a new instance of the JsObject class.
        /// </summary>
        public JsObject()
            : base(StringComparer.Ordinal) {
        }


        public string getParameters(string key,string defaultValue) {
            string ret = defaultValue;
          if (this.ContainsKey(key))
          {
             return this[key].ToString();
          }
          return defaultValue;
        }

        public string getParameters(string key)
        {

            return getParameters(key,"");
        }


        public string getStrValue(string key){
            
          if (this.ContainsKey(key))
          {
              return this[key].ToString(); 
          }
          return "";
        }

        public JsObject getValue(string key)
        {

            if (this.ContainsKey(key))
            {
                return (JsObject)this[key];
            }
            return null;
        }


        /// <summary>
        /// Writes the contents of this Json type using the specified
        /// <see cref="Vlad2Net.Json.IJsWriter"/>.
        /// </summary>
        /// <param name="writer">The Json writer.</param>
        public void Write(IJsWriter writer) {

            if(writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteBeginObject();
            foreach(KeyValuePair<string, IJsType> pair in this) {
                writer.WriteName(pair.Key);
                pair.Value.Write(writer);
            }
            writer.WriteEndObject();
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        public void Add(string key, string item) {

            if(string.IsNullOrEmpty(item))
                base.Add(key, JsString.Empty);
            else
                base.Add(key, new JsString(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        public void Add(string key, bool item) {

            base.Add(key, JsBoolean.Get(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        public void Add(string key, byte item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        [CLSCompliant(false)]
        public void Add(string key, sbyte item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        public void Add(string key, short item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        [CLSCompliant(false)]
        public void Add(string key, ushort item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        [CLSCompliant(false)]
        public void Add(string key, int item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        [CLSCompliant(false)]
        public void Add(string key, uint item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        public void Add(string key, long item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        [CLSCompliant(false)]
        public void Add(string key, ulong item) {

            base.Add(key, new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified key and item to this dictionary.
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <param name="item">The value of the item.</param>
        public void Add(string key, double item) {

            base.Add(key, new JsNumber(item));
        }        

        /// <summary>
        /// Returns the JsTypeCode for this instance.
        /// </summary>
        public JsTypeCode JsTypeCode {

            get { return JsTypeCode.Object; }
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>This method always returns null.</returns>
        public static implicit operator JsObject(JsNull value) {

            return null;
        }

        #endregion
    }
}
