# Catalogue Flow: Gateway + Frontend

This document explains how catalogue data moves through the system, from gateway bootstrapping (`app.ts`) to frontend rendering.

## 1. End-to-End Overview

Current frontend catalogue page uses REST endpoints exposed by the API gateway:

- Frontend calls:
  - `GET /api/catalogue/furniture`
  - `GET /api/catalogue/categories`
- Gateway forwards those requests to the catalogue GraphQL backend through gateway service logic.
- Frontend receives normalized JSON and renders cards, category filters, subcategory chips, and sorting.

There is also a separate direct GraphQL proxy route on the gateway (`/graphql`) in `services/api-gateway/src/app.ts`. The current catalogue page does not use that route; it uses the `/api/catalogue/*` REST endpoints.

## 2. Gateway: What Each File Does

### `services/api-gateway/src/server.ts`

- Loads environment variables via `dotenv/config`.
- Imports Express app from `app.ts`.
- Reads `API_GATEWAY_PORT` from config and starts listening.

### `services/api-gateway/src/app.ts`

- Creates Express app.
- Enables CORS with configured `FRONTEND_ORIGIN` and `credentials: true`.
- Enables JSON parsing with `express.json()`.
- Exposes `GET /health`.
- Defines a GraphQL passthrough endpoint at `app.all("/graphql")`:
  - Forwards method/query/body to `CATALOGUE_GRAPHQL_URL`.
  - Copies most request headers (except `host` and `origin`).
  - Mirrors backend response status/headers/body.
- Mounts REST routes under `/api` with `app.use("/api", routes)`.

### `services/api-gateway/src/routes/index.ts`

- Registers grouped route modules:
  - `/catalogue` -> `catalogue.routes.ts`
  - `/products` -> `product.routes.ts`

### `services/api-gateway/src/routes/catalogue.routes.ts`

Defines catalogue REST endpoints:

- `GET /api/catalogue/furniture` -> `getFurniture`
- `GET /api/catalogue/categories` -> `getCategories`

### `services/api-gateway/src/controllers/catalogue.controller.ts`

Controller responsibilities:

- Converts query params (`page`, `pageSize`) from string to number when present.
- Reads optional `city` filter.
- Calls service methods:
  - `getCatalogueFurniture(city, page, pageSize)`
  - `getCatalogueCategories()`
- Returns service results with `res.json(...)`.

### `services/api-gateway/src/services/catalogue.service.ts`

Service responsibilities:

- Holds GraphQL query strings:
  - `FURNITURE_QUERY` fetches `furniture` and `furnitureTotal`.
  - `CATEGORIES_QUERY` fetches category and subcategory structure.
- `requestCatalogue<T>()` posts to `CATALOGUE_GRAPHQL_URL` using Axios.
- Checks GraphQL errors (`errors[]`) and empty data.
- Exposes:
  - `getCatalogueFurniture(...)`
  - `getCatalogueCategories()`

### `services/api-gateway/src/config/services.ts`

- Centralized env loading and validation (`requireEnv`).
- Provides:
  - `FRONTEND_ORIGIN`
  - `CATALOGUE_GRAPHQL_URL`
  - `API_GATEWAY_PORT`
  - internal downstream service URLs

## 3. Frontend: What Each File Does

### `client/src/main.tsx`

- Creates React root and top-level providers.
- Wires TanStack Router + TanStack Query + auth context.
- Router gets context containing `queryClient` and `auth`.

### `client/src/router.tsx`

- Creates router from generated route tree.
- Injects TanStack Query context (`queryClient`) for route loaders.

### `client/src/integrations/tanstack-query/root-provider.tsx`

- Creates and provides a `QueryClient`.
- Enables caching and async state handling for catalogue queries.

### `client/src/routes/_public/catalogue.tsx`

This is the catalogue page route and main UI logic.

- Route loader prefetches both datasets:
  - `furnitureCategoriesQueryOptions()`
  - `furnitureItemsQueryOptions()`
- Component uses hooks:
  - `useFurnitureCategories()`
  - `useFurnitureItems()`
- Local UI state:
  - selected category
  - selected subcategory
  - sort option
- Derives visible items with `useMemo`:
  - filter by selected category/subcategory
  - sort by price or city
- Renders:
  - category button grid
  - subcategory chips
  - result count + sort dropdown
  - listing cards grid (`ListingCard`)

Note: pagination logic exists but is currently commented out in this route.

### `client/src/hooks/use-furniture.ts`

- Thin wrappers around React Query `useQuery`.
- `useFurnitureItems()`:
  - default mode fetches non-paginated items list.
  - optional paginated mode uses alternate query options and returns `data.furniture`.
- `useFurnitureCategories()` fetches categories list.

### `client/src/features/furniture/queries.ts`

Defines network query functions and query keys:

- `fetchFurnitureItems()` -> `GET /api/catalogue/furniture`
- `fetchFurnitureCategories()` -> `GET /api/catalogue/categories`
- Query keys:
  - `['furniture']`
  - `['categories']`
- Also includes paginated query variants:
  - `furnitureItemsQueryOptions2({ city, page, pageSize })`
  - Query key includes city/page/pageSize for cache separation.

### `client/src/lib/api-client.ts`

- Reads `VITE_API_GATEWAY_URL` from env at build/runtime.
- Builds full request URLs with optional query params.
- Performs `fetch(...)` with `credentials: "include"`.
- Throws on non-2xx status codes.

This file is the direct frontend-to-gateway communication layer.

### `client/src/features/furniture/types.ts`

- Defines shared frontend types used by hooks/components:
  - `ListingItem`
  - `FurnitureCategory`

### `client/src/components/listing-card.tsx`

- Presentational component for each listing.
- Displays:
  - image carousel (with fallback images)
  - title
  - city badge
  - formatted price
  - call-to-action button

### `client/src/config/material-icons.ts`

- Maps category names to Material Symbols icon names.
- Supplies fallback icon cycle for unknown categories.
- Used by catalogue route to render icon buttons.

## 4. Gateway <-> Frontend Communication Details

### Environment wiring

- Frontend uses `VITE_API_GATEWAY_URL` (see `client/src/lib/api-client.ts`).
- In Docker, this value is passed as build arg in:
  - `docker-compose.yml` (`client` service build args)
  - `client/Dockerfile` (`ARG` + `ENV VITE_API_GATEWAY_URL`)

### Request path for furniture list (active flow)

1. Catalogue route renders and prefetches query.
2. `fetchFurnitureItems()` calls `apiClient.get('/api/catalogue/furniture')`.
3. Browser request goes to `${VITE_API_GATEWAY_URL}/api/catalogue/furniture`.
4. Gateway route maps request to `getFurniture` controller.
5. Controller parses query params and calls service.
6. Service executes GraphQL `FurnitureItems` query against `CATALOGUE_GRAPHQL_URL`.
7. Gateway responds JSON with `{ furniture, furnitureTotal }`.
8. Frontend query function returns `data.furniture`.
9. Page filters/sorts in memory and renders `ListingCard` components.

### Request path for categories (active flow)

1. Catalogue route prefetches categories query.
2. `fetchFurnitureCategories()` calls `apiClient.get('/api/catalogue/categories')`.
3. Gateway controller calls service.
4. Service executes GraphQL `FurnitureCategories` query.
5. Gateway returns `{ categories }`.
6. Frontend uses categories to render category/subcategory selectors.

## 5. How Data Is Shown to the User

On the catalogue page (`client/src/routes/_public/catalogue.tsx`):

- Category data drives the top icon button grid.
- Selected category enables subcategory chips.
- Item data is filtered by selected subcategory and sorted by selected criterion.
- Each final item is rendered through `ListingCard`.
- Price and city are visible metadata; images use carousel and fallback behavior.

## 6. Important Notes

- Active catalogue frontend integration is REST over gateway (`/api/catalogue/*`), not direct GraphQL.
- A direct gateway `/graphql` proxy exists and can support future frontend GraphQL use.
- Pagination support exists in hooks/queries but is not currently used by the route UI.
- Local JSON files under `client/src/data/` are not part of the active fetch path for this page.
