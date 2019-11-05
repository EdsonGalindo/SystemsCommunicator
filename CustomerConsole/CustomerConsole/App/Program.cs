using CustomerConsole.Business;
using System;
using System.Configuration;
using System.Threading;

namespace CustomerConsole
{
    class Program
    {
        static string messageBrokerQueueName;

        static void Main(string[] args)
        { 
            #region Properties
            messageBrokerQueueName = ConfigurationManager.AppSettings["messageBrokerQueueName"];
            #endregion
            
            Console.WriteLine("############# Customer Order Service #############");

            ManageOrders();
        }

        static void ManageOrders()
        {
            while(true)
            {
                Console.WriteLine("\n\n=> Informe o código do produto a ser solicitado:");
                var productCode = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(productCode))
                {
                    Console.WriteLine("=> Nenhum código de produto informado. Tente novamente.");
                    Thread.Sleep(2000);
                    continue;
                }

                Console.WriteLine("=> Enviando pedido...");
                Thread.Sleep(2000);
                if (!SendOrder(productCode))
                {
                    Console.WriteLine("=> Não foi possível enviar o pedido, tente novamente.");
                    Thread.Sleep(2000);
                    continue;
                }

                Console.WriteLine("=> Pedido enviado com sucesso!");
                Thread.Sleep(2000);
                
            }
        }

        static bool SendOrder(string productCode)
        {
            var orderResponse = false;

            try
            {
                orderResponse = MessageBroker.SendMessage(messageBrokerQueueName, productCode);
            }
            catch { }

            return orderResponse;
        }
    }
}
