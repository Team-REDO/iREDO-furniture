import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { RouterProvider } from "@tanstack/react-router";
import "./index.css";
import { router, tanStackQueryContext } from "./router";
import { ThemeProvider } from "./components/theme-provider";
import * as TanStackQueryProvider from "./integrations/tanstack-query/root-provider";
import { AuthProvider, useAuth } from "./context/auth.context";

// Separate component so useAuth hook works inside the tree
function InnerApp() {
  const auth = useAuth();
  return <RouterProvider router={router} context={{ auth }} />;
}

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <TanStackQueryProvider.Provider {...tanStackQueryContext}>
        <AuthProvider>
          <InnerApp />
        </AuthProvider>
      </TanStackQueryProvider.Provider>
    </ThemeProvider>
  </StrictMode>,
);
