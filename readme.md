# Project Description

This is a .NET 10 Web API project for machinery with a built in machine simulation to simulate real machinery production, built using .net for the backend and react for the monitoring dashboard, the database used is postgresql.

## ⚙️ Requirements

- .NET SDK 10.0+
- Entity Framework Core (ORM)
- PostgreSQL
- NodeJS(Im using v24.10)

## 🗄️ Database setup

Create a PostgreSQL database:

CREATE DATABASE named electric_db;

## Run Locally

Clone the project

```bash
  git clone https://link-to-project
```

Go to the .NET project directory

```bash
  cd  WebApplication1
```

Update appsettings.json:

```code
      "Default": "Host=localhost;Port=5432;Database=electric_db;Username=postgres;Password=admin123"

```

Start the server

```bash
  dotnet watch run
```

Go to the React Project DIrectory

```bash
  cd  machine-dashboard
```

Install Dependencies

```bash
  npm i
```

then run the app

```bash
npm run dev
```

## API Reference

#### Get all machine

```http
  GET /machines
```

#### Get machine by Id

```http
  GET /machines/${id}
```

| Parameter | Type     | Description                                            |
| :-------- | :------- | :----------------------------------------------------- |
| `id`      | `string` | **Required**. Id dari mesin yang ingin diambil datanya |

#### Add Machine

```http
  POST /machines
```

| Parameter     | Type     | Description                                    |
| :------------ | :------- | :--------------------------------------------- |
| `machineCode` | `string` | **Required**. Kode Dari mesin machine to fetch |
| `MachineName` | `string` | **Required**. Nama dari mesin                  |
| `machineType` | `string` | **Required**. Type dari mesin                  |
| `location`    | `string` | **Required**. Lokasi mesinnya                  |

#### Get Hasil Produksi

```http
  GET /production-results
```

#### Create Hasil Produksi

```http
  POST /production-results
```

| Parameter        | Type     | Description                                        |
| :--------------- | :------- | :------------------------------------------------- |
| `machineId`      | `string` | **Required**. Id dari mesin                        |
| `status`         | `string` | **Required**. Status mesin                         |
| `itemsPerMinute` | `string` | **Required**. Hasil produksi per menit dari mesin  |
| `temperature`    | `string` | **Required**. Temperature mesin                    |
| `operatorName`   | `string` | **Required**. Nama Operator yang menjalankan mesin |

#### Register

```http
  POST /auth/register
```

| Parameter  | Type     | Description              |
| :--------- | :------- | :----------------------- |
| `name`     | `string` | **Required**. Nama User  |
| `email`    | `string` | **Required**. Email User |
| `role`     | `string` | **optional**. Role User  |
| `password` | `string` | **Required**. Password   |

#### Login

```http
  POST /auth/login
```

| Parameter  | Type     | Description              |
| :--------- | :------- | :----------------------- |
| `email`    | `string` | **Required**. Email User |
| `password` | `string` | **Required**. Password   |
