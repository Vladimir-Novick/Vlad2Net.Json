// IJsonType.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a JavaScript Object Notation data type.
    /// </summary>
    public interface IJsType
    {
        /// <summary>
        /// Writes the contents of the Json type using the specified
        /// </summary>
        /// <param name="writer">The Json writer.</param>
        void Write(IJsWriter writer);

        /// <summary>
        /// Gets the <see cref="Vlad2Net.Json.JsTypeCode"/> of the type.
        /// </summary>
        JsTypeCode JsTypeCode {
            get;
        }
    }
}
