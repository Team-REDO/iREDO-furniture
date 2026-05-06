import { Router } from "express";
import catalogueRoutes from "./catalogue.routes.js";
import productRoutes from "./product.routes.js";

const router = Router();

router.use("/catalogue", catalogueRoutes);
router.use("/products", productRoutes);

export default router;
