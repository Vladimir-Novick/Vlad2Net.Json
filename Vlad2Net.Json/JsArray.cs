//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Represents a JavaScript Object Notation Array data type which contains a 
    /// collection of <see cref="Vlad2Net.Json.IJsType"/>s.
    /// </summary>
    [Serializable()]
    public class JsArray : Collection<IJsType>, IJsArray
    {
        #region Protected Interface.

        /// <summary>
        /// Inserts the specified <paramref name="item"/> into this collection at the
        /// specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        protected override sealed void InsertItem(int index, IJsType item) {

            ValidateItem(item);
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Replaces an item at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to replace.</param>
        /// <param name="item">The replacement item.</param>
        protected override sealed void SetItem(int index, IJsType item) {

            ValidateItem(item);
            base.SetItem(index, item);
        }

        /// <summary>
        /// Validates the specified <paramref name="item"/> before insertion or 
        /// setting.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        protected virtual void ValidateItem(IJsType item) {

            // If the user wants to add a null member they should use JsNull.
            if(item == null)
                throw new ArgumentNullException("item");
        }

        #endregion

        #region Public Interface.
     
        /// <summary>
        /// Inialises a new instance of the JsArray class.
        /// </summary>
        public JsArray() {
        }

        /// <summary>
        /// Writes the contents of this Json type using the specified
        /// <see cref="Vlad2Net.Json.IJsWriter"/>.
        /// </summary>
        /// <param name="writer">The Json writer.</param>
        public void Write(IJsWriter writer) {

            if(writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteBeginArray();
            foreach(IJsType jt in this)
                jt.Write(writer);
            writer.WriteEndArray();
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(string item) {

            if(string.IsNullOrEmpty(item))
                base.Add(JsString.Empty);
            else
                base.Add(new JsString(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(bool item) {

            base.Add(JsBoolean.Get(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(byte item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        [CLSCompliant(false)]
        public void Add(sbyte item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(short item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        [CLSCompliant(false)]
        public void Add(ushort item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(int item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        [CLSCompliant(false)]
        public void Add(uint item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(long item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        [CLSCompliant(false)]
        public void Add(ulong item) {

            base.Add(new JsNumber(item));
        }

        /// <summary>
        /// Adds the specified item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(double item) {

            base.Add(new JsNumber(item));
        }        

        /// <summary>
        /// Returns the JsTypeCode for this instance.
        /// </summary>
        public JsTypeCode JsTypeCode {

            get { return JsTypeCode.Array; }
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        /// <param name="value">JsNull value.</param>
        /// <returns>This method always returns null.</returns>
        public static implicit operator JsArray(JsNull value) {

            return null;
        }

        #endregion
    }
}
