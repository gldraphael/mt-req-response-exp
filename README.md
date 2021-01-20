# Quickstart using docker

1. Run RabbitMQ within docker
    ```sh
    docker run  -d
                --name rabbitmq 
                --restart unless-stopped
                -p 15672:15672 -p 5672:5672
                -h therabbit 
                rabbitmq-dev:3.8-management
    ```
2. Build and run the WorkerApp, and the ClientApp
    ```sh
    docker-compose build
    docker-compose up
    ```

If it worked fine, should see something like this:

```json
Starting mt-req-response-exp_worker_1 ... done
Starting mt-req-response-exp_client_1 ... done
Attaching to mt-req-response-exp_worker_1, mt-req-response-exp_client_1
worker_1  | [18:44:20 INF] Starting the bus...
client_1  | [18:44:27 ERR] Timeout waiting for response, RequestId: 00030000-ac16-0242-a744-08d8bd7366a9
worker_1  | [18:44:27 INF] {"Id": 3, "Name": "Gretchen Kovacek", "Phone": "271.270.6667 x775", "Email": "Gretchen58@hotmail.com", "CountryCode": "NU", "$type": "GetInfoResponse"}
worker_1  | [18:44:27 INF] {"Id": 6, "Name": "Gretchen Kovacek", "Phone": "271.270.6667 x775", "Email": "Gretchen58@hotmail.com", "CountryCode": "IQ", "$type": "GetInfoResponse"}
worker_1  | [18:44:27 INF] {"Id": 10, "Name": "Gretchen Kovacek", "Phone": "271.270.6667 x775", "Email": "Gretchen58@hotmail.com", "CountryCode": "AL", "$type": "GetInfoResponse"}
worker_1  | [18:44:27 INF] {"Id": 9, "Name": "Gretchen Kovacek", "Phone": "271.270.6667 x775", "Email": "Gretchen58@hotmail.com", "CountryCode": "RU", "$type": "GetInfoResponse"}
worker_1  | [18:44:27 INF] {"Id": 1, "Name": "Gretchen Kovacek", "Phone": "271.270.6667 x775", "Email": "Gretchen58@hotmail.com", "CountryCode": "GY", "$type": "GetInfoResponse"}
client_1  | MassTransit.RequestTimeoutException: Timeout waiting for response, RequestId: 00030000-ac16-0242-a744-08d8bd7366a9
client_1  |    at MassTransit.Clients.ResponseHandlerConnectHandle`1.GetTask()
client_1  |    at MassTransit.Clients.ClientRequestHandle`1.HandleFault()
client_1  |    at MassTransit.Clients.RequestClient`1.GetResponseInternal[T](SendRequestCallback request, CancellationToken cancellationToken, RequestTimeout timeout)
client_1  |    at ClientApp.WorkerService.GetInfoAsync(Int32 id, Nullable`1 timeout, CancellationToken cancellationToken) in /src/ClientApp/WorkerService.cs:line 30
client_1  | [18:44:32 INF] [0]: {"Id": 1, "Name": "Vicky Casper", "Phone": "1-311-739-3360 x177", "Email": "Vicky.Casper60@hotmail.com", "CountryCode": "MR", "$type": "GetInfoResponse"}
worker_1  | [18:44:37 INF] {"Id": 2, "Name": "Roman Rice", "Phone": "1-483-614-5674 x230", "Email": "Roman22@yahoo.com", "CountryCode": "CD", "$type": "GetInfoResponse"}
client_1  | [18:44:37 INF] [1]: {"Id": 2, "Name": "Roman Rice", "Phone": "1-483-614-5674 x230", "Email": "Roman22@yahoo.com", "CountryCode": "CD", "$type": "GetInfoResponse"}
client_1  | [18:44:42 INF] [2]: {"Id": 3, "Name": "Miranda Kilback", "Phone": "820.414.7111", "Email": "Miranda64@yahoo.com", "CountryCode": "FM", "$type": "GetInfoResponse"}
worker_1  | [18:44:47 INF] {"Id": 4, "Name": "Ross Beier", "Phone": "(580) 741-4421 x1534", "Email": "Ross92@gmail.com", "CountryCode": "EG", "$type": "GetInfoResponse"}
client_1  | [18:44:47 INF] [3]: {"Id": 4, "Name": "Ross Beier", "Phone": "(580) 741-4421 x1534", "Email": "Ross92@gmail.com", "CountryCode": "EG", "$type": "GetInfoResponse"}
client_1  | [18:44:52 INF] [4]: {"Id": 5, "Name": "Arturo Prosacco", "Phone": "323.625.2985", "Email": "Arturo_Prosacco96@yahoo.com", "CountryCode": "FM", "$type": "GetInfoResponse"}
worker_1  | [18:44:57 INF] {"Id": 6, "Name": "Brooke Gutkowski", "Phone": "(989) 434-2012 x747", "Email": "Brooke.Gutkowski77@yahoo.com", "CountryCode": "TJ", "$type": "GetInfoResponse"}
client_1  | [18:44:57 INF] [5]: {"Id": 6, "Name": "Brooke Gutkowski", "Phone": "(989) 434-2012 x747", "Email": "Brooke.Gutkowski77@yahoo.com", "CountryCode": "TJ", "$type": "GetInfoResponse"}
client_1  | [18:45:02 INF] [6]: {"Id": 7, "Name": "Aaron Murphy", "Phone": "1-421-914-3284 x0932", "Email": "Aaron_Murphy@yahoo.com", "CountryCode": "MQ", "$type": "GetInfoResponse"}
worker_1  | [18:45:07 INF] {"Id": 8, "Name": "Leona Fahey", "Phone": "451-619-6472 x0270", "Email": "Leona_Fahey@gmail.com", "CountryCode": "TT", "$type": "GetInfoResponse"}
client_1  | [18:45:07 INF] [7]: {"Id": 8, "Name": "Leona Fahey", "Phone": "451-619-6472 x0270", "Email": "Leona_Fahey@gmail.com", "CountryCode": "TT", "$type": "GetInfoResponse"}
client_1  | [18:45:12 INF] [8]: {"Id": 9, "Name": "Grace Kohler", "Phone": "(444) 896-0923", "Email": "Grace.Kohler@hotmail.com", "CountryCode": "FI", "$type": "GetInfoResponse"}
worker_1  | [18:45:17 INF] {"Id": 10, "Name": "Judith Bayer", "Phone": "1-417-597-5547 x0212", "Email": "Judith.Bayer10@yahoo.com", "CountryCode": "GN", "$type": "GetInfoResponse"}
client_1  | [18:45:17 INF] [9]: {"Id": 10, "Name": "Judith Bayer", "Phone": "1-417-597-5547 x0212", "Email": "Judith.Bayer10@yahoo.com", "CountryCode": "GN", "$type": "GetInfoResponse"}
mt-req-response-exp_client_1 exited with code 0
```

