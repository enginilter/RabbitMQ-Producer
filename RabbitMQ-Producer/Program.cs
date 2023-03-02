
using RabbitMQ.Client;
using System.Text;


//RabbitMQ.Client NuGet Package ile install edilir.

//Bağlantı oluşturma

ConnectionFactory factory = new()
{
    UserName = "engin",
    Password = "engin",
    Port = AmqpTcpEndpoint.UseDefaultPort,
    HostName = "172.16.30.58",
    VirtualHost = "/"
};

for (int i = 1; i <=50; i++)
{
    CreateMessage(i);
    Task.Delay(5000).Wait();
}


void CreateMessage(int i)
{
    
    //Bağlantıyı aktifleştirme
    using IConnection connection = factory.CreateConnection();

    //Kanal Oluşturma

    using IModel channel = connection.CreateModel();


    //Queue Oluşturma
    //queue:hello ismi verilmiş bir queue oluşturulur.
    //durable:false queue kalıcı değildir.
    //exclusive:false queue diğer bağlantılar tarafından erişilemez. // Birden fazla channel tarafından erişilemez
    //autoDelete:false Kuyruk silinsin mi silinmesin mi
    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, false, null);

    //Mesaj Gönderme

    //RabbitMQ kuyruğa atacağı mesajları byte türünden kabul eder.O yüzden gönderilecek mesajları byte türüne dönüştürmek gerekir.
    string message = i + ".)" + "Mebitech";
    byte[] sendMessage = Encoding.UTF8.GetBytes(message);
    i++;

    //Eğer herhangi bir exchange tipi yazılmazsa direct exchange üzerinden ilerler
    //routingKey:Mesaj kuyruğunun ismi yazılır.Direct kullanılacaksa
    channel.BasicPublish(exchange: "", routingKey: "hello", body: sendMessage);

    Console.WriteLine(message);
    



}


