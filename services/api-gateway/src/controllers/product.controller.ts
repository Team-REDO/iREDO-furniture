import type { Request, Response } from "express";
import * as productService from "../services/product.service.js";

export const getProducts = async (req: Request, res: Response) => {
  const data = await productService.getProducts();
  res.json(data);
};

export const createProduct = async (req: Request, res: Response) => {
  const data = await productService.createProduct(req.body);
  res.json(data);
};
