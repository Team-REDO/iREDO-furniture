import { Router } from "express";
import { getCategories, getFurniture } from "../controllers/catalogue.controller.js";

const router = Router();

router.get("/furniture", getFurniture);
router.get("/categories", getCategories);

export default router;