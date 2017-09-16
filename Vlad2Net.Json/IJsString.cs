
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a JavaScipt Object Notation String data type.
    /// </summary>
    public interface IJsString : IJsType, IEquatable<IJsString>, IEquatable<string>,
        IComparable<IJsString>, IComparable<string>
    {
        /// <summary>
        /// Gets the value of the Json string.
        /// </summary>
        string Value {
            get;
        }
    }
}
