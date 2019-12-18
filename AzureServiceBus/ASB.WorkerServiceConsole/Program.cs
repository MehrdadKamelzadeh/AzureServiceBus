using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASB.WorkerServiceConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            RegisterHandler();
            Console.ReadLine();
        }



        static QueueClient queueClient;
        static string sbConnectionString =
            "Endpoint=sb://mehrdadplayground.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Kw7zOb5baMy64uFFUaMUg0sY4kmSWDaTnuK799hiZHw=";
        static string sbQueueName = "eventqueue";

        public static async void RegisterHandler()
        {
            try
            {
                queueClient = new QueueClient(sbConnectionString, sbQueueName);

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };

                queueClient.RegisterMessageHandler(ReceiveMessagesAsync, messageHandlerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}  {DateTime.Now}");
            }

        }

        private static async Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Body)}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
