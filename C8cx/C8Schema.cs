using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using C8cx.DTO;

namespace C8cx
{
    public class C8xSchema
    {
        public string[] ColumnNames { get; set; }
        public Type[] ColumnTypes { get; set; }
        public Dictionary<string, int> ColumnPositionMap { get; set; }
        public int Count
        {
            get
            {
                return ColumnNames.Length;
            }
        }
    }
}
