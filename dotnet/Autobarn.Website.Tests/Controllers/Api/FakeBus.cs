using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Internals;

namespace Autobarn.Website.Tests.Controllers.Api {
    public class FakePubSub : IPubSub {
        public ArrayList Messages { get; set; } = new ArrayList();

        public Task PublishAsync<T>(T message, Action<IPublishConfiguration> configure, CancellationToken cancellationToken = new CancellationToken()) {
            Messages.Add(message);
            return Task.CompletedTask;
        }

        public AwaitableDisposable<SubscriptionResult> SubscribeAsync<T>(string subscriptionId, Func<T, CancellationToken, Task> onMessage, Action<ISubscriptionConfiguration> configure,
            CancellationToken cancellationToken = new CancellationToken()) {
            throw new NotImplementedException();
        }
    }
    public class FakeBus : IBus {
        private readonly FakePubSub fakePubSub;

        public void Dispose() { }

        public FakeBus() {
            this.fakePubSub = new FakePubSub();
        }

        public IPubSub PubSub => fakePubSub;
        public IRpc Rpc { get; }
        public ISendReceive SendReceive { get; }
        public IScheduler Scheduler { get; }
        public IAdvancedBus Advanced { get; }
    }
}
