import { createFileRoute, redirect, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/_protected/_admin")({
  beforeLoad: ({ context }) => {
    // Only allowing users with role 'Admin' to access pages under _protected/_admin/
    if (context.auth.user?.role !== "admin") {
      throw redirect({ to: "/catalogue" }); // Redirect to safe page
    }
  },
  component: () => <Outlet />, // No visual loaded, only passing through for role check
});
