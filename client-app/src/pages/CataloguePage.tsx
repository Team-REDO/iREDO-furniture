// src/pages/CataloguePage.tsx

import { useQuery } from "@tanstack/react-query";
import CatalogueClient from "../apiClient/catalogueClient";

const catalogueClient = new CatalogueClient();

export default function CataloguePage() {
  const {
    data: furniture,
    isLoading,
    isError,
    error,
  } = useQuery({
    queryKey: ["furniture"],
    queryFn: () => catalogueClient.getAll(),
  });

  if (isLoading) {
    return <p>Loading furniture...</p>;
  }

  if (isError) {
    return <p>Error: {error.message}</p>;
  }

  return (
    <main className="container py-4">
      <h1>Catalogue</h1>

      <div className="row g-4">
        {furniture?.map((item) => (
          <div className="col-12 col-md-4" key={item.title}>
            <div className="card h-100">
              {item.images?.[0] && <img src={item.images[0]} className="card-img-top" alt={item.title} />}

              <div className="card-body">
                <h5 className="card-title">{item.title}</h5>
                <p className="card-text">{item.city}</p>
                <p className="card-text">{item.price} kr.</p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </main>
  );
}
