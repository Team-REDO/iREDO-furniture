import { createFileRoute } from "@tanstack/react-router";

// to create a page where admin can manage all users

export const Route = createFileRoute("/_protected/_admin/users")({
  component: RouteComponent,
});

function RouteComponent() {
  return <div>Hello "/_protected/_admin/users"!</div>;
}
