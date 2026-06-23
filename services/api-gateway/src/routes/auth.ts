import { Router } from "express";
import { createProxyMiddleware } from "http-proxy-middleware";
import { FRONTEND_ORIGINS, FRONTEND_REDIRECT_PATH, SERVICES } from "../config/services.js";

const router = Router();

const getFrontendOrigin = (referer?: string) => FRONTEND_ORIGINS.find((origin) => referer?.startsWith(origin)) ?? process.env.FRONTEND_ORIGIN_CONTAINER?.trim() ?? FRONTEND_ORIGINS[0];

// Starts Google login
router.get(
  "/google-login",
  createProxyMiddleware({
    target: SERVICES.user,
    changeOrigin: false,
    pathRewrite: {
      "^/google-login": "/api/auth/google-login",
    },
  }),
);

// This is where AuthController.GoogleResponse() creates the JWT cookie
router.get(
  "/google-response",
  createProxyMiddleware({
    target: SERVICES.user,
    changeOrigin: false,
    cookieDomainRewrite: "",
    pathRewrite: {
      "^/google-response": "/api/auth/google-response",
    },
    on: {
      proxyRes: (proxyRes, req) => {
        if (proxyRes.headers.location === "/catalogue") {
          proxyRes.headers.location = `${getFrontendOrigin(req.headers.referer)}${FRONTEND_REDIRECT_PATH}`;
        }
      },
    },
  }),
);

router.get("/me", async (req, res, next) => {
  try {
    const response = await fetch(`${SERVICES.user}/api/auth/me`, {
      headers: {
        cookie: req.headers.cookie ?? "",
      },
    });

    const text = await response.text();
    res.status(response.status).send(text);
  } catch (error) {
    next(error);
  }
});

router.post("/logout", async (req, res, next) => {
  try {
    const response = await fetch(`${SERVICES.user}/api/auth/logout`, {
      method: "POST",
      headers: {
        cookie: req.headers.cookie ?? "",
      },
    });

    res.clearCookie("token", {
      httpOnly: true,
      sameSite: "lax",
      secure: false,
      path: "/",
    });

    const text = await response.text();
    res.status(response.status).send(text || JSON.stringify({ message: "Logged out" }));
  } catch (error) {
    next(error);
  }
});

export default router;
