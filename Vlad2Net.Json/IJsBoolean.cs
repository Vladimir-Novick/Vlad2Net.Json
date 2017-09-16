// IJsBoolean.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a JavaScipt Object Notation Boolean data type.
    /// </summary>
    public interface IJsBoolean : IJsType, IEquatable<IJsBoolean>, IEquatable<bool>
    {
        /// <summary>
        /// Gets the value of the Json boolean.
        /// </summary>
        bool Value {
            get;
        }
    }
}
