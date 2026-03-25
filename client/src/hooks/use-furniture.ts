import { useQuery } from "@tanstack/react-query";
import type { ListingItem } from "@/components/listing-card";

export type FurnitureCategory = { category: string; subcategories: string[] };

export function useFurnitureItems() {
  return useQuery<ListingItem[]>({
    queryKey: ["furniture"],
    queryFn: async () => {
      const response = await fetch("http://localhost:3000/api/furniture");
      if (!response.ok) throw new Error("Failed to fetch furniture");
      return response.json();
    },
  });
}

export function useFurnitureCategories() {
  return useQuery<FurnitureCategory[]>({
    queryKey: ["categories"],
    queryFn: async () => {
      const response = await fetch("http://localhost:3000/api/categories");
      if (!response.ok) throw new Error("Failed to fetch categories");
      return response.json();
    },
  });
}
