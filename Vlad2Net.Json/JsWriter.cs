// JsWriter.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Provided support for writing JavaScript Object Notation data types to an
    /// underlying <see cref="System.IO.TextWriter"/>.
    /// </summary>    
    public class JsWriter : Disposable, IJsWriter
    {
        #region Private Fields.

        private readonly bool _ownsWriter;
        private TextWriter _writer;
        private JsTokenType _token;
        private Stack<JsStructType> _structStack;

        // Table is indexed by JsTokenType. See PreWrite.
        // Note: jagged arrays are more efficient than multi-dimensional arrays.
        private static readonly ST[][] TRANSITION_TABLE = 
            {
                new ST[] { ST.ERR, ST.SOK, ST.ERR, ST.SOK, ST.ERR, ST.ERR, ST.ERR },
                new ST[] { ST.ERR, ST.SOK, ST.AIA, ST.SOK, ST.ERR, ST.ERR, ST.SOK },
                new ST[] { ST.ERR, ST.AIS, ST.AIA, ST.AIS, ST.AIO, ST.AIO, ST.AIS },
                new ST[] { ST.ERR, ST.ERR, ST.ERR, ST.ERR, ST.AIO, ST.SOK, ST.ERR },
                new ST[] { ST.ERR, ST.AIS, ST.AIA, ST.AIS, ST.AIO, ST.AIO, ST.AIS },
                new ST[] { ST.ERR, ST.SOK, ST.ERR, ST.SOK, ST.ERR, ST.ERR, ST.SOK },
                new ST[] { ST.ERR, ST.SOK, ST.AIA, ST.SOK, ST.AIO, ST.AIO, ST.SOK }
            };

        #endregion

        #region Public Interface.

        /// <summary>
        /// Defines the start of an array. This field is constant.
        /// </summary>
        public const char BeginArray = '[';

        /// <summary>
        /// Defines the end of an array. This field is constant.
        /// </summary>
        public const char EndArray = ']';

        /// <summary>
        /// Defines the start of an object. This field is constant.
        /// </summary>
        public const char BeginObject = '{';

        /// <summary>
        /// Defines the end of an object. This field is constant.
        /// </summary>
        public const char EndObject = '}';

        /// <summary>
        /// Defines the value seperator. This field is constant.
        /// </summary>
        public const char ValueSeperator = ',';

        /// <summary>
        /// Defines the object property name and value seperator. This field is constant.
        /// </summary>       
        public const char NameSeperator = ':';        

        /// <summary>
        /// Initialises a new instance of then JsWriter class using a
        /// <see cref="System.IO.StringWriter"/> as the underlying 
        /// <see cref="System.IO.TextWriter"/>.
        /// </summary>
        public JsWriter()
            : this(new StringWriter(), true) {
        }

        /// <summary>
        /// Initialises a new instance of the JsWriter class and specifies
        /// the underlying <see cref="System.IO.TextWriter"/> and a value indicating
        /// if the instance owns the specified TextWriter.
        /// </summary>
        /// <param name="writer">The underlying text writer.</param>
        /// <param name="ownsWriter">True if this instance owns the specified TextWriter,
        /// otherwise; false.</param>
        public JsWriter(TextWriter writer, bool ownsWriter) {

            if(writer == null)
                throw new ArgumentNullException("writer");

            _writer = writer;
            _ownsWriter = ownsWriter;
            _token = JsTokenType.None;            
            _structStack = new Stack<JsStructType>();
        }        

        /// <summary>
        /// Writes the start of an array to the underlying data stream.
        /// </summary>
        public void WriteBeginArray() {

            CheckDisposed();
            PreWrite(JsTokenType.BeginArray);
            this.Writer.Write(JsWriter.BeginArray);
            PostWrite(JsTokenType.BeginArray);
        }       

        /// <summary>
        /// Writes the end of an array to the underlying data stream.
        /// </summary>
        public void WriteEndArray() {

            CheckDisposed();
            PreWrite(JsTokenType.EndArray);
            this.Writer.Write(JsWriter.EndArray);
            PostWrite(JsTokenType.EndArray);
        }

        /// <summary>
        /// Writes the start of an object to the underlying data stream.
        /// </summary>
        public void WriteBeginObject() {

            CheckDisposed();
            PreWrite(JsTokenType.BeginObject);
            this.Writer.Write(JsWriter.BeginObject);
            PostWrite(JsTokenType.BeginObject);
        }

        /// <summary>
        /// Writes the end of an object to the underlying data stream.
        /// </summary>
        public void WriteEndObject() {

            CheckDisposed();
            PreWrite(JsTokenType.EndObject);
            this.Writer.Write(JsWriter.EndObject);
            PostWrite(JsTokenType.EndObject);
        }

        /// <summary>
        /// Writes a object property name to the underlying data stream.
        /// </summary>
        /// <param name="value">The property name.</param>
        public void WriteName(string value) {

            if(value == null)
                throw new ArgumentNullException("value");

            CheckDisposed();
            PreWrite(JsTokenType.Name);
            this.Writer.Write(JsString.Encode(value));
            this.Writer.Write(JsWriter.NameSeperator);
            PostWrite(JsTokenType.Name);
        }

        /// <summary>
        /// Writes a raw string value to the underlying data stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public void WriteValue(string value) {

            if(value == null)
                throw new ArgumentNullException("value");

            CheckDisposed();
            PreWrite(JsTokenType.Value);
            this.Writer.Write(value);
            PostWrite(JsTokenType.Value);
        }

        /// <summary>
        /// Closes this writer.
        /// </summary>
        public void Close() {

            Dispose(true);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> representation of this instance.</returns>
        public override string ToString() {

            CheckDisposed();

            return this.Writer.ToString();
        }

        #endregion

        #region Protected Interface.

        /// <summary>
        /// Performs any assertions and / or write operations needed before the specified
        /// token is written to the underlying stream.
        /// </summary>
        /// <param name="token">The next token to be written.</param>
        protected virtual void PreWrite(JsTokenType token) {            

            switch(JsWriter.TRANSITION_TABLE[(int)this.CurrentToken][(int)token]) {
                case ST.SOK:
                    // void.
                    break;
                case ST.ERR:
                    Assert(false, token);
                    break;
                case ST.AIO:
                    Assert(this.CurrentStruct == JsStructType.Object, token);
                    break;
                case ST.AIA:
                    Assert(this.CurrentStruct == JsStructType.Array, token);
                    break;
                case ST.AIS:
                    Assert(this.CurrentStruct != JsStructType.None, token);
                    break;
                default:
                    Debug.Fail("JsWriter::PreWrite - Unknown token.");
                    break;
            }
            // This is horrible but without increasing the complexity of the state
            // table it is needed.
            Assert(!(this.CurrentStruct == JsStructType.Object && token != JsTokenType.EndObject &&
                this.CurrentToken != JsTokenType.Name && token != JsTokenType.Name), token);
            // See if we should write a seperator.
            if(!JsWriter.IsStructEnd(token) && (JsWriter.IsStructEnd(this.CurrentToken) ||
                this.CurrentToken == JsTokenType.Value))
                this.Writer.Write(JsWriter.ValueSeperator);
        }

        /// <summary>
        /// Performs any post write operations needed after the specified
        /// token has been written to the underlying stream.
        /// </summary>
        /// <param name="token">The token written.</param>
        protected virtual void PostWrite(JsTokenType token) {

            this.CurrentToken = token;
            switch(token) {
                case JsTokenType.BeginArray:
                    this.StructStack.Push(JsStructType.Array);
                    break;
                case JsTokenType.BeginObject:
                    this.StructStack.Push(JsStructType.Object);
                    break;
                case JsTokenType.EndArray:
                    Debug.Assert(this.CurrentStruct == JsStructType.Array);
                    this.StructStack.Pop();
                    break;
                case JsTokenType.EndObject:
                    Debug.Assert(this.CurrentStruct == JsStructType.Object);
                    this.StructStack.Pop();
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Disposed of this instance.
        /// </summary>
        /// <param name="disposing">True if being called explicitly, otherwise; false
        /// to indicate being called implicitly by the GC.</param>
        protected override void Dispose(bool disposing) {

            if(!base.IsDisposed) {
                try {
                    if(this.OwnsWriter && disposing)
                        ((IDisposable)this.Writer).Dispose();
                } catch {
                } finally {
                    this.Writer = null;                    
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets the current lexical JsonToken.
        /// </summary>
        protected JsTokenType CurrentToken {

            get { return _token; }
            set { _token = value; }
        }

        /// <summary>
        /// Peeks at the top structure on the 
        /// <see cref="P:Vlad2Net.Json.JsWriter.StructStack"/>.
        /// </summary>
        protected JsStructType CurrentStruct {

            get { 
                return this.StructStack.Count > 0 ? this.StructStack.Peek() : 
                    JsStructType.None; 
            }
        }

        /// <summary>
        /// Gets the stack of structure types. DO NOT push 
        /// <see cref="Vlad2Net.Json.JsStructType.None"/> onto the stack.
        /// </summary>
        protected Stack<JsStructType> StructStack {

            get { return _structStack; }
        }

        /// <summary>
        /// Gets a value indicating if this instance owns it's 
        /// <see cref="P:Vlad2Net.Json.JsWriter.Writer"/>.
        /// </summary>
        protected bool OwnsWriter {

            get { return _ownsWriter; }
        }

        /// <summary>
        /// Gets the underlying <see cref="System.IO.TextWriter"/>.
        /// </summary>
        protected TextWriter Writer {

            get { return _writer; }
            private set { _writer = value; }
        }  

        #endregion

        #region Internal Interface.

        internal static bool IsStructEnd(JsTokenType token) {

            return token == JsTokenType.EndArray || token == JsTokenType.EndObject;
        }

        internal static bool IsStructStart(JsTokenType token) {

            return token == JsTokenType.BeginArray || token == JsTokenType.BeginObject;
        }  

        #endregion

        #region Private Impl.

        [Serializable()]
        private enum ST
        {
            /// <summary>
            /// State OK.
            /// </summary>
            SOK,
            /// <summary>
            /// Error.
            /// </summary>
            ERR,
            /// <summary>
            /// Assert in object.
            /// </summary>
            AIO,
            /// <summary>
            /// Assert in array.
            /// </summary>
            AIA,
            /// <summary>
            /// Assert in struct.
            /// </summary>
            AIS
        }

        private void Assert(bool cond, JsTokenType nextToken) {

            if(!cond) {
                StringBuilder sb = new StringBuilder(150);

                sb.Append("Attempted state transition would lead to an invalid JSON output.");
                sb.Append(Environment.NewLine);
                sb.Append("Current Token:\t").Append(this.CurrentToken.ToString());
                sb.Append(Environment.NewLine);
                sb.Append("Attempted Token:\t").Append(nextToken.ToString());
                sb.Append(Environment.NewLine);
                sb.Append("Current Struct:\t").Append(this.CurrentStruct.ToString());
                throw new InvalidOperationException(sb.ToString());
            }
        }

        #endregion
    }
}
