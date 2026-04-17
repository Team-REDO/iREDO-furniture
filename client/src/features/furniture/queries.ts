import { queryOptions } from "@tanstack/react-query";
import { gql } from "graphql-request";
import { graphqlClient } from "@/lib/graphql-client";
import type { FurnitureCategory, ListingItem } from "@/features/furniture/types";

const FURNITURE_ITEMS_QUERY = gql`
  query FurnitureItems {
    furniture {
      title
      price
      city
      subcategory
      images
    }
  }
`;

const FURNITURE_CATEGORIES_QUERY = gql`
  query FurnitureCategories {
    categories {
      category
      subcategories
    }
  }
`;



type FurnitureItemsResult = {
  furniture: ListingItem[];
};

type FurnitureCategoriesResult = {
  categories: FurnitureCategory[];
};

async function fetchFurnitureItems() {
  const data = await graphqlClient.request<FurnitureItemsResult>(FURNITURE_ITEMS_QUERY);
  return data.furniture;
}

async function fetchFurnitureCategories() {
  const data = await graphqlClient.request<FurnitureCategoriesResult>(FURNITURE_CATEGORIES_QUERY);
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

const PAGINATED_FURNITURE_QUERY = gql`
  query FurnitureItems($city: String, $page: Int, $pageSize: Int) {
    furniture(city: $city, page: $page, pageSize: $pageSize) {
      title
      price
      city
      subcategory
      images
    }
    furnitureTotal(city: $city)
  }
`;

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
  return graphqlClient.request<FurnitureItemsResult2>(PAGINATED_FURNITURE_QUERY, params);
}

export function furnitureItemsQueryOptions2(params: FurnitureItemsParams) {
  return queryOptions({
    queryKey: ["furniture", params.city ?? "all", params.page, params.pageSize],
    queryFn: () => fetchFurnitureItems2(params),
  });
}