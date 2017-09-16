// JsTypeFactory.cs
//
// Copyright (C) 2006 Vladimir Novick v_novick@yahoo.com
//


#if NOT_USED

using System;

namespace Vlad2Net.Json
{
    /// <summary>
    /// Represents the default type factory for the <see cref="Vlad2Net.Json"/>
    /// namespace.
    /// </summary>
    public sealed class JsTypeFactory : IJsTypeFactory
    {
        #region Public Interface.

        /// <summary>
        /// Gets the default instance of the factory.
        /// </summary>
        public static readonly JsTypeFactory Instance = new JsTypeFactory();        

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.JsObject"/>.
        /// </summary>
        /// <returns>A <see cref="Vlad2Net.Json.JsObject"/>.</returns>
        public JsObject CreateObject() {

            return new JsObject();
        }

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.JsArray"/>.
        /// </summary>
        /// <returns>A <see cref="Vlad2Net.Json.JsArray"/>.</returns>
        public JsArray CreateArray() {

            return new JsArray();
        }

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.JsString"/> representing
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a <see cref="Vlad2Net.Json.JsString"/> representing
        /// the specified <paramref name="value"/></returns>
        public JsString CreateString(string value) {

            if(value == null)
                return null;
            if(value.Equals(string.Empty))
                return JsString.Empty;
            return new JsString(value);
        }

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.JsNumber"/> representing
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a <see cref="Vlad2Net.Json.JsNumber"/> representing
        /// the specified <paramref name="value"/></returns>
        public JsNumber CreateNumber(double value) {

            return new JsNumber(value);
        }

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.JsBoolean"/> representing
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a <see cref="Vlad2Net.Json.JsBoolean"/> representing
        /// the specified <paramref name="value"/></returns>
        public JsBoolean CreateBoolean(bool value) {

            return JsBoolean.Get(value);
        }

        /// <summary>
        /// Creates a <see cref="Vlad2Net.Json.JsNull"/>.
        /// </summary>
        /// <returns>A <see cref="Vlad2Net.Json.JsNull"/>.</returns>
        public JsNull CreateNull() {

            return JsNull.Null;
        }

        #endregion

        #region Explict Interface.

        IJsObject IJsTypeFactory.CreateObject() {

            return CreateObject();
        }

        IJsArray IJsTypeFactory.CreateArray() {

            return CreateArray();
        }

        IJsString IJsTypeFactory.CreateString(string value) {

            return CreateString(value);
        }

        IJsNumber IJsTypeFactory.CreateNumber(double value) {

            return CreateNumber(value);
        }

        IJsBoolean IJsTypeFactory.CreateBoolean(bool value) {

            return CreateBoolean(value);
        }

        IJsNull IJsTypeFactory.CreateNull() {

            return CreateNull();
        }

        #endregion

        #region Private Impl.

        private JsTypeFactory() {
        }

        #endregion
    }  
}

#endif