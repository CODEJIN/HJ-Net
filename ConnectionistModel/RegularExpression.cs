using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;


public static class RegularExpression
{
    public static bool PositiveIntCheck(string checkString)
    {
        if (checkString == "0") return false;
        return UIntCheck(checkString);
    }
    public static bool UIntCheck(string checkString)
    {
        if (checkString == "") return false;

        Regex regex = new Regex(@"^[0-9]*$");

        return regex.IsMatch(checkString);
    }    
    public static bool IntCheck(string checkString)
    {
        if (checkString == "") return false;

        Regex regex = new Regex(@"^[+-]?[0-9]*$");

        return regex.IsMatch(checkString);
    }
    public static bool DoubleCheck(string checkString)
    {
        if (checkString == "") return false;

        Regex regex = new Regex(@"^[+-]?[0-9]*(\.?[0-9]*)$");

        return regex.IsMatch(checkString);
    }
    public static bool ScientificNotationDoubleCheck(string checkString)
    {
        if (checkString == "") return false;

        Regex regex = new Regex(@"^[+-]?[0-9]+[\.]?[0-9]*[eE][+-]?[0-9]+");

        return regex.IsMatch(checkString);
    }
}
