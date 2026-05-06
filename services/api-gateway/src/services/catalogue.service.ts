import axios from "axios";
import { CATALOGUE_GRAPHQL_URL } from "../config/services.js";

type CatalogueVariables = Record<string, string | number | undefined>;

type CatalogueGraphQLResponse<T> = {
  data?: T;
  errors?: Array<{ message: string }>;
};

const FURNITURE_QUERY = `
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

const CATEGORIES_QUERY = `
  query FurnitureCategories {
    categories {
      category
      subcategories
    }
  }
`;

async function requestCatalogue<T>(query: string, variables?: CatalogueVariables) {
  const response = await axios.post<CatalogueGraphQLResponse<T>>(CATALOGUE_GRAPHQL_URL, {
    query,
    variables,
  });

  if (response.data.errors?.length) {
    throw new Error(response.data.errors[0]?.message ?? "Catalogue GraphQL request failed");
  }

  if (!response.data.data) {
    throw new Error("Catalogue GraphQL response was empty");
  }

  return response.data.data;
}

export async function getCatalogueFurniture(city?: string, page?: number, pageSize?: number) {
  return requestCatalogue<{ furniture: unknown[]; furnitureTotal: number }>(FURNITURE_QUERY, {
    city,
    page,
    pageSize,
  });
}

export async function getCatalogueCategories() {
  return requestCatalogue<{ categories: Array<{ category: string; subcategories: string[] }> }>(CATEGORIES_QUERY);
}