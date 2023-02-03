using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using devio;

var schemaConfig = new SchemaRegistryConfig
{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);

var config = new ConsumerConfig { BootstrapServers = "localhost:9092", GroupId = "devio" };

using var consumer = new ConsumerBuilder<string, Course>(config)
    .SetValueDeserializer(new AvroDeserializer<Course>(schemaRegistry).AsSyncOverAsync())
    .Build();

consumer.Subscribe("courses");

while (true)
{
    var result = consumer.Consume();
    Console.WriteLine($"Message: {result.Message.Key} - {DateTime.Now}");
}