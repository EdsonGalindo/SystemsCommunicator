using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


[ServiceContract]
public interface IMessageBrokerService
{
    [OperationContract]
    MessagePublish PublishMessage(MessagePublish msgPublish);

    [OperationContract]
    MessageRecover RecoverMessage(MessageRecover msgRecover);
}

[DataContract]
public class MessagePublish
{
    string queueName = string.Empty;
    string message = string.Empty;
    bool publishSuceed = true;
    string errorMessage = string.Empty;

    [DataMember]
    public string QueueName
    {
        get { return queueName; }
        set { queueName = value; }
    }

    [DataMember]
    public string Message
    {
        get { return message; }
        set { message = value; }
    }

    [DataMember]
    public bool PublishSuceed
    {
        get { return publishSuceed; }
        set { publishSuceed = value; }
    }

    [DataMember]
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { errorMessage = value; }
    }
}

[DataContract]
public class MessageRecover
{
    string queueName = string.Empty;
    string message = string.Empty;
    bool recoverSuceed = true;
    string errorMessage = string.Empty;

    [DataMember]
    public string QueueName
    {
        get { return queueName; }
        set { queueName = value; }
    }

    [DataMember]
    public string Message
    {
        get { return message; }
        set { message = value; }
    }

    [DataMember]
    public bool RecoverSuceed
    {
        get { return recoverSuceed; }
        set { recoverSuceed = value; }
    }

    [DataMember]
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { errorMessage = value; }
    }
}
