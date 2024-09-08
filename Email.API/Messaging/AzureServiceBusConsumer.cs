using Azure.Messaging.ServiceBus;
using Email.API.Features.DTOs.CartDTOs;
using Email.API.Message;
using Email.API.Services;
using Newtonsoft.Json;
using System.Text;

namespace Email.API.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string userLogQueue;
        private readonly string orderCreateTopic;
        private readonly string orderCreateSubscription;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _userLogProcessor;
        private ServiceBusProcessor _emailOrderPlacedProcessor;

        private readonly EmailService _emailService;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            userLogQueue = _configuration.GetValue<string>("TopicAndQueueNames:UserLoggingQueue");
            orderCreateTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreateSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");
            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _userLogProcessor = client.CreateProcessor(userLogQueue);
            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreateTopic, orderCreateSubscription);
            _emailService = emailService;
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            //Signals processor to begin prccessing the messages 
            await _emailCartProcessor.StartProcessingAsync();

            _userLogProcessor.ProcessMessageAsync += OnUserLogRequestReceived;
            _userLogProcessor.ProcessErrorAsync += ErrorHandler;
            //Signals processor to begin prccessing the messages 
            await _userLogProcessor.StartProcessingAsync();


            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            //Signals processor to begin prccessing the messages 
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _userLogProcessor.StopProcessingAsync();
            await _userLogProcessor.DisposeAsync();


            await _emailOrderPlacedProcessor.StopProcessingAsync();
            await _emailOrderPlacedProcessor.DisposeAsync();
        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            //Receive Message from the Service Bus
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //Deserilize the CartdtoJson to string
            var rewardsMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                await _emailService.LogOrderPlaced(rewardsMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            //Receive Message from the Service Bus
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //Deserilize the CartdtoJson to string
            var cartjMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                 await _emailService.EmailLoggingCart(cartjMessage);
                 await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private async Task OnUserLogRequestReceived(ProcessMessageEventArgs args)
        {
            //Receive Message from the Service Bus
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            //Deserilize the userEmail to string
            string userJsonMessageEmail = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.CreateUserAccountLog(userJsonMessageEmail);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
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
