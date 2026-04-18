import { createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import * as TanStackQueryProvider from './integrations/tanstack-query/root-provider'
import type { AuthContextType } from "./context/auth.context";

export interface RouterContext {
  auth: AuthContextType;
}

const tanStackQueryContext = TanStackQueryProvider.getContext()

export const router = createRouter({
  routeTree,
  context: {
    ...tanStackQueryContext,
    auth: undefined!, // Provided at render time
  },
});


declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

export { tanStackQueryContext }



