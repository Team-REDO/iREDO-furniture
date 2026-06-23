import { createFileRoute, Link } from "@tanstack/react-router";
import { useMemo, useState } from "react";
import { Button } from "@/components/ui/button";
import { furnitureItemsQueryOptions } from "@/features/furniture/queries";
import { useFurnitureItems } from "@/hooks/use-furniture";
import { useAuth } from "@/context/auth.context";

export const Route = createFileRoute("/_public/catalogue/$salesPostGuid")({
  loader: async ({ context }) => {
    await context.queryClient.ensureQueryData(furnitureItemsQueryOptions());
  },
  component: RouteComponent,
});

function RouteComponent() {
  const { user } = useAuth();
  const { salesPostGuid } = Route.useParams();
  const { data: furnitureItems = [] } = useFurnitureItems();
  const [isBuying, setIsBuying] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const item = useMemo(() => furnitureItems.find((furniture) => furniture.salesPostGuid === salesPostGuid), [furnitureItems, salesPostGuid]);

  const handleBuy = async () => {
    if (!item) return;

    setIsBuying(true);
    setError(null);

    try {
      const email = user?.email;

      if (!email) {
        setError("You must be logged in to buy an item.");
        setIsBuying(false);
        return;
      }
      const now = new Date().toISOString();

      const order = {
        email,
        orderGuid: crypto.randomUUID(),
        createdAt: now,
        updatedAt: now,
        status: "Pending",
        orderItems: [
          {
            title: item.title,
            quantity: 1,
            price: item.price,
          },
        ],
      };

      sessionStorage.setItem(
        "lastPurchase",
        JSON.stringify({
          email: order.email,
          orderGuid: order.orderGuid,
          title: item.title,
        }),
      );

      const response = await fetch("http://localhost:8080/api/purchase/checkout", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(order),
      });

      if (!response.ok) {
        throw new Error(`Checkout failed with status ${response.status}`);
      }

      const data = await response.json();

      window.location.href = data.checkoutUrl;
    } catch (err) {
      setError(err instanceof Error ? err.message : "Checkout failed");
      setIsBuying(false);
    }
  };

  if (!item) {
    return (
      <div className="p-6">
        <p>Listing not found.</p>
        <Link to="/catalogue" className="text-primary underline">
          Back to catalogue
        </Link>
      </div>
    );
  }

  const imageUrl = item.images?.[0]?.imageUrl;

  return (
    <div className="mx-auto max-w-5xl p-6">
      <Link to="/catalogue" className="mb-4 inline-block text-sm text-primary underline">
        Back to catalogue
      </Link>

      <div className="grid gap-6 md:grid-cols-2">
        <div className="overflow-hidden rounded-xl border bg-muted">
          {imageUrl ? (
            <img src={imageUrl} alt={item.title} className="h-full max-h-[420px] w-full object-cover" />
          ) : (
            <div className="flex h-[320px] items-center justify-center text-muted-foreground">No image</div>
          )}
        </div>

        <div className="space-y-4">
          <div>
            <h1 className="text-3xl font-bold">{item.title}</h1>
            <p className="text-muted-foreground">{item.city}</p>
          </div>

          <p>{item.description}</p>

          <div className="text-2xl font-semibold">{item.price} DKK</div>

          <Button onClick={handleBuy} disabled={isBuying} className="w-full md:w-auto">
            {isBuying ? "Creating checkout..." : "Buy now"}
          </Button>

          {error && <p className="text-sm text-red-500">{error}</p>}
        </div>
      </div>
    </div>
  );
}
