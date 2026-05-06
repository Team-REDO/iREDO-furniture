import { GraphQLClient } from "graphql-request";

const GRAPHQL_ENDPOINT = import.meta.env.VITE_GRAPHQL_ENDPOINT?.trim();

if (!GRAPHQL_ENDPOINT) {
  throw new Error("VITE_GRAPHQL_ENDPOINT is required");
}

const fetchWithCredentials: typeof fetch = (input, init) => {
  return fetch(input, {
    ...init,
    credentials: "include",
  });
};

export const graphqlClient = new GraphQLClient(GRAPHQL_ENDPOINT, {
  fetch: fetchWithCredentials,
});
