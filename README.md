# Gravel Pit Server API

Welcome to the Gravel Pit Server API project! This API provides functionalities for managing clients, orders, and types in a gravel pit system. The project was developed using Visual Studio.
I mainly created it for my family business and it's doing well. 

It's tightly connected with [Gravelpit client](https://github.com/cztgmng/GravelPitClient) from my other repository.

## Table of Contents

- [Overview](#overview)
- [Getting Started](#getting-started)
- [Endpoints](#endpoints)
- [Configuration](#configuration)
- [License](#license)

## Overview

The Gravel Pit Server API is designed to facilitate interaction with a gravel pit's database, allowing for the management of clients, orders, and related data. This API provides endpoints to create, read, update, and delete resources in a structured and efficient manner.

## Getting Started

To get started with the Gravel Pit Server API, follow these steps:

1. **Clone the Repository:**

   ```git clone https://github.com/cztgmng/GravelpitAPI.git```

2. **Navigate to the Project Directory:**

   ```cd gravelpit-api```

3. **Install Dependencies:**

   Ensure you have all the required dependencies installed. If you're using Visual Studio, dependencies should be managed automatically. Otherwise, you may need to restore NuGet packages.

4. **Run the Project:**

   Open the solution file (`GravelpitAPI.sln`) in Visual Studio

## Endpoints

The API provides the following endpoints:

- `POST /api/AddClient`: Add a new client.
- `POST /api/AddNewOrders`: Add a new order.
- `GET /api/CheckEndpoint`: Check api status.
- `POST /api/DeleteOrder`: Delete an order.
- `POST /api/EditClients`: Edit multiple clients.
- `POST /api/EditOrder`: Edit a specific order.
- `POST /api/EditOrders`: Edit multiple orders.
- `POST /api/EditTypes`: Edit multiple types.
- `GET /api/GetClients`: Retrieve clients.
- `GET /api/GetOrdersForPeriodOfTime`: Retrieve orders for a specific client over a period.
- `GET /api/GetOrdersForPeriodOfTimeForAllClients`: Retrieve orders for all clients over a period.
- `GET /api/GetTypes`: Retrieve types.
- `GET /api/SetSettled`: Set settled status for specific client over a period.

For detailed information on each endpoint, including request parameters and response formats, refer to the API documentation.

## Configuration

The project configuration is managed through command line arguments. for ex. --ip 127.0.0.1 --dbName myDatabse --dbUser root --dbPass myPassword

Default settings:
```
ipAddress = "127.0.0.1";
databaseName = "db";
databaseUserName = "root";
databaseUserPassword = "";
```

## License

This project is licensed under the Creative Commons Attribution-NonCommercial 4.0 International License. See the [LICENSE](LICENSE) file for details.

---

