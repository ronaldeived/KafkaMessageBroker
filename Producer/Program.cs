using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using devio;

var schemaConfig = new SchemaRegistryConfig
{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);

var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

using var producer = new ProducerBuilder<string, Course>(config)
    .SetValueSerializer(new AvroSerializer<Course>(schemaRegistry))
    .Build();

var message = new Message<string, Course>
{
    Key = Guid.NewGuid().ToString(),
    Value = new Course
    {
        Id = Guid.NewGuid().ToString(),
        Description = $"Course of Apache Kafka {Guid.NewGuid().ToString()}"
    }
};

var result = await producer.ProduceAsync("courses", message);

Console.WriteLine(result.Offset);