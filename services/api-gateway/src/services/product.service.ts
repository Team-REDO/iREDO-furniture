import axios from "axios";
import { SERVICES } from "../config/services.js";

export const getProducts = async () => {
  const res = await axios.get(`${SERVICES.product}/products`);
  return res.data;
};

export const createProduct = async (body: any) => {
  const res = await axios.post(`${SERVICES.catalogue}/products`, body);
  return res.data;
};
