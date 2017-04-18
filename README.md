# Publish / Subscribe on .NET Core and Docker

Publish / Subscribe based on [RabbitMQ tutorial](https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html) 
with .NET Core and Docker.

## Setup

1. Install [Docker](http://docker.io).
2. Install [Docker-compose](http://docs.docker.com/compose/install/).
3. Clone this repository

## Usage

Start publisher Web API using *docker-compose*:

```bash
$ docker-compose up -d publisher
```

Start subscriber console app:

```bash
$ docker-compose up subscriber
```

Publish messages using *cURL* and watch it gets received by subscriber:

```bash
$ curl -iX POST -d '"hello world"' -H "Content-Type: application/json" localhost:5000/api/publish
```

Try putting subscriber offline (by pressing CTRL-C), run again after sending more messages and observe that all messages are delivered.

## Why

Deliver messages to multiple consumers that might be living in different processes. Basis for the implementation of architectural patterns such as CQRS, Event Sourcing.
