const API_GATEWAY_URL = import.meta.env.VITE_API_GATEWAY_URL?.trim();

if (!API_GATEWAY_URL) {
  throw new Error("VITE_API_GATEWAY_URL is required");
}

function buildUrl(path: string, query?: Record<string, string | number | undefined>) {
  const url = new URL(path, API_GATEWAY_URL);

  if (query) {
    Object.entries(query).forEach(([key, value]) => {
      if (value !== undefined && value !== "") {
        url.searchParams.set(key, String(value));
      }
    });
  }

  return url.toString();
}

async function getJson<T>(path: string, query?: Record<string, string | number | undefined>) {
  const response = await fetch(buildUrl(path, query), {
    credentials: "include",
  });

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  return response.json() as Promise<T>;
}

export const apiClient = {
  get: getJson,
};