// Copyright 2007 MbUnit Project - http://www.mbunit.com/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace Gallio.Logging
{
    /// <summary>
    /// Defines constants for commonly used mime types.
    /// </summary>
    public static class MimeTypes
    {
        /// <summary>
        /// Plain text data.
        /// </summary>
        public const string PlainText = "text/plain";

        /// <summary>
        /// Xml data.
        /// </summary>
        public const string Xml = "text/xml";
        
        /// <summary>
        /// HTML.
        /// </summary>
        public const string Html = "text/html";

        /// <summary>
        /// Well-formed XHTML.
        /// </summary>
        public const string XHtml = "text/xhtml+xml";

        /// <summary>
        /// PNG image.
        /// </summary>
        public const string Png = "image/png";
    }
}
