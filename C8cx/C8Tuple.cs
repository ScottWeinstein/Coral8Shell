using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace C8cx
{
    public class C8Tuple : IEqualityComparer<C8Tuple>
    {
        internal string[] columnNames;
        protected Dictionary<string, int> columnPositionMapStrMap;

        public C8Tuple(string[] colNames, object[] itemArray)
        {
            this.columnNames = colNames;
            int ii = 0;
            this.columnPositionMapStrMap = colNames.ToDictionary(s => s, s => ii++);
            ItemArray = itemArray;
        }
 
        public C8Tuple(string[] colsIntMap, Dictionary<string, int> colsStrMap, object[] itemArray, DateTime timestamp)
        {
            this.columnNames = colsIntMap;
            this.columnPositionMapStrMap = colsStrMap;
            ItemArray = itemArray;
            MessageTimestamp = timestamp;
        }

        public C8Tuple(C8xSchema td)
        {
            columnPositionMapStrMap = td.ColumnPositionMap;
            columnNames = td.ColumnNames;
            ItemArray = new object[columnNames.Length];
        }
        public C8Tuple(C8xSchema schema, DateTime msgTimeStamp):this(schema)
        {
            this.MessageTimestamp = msgTimeStamp;
        }
        public C8Tuple(C8xSchema tupleDesc, object[] itemArray)
            : this(tupleDesc)
        {
            ItemArray = itemArray;
        }
        protected C8Tuple(C8Tuple tpl)
        {
            this.columnNames = tpl.columnNames;
            this.columnPositionMapStrMap = tpl.columnPositionMapStrMap;
            this.ItemArray = tpl.ItemArray;
        }
        
        public object this[string columnName]
        {
            get
            {
                return this[columnPositionMapStrMap[columnName]];
            }
            set
            {
                this[columnPositionMapStrMap[columnName]] = value;
            }
        }

        public object this[int columnIndex]
        {
            get
            {
                return ItemArray[columnIndex];
            }
            set
            {
                ItemArray[columnIndex] = value;
            }
        }

        public T Field<T>(string columnName)
        {
            return (T)this[columnName];
        }
        public T Field<T>(int columnIndex)
        {
            return (T)this[columnIndex];
        }

        public object[] ItemArray { get; set; }

        public DateTime MessageTimestamp { get; set; }

        public bool Equals(C8Tuple a, C8Tuple b)
        {
            return CompareArray(a.ItemArray, b.ItemArray);
        }

        public int GetHashCode(C8Tuple obj)
        {
            return ItemArray.GetHashCode();
        }
        private static bool CompareArray(Array a, Array b)
        {
            if (((b == null) || (1 != a.Rank)) || ((1 != b.Rank) || (a.Length != b.Length)))
            {
                return false;
            }
            int lowerBound = a.GetLowerBound(0);
            int index = b.GetLowerBound(0);

            int num3 = lowerBound + a.Length;
            while (lowerBound < num3)
            {
                if (!AreElementEqual(a.GetValue(lowerBound), b.GetValue(index)))
                {
                    return false;
                }
                lowerBound++;
                index++;
            }
            return true;
        }
        private static bool AreElementEqual(object a, object b)
        {
            return (object.ReferenceEquals(a, b) || (((!object.ReferenceEquals(a, null) && !object.ReferenceEquals(a, DBNull.Value)) && (!object.ReferenceEquals(b, null) && !object.ReferenceEquals(b, DBNull.Value))) && a.Equals(b)));
        }

    }
}
