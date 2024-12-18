Processing - Docker

{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "RabbitMqSettings": {
        "HostName": "${RabbitMqSettings__HostName}",
        "Port": "${RabbitMqSettings__Port}",
        "RequestQueueName": "${RabbitMqSettings__RequestQueueName}",
        "ResponseQueueName": "${RabbitMqSettings__ResponseQueueName}",
    },
    "ConnectionStrings": {
        "ConnStr": "${ConnectionStrings__ConnStr}"
    }
}

Processing

{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "RabbitMqSettings": {
        "HostName": "localhost",
        "Port": 5672,
        "RequestQueueName": "request_queue",
        "ResponseQueueName": "response_queue"
    },
    "ConnectionStrings": {
        "ConnStr": "Host=127.0.0.1;Port=5432;Database=Orders_DB;User ID=postgres;Password=123456aA;"
    }
}





webApi - Docker

{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "RabbitMqSettings": {
       "HostName": "${RabbitMqSettings__HostName}",
        "Port": "${RabbitMqSettings__Port}",
        "RequestQueueName": "${RabbitMqSettings__RequestQueueName}",
        "ResponseQueueName": "${RabbitMqSettings__ResponseQueueName}"
    }
}


{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "RabbitMqSettings": {
        "HostName": "localhost",
        "Port": 5672,
        "RequestQueueName": "request_queue",
        "ResponseQueueName": "response_queue"
    }
}

