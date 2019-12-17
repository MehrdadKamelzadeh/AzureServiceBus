using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ASB.Services
{
    public class SaveSomethingService : ISaveSomethingService
    {
        static QueueClient queueClient;
        static string sbConnectionString =
            "Endpoint=sb://mehrdadplayground.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Kw7zOb5baMy64uFFUaMUg0sY4kmSWDaTnuK799hiZHw=";
        static string sbQueueName = "eventqueue";

        public void Save()
        {
            try
            {
                queueClient = new QueueClient(sbConnectionString, sbQueueName);
                var message = new Message(Encoding.UTF8.GetBytes("HIIII"));
                queueClient.SendAsync(message);
            }
            catch (Exception e)
            {

            }
            finally
            {
                queueClient.CloseAsync();
            }
        }


        public void RegisterHandler()
        {
            var sbConnectionString =
                "Endpoint=sb://mehrdadplayground.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Kw7zOb5baMy64uFFUaMUg0sY4kmSWDaTnuK799hiZHw=";
            string sbQueueName = "eventqueue";

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
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
                queueClient.CloseAsync();
            }
        }

        static async Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Body)}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }
    }
}
