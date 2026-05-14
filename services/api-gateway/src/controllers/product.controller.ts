import type { Request, Response } from "express";
import * as productService from "../services/product.service.js";

export const getProducts = async (req: Request, res: Response) => {
  const data = await productService.getProducts();
  
  // Apply client-side filtering if city is specified
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
};

export const createProduct = async (req: Request, res: Response) => {
  const data = await productService.createProduct(req.body);
  res.json(data);
};
