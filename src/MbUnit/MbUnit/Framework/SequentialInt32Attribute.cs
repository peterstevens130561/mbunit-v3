// Copyright 2005-2009 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan de Halleux
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
using Gallio.Framework.Data;
using Gallio.Framework.Pattern;
using Gallio.Common.Reflection;
using System.Collections;
using Gallio.Framework.Data.Generation;

namespace MbUnit.Framework
{
    /// <summary>
    /// <para>
    /// Provides a column of sequential <see cref="Int32"/> values as a data source.
    /// </para>
    /// <example>
    /// <code><![CDATA[
    /// [TestFixture]
    /// public class MyTestFixture
    /// {
    ///     [Test]
    ///     public void MyTestMethod1([SequentialInt32(Start = 1, Step = 1, Count = 4)] int value)
    ///     {
    ///         // This test will run 4 times with the values 1, 2, 3 and 4.
    ///     }
    ///     
    ///     [Test]
    ///     public void MyTestMethod2([SequentialInt32(Start = 0, Step = 2, Count = 6)] int value)
    ///     {
    ///         // This test will run 6 times with the values 0, 2, 4, 6, 8 and 10.
    ///     }
    /// }]]></code>
    /// </example>
    /// </summary>
    /// <seealso cref="ColumnAttribute"/>
    [CLSCompliant(false)]
    [AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = true, Inherited = true)]
    public class SequentialInt32Attribute : GenerationDataAttribute
    {
        /// <summary>
        /// Gets or sets the starting value of the sequence.
        /// </summary>
        /// <remarks>
        /// The default value is 0.
        /// </remarks>
        public int Start
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the increment between each value of the sequence.
        /// </summary>
        /// <remarks>
        /// The default value is 1.
        /// </remarks>
        public int Step
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of the sequence.
        /// </summary>
        /// <remarks>
        /// The default value is 10.
        /// </remarks>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a column of sequential <see cref="Int32"/> values.
        /// </summary>
        [CLSCompliant(false)]
        public SequentialInt32Attribute()
        {
            Start = 0;
            Step = 10;
            Count = 1;
        }

        /// <summary>
        /// Returns a generator of random values.
        /// </summary>
        /// <returns>A generator.</returns>
        protected override IGenerator GetGenerator()
        {
            return new SequenceInt32Generator(Start, Step, Count);
        }
    }
}