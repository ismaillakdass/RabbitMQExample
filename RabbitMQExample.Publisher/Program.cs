//Publisher
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQExample.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            // Rabbitmq services inde oluşturduğumuz url
            factory.Uri = new Uri("amqps://taktjuar:y1W2ZxWkT0GZ_iXsjTQaVw9HndVcRyE5@shark.rmq.cloudamqp.com/taktjuar");

            using (var connection = factory.CreateConnection())
            {
                // Kanal oluşturuluyor
                var channel = connection.CreateModel();

                // Kuyruk oluşturuluyor
                // 1.Param= Kuyruk adı, 2. Param= kuyruk fiziksel bellekte oluşup uluşturmama, 3. Param = kuyruğa sadece burdan ulaşıp ulaşmamak yani aslında public olup olmaması, 4. Param = kuyruk da bağlı herhangi bir subscribe kalmadığında kuyruğun silinip silinmemesi 
                channel.QueueDeclare("hello-queue", true,false,false);

                string message = "hello world";

                var messagebody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(string.Empty, "hello-queue",null, messagebody);
                
                Console.WriteLine("Mesaj Gönderildi");
                
                Console.ReadKey();
            }
        }
    }
}
