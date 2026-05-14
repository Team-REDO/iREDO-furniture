import axios from "axios";
import { PRODUCT_VIEW_GRAPHQL_URL } from "../config/services.js";

type ProductViewVariables = Record<string, string | number | undefined>;

type ProductViewGraphQLResponse<T> = {
  data?: T;
  errors?: Array<{ message: string }>;
};

type ProductViewFurniture = {
  title: string;
  price: number;
  city: string;
  category?: {
    name?: string;
  };
  subcategory?: {
    name?: string;
  };
  images?: Array<{
    url: string;
  }>;
};

type ClientFurniture = {
  title: string;
  price: number;
  city: string;
  category: string;
  subcategory: string;
  images: string[];
};

const ALL_FURNITURE_QUERY = `
  query GetAllFurniture {
    allFurniture {
      title
      price
      city
      category {
        name
      }
      subcategory {
        name
      }
      images {
        url
      }
    }
  }
`;

async function requestProductView<T>(query: string, variables?: ProductViewVariables) {
  const response = await axios.post<ProductViewGraphQLResponse<T>>(PRODUCT_VIEW_GRAPHQL_URL, {
    query,
    variables,
  });

  if (response.data.errors?.length) {
    throw new Error(response.data.errors[0]?.message ?? "Product View GraphQL request failed");
  }

  if (!response.data.data) {
    throw new Error("Product View GraphQL response was empty");
  }

  return response.data.data;
}

function transformFurniture(furniture: ProductViewFurniture[]): ClientFurniture[] {
  return furniture.map((item) => ({
    title: item.title,
    price: item.price,
    city: item.city,
    category: item.category?.name || "Uncategorized",
    subcategory: item.subcategory?.name || item.category?.name || "Uncategorized",
    images: item.images?.map((img) => img.url) || [],
  }));
}

export const getProducts = async () => {
  const result = await requestProductView<{ allFurniture: ProductViewFurniture[] }>(ALL_FURNITURE_QUERY);
  return {
    furniture: transformFurniture(result.allFurniture),
  };
};

export const createProduct = async (_body: unknown) => {
  throw new Error("Product creation not yet implemented");
};
