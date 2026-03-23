import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtoolsPanel } from '@tanstack/react-router-devtools';
import { TanStackDevtools } from '@tanstack/react-devtools';
import TanStackQueryDevtools from '../integrations/tanstack-query/devtools';
import type { QueryClient } from '@tanstack/react-query'
import { Header } from '@/components/header'
import { Footer } from '@/components/footer'
import { AppSidebar } from '@/components/app-sidebar'
import { SidebarInset, SidebarProvider } from '@/components/ui/sidebar'

interface RouterContext {
  queryClient: QueryClient
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: () => (
    <>
      <SidebarProvider>
        <AppSidebar />

        <SidebarInset className="min-h-svh">
          <Header />

          <main className="w-full flex-1 p-4 md:p-6">
            <Outlet />
          </main>

          <Footer />
        </SidebarInset>
      </SidebarProvider>

      <TanStackDevtools
        config={{
          position: 'bottom-right',
        }}
        plugins={[
          {
            name: 'Tanstack Router',
            render: <TanStackRouterDevtoolsPanel />,
          },
          TanStackQueryDevtools,
        ]}
      />

    </>
  ),
})
