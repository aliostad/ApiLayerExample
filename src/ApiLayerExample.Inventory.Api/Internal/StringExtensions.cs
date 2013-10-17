using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoreLinq;

namespace ApiLayerExample.Inventory.Api.Internal
{
    internal static class StringExtensions
    {
       
        private static char[] _separators = new []
                                                {
                                                    '/', '\\', '>', '<',
                                                    '\t', ' ', ';', ':',
                                                    '[', ']', '{', '}',
                                                    '(', '@', '=', '?', ')'
                                                };

        public static string ReplaceHttpSeparators(this string s)
        {
            string result = s;
            _separators.ForEach( (ch) => result = result.Replace(ch, '_'));
            return result;
        }
    }
}

/*

 "(" | ")" | "<" | ">" | "@"
                      | "," | ";" | ":" | "\" | <">
                      | "/" | "[" | "]" | "?" | "="
                      | "{" | "}" 
*/