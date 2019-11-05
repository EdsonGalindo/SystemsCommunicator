using MessageBrokerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderReceiverConsole.Business
{
    public static class MessageBroker
    {
        private static MessageBrokerServiceClient messageBrokerServiceClient = new MessageBrokerServiceClient();

        public static bool SendMessage(string queue, string message)
        {
            try
            {
                MessagePublish messagePublish = new MessagePublish();
                messagePublish.QueueName = queue;
                messagePublish.Message = message;

                messagePublish = messageBrokerServiceClient.PublishMessageAsync(messagePublish).Result;

                if (!messagePublish.PublishSuceed)
                    throw new Exception(messagePublish.ErrorMessage);

                return true;
            }
            catch //(Exception ex)
            {
                //todo: Log error message in ex.message propertie
                return false;
            }
        }

        public static string RecoverMessage(string queue)
        {
            try
            {
                MessageRecover messageRecover = new MessageRecover();
                messageRecover.QueueName = queue;

                messageRecover = messageBrokerServiceClient.RecoverMessageAsync(messageRecover).Result;

                if (!messageRecover.RecoverSuceed)
                    throw new Exception(messageRecover.ErrorMessage);

                return messageRecover.Message;
            }
            catch //(Exception ex)
            {
                //todo: Log error message in ex.message propertie
                return string.Empty;
            }
        }
    }
}
