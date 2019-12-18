using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ASB.WorkerServiceApp
{
    public class SpectrumWorkerService
    {
        static QueueClient queueClient;
        static string sbConnectionString =
            "Endpoint=sb://mehrdadplayground.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Kw7zOb5baMy64uFFUaMUg0sY4kmSWDaTnuK799hiZHw=";
        static string sbQueueName = "eventqueue";

        public void Stop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }

        public void Start()
        {
            WriteToFile("Service is started at " + DateTime.Now);
            RegisterHandler();
        }

        public async void RegisterHandler()
        {
            try
            {
                WriteToFile("Handlers started " + DateTime.Now);

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
                WriteToFile($"{ex.Message}  {DateTime.Now}");
            }
            finally
            {
                await queueClient.CloseAsync();
            }
        }

        private async Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            WriteToFile($"Received message: {Encoding.UTF8.GetString(message.Body)}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            WriteToFile(exceptionReceivedEventArgs.Exception.Message);
            return Task.CompletedTask;
        }
 

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!System.IO.Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
