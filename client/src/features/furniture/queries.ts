import { queryOptions } from "@tanstack/react-query";
import { apiClient } from "@/lib/api-client";
import type { FurnitureCategory, ListingItem } from "@/features/furniture/types";

type FurnitureItemsResult = {
  furniture: ListingItem[];
  furnitureTotal?: number;
};

type FurnitureCategoriesResult = {
  categories: FurnitureCategory[];
};

async function fetchFurnitureItems() {
  const data = await apiClient.get<FurnitureItemsResult>("/api/catalogue/furniture");
  return data.furniture;
}

async function fetchFurnitureCategories() {
  const data = await apiClient.get<FurnitureCategoriesResult>("/api/catalogue/categories");
  return data.categories;
}

export function furnitureItemsQueryOptions() {
  return queryOptions({
    queryKey: ["furniture"],
    queryFn: fetchFurnitureItems,
  });
}

export function furnitureCategoriesQueryOptions() {
  return queryOptions({
    queryKey: ["categories"],
    queryFn: fetchFurnitureCategories,
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
  return apiClient.get<FurnitureItemsResult2>("/api/catalogue/furniture", params);
}

export function furnitureItemsQueryOptions2(params: FurnitureItemsParams) {
  return queryOptions({
    queryKey: ["furniture", params.city ?? "all", params.page, params.pageSize],
    queryFn: () => fetchFurnitureItems2(params),
  });
}