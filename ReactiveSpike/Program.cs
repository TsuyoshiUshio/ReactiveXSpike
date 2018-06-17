using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace ReactiveSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new NumberObservable();

            var subscriber1 = source.Subscribe(
                value => Console.WriteLine($"OnNext({value}) called."),
                ex => Console.WriteLine($"OnError({ex.Message}) called."),
                () => Console.WriteLine("OnCompleted() called."));

            var subscriber2 = source.Subscribe(
                value => Console.WriteLine($"OnNext({value}) called."),
                ex => Console.WriteLine($"OnError({ex.Message}) called."),
                () => Console.WriteLine("OnCompleted() called."));

            Console.WriteLine("## Execute(1) ");
            source.Execute(1);

            Console.WriteLine("## Dispose");
            subscriber2.Dispose();

            Console.WriteLine("## Execute(2)");
            source.Execute(2);
            Console.WriteLine("## Execute(0)");
            source.Execute(0);

            var subscriber3 = source.Subscribe(
                value => Console.WriteLine($"OnNext({value}) called."),
            ex => Console.WriteLine($"OnError({ex.Message}) called."),
                () => Console.WriteLine("OnCompleted() called."));
            Console.WriteLine("## Completed");
            source.Completed();

            Console.ReadLine();
        }
    }
    
    class NumberObservable : IObservable<int>
    {
        private Subject<int> source = new Subject<int>();

        public void Execute(int value)
        {
            if (value == 0)
            {
                this.source.OnError(new Exception("value is 0"));
                this.source = new Subject<int>();
                return;
            }

            this.source.OnNext(value);
        }

        public void Completed()
        {
            this.source.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            return this.source.Subscribe(observer);
        }

    
    }
}
