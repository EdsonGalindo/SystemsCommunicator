using MessageBrokerService;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerConsole.Business
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
    }
}
