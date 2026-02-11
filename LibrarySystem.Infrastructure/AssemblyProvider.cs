using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LibrarySystem.Infrastructure;

public static class AssemblyProvider
{
    public static Assembly GetAssembly() => Assembly.GetExecutingAssembly();
}

