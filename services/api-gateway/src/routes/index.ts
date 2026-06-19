import { Router } from "express";
import authRoutes from "./auth.js";
import productRoutes from "./product.routes.js";

const router = Router();

router.use("/auth", authRoutes);
router.use("/products", productRoutes);

export default router;
