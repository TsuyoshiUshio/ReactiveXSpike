using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace FactoryMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = Observable.Defer<int>(() =>
            {
                Console.WriteLine("# Defer method called.");
                var s = new ReplaySubject<int>();
                s.OnNext(1);
                s.OnNext(2);
                s.OnNext(3);
                s.OnCompleted();
                return s.AsObservable();

            });

            var subscription = source.Subscribe(
                i => Console.WriteLine($"OnNext({i})"),
                ex => Console.WriteLine($"OnError({ex.Message})"),
                () => Console.WriteLine("Completed()"));

            var subscription2 = source.Subscribe(
    i => Console.WriteLine($"OnNext({i})"),
    ex => Console.WriteLine($"OnError({ex.Message})"),
    () => Console.WriteLine("Completed()"));

            subscription.Dispose();
            subscription2.Dispose();

            Console.ReadLine();
        }
    }
}
