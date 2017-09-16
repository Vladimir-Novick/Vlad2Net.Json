// JsTypeCode.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines the different types of Json structures and primitives.
    /// </summary>
    [Serializable()]
    public enum JsTypeCode
    {
        /// <summary>
        /// A unicode encoded string.
        /// </summary>
        String,
        /// <summary>
        /// A number.
        /// </summary>
        Number,
        /// <summary>
        /// A boolean value represented by literal "true" and "false".
        /// </summary>
        Boolean,
        /// <summary>
        /// A null value.
        /// </summary>
        Null,
        /// <summary>
        /// A structured object containing zero or more name/value pairs, delimited by 
        /// curly brackets.
        /// </summary>
        Object,
        /// <summary>
        /// An unordered collection of values, delimted by square brackets.
        /// </summary>
        Array
    }
}
