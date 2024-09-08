using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Rewards.API.Message;
using Rewards.API.Services;
using System.Text;

namespace Rewards.API.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderCreateTopic;
        private readonly string orderCreateSubscription;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _rewardProcessor;
        private readonly RewardService _rewardService;

        public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardServic)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            orderCreateTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreateSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");
            var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(orderCreateTopic, orderCreateSubscription);
            _rewardService = rewardServic;
        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardsRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            //Signals processor to begin prccessing the messages 
            await _rewardProcessor.StartProcessingAsync();

        }

        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();
        }

        private async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs args)
        {
            //Receive Message from the Service Bus
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //Deserilize the CartdtoJson to string
            var objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                 await _rewardService.UpdateRewards(objMessage);
                 await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"{args.Exception.ToString()}");
            return Task.CompletedTask;
        }

    }
}
