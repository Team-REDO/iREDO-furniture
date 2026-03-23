import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { RouterProvider } from '@tanstack/react-router'
import './index.css'
import { router, tanStackQueryContext } from './router'
import { ThemeProvider } from './components/theme-provider'
import * as TanStackQueryProvider from './integrations/tanstack-query/root-provider'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <TanStackQueryProvider.Provider {...tanStackQueryContext}>
        <RouterProvider router={router} />
      </TanStackQueryProvider.Provider>
    </ThemeProvider>
  </StrictMode>,
)
