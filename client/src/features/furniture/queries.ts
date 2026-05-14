import { queryOptions } from "@tanstack/react-query";
import { apiClient } from "@/lib/api-client";
import type { ListingItem } from "@/features/furniture/types";

type FurnitureItemsResult = {
  furniture: ListingItem[];
  furnitureTotal?: number;
};

async function fetchFurnitureItems() {
  const data = await apiClient.get<FurnitureItemsResult>("/api/products/furniture");
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
  return apiClient.get<FurnitureItemsResult2>("/api/products/furniture", params);
}

export function furnitureItemsQueryOptions2(params: FurnitureItemsParams) {
  return queryOptions({
    queryKey: ["furniture", params.city ?? "all", params.page, params.pageSize],
    queryFn: () => fetchFurnitureItems2(params),
  });
}