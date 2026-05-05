import { Router } from "express";
import { getProducts, createProduct } from "../controllers/product.controller.js";

import { authenticateJWT } from "../middleware/auth.middleware.js";
import { authorize } from "../middleware/rbac.middleware.js";

const router = Router();

// Public
router.get("/", getProducts);

// Admin only
router.post("/", authenticateJWT, authorize(["admin"]), createProduct);

export default router;
