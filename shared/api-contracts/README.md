# _- TEMPLATE -_
# **API Contract** 

## **Service Overview**
| **Field**               | **Value**                                   |
| ------------------- | --------------------------------------- |
| Service Name        |                                         |
| Service Type        | REST / GraphQL / gRPC / Webhook / Event |
| Version             | v1                                      |
| Base URL / Endpoint | localhost:8080/customerservice          |
| Authentication      | API Key / OAuth2 / JWT / mTLS           |
| Contact developer   |                                         |

## **API Definitions**
Examples:
1. [REST](rest-api)
2. [GraphQL](graphql-api)
3. [gRPC](grpc-api)
   
More:
4. [ ** - Optionals for API's - **](4.-optional-add-ons)

## 1. REST API - EXAMPLE
### **Endpoints**
| **Header**        | Required | Example          |
| ------------- | -------- | ---------------- |
| Authorization | Yes      | Bearer xxx       |
| Content-Type  | Yes      | application/json |


| **Method** | **Path**            | Description     |
| ------ | --------------- | --------------- |
| GET    | /customers/{id} | Fetch customer  |
| POST   | /customers      | Create customer |
| PUT    | /cutomers/{id}  | Update customer |
... etc.

### **Path Params**
| Name | Type | Required |
| ---- | ---- | -------- |
| id   | UUID | Yes      |

### **Query Params**
| Name | Type    | Required | Default |
| ---- | ------- | -------- | ------- |
| page | integer | No       | 1       |

### **Request Body**
`{
  "name": "John Doe",
  "email": "john@example.com"
}`
#### **200 ok - Response Body**
`{
  "id": "cust_001",
  "name": "John Doe",
  "email": "john@example.com"
}`
#### **404 error - Response Body**
`{
  "code": "CUSTOMER_NOT_FOUND",
  "message": "Customer does not exist"
}`

## ** 2. GraphQL API - EXAMPLE ** 
### **Endpoint**
`/graphql`

### **Queries**
`query GetCustomer($id: ID!) {
  customer(id: $id) {
    id
    name
    email
  }
}`

### **Mutations**
`mutation CreateCustomer($input: CustomerInput!) {
  createCustomer(input: $input) {
    id
  }
}`

### **Error Format**
`{
  "errors": [
    {
      "message": "Unauthorized"
    }
  ]
}`

## ** 3. gRPC API - EXAMPLE ** 
#### ** Service Definition **
##### ** Proto **
`service CustomerService {
  rpc GetCustomer (CustomerRequest) returns (CustomerResponse);
}`

### **Request Message**
##### ** Proto **
`message CustomerRequest {
  string id = 1;
}`

### **Response Message**
##### ** Proto **
`message CustomerResponse {
  string id = 1;
  string name = 2;
}`

## ** 4. Optional Add-ons **

You can extend with:

- OpenAPI/Swagger links
- Diagrams
  ... But these are not necessary



