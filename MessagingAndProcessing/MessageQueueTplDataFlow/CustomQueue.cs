using MsgConsumer;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MessageQueueTplDataFlow
{
    public interface ICustomQueue<T>
    {
        Task<bool> Enqueue(T msg);
    }

    public class CustomQueue<T> : ICustomQueue<T>
    {
        private ActionBlock<T> actionBlock;

        public CustomQueue(IConsumerCode<T> consumerCode)
        {
            actionBlock = new ActionBlock<T>(consumerCode.ProcessMessage);
        }

        public async Task<bool> Enqueue(T msg)
        {
            return await actionBlock.SendAsync(msg);
        }
    }
}
