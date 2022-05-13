using MessageQueueTplDataFlow;
using System;
using System.Threading.Tasks;

namespace MsgProducer
{
    public interface IProducerCode<T>
    {
        Task<bool> PostMessage(T msg);
    }

    public class ProducerCode<T> : IProducerCode<T>
    {
        private ICustomQueue<T> myQueue;
        public ProducerCode(ICustomQueue<T> customeQueue)
        {
            myQueue = customeQueue;
        }

        public async Task<bool> PostMessage(T msg)
        {
            return await myQueue.Enqueue(msg);
        }
    }
}
