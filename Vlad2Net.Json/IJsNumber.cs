//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a JavaScipt Object Notation Number data type.
    /// </summary>
    public interface IJsNumber : IJsType, IComparable<IJsNumber>, IEquatable<IJsNumber>,
        IComparable<double>, IEquatable<double>, IFormattable
    {
        /// <summary>
        /// Gets the value of the Json number.
        /// </summary>
        double Value {
            get;
        }
    }

}
