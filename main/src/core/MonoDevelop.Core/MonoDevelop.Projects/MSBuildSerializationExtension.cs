﻿//
// MSBuildSerializationExtension.cs
//
// Author:
//       Lluis Sanchez Gual <lluis@xamarin.com>
//
// Copyright (c) 2015 Xamarin, Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using MonoDevelop.Projects.Formats.MSBuild;
using MonoDevelop.Core;
using System.Threading.Tasks;

namespace MonoDevelop.Projects
{
	class MSBuildSerializationExtension: ProjectServiceExtension
	{
		public override bool FileIsObjectOfType (string file, Type type)
		{
			foreach (var f in MSBuildFileFormat.GetSupportedFormats ()) {
				if (f.CanReadFile (file, type))
					return true;
			}
			return base.FileIsObjectOfType (file, type);
		}

		public override async Task<SolutionItem> LoadSolutionItem (ProgressMonitor monitor, SolutionLoadContext ctx, string fileName, MSBuildFileFormat expectedFormat, string typeGuid, string itemGuid)
		{
			foreach (var f in MSBuildFileFormat.GetSupportedFormats ()) {
				if (f.CanReadFile (fileName, typeof(SolutionItem)))
					return await MSBuildProjectService.LoadItem (monitor, fileName, expectedFormat, typeGuid, itemGuid, ctx);
			}
			return await base.LoadSolutionItem (monitor, ctx, fileName, expectedFormat, typeGuid, itemGuid);
		}

		public override async Task<WorkspaceItem> LoadWorkspaceItem (ProgressMonitor monitor, string fileName)
		{
			foreach (var f in MSBuildFileFormat.GetSupportedFormats ()) {
				if (f.CanReadFile (fileName, typeof(WorkspaceItem)))
					return (WorkspaceItem) await f.ReadFile (fileName, typeof(WorkspaceItem), monitor);
			}
			return await base.LoadWorkspaceItem (monitor, fileName);
		}
	}
}
