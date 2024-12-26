# Sistema de Cadastro de Pedidos 🛒

Este projeto é um sistema de backend para **cadastro de pedidos**, implementado com **Clean Architecture**, **.NET 8**, **RabbitMQ com RPC**, **Mediator**, e **Entity Framework**.

## Funcionalidades 🚀

- **Cadastro de Pedidos**: Permite o cadastro, consulta, atualização e remoção de pedidos no sistema.
- **Comunicação via RabbitMQ**: Utiliza RabbitMQ para comunicação assíncrona entre os serviços com o padrão RPC (Remote Procedure Call).
- **MediatR**: Utiliza o MediatR para a comunicação entre os diferentes componentes, aplicando o padrão Mediator.
- **Persistência com Entity Framework**: Utiliza o Entity Framework para persistência de dados no banco de dados.

## Tecnologias 🛠️

- **.NET 8**: Framework principal para o desenvolvimento do sistema.
- **RabbitMQ**: Servidor de mensageria para comunicação assíncrona.
- **MediatR**: Biblioteca para implementar o padrão Mediator.
- **Entity Framework**: ORM para persistência de dados em bancos relacionais.
- **Clean Architecture**: Estrutura que organiza o código de forma a separá-lo em camadas, promovendo a manutenção e escalabilidade do sistema.

## Estrutura do Projeto 📂

O projeto é dividido nas seguintes camadas, seguindo os princípios da Clean Architecture:

- **Application**: Contém referências para o **Domain**, **Exception** e **UseCase**.
  - **Domain**: Definições das entidades e interfaces de domínio.
  - **Exception**: Classes para tratamento de exceções e erros do sistema.
  - **UseCase**: Lógica de negócios e casos de uso específicos.
  
- **Composition**: Camada responsável pela composição das dependências, com referências para **Application** e **Infrastructure**.
  
- **Domain**: Contém as entidades e modelos principais, representando as regras de negócio do sistema.
  
- **Exception**: Contém classes de exceções customizadas e tratamento de erros globais.
  
- **Infrastructure**: Implementações de dependências externas, como **Persistence** e **UseCase**.
  - **Persistence**: Implementações de acesso a banco de dados e repositórios.
  - **UseCase**: Implementações dos casos de uso da infraestrutura.
  
- **Localization**: Gerencia a internacionalização e localização de mensagens e dados.
  
- **Messaging**: Contém lógica de comunicação entre serviços via mensagens, com referências a **Exception** e **UseCase**.
  
- **Persistence**: Contém implementações relacionadas à persistência de dados e acesso ao banco de dados, com dependências de **Application**.
  
- **Test**: Contém os testes automatizados do sistema.
  
- **UseCase**: Contém a lógica de negócios e serviços do sistema, com referências para **Domain**, **Exception** e **Localization**.
  
- **WebAPI**: A camada de API, responsável por expor os endpoints RESTful do sistema. Depende de **ProcessAPI** (com referências para **Application**, **Composition**, **Exception** e **Messaging**) e **WebAPI** (com referências para **Messaging** e **UseCase**).


## 1. Configurações de Ambiente 🛠️

O projeto utiliza arquivos `appsettings.json` para configurar os diferentes ambientes de execução. As configurações para cada serviço estão descritas abaixo:


### ProcessingAPI 🛠️

#### Configuração Local 🖥️
Arquivo: `\ProjProcessOrders.ProcessingAPI\appsettings.json`

```json
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
```

#### Configuração Docker 🐳
**Arquivo:** `\ProjProcessOrders.ProcessingAPI\appsettings.json`

```json
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
  },
  "ConnectionStrings": {
    "ConnStr": "${ConnectionStrings__ConnStr}"
  }
}
```

### WebAPI 🛠️

#### Configuração Local 🖥️
Arquivo: `\ProjProcessOrders.WebAPI\appsettings.json`

```json
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
```


#### Configuração Docker 🐳
**Arquivo:** `\ProjProcessOrders.WebAPI\appsettings.json`

```json
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
```

## 2. Instalação e Execução 🖥️

Para rodar o projeto localmente ou via Docker, siga os passos abaixo:

## 3. Requisitos
- **.NET 8 SDK** - Baixe e instale a versão mais recente do .NET 8 SDK.
- **Docker** - Instale o Docker para rodar os containers.
- **PostgreSQL** - O banco de dados PostgreSQL precisa estar rodando na máquina local ou em um container Docker.

## 4. Rodando o Projeto Localmente

1. Clone o repositório:
   ```bash
   git clone https://github.com/ralfsniper0102/PrjProcessOrders.git
   ```
2. Navegue até o diretório do projeto:
   ```bash
   cd ProjProcessOrders
   ```
3. Restaure as dependências:
   ```bash
   dotnet restore
   ```
4. Execute o projeto:
   ```bash
   dotnet run
   ```

## 5. Rodando via Docker

Para rodar o projeto com Docker, execute o comando abaixo:
```bash
docker-compose up
```


## 6. Arquitetura e Fluxo de Dados 📊

O sistema segue os princípios da **Clean Architecture** para separar as responsabilidades e promover a escalabilidade. A comunicação entre os serviços é feita via RabbitMQ, usando o padrão **RPC (Remote Procedure Call)**.

O fluxo básico de um pedido pode ser descrito da seguinte forma:

1. O cliente faz uma requisição via **WebAPI** para cadastrar um novo pedido.
2. A **WebAPI** envia uma mensagem para o RabbitMQ, colocando o pedido na **RequestQueue**.
3. A **ProcessingAPI** consome a mensagem da fila, processa o pedido e armazena as informações no banco de dados usando **Entity Framework**.
4. Após o processamento, uma resposta é enviada para a **ResponseQueue** do RabbitMQ.
5. A **WebAPI** recebe a resposta e retorna ao cliente.

## 6. Testes Automatizados 🧪

O sistema possui testes automatizados para garantir a qualidade do código. Eles estão localizados na pasta **Test**.

Para rodar os testes:

1. Certifique-se de que todas as dependências estão restauradas:
   ```bash
   dotnet restore
   ```

2. Execute os testes:
   ```bash
   dotnet test
   ```

Os testes utilizam o **xUnit** para garantir o funcionamento correto dos casos de uso e da comunicação entre os serviços.



