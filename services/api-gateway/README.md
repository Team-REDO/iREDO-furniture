## To run the gateway container in docker compose

api-gateway:
  build: ./services/api-gateway
  ports:
    - "3100:3000" # external:internal
  environment:
    - NODE_ENV=production

For dev: NODE_ENV=development
___

## Build the image
docker build -t api-gateway:latest ./services/api-gateway

## Run the container
docker run -p 3000:3000 api-gateway:latest