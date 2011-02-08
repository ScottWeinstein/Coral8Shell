using System;
using C8cx;
public partial class outTest1C8Stream : IStreamedDataSource<Out_2C8Tuple>, IDisposable
{
    public event Action<Out_2C8Tuple> DataReceived;

	public outTest1C8Stream(string streamUrl)
	{
	    DataReceived += d => { };
		_c8stream = new C8cx.C8OutputStream(streamUrl); 
		_c8stream.DataReceived += (tpl) => DataReceived(new Out_2C8Tuple(tpl)); 
	}
	
	public void Connect()
    {
		_c8stream.Connect();
	}

    public void Dispose()
    {
        _c8stream.Dispose();		
    }

    private C8OutputStream _c8stream;
    public IStreamedDataSource<Out_2C8Tuple> Where(Predicate<Out_2C8Tuple> p)
    {
        throw new NotSupportedException("");
    }

}
