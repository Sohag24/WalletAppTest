using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class RabbitMQHelper
{
    private readonly string _hostname;
    private readonly string _username;
    private readonly string _password;

    public RabbitMQHelper(string hostname, string username, string password)
    {
        _hostname = hostname;
        _username = username;
        _password = password;
    }

    public void PublishMessage(string message, string queueName)
    {
        var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }

    //public void ReceiveMessages(string queueName, Action<string> messageHandler)
    //{
    //    var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
    //    using (var connection = factory.CreateConnection())
    //    using (var channel = connection.CreateModel())
    //    {
    //        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    //        var consumer = new EventingBasicConsumer(channel);
    //        consumer.Received += (model, ea) =>
    //        {
    //            var body = ea.Body.ToArray();
    //            var message = Encoding.UTF8.GetString(body);
    //            messageHandler(message);
    //        };
    //        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    //        Console.WriteLine(" [*] Waiting for messages. Press CTRL+C to exit.");
    //        Console.ReadLine();
    //    }
    //}

    public async Task<string> ReceiveMessages(string queueName)
    {
        var factory =  new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            string message = "";
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                message += Encoding.UTF8.GetString(body)+",";
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            await Task.Delay(2000);
            return message;
        }
    }
}
