//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.Collections.Generic;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a JavaScript Object Notation Object data type.
    /// </summary>
    public interface IJsObject : IJsType, IDictionary<string, IJsType> { }
}
