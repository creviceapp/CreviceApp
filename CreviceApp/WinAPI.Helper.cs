﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Crevice.WinAPI.Helper
{
    using Crevice.Logging;

    public static class ExceptionThrower
    {
        public static void ThrowLastWin32Error()
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
    
    public class WinAPILogger
    {
        private readonly StringBuilder buffer = new StringBuilder();
        public WinAPILogger(string name)
        {
            Add("Calling a native method: {0}", name);
        }

        public void Add(string str)
        {
            buffer.AppendFormat(str);
            buffer.AppendLine();
        }
        
        public void Add(string str, params object[] objects)
        {
            buffer.AppendFormat(str, objects);
            buffer.AppendLine();
        }

        public void Success()
        {
            Add("Success");
            Verbose.Print(buffer.ToString());
        }

        public void Fail()
        {
            Add("Failed");
            Verbose.Print(buffer.ToString());
        }

        public void FailWithErrorCode()
        {
            Add("Failed; ErrorCode: {0}", Marshal.GetLastWin32Error());
            Verbose.Print(buffer.ToString());
        }
    }
}