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

using System.Collections.Generic;
using Gallio.Model.Execution;

namespace Gallio.Model
{
    /// <summary>
    /// <para>
    /// A test object represents a single instance of a test that has been
    /// generated from a <see cref="ITemplate" /> using particular
    /// bindings in a particular scope.
    /// </para>
    /// <para>
    /// A test may depend on one or more other tests.  When a test
    /// fails, the tests that depend on it are also automatically
    /// considered failures.  Moreover, the test harness ensures
    /// that a test will only run once all of its dependencies have
    /// completed execution successfully.  A run-time error will
    /// occur when the system detects the presence of circular test dependencies
    /// or attempts to execute a test concurrently with its dependencies.
    /// </para>
    /// <para>
    /// A test contain child tests.  The children of a test are executed
    /// in dependency order within the scope of the parent test.  Thus the parent
    /// test may setup/teardown the execution environment used to execute
    /// its children.  Tests that belong to different subtrees are executed in
    /// relative isolation within the common environment established by their common parent.
    /// </para>
    /// <para>
    /// The object model distinguishes between tests that represent individual test cases
    /// and other test containers.  Test containers are skipped if they do not
    /// contain any test cases or if none of their test cases have been selected for execution.
    /// </para>
    /// </summary>
    public interface ITest : ITestComponent, IModelTreeNode<ITest>
    {
        /// <summary>
        /// Gets or sets whether this test represents an individual test case
        /// as opposed to a test container such as a fixture or suite.  The value of
        /// this property can be used by the test harness to avoid processing containers
        /// that have no test cases.  It can also be used by the reporting infrastructure
        /// to constrain output statistics to test cases only.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Not all test cases are leaf nodes in the test tree and vice-versa.       
        /// </para>
        /// <para>
        /// This value is defined as a property rather than as a metadata key because it
        /// significantly changes the semantics of test execution.
        /// </para>
        /// </remarks>
        bool IsTestCase { get; set; }

        /// <summary>
        /// Gets the template binding from which the test was produced.
        /// </summary>
        ITemplateBinding TemplateBinding { get; }

        /// <summary>
        /// Gets the list of the dependencies of this test.
        /// </summary>
        IList<ITest> Dependencies { get; }

        /// <summary>
        /// Gets a <see cref="ITestController" /> <see cref="Factory{T}" /> to run this tes
        /// and all of its children.  Returns null if this test is merely a container for
        /// other tests or if it otherwise does not require or provide its own controller.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The top-most test that returns a non-null <see cref="Factory{T}" /> is
        /// referred to as the master test.  It may contain other tests that also have
        /// non-null factories but there is no built-in mechanism provided to delegate control
        /// from one controller to another in the middle of its execution.
        /// Thus the nested controller will only be executed if the master controller chooses
        /// to implement a suitable delegation policy of this sort.
        /// </para>
        /// <para>
        /// During test execution, the test plan scans the list of tests to be executed
        /// in depth-first order to locate the master tests for each subtree.  The test plan
        /// instantiates and invokes a test controller for each master test.
        /// </para>
        /// <para>
        /// For example, the top-level test created by a <see cref="ITestFramework" />
        /// is usually a master test.  The <see cref="ITestController" /> created by the
        /// framework's master test will then take care of setting up the environment for the entire
        /// subtree beneath it and then actually running the tests.  Thus the interaction
        /// between the test harness and test controllers (as mediated by master tests) represents
        /// a division of labor among multiple possible test execution strategies.
        /// </para>
        /// </remarks>
        /// <returns>The test controller factory, or null if this test cannot produce
        /// a controller (and consequently is not a master test according to the definition above)</returns>
        Factory<ITestController> TestControllerFactory { get; }
    }
}
