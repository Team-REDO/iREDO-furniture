import { GraphQLClient } from "graphql-request";

function resolveGraphQLEndpoint(rawEndpoint: string | undefined) {
  const endpoint = (rawEndpoint ?? "/graphql").trim();

  if (/^https?:\/\//i.test(endpoint)) {
    return endpoint;
  }

  if (endpoint.startsWith("/")) {
    if (typeof window !== "undefined") {
      return new URL(endpoint, window.location.origin).toString();
    }

    return `http://localhost:5173${endpoint}`;
  }

  return `http://${endpoint}`;
}

const GRAPHQL_ENDPOINT = resolveGraphQLEndpoint(import.meta.env.VITE_GRAPHQL_ENDPOINT);

const fetchWithCredentials: typeof fetch = (input, init) => {
  return fetch(input, {
    ...init,
    credentials: "include",
  });
};

export const graphqlClient = new GraphQLClient(GRAPHQL_ENDPOINT, {
  fetch: fetchWithCredentials,
});
