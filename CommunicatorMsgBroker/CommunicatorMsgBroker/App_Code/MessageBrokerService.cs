using GetVisitantsBehaviorWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

public class MessageBrokerService : IMessageBrokerService
{
	public MessagePublish PublishMessage(MessagePublish msgPublish)
	{
        try
        {
            #region Validates message publish object
            if (msgPublish == null || string.IsNullOrWhiteSpace(msgPublish.QueueName.Trim()) ||
                string.IsNullOrWhiteSpace(msgPublish.Message.Trim()))
            {
                throw new ArgumentNullException("Argumento inválido!");
            }
            #endregion

            #region Opens the channel and connection to RabbitMQ server
            ConnectionRabbitMQ connectionRabbitMQ = new ConnectionRabbitMQ();
            connectionRabbitMQ.GetConnection();
            connectionRabbitMQ.routingKey = msgPublish.QueueName;
            connectionRabbitMQ.OpenChannel();
            #endregion

            #region Publishes the message on RabbitMQ
            msgPublish.PublishSuceed = connectionRabbitMQ.PublishMessage(msgPublish.Message);
            #endregion

            #region Closes the connection to RabbitMQ
            try
            {
                connectionRabbitMQ.CloseChannel();
            }
            catch { }
            #endregion
        }
        catch (Exception e)
        {
            msgPublish.PublishSuceed = false;
            msgPublish.ErrorMessage = e.Message;
        }

        return msgPublish;
	}

	public MessageRecover RecoverMessage(MessageRecover msgRecover)
	{
        try
        {
            #region Validates message publish object
            if (msgRecover == null || string.IsNullOrWhiteSpace(msgRecover.QueueName.Trim()))
            {
                throw new ArgumentNullException("Argumento inválido!");
            }
            #endregion

            #region Opens the channel and connection to RabbitMQ server
            ConnectionRabbitMQ connectionRabbitMQ = new ConnectionRabbitMQ();
            connectionRabbitMQ.GetConnection();
            connectionRabbitMQ.routingKey = msgRecover.QueueName;
            connectionRabbitMQ.OpenChannel();
            #endregion
            
            #region Reads messages from RabbitMQ queue
            var messageGetResult = connectionRabbitMQ.GetIndividualMessage();

            #region Verifies a message was returned
            if (messageGetResult == null)
            {
                msgRecover.RecoverSuceed = false;
                msgRecover.ErrorMessage = "No messages avaiable on the queue at the moment.";
                return msgRecover;
            }

            msgRecover.RecoverSuceed = true;
            #endregion

            #region Defines the message to be returned in the service response
            msgRecover.Message = System.Text.Encoding.UTF8.GetString(messageGetResult.Body);
            #endregion

            #endregion

            #region Closes the connection to RabbitMQ
            connectionRabbitMQ.channel.BasicAck(messageGetResult.DeliveryTag, false);
            connectionRabbitMQ.CloseChannel();
            #endregion

        }
        catch (Exception e)
        {
            msgRecover.RecoverSuceed = false;
            msgRecover.ErrorMessage = e.Message;
        }

        return msgRecover;
	}
}
