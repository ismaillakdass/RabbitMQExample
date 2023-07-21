//Subscriber


using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

                // Subscriber tarafında bu isimde kuyruk varsa bu satırın bir zararı yok eğer publisher tarafında böyle bir kuyruk yoksa oluşturur varsa olanı kullanır. Publisher tarafında ki parametrelerle aynı olması önemli.
                //channel.QueueDeclare("hello-queue", true, false, false);

                var subscriber = new EventingBasicConsumer(channel);

                //Kanal üzerindeki kuyruğa ulaşıyoruz 1. param bize true ile kuyruktan mesaj alındığında mesaj otomatik silinsini ifade eder, false da ise mesajı aldığımızda mesaj kalsın silmek için ben sana dönüş yapacağım anlamına gelir ve silme işlemi bizim kontrolümüz altında olur. , 2. paramda subscribe diğer adıyla consumer i veriyoruz.
                channel.BasicConsume("hello-queue",true,subscriber);

                subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
                {
                    var message= Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine("Gelen Mesaj : " + message);
                };

                Console.ReadKey();
            }
        }

    }
}
