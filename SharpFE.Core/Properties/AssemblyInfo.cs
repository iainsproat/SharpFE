//-----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SharpFE")]
[assembly: AssemblyDescription("SharpFE is a small, adaptable and embeddable library for the quick implicit finite element analysis of static structures.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Iain Sproat")]
[assembly: AssemblyProduct("SharpFE")]
[assembly: AssemblyCopyright("Copyright 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// This sets the default COM visibility of types in the assembly to invisible.
// If you need to expose a type to COM, use [ComVisible(true)] on that type.
[assembly: ComVisible(false)]

// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all the values or you can use the default the Revision and 
// Build Numbers by using the '*' as shown below:
[assembly: AssemblyVersion("0.0.3")]

// The assembly should be CLS compliant for use across the .Net framework
[assembly: CLSCompliant(true)]

// The internals of this assembly should be exposed to the unit testing framework
[assembly: InternalsVisibleTo("SharpFE.Core.Tests")]