const requireEnv = (name: string) => {
  const value = process.env[name]?.trim();

  if (!value) {
    throw new Error(`${name} is required`);
  }

  return value;
};

export const SERVICES = {
  product: requireEnv("PRODUCT_SERVICE_URL"),
  catalogue: requireEnv("CATALOGUE_SERVICE_URL"),
  user: requireEnv("USER_SERVICE_URL"),
  purchase: requireEnv("PURCHASE_SERVICE_URL"),
};

export const FRONTEND_ORIGIN = requireEnv("FRONTEND_ORIGIN");
export const CATALOGUE_GRAPHQL_URL = requireEnv("CATALOGUE_GRAPHQL_URL");
export const PRODUCT_VIEW_GRAPHQL_URL = requireEnv("PRODUCT_VIEW_GRAPHQL_URL");
export const API_GATEWAY_PORT = requireEnv("API_GATEWAY_PORT");
