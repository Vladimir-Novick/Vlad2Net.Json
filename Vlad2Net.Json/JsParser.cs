// JsParser.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
// 2010 insert unicode encoding request support
//


using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Provided support for parsing JavaScript Object Notation data types from
    /// an underlying <see cref="System.IO.TextReader"/>.
    /// </summary>    
    public class JsParser : Disposable
    {
        #region Private Fields.

        private int _depth;
        private int _maxDepth = JsParser.DEFAULT_MAX_DEPTH;        
        private readonly bool _ownsReader;        
        private TextReader _rdr;        

        private const int DEFAULT_MAX_DEPTH = 20;

        #endregion

        #region Public Interface.

        /// <summary>
        /// 
        /// </summary>
        [Serializable()]
        public enum TokenType
        {
            /// <summary>
            /// Begin array token.
            /// </summary>
            BeginArray,
            /// <summary>
            /// End array token.
            /// </summary>
            EndArray,
            /// <summary>
            /// Begin object token.
            /// </summary>
            BeginObject,
            /// <summary>
            /// End object token.
            /// </summary>
            EndObject,
            /// <summary>
            /// Member seperator token
            /// </summary>
            ValueSeperator,
            /// <summary>
            /// Object property name / value seperator token.
            /// </summary>
            NameSeperator,
            /// <summary>
            /// Start of string token.
            /// </summary>
            String,
            /// <summary>
            /// Start of number token.
            /// </summary>
            Number,
            /// <summary>
            /// Start of boolean token.
            /// </summary>
            Boolean,
            /// <summary>
            /// Start of null token.
            /// </summary>
            Null,
            /// <summary>
            /// End of input token.
            /// </summary>
            EOF,
        }

        /// <summary>
        /// Initialises a new instance of the JsParser class and specifies the source
        /// <see cref="System.IO.TextReader"/> and a value indicating if the instance
        /// owns the specified TextReader.
        /// </summary>
        /// <param name="rdr">The underlying TextReader.</param>
        /// <param name="ownsReader">True if this instance owns the TextReader, otherwise; 
        /// false.</param>
        public JsParser(TextReader rdr, bool ownsReader) {

            if(rdr == null)
                throw new ArgumentNullException("rdr");

            _rdr = rdr;
            _ownsReader = ownsReader;
        }

        /// <summary>
        /// Classifies the next token on the underlying stream.
        /// </summary>
        /// <returns>The classification of the next token from the underlying stream.</returns>
        public TokenType NextToken() {

            CheckDisposed();

            int ch = Peek(true);

            if(ch < 0)
                return TokenType.EOF;
            switch(ch) {
                case JsWriter.BeginArray:
                    return TokenType.BeginArray;
                case JsWriter.EndArray:
                    return TokenType.EndArray;
                case JsWriter.BeginObject:
                    return TokenType.BeginObject;
                case JsWriter.EndObject:
                    return TokenType.EndObject;
                case JsWriter.NameSeperator:
                    return TokenType.NameSeperator;
                case JsWriter.ValueSeperator:
                    return TokenType.ValueSeperator;
                case '"':
                    return TokenType.String;
                case 'n':
                case 'N':
                    return TokenType.Null;
                case 'f':
                case 'F':
                case 't':
                case 'T':
                    return TokenType.Boolean;
                case '.':
                case '-':
                case '+':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return TokenType.Number;                
                default:
                    throw new FormatException(string.Format(
                        "The character '{0}' does not start any valid token.",
                        ((char)ch).ToString()));
            }
        }

        /// <summary>
        /// Parses a <see cref="Vlad2Net.Json.JsBoolean"/> from the underlying
        /// stream.
        /// </summary>
        /// <returns>The parsed JsBoolean.</returns>
        public JsBoolean ParseBoolean() {
            
            AssertNext(TokenType.Boolean);

            int ch = Peek();

            if(ch == 'f' || ch == 'F') {
                if(Match(JsBoolean.FalseString))
                    return JsBoolean.False;
            } else if(Match(JsBoolean.TrueString))
                return JsBoolean.True;

            throw new FormatException("The input contains a malformed Json-Boolean.");
        }

        /// <summary>
        /// Parses a <see cref="Vlad2Net.Json.JsNull"/> from the underlying
        /// stream.
        /// </summary>
        /// <returns>The parsed JsNull.</returns>
        public JsNull ParseNull() {
            
            AssertNext(TokenType.Null);
            if(Match(JsNull.NullString))
                return JsNull.Null;
            throw new FormatException("The input contains a malformed Json-Null.");
        }

        /// <summary>
        /// Parses a <see cref="Vlad2Net.Json.JsNumber"/> from the underlying
        /// stream.
        /// </summary>
        /// <returns>The parsed JsNumber.</returns>
        public JsNumber ParseNumber() {
            
            AssertNext(TokenType.Number);

            int ch;
            double value;
            StringBuilder sb = new StringBuilder();

            while((ch = Peek()) > -1 && JsParser.IsNumberComponent(ch))
                sb.Append((char)Read());

            if(double.TryParse(sb.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture,
                out value))
                return new JsNumber(value);

            throw new FormatException("The input contains a malformed Json-Number.");
        }

        /// <summary>
        /// Parses a <see cref="Vlad2Net.Json.JsObject"/> and all contained types 
        /// from the underlying stream.
        /// </summary>
        /// <returns>The parsed JsObject.</returns>
        public virtual JsObject ParseObject() {

            AssertNext(TokenType.BeginObject);
            AssertDepth(++this.Depth);

            string key;
            TokenType type;
            SpState state = SpState.Initial;
            JsObject obj = new JsObject();
            // Move into the object.            
            for(Read(); ; ) {
                switch(NextToken()) {
                    case TokenType.String:
                        key = ParseStringImpl();
                        if(NextToken() != TokenType.NameSeperator)
                            goto error;
                        Read();
                        break;
                    case TokenType.ValueSeperator:
                        if(state != SpState.SepValid)
                            goto error;
                        Read();
                        // Empty members are illegal.
                        state = SpState.ReqValue;
                        continue;
                    case TokenType.EndObject:
                        if(state == SpState.ReqValue)
                            goto error;
                        Read();
                        --this.Depth;
                        return obj;
                    default:
                        goto error;
                }
                switch(type = NextToken()) {
                    case TokenType.EndArray:
                    case TokenType.EndObject:
                    case TokenType.NameSeperator:
                    case TokenType.EOF:
                    case TokenType.ValueSeperator:
                        goto error;
                    default:
                        obj.Add(key, ParseNext(type));
                        state = SpState.SepValid;
                        break;
                }
            }
error:
            throw new FormatException("The input contains a malformed Json-Object.");
        }

        /// <summary>
        /// Parses a <see cref="Vlad2Net.Json.JsArray"/> and all contained types 
        /// from the underlying stream.
        /// </summary>
        /// <returns>The parsed JsArray.</returns>
        public virtual JsArray ParseArray() {

            AssertNext(TokenType.BeginArray);
            AssertDepth(++this.Depth);

            TokenType type;
            SpState state = SpState.Initial;
            JsArray arr = new JsArray();
            // Move into the array.            
            for(Read(); ; ) {
                switch(type = NextToken()) {
                    case TokenType.EndArray:
                        if(state == SpState.ReqValue)
                            goto error;
                        Read();
                        --this.Depth;
                        return arr;
                    case TokenType.ValueSeperator:
                        if(state != SpState.SepValid)
                            goto error;
                        Read();
                        // Empty elements are illegal.
                        state = SpState.ReqValue;
                        break;
                    case TokenType.EndObject:
                    case TokenType.EOF:
                    case TokenType.NameSeperator:
                        goto error;
                    default:
                        arr.Add(ParseNext(type));
                        state = SpState.SepValid;
                        break;
                }
            }
error:
            throw new FormatException("The input contains a malformed Json-Array.");
        }

        /// <summary>
        /// Parses a <see cref="Vlad2Net.Json.JsString"/> from the underlying stream.
        /// </summary>
        /// <returns>The parsed JsString.</returns>
        public JsString ParseString() {

            return new JsString(ParseStringImpl());
        }

        /// <summary>
        /// Parses the next type from the underlying stream.
        /// </summary>
        /// <returns>The next type from the underlying stream.</returns>
        public IJsType ParseNext() {

            return ParseNext(NextToken());
        }

        /// <summary>
        /// Parses the specified <paramref name="type"/> from the underlying stream.
        /// </summary>
        /// <param name="type">The type to parse.</param>
        /// <returns>The type parsed from the underlying stream.</returns>
        public virtual IJsType ParseNext(TokenType type) {

            switch(type) {
                case TokenType.BeginArray:
                    return ParseArray();
                case TokenType.BeginObject:
                    return ParseObject();
                case TokenType.String:
                    return ParseString();
                case TokenType.Number:
                    return ParseNumber();
                case TokenType.Boolean:
                    return ParseBoolean();
                case TokenType.Null:
                    return ParseNull();
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException(
                        "type", type.GetHashCode(), typeof(TokenType));
            }
        }

        /// <summary>
        /// Closes this parser.
        /// </summary>
        public void Close() {

            Dispose(true);
        }

        /// <summary>
        /// Gets the current depth of the parser.
        /// </summary>
        public int Depth {

            get { return _depth; }
            protected set { _depth = value; }
        }

        /// <summary>
        /// Gets or sets the maximum depth of structures before a 
        /// <see cref="System.FormatException"/> is thrown.
        /// </summary>
        public int MaximumDepth {

            get { return _maxDepth; }
            set {
                if(value < 1)
                    throw new ArgumentOutOfRangeException("value");
                _maxDepth = value;
            }
        }

        #endregion

        #region Protected Interface.

        /// <summary>
        /// Peeks at and returns a single character from the underlying stream.
        /// </summary>
        /// <returns>The character, otherwise; -1 if the end of the stream has been reached.</returns>
        protected int Peek() {

            CheckDisposed();

            return this.Reader.Peek();
        }

        /// <summary>
        /// Peeks at the next character from the underlying stream and specifies a value
        /// which indicates whether white space characters should be advanced over.
        /// </summary>
        /// <param name="skipWhite">True to skip white space characters, otherwise; false.</param>
        /// <returns>The next character from the underlying stream, or -1 if the end
        /// has been reached.</returns>
        protected int Peek(bool skipWhite) {

            CheckDisposed();

            if(!skipWhite)
                return this.Reader.Peek();

            int ch;

            while((ch = this.Reader.Peek()) > 0) {
                if(!char.IsWhiteSpace((char)ch))
                    return ch;
                this.Reader.Read();
            }

            return -1;
        }

        /// <summary>
        /// Reads and returns a single character from the underlying stream.
        /// </summary>
        /// <returns>The character, otherwise; -1 if the end of the stream has been reached.</returns>
        protected int Read() {

            CheckDisposed();

            return this.Reader.Read();
        }

        /// <summary>
        /// Reads the next character from the underlying stream and specified a value
        /// which indicates whether white space characters should be skipped.
        /// </summary>
        /// <param name="skipWhite">True to skip white space characters, otherwise; false.</param>
        /// <returns>The next character from the underlying stream, or -1 if the end
        /// has been reached.</returns>
        protected int Read(bool skipWhite) {

            CheckDisposed();

            if(!skipWhite)
                return this.Reader.Read();

            int ch;

            while((ch = this.Reader.Read()) > 0) {
                if(!char.IsWhiteSpace((char)ch))
                    return ch;
            }

            return -1;
        }

        /// <summary>
        /// Asserts that the specified depth does not exceed 
        /// <see cref="P:Vlad2Net.Json.JsParser.MaximumDepth"/>. If the depth has been
        /// exceeded, a <see cref="System.FormatException"/> is thrown.
        /// </summary>
        /// <param name="depth">The depth.</param>
        protected void AssertDepth(int depth) {

            if(depth > this.MaximumDepth)
                throw new FormatException(string.Format(
                    "The maximum depth of {0} has been exceeded.", this.MaximumDepth.ToString()));
        }

        /// <summary>
        /// Disposed of this instance.
        /// </summary>
        /// <param name="disposing">True if being called explicitly, otherwise; false
        /// to indicate being called implicitly by the GC.</param>
        protected override void Dispose(bool disposing) {

            if(!base.IsDisposed) {
                try {
                    if(this.OwnsReader && disposing)
                        ((IDisposable)this.Reader).Dispose();
                } catch {
                } finally {
                    this.Reader = null;                    
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets a value indicating if this instance owns it's 
        /// <see cref="P:Vlad2Net.Json.TextParser.Reader"/>.
        /// </summary>
        protected bool OwnsReader {

            get { return _ownsReader; }
        }

        /// <summary>
        /// Gets the underlying <see cref="System.IO.TextReader"/>.
        /// </summary>
        protected TextReader Reader {

            get { return _rdr; }
            private set { _rdr = value; }
        }

        #endregion     

        #region Private Impl.

        /// <summary>
        /// Structure parse state.
        /// </summary>
        private enum SpState
        {
            /// <summary>
            /// Initial.
            /// </summary>
            Initial,
            /// <summary>
            /// A value is required.
            /// </summary>
            ReqValue,
            /// <summary>
            /// A seperator is currently valid.
            /// </summary>
            SepValid,
        }

        private bool Match(string value) {

            // This method assumes that value is in lower case.

            int ch;

            for(int i = 0; i < value.Length; ++i) {
                if((ch = Read()) < 0 || char.ToLowerInvariant((char)ch) != value[i])
                    return false;
            }

            return true;
        }

        private string ParseStringImpl() {

            AssertNext(TokenType.String);

            int ch;
            StringBuilder sb = new StringBuilder();
            // Move into the string.
            Read();
            while((ch = Read()) > -1) {
                if(ch == '"')
                    return sb.ToString();
               if (ch == '%')
                {
                    ch = Read();
                    if (ch == 'u')
                    {
                        sb.Append(ParseUnicode());
                    }
                    else
                    {
                        sb.Append(ParseURLEncoding(ch));
                    }
                }  else 
                if (ch != '\\')
                    sb.Append((char)ch);
                else {
                    if((ch = Read()) < 0)
                        goto error;
                    switch(ch) {
                        case '"':
                            sb.Append('"');
                            break;
                        case '/':
                            sb.Append('/');
                            break;
                        case '\\':
                            sb.Append('\\');
                            break;
                        case 'b':
                            sb.Append('\b');
                            break;
                        case 'f':
                            sb.Append('\f');
                            break;
                        case 'n':
                            sb.Append('\n');
                            break;
                        case 'r':
                            sb.Append('\r');
                            break;
                        case 't':
                            sb.Append('\t');
                            break;
                        case 'u':
                            sb.Append(ParseUnicode());
                            break;
                        default:
                            goto error;
                    }
                }
            }
error:
            throw new FormatException("The input contains a malformed Json-String.");
        }


        private char ParseURLEncoding(int c)
        {
            int ch1 = c;
            int ch2 = Read();
            return (char) (JsParser.FromHex(ch1) << 4 | JsParser.FromHex(ch2));
        }

        private char ParseUnicode() {

            int ch1 = Read();
            int ch2 = Read();
            int ch3 = Read();
            int ch4 = Read();

            if(ch1 > -1 && ch2 > -1 && ch3 > -1 && ch4 > -1)
                return (char)(JsParser.FromHex(ch1) << 12 | JsParser.FromHex(ch2) << 8 |
                    JsParser.FromHex(ch3) << 4 | JsParser.FromHex((ch4)));

            throw new FormatException("The input contains a malformed character escape.");
        }

        private static int FromHex(int ch) {

            if(ch >= '0' && ch <= '9')
                return ch - '0';
            if(ch >= 'a' && ch <= 'f')
                return (ch - 'a') + 10;
            if(ch >= 'A' && ch <= 'F')
                return (ch - 'A') + 10;

            throw new FormatException("The input contains a malformed hexadecimal character escape.");
        }

        private static bool IsNumberComponent(int ch) {

            return (ch >= '0' && ch <= '9') || ch == '-' || ch == '+' || ch == '.' ||
                ch == 'e' || ch == 'E';
        }

        private void AssertNext(TokenType type) {

            if(NextToken() != type)
                throw new InvalidOperationException(
                    string.Format("Method must only be called when {0} is the next token.",
                        type.ToString()));
        }

        #endregion
    }
}
