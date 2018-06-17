using System;
using System.Collections.Generic;

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
        private List<IObserver<int>> observers = new List<IObserver<int>>();

        public void Execute(int value)
        {
            if (value == 0)
            {
                foreach (var obs in observers)
                {
                    obs.OnError(new Exception("value is 0"));
                }
                this.observers.Clear();
                return;
            }
            foreach (var obs in observers)
            {
                obs.OnNext(value);
            }
        }

        public void Completed()
        {
            foreach (var obs in observers)
            {
                obs.OnCompleted();
            }
            this.observers.Clear();
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            this.observers.Add(observer);
            return new RemoveListDisposable(observers, observer);
        }

        private class RemoveListDisposable : IDisposable
        {
            private List<IObserver<int>> observers = new List<IObserver<int>>();
            private IObserver<int> observer;

            public RemoveListDisposable(List<IObserver<int>> observers, IObserver<int> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }
            public void Dispose()
            {
                if (this.observers == null)
                {
                    return;
                }

                if (observers.IndexOf(observer) != -1)
                {
                    this.observers.Remove(observer);
                }

                this.observers = null;
                this.observer = null;
            }
        }
    }
}
