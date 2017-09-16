// JsTokenType.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines the high level Json tokens.
    /// </summary>
    [Serializable()]
    public enum JsTokenType
    {
        /// <summary>
        /// No token.
        /// </summary>
        None = 0,
        /// <summary>
        /// The start of array token.
        /// </summary>
        BeginArray = 1,
        /// <summary>
        /// The end of array token.
        /// </summary>
        EndArray = 2,
        /// <summary>
        /// The start of object token.
        /// </summary>
        BeginObject = 3,
        /// <summary>
        /// The end of object token.
        /// </summary>
        EndObject = 4,
        /// <summary>
        /// An object property name token.
        /// </summary>
        Name = 5,
        /// <summary>
        /// A value token.
        /// </summary>
        Value = 6,
    }

}
