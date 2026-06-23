import { Router } from "express";
import { SERVICES } from "../config/services.js";

const router = Router();

router.post("/checkout", async (req, res, next) => {
  try {
    const response = await fetch(`${SERVICES.purchase}/purchase/checkout`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(req.body),
    });

    const data = await response.json();

    res.status(response.status).json(data);
  } catch (error) {
    next(error);
  }
});

export default router;
