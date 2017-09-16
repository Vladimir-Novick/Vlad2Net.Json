//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//

using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Defines a JavaScript Object Notation writer.
    /// </summary>
    public interface IJsWriter
    {
        /// <summary>
        /// Writes the start of an array to the underlying data stream.
        /// </summary>
        void WriteBeginArray();        

        /// <summary>
        /// Writes the end of an array to the underlying data stream.
        /// </summary>
        void WriteEndArray();

        /// <summary>
        /// Writes the start of an object to the underlying data stream.
        /// </summary>
        void WriteBeginObject();        

        /// <summary>
        /// Writes the end of an object to the underlying data stream.
        /// </summary>
        void WriteEndObject();

        /// <summary>
        /// Writes a object property name to the underlying data stream.
        /// </summary>
        /// <param name="value">The property name.</param>
        void WriteName(string value);

        /// <summary>
        /// Writes a raw string value to the underlying data stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        void WriteValue(string value);        
    }
}
