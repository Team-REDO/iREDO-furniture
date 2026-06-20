import { Router } from "express";
import { createProxyMiddleware } from "http-proxy-middleware";
import { SERVICES } from "../config/services.js";

const router = Router();

router.use(
  "/",
  createProxyMiddleware({
    target: SERVICES.user,
    changeOrigin: false,
    pathRewrite: {
      "^/": "/api/auth/",
    },
  }),
);

export default router;
