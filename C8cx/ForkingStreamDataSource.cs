using System;

namespace C8cx
{
    public class ForkingStreamDataSource<T, TSource> : IStreamedDataSource<T>
    {
        public event Action<T> DataReceived;
        public ForkingStreamDataSource(IStreamedDataSource<TSource> source)
        {
            source.DataReceived += data => OnSourceData(data);
        }
        public ForkingStreamDataSource(IStreamedDataSource<TSource> source,
            Predicate<TSource> sourceFilter,
            Func<TSource, T> map)
        {
            Map = map;
            SourceFilter = sourceFilter;
            source.DataReceived += data => OnSourceData(data);
        }
        private void OnSourceData(TSource srcData)
        {
            if (DataReceived == null || Map == null) return;
            if (SourceFilter != null && !SourceFilter(srcData)) return;

            T resultData = Map(srcData);
            if (_whereFilter != null && !_whereFilter(resultData)) return;

            foreach (Action<T> del in DataReceived.GetInvocationList())
            {
                var d2 = del;
                del.BeginInvoke(resultData, cb => d2.EndInvoke(cb), null);
            }
        }
        public IStreamedDataSource<T> Where(Predicate<T> p)
        {
            _whereFilter = p;
            return this;
        }

        protected Predicate<TSource> SourceFilter { private get; set; }
        protected Func<TSource, T> Map { private get; set; }

        private Predicate<T> _whereFilter = null;
    }
}
