// JsStructType.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines the JavaScript Object Notation structure types.
    /// </summary>
    [Serializable()]
    public enum JsStructType
    {
        /// <summary>
        /// No structure.
        /// </summary>
        None,
        /// <summary>
        /// A Json array structure.
        /// </summary>
        Array,
        /// <summary>
        /// A Json object structure.
        /// </summary>
        Object
    }
}
