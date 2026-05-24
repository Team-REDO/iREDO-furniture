import { useQuery } from "@tanstack/react-query";
import { furnitureCategoriesQueryOptions, furnitureItemsQueryOptions, furnitureItemsQueryOptions2 } from "@/features/furniture/queries";
export type { FurnitureCategory, ListingItem } from "@/features/furniture/types";

export interface UseFurnitureItemsOptions {
  usePagination?: boolean;
  page?: number;
  pageSize?: number;
  city?: string;
}

export function useFurnitureItems(options: UseFurnitureItemsOptions = {}) {
  const { usePagination = false, page = 1, pageSize = 12, city } = options;

  if (usePagination) {
    return useQuery({
      ...furnitureItemsQueryOptions2({ page, pageSize, city }),
      select: (data) => data.furniture,
    });
  }

  return useQuery(furnitureItemsQueryOptions());
}

export function useFurnitureCategories() {
  return useQuery(furnitureCategoriesQueryOptions());
}
