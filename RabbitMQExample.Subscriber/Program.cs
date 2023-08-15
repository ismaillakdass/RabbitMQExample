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
                // Kanal oluşturuluyor. 
                var channel = connection.CreateModel();

                //1. param da 0 bana her boyuttan mesajı ver, 2. param da bana kaç kaç mesaj verilsin, 3. param true ise subcriber"consumer" kaç tane ise paylaştırır false de 2. parametrede belirtilen adet kadar gönderir yani 2 suncriber varsa 5 mesaj varsa 1. ye 3 ikinciye 2 tane olacak şekilde biz false diyoruz. 1 er 1 er gönderecek
                channel.BasicQos(0, 1, false);

                // Subscriber tarafında bu isimde kuyruk varsa bu satırın bir zararı yok eğer publisher tarafında böyle bir kuyruk yoksa oluşturur varsa olanı kullanır. Publisher tarafında ki parametrelerle aynı olması önemli.
                //channel.QueueDeclare("hello-queue", true, false, false);

                var subscriber = new EventingBasicConsumer(channel);

                //Kanal üzerindeki kuyruğa ulaşıyoruz 1. param bize true ile kuyruktan mesaj alındığında mesaj otomatik silinsini ifade eder, false da ise mesajı aldığımızda mesaj kalsın silmek için ben sana dönüş yapacağım anlamına gelir ve silme işlemi bizim kontrolümüz altında olur. , 2. paramda subscribe diğer adıyla consumer i veriyoruz.
                channel.BasicConsume("hello-queue",false,subscriber);

                //Kuyruğu çekiyoruz
                subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
                {
                    var message= Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine("Gelen Mesaj : " + message);

                    //Yukarıda ki BasicConsume ile 2. parametrede trudan false çekince mesajları biz kendimiz silmemek için aksiyon alıyoruz.
                    // BasicAck da e. parametresi ile mevcut mesajı aldırıyoruz, 2. parametrede ise işlenmiş ama rabitmq ya gitmemiş mesajları da rabbitmq yu haberdar eder ama biz false yapıyoruz

                    channel.BasicAck(e.DeliveryTag, false);
                };

                Console.ReadKey();
            }
        }
    }                
}
