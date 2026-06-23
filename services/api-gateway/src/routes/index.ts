import { Router } from "express";
import authRoutes from "./auth.js";
import productRoutes from "./product.routes.js";
import purchaseRoutes from "./purchase.js";

const router = Router();

router.use("/auth", authRoutes);
router.use("/products", productRoutes);
router.use("/purchase", purchaseRoutes);

export default router;
