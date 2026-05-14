# **API Contract**
### - TEMPLATE -

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
1. REST
2. GraphQL

## 1. REST API - EXAMPLE

| **Header**        | Required | Example          |
| ------------- | -------- | ---------------- |
| Authorization | Yes      | Bearer xxx       |
| Content-Type  | Yes      | application/json |

### **Endpoints**
| **Method** | Path            | Description     |
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

## **2. GraphQL API **
Endpoint




