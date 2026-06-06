import express from "express";
import cors from "cors";
import morgan from "morgan";
import routes from "./routes/index.js";
import { FRONTEND_ORIGIN } from "./config/services.js";

const app = express();
const CORS_OPTIONS = {
  origin: FRONTEND_ORIGIN,
  credentials: true,
};

app.use(cors(CORS_OPTIONS));
app.use(express.json());
// HTTP request logging
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
