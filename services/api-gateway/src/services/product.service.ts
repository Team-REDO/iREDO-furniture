import axios from "axios";
import { buildGraphQLEndpoint, PRODUCT_VIEW_SERVICE_URL } from "../config/services.js";

type ProductViewVariables = Record<string, string | number | undefined>;

type ProductViewGraphQLResponse<T> = {
  data?: T;
  errors?: Array<{ message: string }>;
};

// type ProductViewFurnitureConnection = {
//   allFurniture: ProductViewVariables[];
// };

// type ProductViewFurniture = {
//   title: string;
//   price: number;
//   zip_code: string;
//   categories?: Array<{
//     name?: string;
//     subcats?: Array<{
//       name?: string;
//     }>;
//   }>;
//   color?: {
//     name?: string;
//   };
//   images?: Array<{
//     url: string;
//   }>;
// };

// type TProductViewFurniture = {
//   salesPostGuid: string;
//   personGuid: string;
//   title: string;
//   description?: string;
//   size?: string;
//   condition?: string | number;
//   quantity?: number;
//   price: number;
//   city?: string;
//   colors?: Array<{
//     name?: string;
//     href?: string;
//   }>;
//   categories?: Array<{
//     categoryName?: string;
//     subcategories?: Array<{
//       subcategoryName?: string;
//     }>;
//   }>;
//   images?: Array<{
//     imageUrl?: string;
//   }>;
// };

type TProductViewFurniture = {
  id: string;
  salesPostGuid: string;
  personGuid: string;
  title: string;
  description: string;
  size: string;
  quantity: number;
  price: number;
  condition: string;
  city: string;
  modifiedAt: string;
  colors: Array<{
    name: string;
    href: string;
  }>;
  categories: Array<{
    categoryName: string;
    subcategories: Array<{
      subcategoryName: string;
    }>;
  }>;
  images: Array<{
    imageUrl: string;
  }>;
};

// type TClientFurniture = {
//   salesPostGuid: string;
//   personGuid: string;
//   title: string;
//   description?: string;
//   size?: string;
//   condition?: string | number;
//   quantity?: number;
//   price: number;
//   city?: string;
//   colors: Array<{
//     name?: string;
//     href?: string;
//   }>;
//   categories: Array<{
//     categoryName?: string;
//     subcategories?: Array<{
//       subcategoryName?: string;
//     }>;
//   }>;
//   images: string[];
// };

type TClientFurniture = {
  salesPostGuid: string;
  personGuid: string;
  title: string;
  description?: string;
  size?: string;
  condition?: string | number;
  quantity?: number;
  price: number;
  city?: string;
  colors: Array<{
    name?: string;
    href?: string;
  }>;
  categories: Array<{
    name?: string;
    subcats?: Array<{
      name?: string;
    }>;
  }>;
  images: string[];
};

// type ClientFurniture = {
//   title: string;
//   price: number;
//   city: string;
//   categories?: Array<{
//     name?: string;
//     subcats?: Array<{
//       name?: string;
//     }>;
//   }>;
//   images: string[];
// };

// const ALL_FURNITURE_QUERY = `
//   query GetAllFurniture {
//     allFurniture {
//       title
//       price
//       zip_code
//       categories {
//         name
//         subcats {
//           name
//         }
//       }
//       color {
//         name
//       }
//       images {
//         url
//       }
//     }
//   }
// `;

// const ALL_FURNITURE_QUERY = `
//   query GetAllFurniture {
//     allFurniture {
//       nodes {
//         salesPostGuid
//         personGuid
//         title
//         description
//         size
//         condition
//         quantity
//         price
//         city
//         colors {
//           name
//           href
//         }
//         categories {
//           categoryName
//           subcategories {
//             subcategoryName
//           }
//         }
//         images {
//           imageUrl
//         }
//       }
//     }
//   }
// `;

const ALL_FURNITURE_QUERY = `
  query GetAllFurniture {
    allFurniture(first: 25) {
      totalCount
      nodes {
        id
        salesPostGuid
        personGuid
        title
        description
        size
        quantity
        price
        condition
        city
        modifiedAt
        colors {
          name
          href
        }
        categories {
          categoryName
          subcategories {
            subcategoryName
          }
        }
        images {
          imageUrl
        }
      }
    }
  }
`;

async function requestProductView<T>(query: string, variables?: ProductViewVariables) {
  const url = buildGraphQLEndpoint(PRODUCT_VIEW_SERVICE_URL);
  console.log("ProductView request ->", url, { query: query.replace(/\s+/g, " ").trim(), variables });
  const response = await axios.post<ProductViewGraphQLResponse<T>>(
    url,
    {
      query,
      variables,
    },
    {
      validateStatus: () => true,
    },
  );
  console.log("ProductView response ->", response.status);

  if (response.status >= 400) {
    const message = response.data?.errors?.[0]?.message ?? `Product View GraphQL request failed with status ${response.status}`;
    throw new Error(message);
  }

  if (response.data.errors?.length) {
    throw new Error(response.data.errors[0]?.message ?? "Product View GraphQL request failed");
  }

  if (!response.data.data) {
    throw new Error("Product View GraphQL response was empty");
  }

  console.log("RESPONSE:", response.data.data);
  return response.data.data;
}

function transformFurniture(furniture: TProductViewFurniture[]): TProductViewFurniture[] {
  return furniture.map((item) => ({
    ...item,
    colors: item.colors ?? [],
    categories: item.categories ?? [],
    images: item.images ?? [],
  }));
}

// function transformFurniture(furniture: TProductViewFurniture[]): TClientFurniture[] {
//   return furniture.map((item) => ({
//     title: item.title,
//     price: item.price,
//     city: item.zip_code,
//     categories: item.categories ?? [],
//     images: item.images?.map((img) => img.url) || [],
//   }));
// }

// export const getProducts = async () => {
//   const result = await requestProductView<{ allFurniture: any[] }>(ALL_FURNITURE_QUERY);
//   return {
//     furniture: transformFurniture(result.allFurniture),
//   };

// };

export const getProducts = async () => {
  const result = await requestProductView<{
    allFurniture: {
      totalCount: number;
      nodes: TProductViewFurniture[];
    };
  }>(ALL_FURNITURE_QUERY);

  return {
    furniture: transformFurniture(result.allFurniture.nodes),
    furnitureTotal: result.allFurniture.totalCount,
  };
};

export const createProduct = async (_body: unknown) => {
  throw new Error("Product creation not yet implemented");
};
