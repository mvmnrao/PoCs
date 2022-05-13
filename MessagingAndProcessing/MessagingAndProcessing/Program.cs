using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MessageQueueTplDataFlow;
using MsgProducer;
using System;
using MsgConsumer;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace MessagingAndProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                services.AddTransient<IProducerCode<string>, ProducerCode<string>>()
                .AddTransient<IConsumerCode<string>, ConsumerCode<string>>()
                .AddTransient<ICustomQueue<string>, CustomQueue<string>>())
                .Build();

            DefineScopeAndProceMessages(host.Services);

            host.RunAsync().Wait();

            Console.WriteLine("--- Done ---");
        }

        static void DefineScopeAndProceMessages(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            IProducerCode<string> producer = provider.GetRequiredService<IProducerCode<string>>();

            IEnumerable<string> messages = Enumerable.Range(0, 100).Select(num => $"Number - {num}");
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 10;

            Parallel.ForEach<string>(
                messages,
                parallelOptions,
                (msg) =>
                {
                    msg = $"{msg} - {Thread.GetCurrentProcessorId()} - {Thread.CurrentThread.ManagedThreadId}";
                    producer.PostMessage(msg);

                    if(msg.Contains("55"))
                    {
                        Thread.Sleep(5000);
                    }
                }
            );

            Thread.Sleep(1000);

            producer.PostMessage("Hello Dhyuthi");
        }
    }
}