using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyProduct("MVC.Utilities.Tests (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
    [assembly: AssemblyProduct("MVC.Utilities.Tests (Release)")]
    [assembly: AssemblyConfiguration("Release")]
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MVC.Utilities.Tests")]
[assembly: AssemblyDescription("Tests for MVC.Utilities and MVC.Utilties.BCrypt")]
[assembly: AssemblyTrademark("")]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("23bad5c1-01c2-48aa-87e2-7698c49a1477")]