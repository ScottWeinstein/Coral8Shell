using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;

namespace C8cx
{
    public class C8OutputStream : IStreamedDataSource<C8Tuple>, IDisposable
    {
        public event Action<C8Tuple> DataReceived;

        private WebResponse _resp;
        Action readworker;
        public C8OutputStream(string subscribeUrl)
        {
  //          _cancelARE = new AutoResetEvent(false);
            DataReceived += d => { };
            
            readworker = () =>
            {
//                var waiters = new WaitHandle[] { _cancelARE, null };

                string rurl = C8Core.ResolveUrl(subscribeUrl);
                var core = new C8Core(subscribeUrl);
                C8xSchema td = core.GetTD(subscribeUrl);

                WebRequest _req = WebRequest.Create(rurl);
                _req.Headers.Add("X-C8-StreamFormat", "CSV");
                _req.Headers.Add("X-C8-StreamFormatOptions", "TitleRow=false");
                _req.Method = "GET";
                _resp = _req.GetResponse();
                AsyncStreamReader asyncStreamReader = new AsyncStreamReader(_resp.GetResponseStream(), Encoding.UTF8);
                asyncStreamReader.LineRead += (_, buff) =>
                {
                    if (!string.IsNullOrEmpty(buff))
                    {
                        C8Tuple tpl = ConvertCSVtoC8Tuple(buff, td);
                        DataReceived(tpl);
                    }
                };
                asyncStreamReader.ReadLineAsync();
            };
        }


        public void Connect()
        {
            readworker.BeginInvoke(cb=>readworker.EndInvoke(cb),null);
        }


        // TODO - look into using   http://www.codeproject.com/KB/database/CsvReader.aspx or some other real CSV parser
        static C8Tuple ConvertCSVtoC8Tuple(string buff, C8xSchema td)
        {
            var fields = buff.Split(',');
            if (fields.Length != td.Count)
            {
                throw new ArgumentException("Column count error");
            }
            object[] items = new object[td.Count];

            for (int ii = 0; ii < td.Count; ii++)
            {
                Type colType = td.ColumnTypes[ii];
                string item = fields[ii];
                object val = item;
                if (colType == typeof(DateTime))
                {
                    long ticks = long.Parse(item) * 10;
                    val = C8Core.Epoch.AddTicks(ticks).ToLocalTime();
                }
                else if (colType == typeof(bool))
                {
                    val = bool.Parse(item);
                }
                else if (colType == typeof(int))
                {
                    val = int.Parse(item);
                }
                else if (colType == typeof(double))
                {
                    val = double.Parse(item);
                }
                else if (colType == typeof(long))
                {
                    val = long.Parse(item);
                }
                items[ii] = val;
            }
            return new C8Tuple(td, items);
        }
        
        public void Dispose()
        {
            _resp.Close();
        }

//        private AutoResetEvent _cancelARE;
        public IStreamedDataSource<C8Tuple> Where(Predicate<C8Tuple> p)
        {
            throw new NotSupportedException("");
        }

    }
}

