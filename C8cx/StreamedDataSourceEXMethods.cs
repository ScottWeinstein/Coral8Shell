using System;

namespace C8cx
{
    public static class StreamedDataSourceEXMethods
    {
        public static IStreamedDataSource<TResult> Fork<TSource, TResult>(
            this IStreamedDataSource<TSource> source,
            Predicate<TSource> filter,
            Func<TSource, TResult> map)
        {
            return new ForkingStreamDataSource<TResult, TSource>(source, filter, map);
        }
    }
}
