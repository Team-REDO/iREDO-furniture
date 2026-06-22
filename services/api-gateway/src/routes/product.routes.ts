import { Router } from "express";
import { getProducts, createProduct } from "../controllers/product.controller.js";

import { authenticateJWT } from "../middleware/auth.middleware.js";
import { authorize } from "../middleware/rbac.middleware.js";

const router = Router();

// Public - Get all products/furniture
router.get("/", getProducts);
router.get("/furniture", getProducts);

// Admin only - Create product
router.post("/", authenticateJWT, authorize(["admin"]), createProduct);

export default router;
