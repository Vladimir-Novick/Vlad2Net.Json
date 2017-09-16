// JsTypeSkeleton.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a base class for most Json types. This class is abstract.
    /// </summary>
    [Serializable()]
    public abstract class JsTypeSkeleton : IJsType
    {
        #region Private Fields.

        private readonly JsTypeCode _typeCode;

        #endregion

        #region Protected Interface.

        /// <summary>
        /// Initialises a new instance of the JsTypeSkeleton class and specifies the 
        /// type code.
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        protected JsTypeSkeleton(JsTypeCode typeCode) {

            _typeCode = typeCode;
        }

        #endregion

        #region Public Interface.

        /// <summary>
        /// When overriden in a derived class; writes the contents of the Json type 
        /// to the specified <see cref="Vlad2Net.Json.IJsWriter"/>.
        /// </summary>
        /// <param name="writer">The Json writer.</param>
        public abstract void Write(IJsWriter writer);

        /// <summary>
        /// Gets the type code of this Json type.
        /// </summary>
        public JsTypeCode JsTypeCode {

            get { return _typeCode; }
        }

        #endregion
    }
}
