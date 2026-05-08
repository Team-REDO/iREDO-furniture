## Docker

### Build the image

```bash
docker build -t iredo-furniture-api-gateway:latest ./services/api-gateway
```

### Run the container standalone

To run the gateway container independently (e.g., when the catalogue test server is running on `localhost:4200`):

```bash
docker run -p 3100:3100 \
  -e API_GATEWAY_PORT=3100 \
  -e FRONTEND_ORIGIN=http://localhost:5173 \
  -e CATALOGUE_GRAPHQL_URL=http://host.docker.internal:4200/graphql \
  -e PRODUCT_SERVICE_URL=http://host.docker.internal:4100 \
  -e CATALOGUE_SERVICE_URL=http://host.docker.internal:4200 \
  -e USER_SERVICE_URL=http://host.docker.internal:4300 \
  -e PURCHASE_SERVICE_URL=http://host.docker.internal:4400 \
  iredo-furniture-api-gateway:latest
```

The gateway will be available at `http://localhost:3100`. The `host.docker.internal` host allows the container to reach services running on your host machine.

### Run with Docker Compose

From the project root:

```bash
docker compose up
```

This starts both the API gateway and the client in containers. The gateway will be available at `http://localhost:3100` and the client at `http://localhost:5173`.

To rebuild the images:

```bash
docker compose up --build
```

To stop the containers:

```bash
docker compose down
```
