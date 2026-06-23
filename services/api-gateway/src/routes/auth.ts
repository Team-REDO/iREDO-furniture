import { Router } from "express";
import jwt from "jsonwebtoken";
import { createProxyMiddleware } from "http-proxy-middleware";
import { SERVICES } from "../config/services.js";

const router = Router();

router.get("/me", (req, res) => {
  const token = req.cookies?.token;

  if (!token) {
    return res.status(401).json({ message: "Not logged in" });
  }

  try {
    const decoded = jwt.verify(token, process.env.USER_API_Jwt_key!);

    return res.json({
      user: decoded,
    });
  } catch {
    return res.status(401).json({ message: "Invalid token" });
  }
});

router.post("/logout", (_req, res) => {
  res.clearCookie("token", {
    httpOnly: true,
    sameSite: "lax",
    secure: false,
  });

  return res.json({ message: "Logged out" });
});

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
