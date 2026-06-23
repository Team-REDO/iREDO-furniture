import { createFileRoute, Link } from "@tanstack/react-router";
import { useEffect, useState } from "react";

export const Route = createFileRoute("/_public/payment-success")({
  component: RouteComponent,
});

function RouteComponent() {
  const [message, setMessage] = useState("Confirming payment success...");

  useEffect(() => {
    const storedPurchase = sessionStorage.getItem("lastPurchase");

    if (!storedPurchase) {
      setMessage("Payment completed, but no local purchase data was found.");
      return;
    }

    fetch("http://localhost:8080/api/purchase/payment-success", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: storedPurchase,
    })
      .then(async (response) => {
        if (!response.ok) {
          throw new Error(`Gateway returned ${response.status}`);
        }

        setMessage("Payment success was sent to the gateway. Email event published.");
      })
      .catch((error) => {
        setMessage(`Payment succeeded, but email event failed: ${error.message}`);
      });
  }, []);

  return (
    <div className="p-8">
      <h1 className="text-3xl font-bold">Payment successful</h1>
      <p className="mt-2 text-muted-foreground">{message}</p>

      <Link to="/catalogue" className="mt-4 inline-block text-primary underline">
        Back to catalogue
      </Link>
    </div>
  );
}
