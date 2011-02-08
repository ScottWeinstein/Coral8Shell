using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace C8cx
{
    public static class EntentionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }
    }

    namespace DTO
    {
        public partial class Status
        {
            public static Status Deserialize(string xml)
            {
                using (var xrdr = XmlReader.Create(new StringReader(xml)))
                {
                    var xser = new XmlSerializer(typeof(Status));
                    return (Status)xser.Deserialize(xrdr);
                }
            }
        }

        public partial class TupleDescriptor
        {
            public static TupleDescriptor Deserialize(string xml)
            {
                using (var xrdr = XmlReader.Create(new StringReader(xml)))
                {
                    var xser = new XmlSerializer(typeof(TupleDescriptor));
                    return (TupleDescriptor)xser.Deserialize(xrdr);
                }
            }
        }

    }

}
