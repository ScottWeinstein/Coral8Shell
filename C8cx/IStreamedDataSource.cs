using System;

namespace C8cx
{
    public interface IStreamedDataSource<T>
    {
        event Action<T> DataReceived;
        IStreamedDataSource<T> Where(Predicate<T> p);
    }
}
