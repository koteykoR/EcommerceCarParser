using System;

namespace EcommerceCarsParser.Extension
{
    public static class ObjectExtension
    {
        public static TResult With<TInput, TResult>(this TInput value, Func<TInput, TResult> func, TResult failResult)
            where TInput : class 
            => value is null ? failResult : func(value);
    }
}
