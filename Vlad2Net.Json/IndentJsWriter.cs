//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


using System;
using System.IO;
using System.Diagnostics;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Provided support for writing JavaScript Object Notation data types to an
    /// underlying <see cref="System.IO.TextWriter"/> whilst indenting the output.
    /// </summary>   
    public class IndentJsWriter : JsWriter
    {
        #region Private Fields.

        private int _indentLevel;
        private string _indentString = "\t";

        #endregion

        #region Public Interface.

        /// <summary>
        /// Initialises a new instance of then IndentedJsWriter class using a
        /// <see cref="System.IO.StringWriter"/> as the underlying 
        /// <see cref="System.IO.TextWriter"/>.
        /// </summary>
        public IndentJsWriter()
            : base() {
        }

        /// <summary>
        /// Initialises a new instance of the IndentedJsWriter class and specifies
        /// the underlying <see cref="System.IO.TextWriter"/> and a value indicating
        /// if the instance owns the specified TextWriter.
        /// </summary>
        /// <param name="writer">The underlying text writer.</param>
        /// <param name="ownsWriter">True if this instance owns the specified TextWriter,
        /// otherwise; false.</param>
        public IndentJsWriter(TextWriter writer, bool ownsWriter)
            : base(writer, ownsWriter) {
        }

        /// <summary>
        /// Gets or sets the string used to indent the output.
        /// </summary>
        public string IndentString {

            get { return _indentString; }
            set {
                if(value == null)
                    throw new ArgumentNullException("value");
                _indentString = value;
            }
        }

        #endregion

        #region Protected Interface.

        /// <summary>
        /// Performs any assertions and / or write operations needed before the specified
        /// token is written to the underlying stream.
        /// </summary>
        /// <param name="token">The next token to be written.</param>
        protected override void PreWrite(JsTokenType token) {

            base.PreWrite(token);
            // Firstly, see if a new line is required.
            switch(base.CurrentStruct) {
                case JsStructType.Array:
                    // Every array element starts on a new line.
                    base.Writer.WriteLine();
                    break;
                case JsStructType.Object:
                    // Don't write primitives on a new line.
                    if(token != JsTokenType.Value)
                        base.Writer.WriteLine();                    
                    break;
                case JsStructType.None:
                    break;
                default:
                    Debug.Fail("IndentJsWriter::PreWrite - Unknown base.CurrentStruct.");
                    break;
            }
            // Secondly, see if the indent should be written and / or adjusted.
            switch(token) {
                case JsTokenType.BeginArray:
                case JsTokenType.BeginObject:
                    WriteIndent();
                    ++this.IndentLevel;
                    break;
                case JsTokenType.EndArray:
                case JsTokenType.EndObject:
                    --this.IndentLevel;
                    WriteIndent();
                    break;
                case JsTokenType.Name:
                    WriteIndent();
                    break;
                case JsTokenType.Value:
                    // Primtives are not written on a new line when in an object.
                    if(base.CurrentStruct != JsStructType.Object)
                        WriteIndent();
                    else
                        base.Writer.Write(" ");
                    break;
                case JsTokenType.None:
                    break;
                default:
                    Debug.Fail("IndentedJsWriter::PreWrite - Unknown token.");
                    break;
            }
        }

        /// <summary>
        /// Writes the indent to the underlying stream.
        /// </summary>
        protected void WriteIndent() {

            for(int i = 0; i < this.IndentLevel; ++i)
                base.Writer.Write(this.IndentString);           
        }

        /// <summary>
        /// Gets or sets the indent level.
        /// </summary>
        protected int IndentLevel {

            get { return _indentLevel; }
            set {
                if(value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _indentLevel = value;
            }
        }

        #endregion
    }
}
