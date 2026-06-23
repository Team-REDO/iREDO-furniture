import express from "express";
import cors from "cors";
import type { CorsOptions } from "cors";
import { createProxyMiddleware } from "http-proxy-middleware";
import morgan from "morgan";
import routes from "./routes/index.js";
import { FRONTEND_ORIGINS, FRONTEND_REDIRECT_PATH, SERVICES } from "./config/services.js";

const app = express();

export const CORS_OPTIONS: CorsOptions = {
  origin: FRONTEND_ORIGINS,
  credentials: true,
};

app.use(cors(CORS_OPTIONS));

app.get(
  "/signin-google",
  createProxyMiddleware({
    target: SERVICES.user,
    changeOrigin: false,
    pathRewrite: {
      "^/signin-google": "/signin-google",
    },
    on: {
      proxyRes: (proxyRes, req) => {
        if (proxyRes.statusCode && proxyRes.statusCode >= 300 && proxyRes.statusCode < 400) {
          const referer = req.headers.referer;

          const frontendOrigin = FRONTEND_ORIGINS.find((origin) => referer?.startsWith(origin)) ?? FRONTEND_ORIGINS[0];

          proxyRes.headers.location = `${frontendOrigin}${FRONTEND_REDIRECT_PATH}`;
        }
      },
    },
  }),
);

app.use(express.json());
app.use(morgan("combined"));

app.get("/health", (_req, res) => {
  res.json({ status: "ok" });
});

app.options("/graphql", cors(CORS_OPTIONS));

app.use("/api", routes);

// Global error handler - log stack and return JSON error
app.use((err: unknown, _req: any, res: any, _next: any) => {
  console.error("Unhandled Error:", err instanceof Error ? (err.stack ?? err.message) : err);
  const message = err instanceof Error ? err.message : "Internal server error";
  res.status(500).json({ message });
});

export default app;
