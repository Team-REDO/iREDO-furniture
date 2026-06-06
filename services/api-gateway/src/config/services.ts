const requireEnv = (name: string) => {
  const value = process.env[name]?.trim();

  if (!value) {
    throw new Error(`${name} is required`);
  }

  return value;
};

export const SERVICES = {
  product: requireEnv("PRODUCT_VIEW_SERVICE_URL"),
  // user: requireEnv("USER_SERVICE_URL"),
  // purchase: requireEnv("PURCHASE_SERVICE_URL"),
};

export const FRONTEND_ORIGIN = requireEnv("FRONTEND_ORIGIN");
export const PRODUCT_VIEW_SERVICE_URL = requireEnv("PRODUCT_VIEW_SERVICE_URL");
export const API_GATEWAY_PORT = requireEnv("API_GATEWAY_PORT");

export const buildGraphQLEndpoint = (baseUrl: string) => new URL("/graphql", baseUrl).toString();
