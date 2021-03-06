﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lempa.Tools.Database
{
    public static class SecurityStatic
    {
        public static Security Db = new Security();
    }

    public class Security
    {
        public string Fnuttify(string value)
        {
            if (value == null)
                return "NULL";
            else if (value.Trim().Equals(String.Empty))
                return "NULL";
            else
                return "'" + value.Replace("'", "''") + "'";
        }

        public string FnuttifyDontAddBeginingOrEnd(string value)
        {
            if (value == null)
                return "NULL";
            else if (value.Trim().Equals(String.Empty))
                return "NULL";
            else
                return value.Replace("'", "''");
        }

        public bool ContainsSqlInjections(string value)
        {
            if (value.Contains("'"))
                return true;
            return false;

        }
    }

}



