import { queryOptions } from "@tanstack/react-query";
import type { ListingItem } from "@/features/furniture/types";

type FurnitureItemsResult = {
  furniture: ListingItem[];
  furnitureTotal?: number;
};

async function fetchFurnitureItems() {
  const base = import.meta.env && (import.meta.env.VITE_API_GATEWAY_URL as string);
  if (!base) throw new Error("VITE_API_GATEWAY_URL must be set in the environment");
  const res = await fetch(`${base.replace(/\/$/, "")}/api/products/furniture`, { credentials: "include" });
  if (!res.ok) throw new Error(`Request failed with status ${res.status}`);
  const data = (await res.json()) as FurnitureItemsResult;
  return data.furniture;
}

export function furnitureItemsQueryOptions() {
  return queryOptions({
    queryKey: ["furniture"],
    queryFn: fetchFurnitureItems,
  });
}

// ___________________________

type FurnitureItemsResult2 = {
  furniture: ListingItem[];
  furnitureTotal: number;
};

type FurnitureItemsParams = {
  city?: string;
  page: number;
  pageSize: number;
};

async function fetchFurnitureItems2(params: FurnitureItemsParams) {
  const base = import.meta.env && (import.meta.env.VITE_API_GATEWAY_URL as string);
  if (!base) throw new Error("VITE_API_GATEWAY_URL must be set in the environment");
  const url = new URL(`${base.replace(/\/$/, "")}/api/products/furniture`);
  if (params) {
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== "") url.searchParams.set(key, String(value));
    });
  }
  const res = await fetch(url.toString(), { credentials: "include" });
  if (!res.ok) throw new Error(`Request failed with status ${res.status}`);
  return (await res.json()) as FurnitureItemsResult2;
}

export function furnitureItemsQueryOptions2(params: FurnitureItemsParams) {
  return queryOptions({
    queryKey: ["furniture", params.city ?? "all", params.page, params.pageSize],
    queryFn: () => fetchFurnitureItems2(params),
  });
}
