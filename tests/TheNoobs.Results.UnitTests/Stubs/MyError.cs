using System;
using System.Collections.Generic;
using System.Text;
using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.UnitTests.Stubs
{
    public class MyError : Fail
    {
        public Exception Ex { get; }

        public MyError(string message, Exception ex) : base(message)
        {
            Ex = ex;
        }
    }
}
