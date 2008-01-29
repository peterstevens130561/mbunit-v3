// Copyright 2008 MbUnit Project - http://www.mbunit.com/
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
using System.Collections.Generic;
using System.Windows.Forms;
using Gallio.Icarus.Core.CustomEventArgs;
using Gallio.Model;
using Gallio.Model.Filters;
using Gallio.Model.Serialization;
using Gallio.Runner.Reports;

namespace Gallio.Icarus.Interfaces
{
    public interface IProjectAdapterView
    {
        event EventHandler<AddAssembliesEventArgs> AddAssemblies;
        event EventHandler<EventArgs> RemoveAssemblies;
        event EventHandler<SingleStringEventArgs> RemoveAssembly;
        event EventHandler<GetTestTreeEventArgs> GetTestTree;
        event EventHandler<EventArgs> RunTests;
        event EventHandler<EventArgs> GenerateReport;
        event EventHandler<EventArgs> StopTests;
        event EventHandler<SetFilterEventArgs> SetFilter;
        event EventHandler<EventArgs> GetReportTypes;
        event EventHandler<SaveReportAsEventArgs> SaveReportAs;
        event EventHandler<SingleStringEventArgs> SaveProject;
        event EventHandler<OpenProjectEventArgs> OpenProject;
        event EventHandler<EventArgs> NewProject;
        TreeNode[] TestTreeCollection { set; }
        ListViewItem[] Assemblies { set; }
        string StatusText { set; }
        string ReportPath { set; }
        IList<string> ReportTypes { set; }
        Exception Exception { set; }
        int CompletedWorkUnits { set; }
        int TotalWorkUnits { set; }
        int TotalTests { set; }
        void Update(TestData testData, TestStepRun testStepRun);
        void ApplyFilter(Filter<ITest> filter);
        void ThreadedRemoveAssembly(string assembly);
        void CreateFilter(TreeNodeCollection nodes);
        void ReloadTree();
        void SaveReport(string fileName, string reportType);
        void WriteToLog(string logName, string logBody);
        void Reset();
        void CreateReport();
    }
}