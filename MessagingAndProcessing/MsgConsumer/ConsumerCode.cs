using System;
using System.Threading;

namespace MsgConsumer
{
    public interface IConsumerCode<T>
    {
        void ProcessMessage(T msg);
    }

    public class ConsumerCode<T> : IConsumerCode<T>
    {
        public void ProcessMessage(T msg)
        {
            Thread.Sleep(10);
            Console.WriteLine($"Procced by consumer. Message - {msg}");
        }
    }
}
