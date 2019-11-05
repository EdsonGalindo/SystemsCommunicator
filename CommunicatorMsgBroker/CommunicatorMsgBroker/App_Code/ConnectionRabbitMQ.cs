﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using System.Configuration;

namespace GetVisitantsBehaviorWebApp.Models
{
	/// <summary>
    /// This is a class for managing RabbitMQ connections and to executing commands
    /// </summary>
    public class ConnectionRabbitMQ
    {
        private IConnection connection;
        public IModel channel;
        public string username = ConfigurationManager.AppSettings["messageQueueUser"];
        public string password = ConfigurationManager.AppSettings["messageQueuePwd"];
        public string virtualHost = ConfigurationManager.AppSettings["messageQueueVirtualHost"];
        public string hostName = ConfigurationManager.AppSettings["messageQueueHostName"];
        public int port = int.Parse(ConfigurationManager.AppSettings["messageQueueHostPort"]);
        public string exchangeName = "";

        internal void PublishMessage(MessagePublish messagePublish)
        {
            throw new NotImplementedException();
        }

        public string routingKey = "";

		/// <summary>
        /// Creates a new connection to a RabbitMQ server
        /// </summary>
        public void GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = username;
            factory.Password = password;
            factory.VirtualHost = virtualHost;
            factory.HostName = hostName;
            factory.Port = port;

            connection = factory.CreateConnection();
        }

		/// <summary>
        /// Opens a channel using a connection of a RabbitMQ server
        /// </summary>
		public void OpenChannel()
        {
            channel = connection.CreateModel();
            channel.QueueDeclare(routingKey, true, false, false, null);
        }

		/// <summary>
        /// Closes a connection of a RabbitMQ server
        /// </summary>
		public void CloseChannel()
        {
            channel.Close();
            connection.Close();
        }

		/// <summary>
        /// Sends a message to a RabbitMQ server
        /// </summary>
        /// <param name="message">A message to be sent to the RabbitMQ server</param>
        /// <returns></returns>
		public bool PublishMessage(string message)
        {
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName, routingKey, null, messageBytes);
            }
            catch { return false; }

            return true;
        }

        /// <summary>
        /// Recover a individual message from message queue
        /// </summary>
        /// <returns></returns>
        public BasicGetResult GetIndividualMessage()
        {
            bool noAck = false;

            return channel.BasicGet(routingKey, noAck);
        }

    }
}
