import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/payment-cancel")({
    
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="p-8">
      <h1 className="text-3xl font-bold">Payment cancelled</h1>
      <p className="mt-2 text-muted-foreground">The checkout was cancelled.</p>

      <Link to="/catalogue" className="mt-4 inline-block text-primary underline">
        Back to catalogue
      </Link>
    </div>
  );
}
