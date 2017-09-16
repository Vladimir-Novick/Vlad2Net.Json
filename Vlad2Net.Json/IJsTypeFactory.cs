// IJsTypeFactory.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


#if NOT_USED

using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a factory for JavaScript Object Notation data types.
    /// </summary>
    public interface IJsTypeFactory
    {
        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.IJsObject"/>.
        /// </summary>
        /// <returns>A <see cref="Vlad2Net.Json.IJsObject"/>.</returns>
        IJsObject CreateObject();

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.IJsArray"/>.
        /// </summary>
        /// <returns>A <see cref="Vlad2Net.Json.IJsArray"/>.</returns>
        IJsArray CreateArray();

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.IJsString"/> representing
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a <see cref="Vlad2Net.Json.IJsString"/> representing
        /// the specified <paramref name="value"/></returns>
        IJsString CreateString(string value);

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.IJsNumber"/> representing
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a <see cref="Vlad2Net.Json.IJsNumber"/> representing
        /// the specified <paramref name="value"/></returns>
        IJsNumber CreateNumber(double value);

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.IJsBoolean"/> representing
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a <see cref="Vlad2Net.Json.IJsBoolean"/> representing
        /// the specified <paramref name="value"/></returns>
        IJsBoolean CreateBoolean(bool value);

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.IJsNull"/>.
        /// </summary>
        /// <returns>A <see cref="Vlad2Net.Json.IJsNull"/>.</returns>
        IJsNull CreateNull();
    }
}

#endif