# API Documentation

## Overview

This document provides documentation for the combined Cliente and Usuarios APIs, which allow managing client and user information.

Base URL: `/api`

## Clientes Endpoints

### 1. Get Clients

#### Request

- **Method:** `GET`
- **Endpoint:** `/clientes`
- **Parameters:**
  - `filter` (optional): Filter clients by name, last name, or DNI.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.

#### Example

```http
GET /api/clientes/clientes?filter=John&skip=0&limit=10
```

#### Response

```json
{
  "success": 1,
  "message": "Clientes obtenidos correctamente",
  "data": [
    {
      "id": 1,
      "nombre": "John",
      "apellido": "Doe",
      "dni": "123456789",
      "correo": "john.doe@example.com",
      "estado": true
    },
    // Additional client objects...
  ],
  "totalCount": 100
}
```

### 2. Get Client by ID

#### Request

- **Method:** `GET`
- **Endpoint:** `/clientes/{id}`
- **Parameters:**
  - `id` (required): ID of the client.

#### Example

```http
GET /api/clientes/clientes/1
```

#### Response

```json
{
  "success": 1,
  "message": "Cliente obtenido correctamente",
  "data": [
    {
      "id": 1,
      "nombre": "John",
      "apellido": "Doe",
      "dni": "123456789",
      "correo": "john.doe@example.com",
      "estado": true
    }
  ],
  "totalCount": 1
}
```

### 3. Add Client

#### Request

- **Method:** `POST`
- **Endpoint:** `/clientes`
- **Body:**
  - `nombre` (required): First name of the client.
  - `apellido` (required): Last name of the client.
  - `dni` (required): DNI (identification number) of the client.
  - `correo` (required): Email address of the client.

#### Example

```http
POST /api/clientes/clientes
{
  "nombre": "Jane",
  "apellido": "Doe",
  "dni": "987654321",
  "correo": "jane.doe@example.com"
}
```

#### Response

```json
{
  "success": 1,
  "message": "Cliente creado correctamente",
  "data": [
    {
      "id": 101,
      "nombre": "Jane",
      "apellido": "Doe",
      "dni": "987654321",
      "correo": "jane.doe@example.com",
      "estado": true
    }
  ],
  "totalCount": 1
}
```

### 4. Update Client

#### Request

- **Method:** `PATCH`
- **Endpoint:** `/clientes`
- **Body:**
  - `id` (required): ID of the client to update.
  - Additional fields (optional): Updated values for `nombre`, `apellido`, `dni`, or `correo`.

#### Example

```http
PATCH /api/clientes/clientes
{
  "id": 1,
  "nombre": "UpdatedName"
}
```

#### Response

```json
{
  "success": 1,
  "message": "Cliente actualizado correctamente",
  "data": [
    {
      "id": 1,
      "nombre": "UpdatedName",
      "apellido": "Doe",
      "dni": "123456789",
      "correo": "john.doe@example.com",
      "estado": true
    }
  ],
  "totalCount": 1
}
```

### 5. Delete Client

#### Request

- **Method:** `DELETE`
- **Endpoint:** `/clientes/{id}`
- **Parameters:**
  - `id` (required): ID of the client to delete.

#### Example

```http
DELETE /api/clientes/clientes/1
```

#### Response

```json
{
  "success": 1,
  "message": "Cliente eliminado correctamente",
  "data": null,
  "totalCount": 1
}
```

### 6. Full Delete Client

#### Request

- **Method:** `DELETE`
- **Endpoint:** `/clientes/full/{id}`
- **Parameters:**
  - `id` (required): ID of the client to fully delete (including database removal).

#### Example

```http
DELETE /api/clientes/clientes/full/1
```

#### Response

```json
{
  "success": 1,
  "message": "Cliente eliminado correctamente",
  "data": null,
  "totalCount": 1
}
```

## Usuarios Endpoints

### 1. User Authentication

#### Request

- **Method:** `POST`
- **Endpoint:** `/usuarios/login`
- **Body:**
  - `correo` (required): User email.
  - `clave` (required): User password.

#### Example

```http
POST /api/usuarios/login
{
  "correo": "admin@example.com",
  "clave": "adminpassword"
}
```

#### Response

```json
{
  "success": 1,
  "message": "Bienvenido admin@example.com",
  "data": [
    {
      "id": 1,
      "idRol": 1,
      "rol": {
        "id": 1,
        "nombre": "Administrador"
      },
      "correo": "admin@example.com",
      "nombre": "Admin",
      "apellido": "User",
      "estado": true,
      "dni": "1234567890"
    }
  ],
  "totalCount": 1
}
```

### 2. Get Users

#### Request

- **Method:** `GET`
- **Endpoint:** `/usuarios`
- **Parameters:**
  - `filter` (optional): Filter users by name, last name, or DNI.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.

#### Example

```http
GET /api/usuarios/usuarios?filter=John&skip=0&limit=10
```

#### Response

```json
{
  "success": 1,
  "message": "Usuarios obtenidos correctamente",
  "data": [
    {
      "id": 1,
      "idRol": 1,
      "rol": {
        "id": 1,
        "nombre": "Administrador"
      },
      "correo": "admin@example.com",
      "nombre": "Admin",
      "apellido": "User",
      "estado": true,
      "dni": "1234567890"
    },
    // Additional user objects...
  ],
  "totalCount": 100
}
```

### 3. Get User by ID

#### Request

- **Method:** `GET`
- **Endpoint:** `/usuarios/{id}`
- **Parameters:**
  - `id` (required):

 ID of the user.

#### Example

```http
GET /api/usuarios/usuarios/1
```

#### Response

```json
{
  "success": 1,
  "message": "Usuario obtenido correctamente",
  "data": [
    {
      "id": 1,
      "idRol": 1,
      "rol": {
        "id": 1,
        "nombre": "Administrador"
      },
      "correo": "admin@example.com",
      "nombre": "Admin",
      "apellido": "User",
      "estado": true,
      "dni": "1234567890"
    }
  ],
  "totalCount": 1
}
```

### 4. Delete User

#### Request

- **Method:** `DELETE`
- **Endpoint:** `/usuarios/{id}`
- **Parameters:**
  - `id` (required): ID of the user to delete.

#### Example

```http
DELETE /api/usuarios/usuarios/1
```

#### Response

```json
{
  "success": 1,
  "message": "Usuario eliminado correctamente",
  "data": null,
  "totalCount": 1
}
```

### 5. Update User

#### Request

- **Method:** `PATCH`
- **Endpoint:** `/usuarios`
- **Body:**
  - `id` (required): ID of the user to update.
  - Additional fields (optional): Updated values for `nombre`, `apellido`, `dni`, or `correo`.

#### Example

```http
PATCH /api/usuarios/usuarios
{
  "id": 1,
  "nombre": "UpdatedName"
}
```

#### Response

```json
{
  "success": 1,
  "message": "Usuario actualizado correctamente",
  "data": [
    {
      "id": 1,
      "idRol": 1,
      "rol": {
        "id": 1,
        "nombre": "Administrador"
      },
      "correo": "admin@example.com",
      "nombre": "UpdatedName",
      "apellido": "User",
      "estado": true,
      "dni": "1234567890"
    }
  ],
  "totalCount": 1
}
```

### 6. Add User

#### Request

- **Method:** `POST`
- **Endpoint:** `/usuarios`
- **Body:**
  - `nombre` (required): First name of the user.
  - `apellido` (required): Last name of the user.
  - `dni` (required): DNI (identification number) of the user.
  - `correo` (required): Email address of the user.
  - `clave` (required): User password.

#### Example

```http
POST /api/usuarios/usuarios
{
  "nombre": "Jane",
  "apellido": "Doe",
  "dni": "987654321",
  "correo": "jane.doe@example.com",
  "clave": "password123"
}
```

#### Response

```json
{
  "success": 1,
  "message": "Usuario creado correctamente",
  "data": [
    {
      "id": 101,
      "idRol": 2,
      "rol": {
        "id": 2,
        "nombre": "Usuario Estándar"
      },
      "correo": "jane.doe@example.com",
      "nombre": "Jane",
      "apellido": "Doe",
      "estado": true,
      "dni": "987654321"
    }
  ],
  "totalCount": 1
}
```

## Error Handling

In case of an error, the response will include a descriptive error message.

```json
{
  "success": 0,
  "message": "No se encontraron clientes/usuarios",
  "data": null,
  "totalCount": 0
}
```

---

**Note:** Replace placeholders such as `{id}` with actual values in your requests.