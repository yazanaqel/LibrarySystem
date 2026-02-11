using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LibrarySystem.Application;

public static class AssemblyProvider
{
    public static Assembly GetAssembly() => Assembly.GetExecutingAssembly();
}
