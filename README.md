# iREDO-furniture

Run the Compose stack using a separate env file for interpolation values (keeps per-service runtime env files separate):

```bash
docker compose --env-file .env.compose up -d --build
```

This reads interpolation variables from `.env.compose` while each service continues to use its own `env_file` at runtime.
