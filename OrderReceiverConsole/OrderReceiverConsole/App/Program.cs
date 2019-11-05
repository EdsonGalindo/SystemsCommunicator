using OrderReceiverConsole.Business;
using System;
using System.Configuration;
using System.Threading;

namespace OrderReceiverConsole
{
    class Program
    {
        static string messageBrokerOrdersQueueName;

        static void Main(string[] args)
        {
            #region Properties
            messageBrokerOrdersQueueName = ConfigurationManager.AppSettings["messageBrokerOrdersQueueName"];
            #endregion

            Console.WriteLine("############# Order Receiver Service #############");

            ManageCustomerOrders();
        }

        static void ManageCustomerOrders()
        {
            while (true)
            {
                Console.WriteLine("\n\n=> Buscando pedidos de clientes...");

                Thread.Sleep(3000);

                var orderRecoverResponse = RecoverOrder();

                if (string.IsNullOrEmpty(orderRecoverResponse))
                {
                    Console.WriteLine("=> Não há pedidos para processamento no momento.");
                    Thread.Sleep(5000);
                    continue;
                }

                Console.WriteLine("=> Pedido localizado para o produto: " + orderRecoverResponse);
                Console.WriteLine("Pressione uma tecla para continuar a busca de pedidos");
                Console.ReadLine();
            }
        }

        static string RecoverOrder()
        {
            var orderRecoverResponse = string.Empty;

            try
            {
                orderRecoverResponse = MessageBroker.RecoverMessage(messageBrokerOrdersQueueName);
            }
            catch { }

            return orderRecoverResponse;
        }
    }
}
