import type { Request, Response } from "express";
import * as productService from "../services/product.service.js";

export const getProducts = async (req: Request, res: Response) => {
  try {
    const data = await productService.getProducts();

    if (req.query.city && typeof req.query.city === "string") {
      const filteredData = {
        ...data,
        furniture: data.furniture.filter(
          (item: any) => item.city.toLowerCase() === req.query.city?.toString().toLowerCase()
        ),
      };
      res.json(filteredData);
      return;
    }

    res.json(data);
  } catch (error) {
    const message = error instanceof Error ? error.message : "Product service unavailable";
    res.status(502).json({ message });
  }
};

export const createProduct = async (req: Request, res: Response) => {
  const data = await productService.createProduct(req.body);
  res.json(data);
};
