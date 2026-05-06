import type { Request, Response } from "express";
import { getCatalogueCategories, getCatalogueFurniture } from "../services/catalogue.service.js";

function toOptionalNumber(value: unknown) {
  if (typeof value !== "string") {
    return undefined;
  }

  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : undefined;
}

export const getFurniture = async (req: Request, res: Response) => {
  const data = await getCatalogueFurniture(
    typeof req.query.city === "string" ? req.query.city : undefined,
    toOptionalNumber(req.query.page),
    toOptionalNumber(req.query.pageSize),
  );

  res.json(data);
};

export const getCategories = async (_req: Request, res: Response) => {
  const data = await getCatalogueCategories();
  res.json(data);
};