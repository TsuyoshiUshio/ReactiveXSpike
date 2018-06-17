using System;
using System.Reactive.Linq;

namespace FactoryMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = Observable.Range(1, 10);
            var subscription = source.Subscribe(
                i => Console.WriteLine($"OnNext({i})"),
                ex => Console.WriteLine($"OnError({ex.Message})"),
                () => Console.WriteLine("Completed()"));
            subscription.Dispose();

            Console.ReadLine();
        }
    }
}
