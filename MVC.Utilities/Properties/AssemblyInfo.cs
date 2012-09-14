using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyProduct("MVC.Utilities (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
    [assembly: AssemblyProduct("MVC.Utilities (Release)")]
    [assembly: AssemblyConfiguration("Release")]
#endif


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MVC.Utilities")]
[assembly: AssemblyDescription("Utility classes designed for ASP.NET MVC; deals with encryption, routing, caching, authorization, and various other security issues. Designed by used with Dependency Injection.")]
[assembly: AssemblyTrademark("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8574f8cd-55cf-4d63-8eb4-f98396058297")]
