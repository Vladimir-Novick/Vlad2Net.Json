//
// Copyright (C) 2006 Vladimir Novick ( v_novick@yahoo.com )
//


using System;
using System.Collections.Generic;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a JavaScript Object Notation Array data type.
    /// </summary>
    public interface IJsArray : IJsType, IList<IJsType> { }
}
