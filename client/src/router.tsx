import { createRouter } from '@tanstack/react-router'
import { routeTree } from './routeTree.gen'
import * as TanStackQueryProvider from './integrations/tanstack-query/root-provider'

const tanStackQueryContext = TanStackQueryProvider.getContext()

const router = createRouter({
  routeTree,
  context: {
    ...tanStackQueryContext,
  },
})

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

export { router, tanStackQueryContext }
