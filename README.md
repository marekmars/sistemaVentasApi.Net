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
  - `orderBy` (optional): Property by which to order the results (e.g., "name", "lastname").
  - `desc` (optional): Set to true for descending order, false or omit for ascending order.

#### Example

```http
GET /api/clientes?filter=John&skip=0&limit=10&orderBy=name&desc=true
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
GET /api/clientes/1
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
POST /api/clientes
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
PATCH /api/clientes
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
DELETE /api/clientes/1
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
DELETE /api/clientes/full/1
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
  - `orderBy` (optional): Property by which to order the results (e.g., "name", "lastname").
  - `desc` (optional): Set to true for descending order, false or omit for ascending order.

#### Example

```http
GET /api/usuarios?filter=John&skip=0&limit=10&orderBy=name&desc=true
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
GET /api/usuarios/1
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
DELETE /api/usuarios/1
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
PATCH /api/usuarios
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
POST /api/usuarios
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


## Ventas Endpoints

### 1. Get Sale by ID

- **Request**
  - **Method:** `GET`
  - **Endpoint:** `/ventas/{Id}`

- **Parameters:**
  - `Id` (required): ID of the sale.

- **Example**
  ```http
  GET /api/ventas/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Venta obtenida correctamente",
    "data": [
      {
        "id": 1,
        "idCliente": 39,
        "cliente": {
          "id": 39,
          "nombre": "Juan",
          "apellido": "González",
          "dni": "12345678A",
          "correo": "juan.gonzalez@example.com",
          "estado": true
        },
        "fecha": "2024-01-18T11:17:58",
        "total": 840.00,
        "estado": true,
        "conceptos": [
          // Concepto objects...
        ]
      }
    ],
    "totalCount": 1
  }
  ```

### 2. Get Sales

- **Request**
  - **Method:** `GET`
  - **Endpoint:** `/ventas`

- **Parameters:**
  - `filter` (optional): Filter sales by client name, last name, or DNI.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.
  - `orderBy` (optional): Property by which to order the results (e.g., "date", "total").
  - `desc` (optional): Set to true for descending order, false or omit for ascending order.

- **Example**
  ```http
  GET /api/ventas?filter=Juan&skip=0&limit=10&orderBy=date&desc=true
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Ventas obtenidas correctamente",
    "data": [
      {
        "id": 1,
        "idCliente": 39,
        "cliente": {
          "id": 39,
          "nombre": "Juan",
          "apellido": "González",
          "dni": "12345678A",
          "correo": "juan.gonzalez@example.com",
          "estado": true
        },
        "fecha": "2024-01-18T11:17:58",
        "total": 840.00,
        "estado": true,
        "conceptos": [
          // Concepto objects...
        ]
      },
      // Additional sale objects...
    ],
    "totalCount": 100
  }
  ```

### 3. Add Sale

- **Request**
  - **Method:** `POST`
  - **Endpoint:** `/ventas`

- **Body:**
  - `idCliente` (required): ID of the client for the sale.
  - `conceptos` (required): List of conceptos for the sale.

- **Example**
  ```http
  POST /api/ventas
  {
    "idCliente": 39,
    "conceptos": [
      {
        "idProducto": 3,
        "cantidad": 1,
        "precioUnitario": 250.00
      },
      // Additional concepto objects...
    ]
  }
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Venta creada correctamente",
    "data": [
      {
        "id": 101,
        "idCliente": 39,
        "cliente": {
          "id": 39,
          "nombre": "Juan",
          "apellido": "González",
          "dni": "12345678A",
          "correo": "juan.gonzalez@example.com",
          "estado": true
        },
        "fecha": "2024-02-12T12:34:56",
        "total": 1200.00,
        "estado": true,
        "conceptos": [
          // Concepto objects...
        ]
      }
    ],
    "totalCount": 1
  }
  ```

### 4. Update Sale

- **Request**
  - **Method:** `PATCH`
  - **Endpoint:** `/ventas`

- **Body:**
  - `id` (required): ID of the sale to update.
  - Additional fields (optional): Updated values for `idCliente`, `conceptos`.

- **Example**
  ```http
  PATCH /api/ventas
  {
    "id": 1,
    "conceptos": [
      {
        "idProducto": 3,
        "cantidad": 2,
        "precioUnitario": 300.00
      },
      // Additional updated concepto objects...
    ]
  }
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Venta actualizada correctamente",
    "data": [
      {
        "id": 1,
        "idCliente": 39,
        "cliente": {
          "id": 39,
          "nombre": "Juan",
          "apellido": "González",
          "dni": "12345678A",
          "correo": "juan.gonzalez@example.com",
          "estado": true
        },
        "fecha": "2024-02-12T12:34:56",
        "total": 900.00,
        "estado": true,
        "conceptos": [
          // Updated concepto objects...
        ]
      }
    ],
    "totalCount": 1
  }
  ```

### 5. Delete Sale

- **Request**
  - **Method:** `DELETE`
  - **Endpoint:** `/ventas/{id}`

- **Parameters:**
  - `id` (required): ID of the sale to delete.

- **Example**
  ```http
  DELETE /api/ventas/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Venta eliminada correctamente",
    "data": null,
    "totalCount": 1
  }
  ```

### 6. Full Delete Sale

- **Request**
  - **Method:** `DELETE`
  - **Endpoint:** `/ventas/full/{id}`

- **Parameters:**
  - `id` (required): ID of the sale to fully delete (including database removal).

- **Example**
  ```http
  DELETE /api/ventas/full/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Venta eliminada correctamente",
    "data": null,
    "totalCount": 1
  }
  ```
## Productos Endpoints

### 1. Get Product by ID

- **Request**
  - **Method:** `GET`
  - **Endpoint:** `/productos/{id}`

- **Parameters:**
  - `id` (required): ID of the product.

- **Example**
  ```http
  GET /api/productos/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Producto obtenido correctamente",
    "data": [
      {
        "id": 1,
        "nombre": "Producto A",
        "descripcion": "Descripción del Producto A",
        "precio": 120.00,
        "estado": true
      }
    ],
    "totalCount": 1
  }
  ```

### 2. Get Products

- **Request**
  - **Method:** `GET`
  - **Endpoint:** `/productos`

- **Parameters:**
  - `filter` (optional): Filter products by name or description.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.
  - `orderBy` (optional): Property by which to order the results (e.g., "name", "price").
  - `desc` (optional): Set to true for descending order, false, or omit for ascending order.

- **Example**
  ```http
  GET /api/productos?filter=Producto&skip=0&limit=10&orderBy=name&desc=true
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Productos obtenidos correctamente",
    "data": [
      {
        "id": 1,
        "nombre": "Producto A",
        "descripcion": "Descripción del Producto A",
        "precio": 120.00,
        "estado": true
      },
      // Additional product objects...
    ],
    "totalCount": 100
  }
  ```

### 3. Add Product

- **Request**
  - **Method:** `POST`
  - **Endpoint:** `/productos`

- **Body:**
  - `nombre` (required): Name of the product.
  - `descripcion` (required): Description of the product.
  - `precio` (required): Price of the product.

- **Example**
  ```http
  POST /api/productos
  {
    "nombre": "Nuevo Producto",
    "descripcion": "Descripción del Nuevo Producto",
    "precio": 99.99
  }
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Producto creado correctamente",
    "data": [
      {
        "id": 101,
        "nombre": "Nuevo Producto",
        "descripcion": "Descripción del Nuevo Producto",
        "precio": 99.99,
        "estado": true
      }
    ],
    "totalCount": 1
  }
  ```

### 4. Update Product

- **Request**
  - **Method:** `PATCH`
  - **Endpoint:** `/productos`

- **Body:**
  - `id` (required): ID of the product to update.
  - Additional fields (optional): Updated values for `nombre`, `descripcion`, `precio`.

- **Example**
  ```http
  PATCH /api/productos
  {
    "id": 1,
    "nombre": "Producto Actualizado"
  }
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Producto actualizado correctamente",
    "data": [
      {
        "id": 1,
        "nombre": "Producto Actualizado",
        "descripcion": "Descripción del Producto A",
        "precio": 120.00,
        "estado": true
      }
    ],
    "totalCount": 1
  }
  ```

### 5. Delete Product

- **Request**
  - **Method:** `DELETE`
  - **Endpoint:** `/productos/{id}`

- **Parameters:**
  - `id` (required): ID of the product to delete.

- **Example**
  ```http
  DELETE /api/productos/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Producto eliminado correctamente",
    "data": null,
    "totalCount": 1
  }
  ```

### 6. Full Delete Product

- **Request**
  - **Method:** `DELETE`
  - **Endpoint:** `/productos/full/{id}`

- **Parameters:**
  - `id` (required): ID of the product to fully delete (including database removal).

- **Example**
  ```http
  DELETE /api/productos/full/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Producto eliminado correctamente",
    "data": null,
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
