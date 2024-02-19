# Sales System API (.NET)

## Overview

The API crafted using the .NET framework, is a versatile solution tailored for seamless integration into a wide range of sales system projects. Specifically designed for managing products, users, categories, authentication, and related functionalities in a JSON-formatted structure, this API proves valuable for prototyping and developing sales systems for various businesses..

## Key Features:

- 🌐 Comprehensive CRUD operations (Create, Read, Update, Delete).
- 🌐 Utilizes a RESTful API for efficient and scalable communication.
- 🌐 Implements pagination functionality for handling large datasets effectively.
- 🌐 Ensures secure authentication through the use of JSON Web Tokens (JWT).
- 🌐 Allows filtering of products based on category, title, and price range.
- 🌐 User creation includes the ability to check for pre-existing accounts.
- 🌐 Developed in C# and .NET framework.


## Usage and Applications:

Developed within the .NET framework, the Fake Store API is well-suited for creating sales systems tailored to businesses. It offers practical features for managing data efficiently, ensuring security, and enhancing the overall user experience.

Adhering to best practices when utilizing the Sales System API within a .NET environment will contribute to the efficiency of your project development. Explore the capabilities of this API to simplify and elevate your software development processes in the context of sales systems for businesses.

Base URL: `/api`

# Clients Endpoints

## 1. Get Clients

### Request

- **Method:** `GET`
- **Endpoint:** `/clients`
- **Parameters:**
  - `filter` (optional): Filter clients by name, last name, or DNI.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.
  - `orderBy` (optional): Property by which to order the results (e.g., "name", "lastname").
  - `desc` (optional): Set to true for descending order, false or omit for ascending order.

### Example

```http
GET /api/clients?filter=John&skip=0&limit=10&orderBy=name&desc=true
```

### Response

```json
{
  "success": 1,
  "message": "Clientes obtenidos correctamente",
  "data": [
    {
      "id": 1,
      "name": "John",
      "lastname": "Doe",
      "idcard": "123456789",
      "mail": "john.doe@example.com",
      "state": 1
    },
    // Additional client objects...
  ],
  "totalCount": 100
}
```

## 2. Get Client by ID

### Request

- **Method:** `GET`
- **Endpoint:** `/clients/{id}`
- **Parameters:**
  - `id` (required): ID of the client.

### Example

```http
GET /api/clients/1
```

### Response

```json
{
  "success": 1,
  "message": "Cliente obtenido correctamente",
  "data": [
    {
      "id": 1,
      "name": "John",
      "lastname": "Doe",
      "idcard": "123456789",
      "mail": "john.doe@example.com",
      "state": true
    }
  ],
  "totalCount": 1
}
```

## 3. Add Client

### Request

- **Method:** `POST`
- **Endpoint:** `/clients`
- **Body:**
  - `name` (required): First name of the client.
  - `lastname` (required): Last name of the client.
  - `idcard` (required): Id Card (identification number) of the client.
  - `mail` (required): Email address of the client.

### Example

```http
POST /api/clients
{
  "name": "Jane",
  "lastname": "Doe",
  "idcard": "987654321",
  "mail": "jane.doe@example.com"
}
```

### Response

```json
{
  "success": 1,
  "message": "Cliente creado correctamente",
  "data": [
    {
      "id": 101,
      "name": "Jane",
      "lastname": "Doe",
      "idcard": "987654321",
      "mail": "jane.doe@example.com",
      "state": true
    }
  ],
  "totalCount": 1
}
```

## 4. Update Client

### Request

- **Method:** `PATCH`
- **Endpoint:** `/clients`
- **Body:**
  - `id` (required): ID of the client to update.
  - Additional fields (optional): Updated values for `name`, `lastname`, `idcard`, or `mail`.

### Example

```http
PATCH /api/clients
{
  "id": 1,
  "name": "UpdatedName"
}
```

### Response

```json
{
  "success": 1,
  "message": "Cliente actualizado correctamente",
  "data": [
    {
      "id": 1,
      "name": "UpdatedName",
      "lastname": "Doe",
      "idcard": "123456789",
      "mail": "john.doe@example.com",
      "state": 1
    }
  ],
  "totalCount": 1
}
```

## 5. Delete Client (Logic)

### Request

- **Method:** `DELETE`
- **Endpoint:** `/clients/{id}`
- **Parameters:**
  - `id` (required): ID of the client to delete.

### Example

```http
DELETE /api/clients/1
```

### Response

```json
{
  "success": 1,
  "message": "Cliente eliminado correctamente",
  "data": null,
  "totalCount": 1
}
```

## 6. Full Delete Client

### Request

- **Method:** `DELETE`
- **Endpoint:** `/clients/fulldelete/{id}`
- **Parameters:**
  - `id` (required): ID of the client to fully delete (including database removal).

### Example

```http
DELETE /api/clients/fulldelete/1
```

### Response

```json
{
  "success": 1,
  "message": "Cliente eliminado correctamente",
  "data": null,
  "totalCount": 1
}
```

## Pagination and Filtering in Get Clients

When using the `Get Clients` endpoint, you can apply pagination and filtering to retrieve specific sets of data.

### Pagination

- Use the `skip` parameter to skip a certain number of records.
- Use the `limit` parameter to set the maximum number of records to retrieve.

**Example:**
```markdown
GET /api/clients?skip=10&limit=5
```
This will skip the first 10 records and retrieve the next 5 records.

### Filtering

- Use the `filter` parameter to filter clients by name, last name, or id card.
- Use the `orderBy` parameter to specify the property by which to order the results.
- Use the `desc` parameter to set the order as descending (true) or ascending (false).

**Example:**
```markdown
GET /api/clients?filter=Doe&orderBy=name&desc=false
```
This will filter clients with the last name "Doe" and order the results by the first name in ascending order.

## Data Model

| Atribute         | Type       | Description                                |
|------------------|------------|--------------------------------------------|
| id               | Long       | El ID del producto.                        |
| name             | String     | El name del producto.                    |
| last_name        | String     | La descripción del producto.               |
| id_card          | String     | El price del producto.                    |
| mail             | String     | La cantidad de stock del producto.         |
| state            | Byte       | La URL de la imagen del producto.          |

## Validations
- **Name:** Minimum length of 3 characters.
- **LastName:** Minimum length of 3 characters.
- **IdCard:** Must have 8 digits.
- **Mail:** Valid email address. Must not exist in the database for other clients with a different ID and a non-zero state.


# Users Endpoints

## 1. User Authentication
- **Admin Role:**
  - Admins have full access to all user-related endpoints, including adding, updating, and deleting users.

- **Employee Role:**
  - Employees have restricted access and can only retrieve information about users but cannot perform actions like adding, updating, or deleting users.

- If the email and password do not match, the server will respond with a 401 error.

- If the email and password match, the server will respond with a 200 OK and include the bearer token and the user including the role.

- The bearer token is a JWT that contains user information and has a duration of 150 hours.

- The bearer token must be included in the header of all requests.


### Request

- **Method:** `POST`
- **Endpoint:** `/users/login`
- **Body:**
  - `mail` (required): User email.
  - `password` (required): User password.

### Example

```http
POST /api/users/login
{
  "mail": "admin@example.com",
  "password": "adminpassword"
}
```

### Response

```json
{
  "success": 1,
  "message": "Bienvenido admin@example.com",
  "data": [
    {
       "correo": "admin@example.com",
       "rol": "Admin",
       "token": "qwewqdwqe6w78qewq78q8we892NJK98sanKLJOI0lkjKLJGYUGVI98797JKLHJKH79jki7"
    }
  ],
  "totalCount": 1
}
```

## 2. Get Users

### Request

- **Method:** `GET`
- **Endpoint:** `/users`
- **Parameters:**
  - `filter` (optional): Filter users by name, last name, or DNI.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.
  - `orderBy` (optional): Property by which to order the results (e.g., "name", "lastname").
  - `desc` (optional): Set to true for descending order, false or omit for ascending order.

### Example

```http
GET /api/users?filter=John&skip=0&limit=10&orderBy=name&desc=true
```

### Response

```json
{
  "success": 1,
  "message": "Usuarios obtenidos correctamente",
  "data": [
    {
      "id": 1,
      "idRole": 1,
      "role": {
        "id": 1,
        "name": "Admin"
      },
      "mail": "admin@example.com",
      "name": "Admin",
      "password": "123456",
      "id_role": 2,
      "lastname": "User",
      "state": 1,
      "idcard": "1234567890"
    },
    // Additional user objects...
  ],
  "totalCount": 100
}
```

## 3. Get User by ID

### Request

- **Method:** `GET`
- **Endpoint:** `/users/{id}`
- **Parameters:**
  - `id` (required):

 ID of the user.

### Example

```http
GET /api/users/1
```

### Response

```json
{
  "success": 1,
  "message": "Usuario obtenido correctamente",
  "data": [
    {
      "id": 1,
      "idRole": 1,
      "role": {
        "id": 1,
        "name": "Administrador"
      },
      "mail": "admin@example.com",
      "password": "123456",
      "id_role": 2,
      "name": "Admin",
      "lastname": "User",
      "state": true,
      "idcard": "1234567890"
    }
  ],
  "totalCount": 1
}
```

## 4. Add User

### Request

- **Method:** `POST`
- **Endpoint:** `/users`
- **Body:**
  - `name` (required): First name of the user.
  - `lastname` (required): Last name of the user.
  - `idcard` (required): DNI (identification number) of the user.
  - `idrole` (required): Id of the selected role.
  - `main` (required): Email address of the user.
  - `password` (required): User password.

### Example

```http
POST /api/usuarios
{
  "name": "Jane",
  "lastname": "Doe",
  "password": "123456",
  "id_role": 2,
  "idcard": "987654321",
  "mail": "jane.doe@example.com",
  "password": "password123"
}
```

### Response

```json
{
  "success": 1,
  "message": "Usuario creado correctamente",
  "data": [
    {
      "id": 101,
      "idRole": 2,
      "role": {
        "id": 2,
        "name": "Usuario Estándar"
      },
      "mail": "jane.doe@example.com",
      "name": "Jane",
      "lastname": "Doe",
      "state": true,
      "idcard": "987654321"
    }
  ],
  "totalCount": 1
}
```

## 5. Update User

### Request

- **Method:** `PATCH`
- **Endpoint:** `/users`
- **Body:**
  - `id` (required): ID of the user to update.
  - Additional fields (optional): Updated values for `name`, `lastname`, `idcard`,`password`,`role`, or `mail`.

### Example

```http
PATCH /api/users
{
  "id": 1,
  "name": "UpdatedName"
}
```

### Response

```json
{
  "success": 1,
  "message": "Usuario actualizado correctamente",
  "data": [
    {
      "id": 1,
      "idRole": 1,
      "role": {
        "id": 1,
        "name": "Administrador"
      },
      "mail": "admin@example.com",
      "password": "123456",
      "id_role": 2,
      "name": "UpdatedName",
      "lastname": "User",
      "state": true,
      "idcard": "1234567890"
    }
  ],
  "totalCount": 1
}
```

## 6. Delete User (Logic)

### Request

- **Method:** `DELETE`
- **Endpoint:** `/users/{id}`
- **Parameters:**
  - `id` (required): ID of the user to delete.

### Example

```http
DELETE /api/users/1
```

### Response

```json
{
  "success": 1,
  "message": "Usuario eliminado correctamente",
  "data": null,
  "totalCount": 1
}
```

## 7. Full Delete User

### Request

- **Method:** `DELETE`
- **Endpoint:** `/clients/fulldelete/{id}`
- **Parameters:**
  - `id` (required): ID of the user to fully delete (including database removal).

### Example

```http
DELETE /api/users/fulldelete/1
```

### Response

```json
{
  "success": 1,
  "message": "Usuario eliminado correctamente",
  "data": null,
  "totalCount": 1
}
```

## Pagination and Filtering in Get Users

When using the `Get Users` endpoint, you can apply pagination and filtering to retrieve specific sets of data.

### Pagination

- Use the `skip` parameter to skip a certain number of records.
- Use the `limit` parameter to set the maximum number of records to retrieve.

**Example:**
```markdown
GET /api/Users?skip=10&limit=5
```
This will skip the first 10 records and retrieve the next 5 records.

### Filtering

- Use the `filter` parameter to filter users by name, last name, or id card.
- Use the `orderBy` parameter to specify the property by which to order the results.
- Use the `desc` parameter to set the order as descending (true) or ascending (false).

**Example:**
```markdown
GET /api/users?filter=Doe&orderBy=name&desc=1
```
This will filter users with the last name "Doe" and order the results by the first name in descending order.

## Data Model

| Attribute        | Type       | Description                            |
|-------------------|------------|----------------------------------------|
| id                | Long       | The ID of the user.                 |
| name              | String     | The name of the user.               |
| id_role           | Long       | The id role of the user |
| password          | String     | the password of the user |
| last_name         | String     | The last name of the user.        |
| id_card           | String     | The identification number of the user.              |
| mail              | String     | The email of the user. |
| state             | Byte       | The state of the user.          |


## Validations
- **IdRole:** Must be a positive integer and exist in the database.
- **Mail:** Valid email address. Must not exist in the database for other users with a different ID and a state of 1.
- **Password:** Minimum length of 6 characters.
- **Name:** Minimum length of 3 characters.
- **LastName:** Minimum length of 3 characters.
- **IdCard:** Must have 8 digits.

# Roles Endpoints
## 1. Get Roles
### Request
- **Method:** `GET`
- **Endpoint:** `/roles`
### Example
``` 
http
GET /api/roles

```
### Response
``` json

{
  "success": 1,
  "message": "Roles retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "Admin"
    },
    {
      "id": 2,
      "name": "Seller"
    },
    // Additional role objects...
  ],
  "totalCount": 5
}
```
## Data Model

| Attribute         | Type       | Description                                |
|------------------|------------|--------------------------------------------|
| id               | Long       | The id of the role.                        |
| name             | String     | The name of the role.                      |


# Sales Endpoints

## 1. Get Sales

### Request

- **Method:** `GET`
- **Endpoint:** `/api/sales`
- **Parameters:**
  - `filter` (optional): Filter sales by client name, last name, or DNI.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.
  - `orderBy` (optional): Property by which to order the results (e.g., "date", "total").
  - `desc` (optional): Set to true for descending order, false or omit for ascending order.

**Example**

```http
 GET /api/sales?filter=Juan&skip=0&limit=10&orderBy=date&desc=true
```

### Response

```json
  {
    "success": 1,
    "message": "Ventas obtenidas correctamente",
    "data": [
      {
        "id": 1,
        "idClient": 39,
        "client": {
          "id": 39,
          "name": "Juan",
          "lastname": "González",
          "idcard": "12345678A",
          "mail": "juan.gonzalez@example.com",
          "state": true
        },
        "date": "2024-01-18T11:17:58",
        "total": 840.00,
        "state": true,
        "concepts": [
          // Concept objects...
        ]
      },
      // Additional sale objects...
    ],
    "totalCount": 100
  }
  ```

## 2. Get Sale by ID

### Request

- **Method:** `GET`
- **Endpoint:** `/api/sales/{Id}`

**Example**

```http
GET /api/sales/1
```

### Response

```json
{
  "success": 1,
  "message": "Venta obtenida correctamente",
  "data": [
    {
      "id": 1,
      "idClient": 101,
      "date": "2024-02-15T10:30:00",
      "total": 150.00,
      "state": 1,
      "concepts": [
        {
          "id": 1,
          "quantity": 2,
          "unitaryPrice": 75.00,
          "import": 150.00,
          "idProduct": 201,
          "state": 1
        }
        // Additional concept objects...
      ]
    }
  ],
  "totalCount": 1
}
```

## 3. Add Sale

### Request

- **Method:** `POST`
- **Endpoint:** `/api/sales`
- **Body:**
  - `idClient` (required): ID of the client for the sale.
  - `concepts` (required): List of concepts for the sale, including `quantity`, `unitaryPrice`, `import`, `idProduct`.

### Example

```http
POST /api/sales
{
  "idClient": 101,
  "concepts": [
    {
      "quantity": 2,
      "unitaryPrice": 75.00,
      "import": 150.00,
      "idProduct": 201
    }
    // Additional concept objects...
  ]
}
```

### Response

```json
{
  "success": 1,
  "message": "Venta creada correctamente",
  "data": [
    {
      "id": 102,
      "idClient": 101,
      "date": "2024-02-15T12:45:00",
      "total": 300.00,
      "state": 1,
      "concepts": [
        {
          "id": 201,
          "quantity": 2,
          "unitaryPrice": 150.00,
          "import": 300.00,
          "idProduct": 301,
          "state": 1
        }
        // Additional concept objects...
      ]
    }
  ],
  "totalCount": 1
}
```

## 5. Update Sale

### Request

- **Method:** `PATCH`
- **Endpoint:** `/api/sales`
- **Body:**
  - `id` (required): ID of the sale to update.
  - Additional fields (optional): Updated values for `idClient`, `concepts`.

### Example

```http
PATCH /api/sales
{
  "id": 102,
  "concepts": [
    {
      "quantity": 3,
      "unitaryPrice": 100.00,
      "import": 300.00,
      "idProduct": 201
    }
    // Additional updated concept objects...
  ]
}
```

### Response

```json
{
  "success": 1,
  "message": "Venta actualizada correctamente",
  "data": [
    {
      "id": 102,
      "idClient": 101,
      "date": "2024-02-15T12:45:00",
      "total": 300.00,
      "state": 1,
      "concepts": [
        {
          "id": 201,
          "quantity": 3,
          "unitaryPrice": 100.00,
          "import": 300.00,
          "idProduct": 301,
          "state": 1
        }
        // Additional updated concept objects...
      ]
    }
  ],
  "totalCount": 1
}
```

## 6. Delete Sale (Logic)

### Request

- **Method:** `DELETE`
- **Endpoint:** `/api/sales/{Id}`
- **Parameters:**
  - `Id` (required): ID of the sale to delete.

### Example

```http
DELETE /api/sales/102
```

### Response

```json
{
  "success": 1,
  "message": "Venta eliminada correctamente",
  "data": null,
  "totalCount": 1
}
```

## 7. Full Delete Sale

### Request

- **Method:** `DELETE`
- **Endpoint:** `/api/sales/fulldelete/{Id}`
- **Parameters:**
  - `Id` (required): ID of the sale to fully delete.

### Example

```http
DELETE /api/sales/fulldelete/102
```

### Response

```json
{
  "success": 1,
  "message": "Venta eliminada permanentemente correctamente",
  "data": null,
  "totalCount": 1
}
```

## Pagination and Filtering in Get Sales

When using the `Get Sales` endpoint, you can apply pagination and filtering to retrieve specific sets of data.

### Pagination

- Use the `skip` parameter to skip a certain number of records.
- Use the `limit` parameter to set the maximum number of records to retrieve.

**Example:**
```markdown
GET /api/sales?skip=10&limit=5
```
This will skip the first 10 records and retrieve the next 5 records.

### Filtering

- Use the `filter` parameter to filter users by client name, client last name, date, or product name.
- Use the `orderBy` parameter to specify the property by which to order the results (date, client name or last name and total price).
- Use the `desc` parameter to set the order as descending (1) for descending order.

**Example:**
```markdown
GET /api/sales?filter=Doe&orderBy=date&desc=1
```
This will filter sales with the client last name "Doe" and order the results by the date in descending order.

## Data Model

### Sale

| Attribute | Type         | Description                                  | 
|-----------|--------------|----------------------------------------------|
| id        | Long         | ID of the sale                                | 
| idClient  | Long         | ID of the client for the sale.               | 
| concepts  | List         | List of concepts for the sale.               | 
| date      | DateTime     | Date of the sale.                            | 
| total     | Decimal      | Total amount of the sale.                    | 
| state     | Byte         | State of the sale.                           | 

### Concept

| Attribute    | Type         | Description                                  | 
|--------------|--------------|----------------------------------------------|
| id           | Long         | ID
| quantity     | Int          | Quantity of the product in the concept.      | 
| unitaryPrice | Decimal      | Unitary price of the product in the concept. | 
| import       | Decimal      | Total import of the concept.                 | 
| idProduct    | Long         | ID of the product in the concept.            | 
| state        | Byte         | State of the concept.                        | 

### Validations

- **idClient:** Must be a positive integer and exist in the database.
- **concepts:** Must have at least one concept with valid data.
- **quantity:** Must be a positive integer.
- **unitaryPrice:** Must be a positive decimal.
- **import:** Must be a positive decimal.
- **idProduct:** Must be a positive integer, exist in the database and have the requested stock.
- **state:** Must be a byte value.



# Products Endpoints

## 1. Get Product by ID

- **Request**
  - **Method:** `GET`
  - **Endpoint:** `/products/{id}`

- **Parameters:**
  - `id` (required): ID of the product.

- **Example**
  ```http
  GET /api/products/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Product obtenido correctamente",
    "data": [
      {
        "id": 1,
        "name": "Product A",
        "description": "Descripción del Product A",
        "unitaryPrice": 650.5,
        "cost": 300,
        "stock":150,
        "state": 1,
        "image_url":"www.imgs/e2132ui13ksjha.jpg"
      }
    ],
    "totalCount": 1
  }
  ```

## 2. Get Products

- **Request**
  - **Method:** `GET`
  - **Endpoint:** `/products`

- **Parameters:**
  - `filter` (optional): Filter products by name or description.
  - `skip` (optional): Number of records to skip.
  - `limit` (optional): Maximum number of records to retrieve.
  - `orderBy` (optional): Property by which to order the results (e.g., "name", "price").
  - `desc` (optional): Set to true for descending order, false, or omit for ascending order.

- **Example**
  ```http
  GET /api/products?filter=potatoes&skip=0&limit=10&orderBy=name&desc=true
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Products obtenidos correctamente",
    "data": [
      {
        "id": 1,
        "name": "Product A",
        "description": "Descripción del Product A",
        "unitaryPrice": 650.5,
        "cost": 300,
        "stock":150,
        "state": 1,
        "image_url":"www.imgs/e2132ui13ksjha.jpg"
      },
      // Additional product objects...
    ],
    "totalCount": 100
  }
  ```

## 3. Add Product

- **Request**
  - **Method:** `POST`
  - **Endpoint:** `/products`

- **Body:**
  - `name` (required): Name of the product.
  - `description` (required): Description of the product.
  - `unitaryPrice` (required): Price of the product.
  - `cost`: (required): Cost of the product.,
  - `stock`:(required): Stock number of the product.,
  - `state`: (required): State of the product.,
  - `image_url`:(optional): Description of the product.

- **Example**
  ```http
  POST /api/products
  {
        "name": "Product A",
        "description": "Descripción del Product A",
        "unitaryPrice": 650.5,
        "cost": 300,
        "stock":150,
        "state": true,
        "image_url":"www.imgs/e2132ui13ksjha.jpg"
  }
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Product creado correctamente",
    "data": [
      {
        "id": 101,
        "name": "Product A",
        "description": "Descripción del Product A",
        "unitaryPrice": 650.5,
        "cost": 300,
        "stock":150,
        "state": true,
        "image_url":"www.imgs/e2132ui13ksjha.jpg"
      }
    ],
    "totalCount": 101
  }
  ```

## 4. Update Product

- **Request**
  - **Method:** `PATCH`
  - **Endpoint:** `/products`

- **Body:**
  - `id` (required): ID of the product to update.
  - Additional fields (optional): Updated values for `name`, `description`,  `unitaryPrice`,  `cost`: ,  `stock` , `image_url`.

- **Example**
  ```http
  PATCH /api/products
  {
    "id": 1,
    "name": "Product Actualizado"
  }
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Product actualizado correctamente",
    "data": [
      {
        "id": 1,
        "name": "Product Actualizado",
        "description": "Descripción del Product A",
        "unitaryPrice": 650.5,
        "cost": 300,
        "stock":150,
        "state": true,
        "image_url":"www.imgs/e2132ui13ksjha.jpg"
      }
    ],
    "totalCount": 1
  }
  ```

## 5. Delete Product

- **Request**
  - **Method:** `DELETE`
  - **Endpoint:** `/products/{id}`

- **Parameters:**
  - `id` (required): ID of the product to delete.

- **Example**
  ```http
  DELETE /api/products/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Product eliminado correctamente",
    "data": null,
    "totalCount": 1
  }
  ```

## 6. Full Delete Product

- **Request**
  - **Method:** `DELETE`
  - **Endpoint:** `/products/fulldelete/{id}`

- **Parameters:**
  - `id` (required): ID of the product to fully delete (including database removal).

- **Example**
  ```http
  DELETE /api/products/full/1
  ```

- **Response**
  ```json
  {
    "success": 1,
    "message": "Product eliminado correctamente",
    "data": null,
    "totalCount": 1
  }
  ```
## Pagination and Filtering in Get Products

When using the `Get Products` endpoint, you can apply pagination and filtering to retrieve specific sets of data.

### Pagination

- Use the `skip` parameter to skip a certain number of records.
- Use the `limit` parameter to set the maximum number of records to retrieve.

**Example:**
```markdown
GET /api/products?skip=10&limit=5
```
This will skip the first 10 records and retrieve the next 5 records.

### Filtering

- Use the `filter` parameter to filter users by name.
- Use the `min` parameter to add a min price value.
- Use the `max` parameter to add a max price value.
- Use the `orderBy` parameter to specify the property by which to order the results.
- Use the `desc` parameter to set the order as descending (true) or ascending (false).

**Example:**
```markdown
GET /api/products?filter=potatoes&orderBy=name&min=150&max=500&orderBy=price&desc=1
```
This will filter products with the word potato included in the name, between 150 and 500 and order the results by the price in descending order.

## Data Model

| Attribute         | Type       | Description                                |
|------------------|------------|--------------------------------------------|
| id               | Long       | ID of the product.                         |
| name             | String     | Name of the product.                       |
| description      | String     | La descripción of the product.             |
| unitary_price    | Decimal    | Price of the product.                      |
| cost             | Decimal    | Cost of the product.                       |
| stock            | Int        | Stock quantity of the product.             |
| state            | Byte       | State of the product                       |
| image_url        | String     | Image URL of the product.                  |


## Validations

- **IdRole:** Must be a positive integer and exist in the database.

- **Mail:** Valid email address. Must not exist in the database for other users with a different ID and a state of 1.

- **Password:** Minimum length of 6 characters.

- **Name:** Minimum length of 3 characters.

- **LastName:** Minimum length of 3 characters.

- **IdCard:** Must have 8 digits.

# Error Handling

In case of an error, the response will include a descriptive error message.

```json
{
  "success": 0,
  "message": "No se encontraron clientes/usuarios",
  "data": null,
  "totalCount": 0
}
```


**Note:** Replace placeholders such as `{id}` with actual values in your requests.
