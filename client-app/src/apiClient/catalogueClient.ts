export type ListingItem = {
  title: string;
  price: number;
  city: string;
  categories?: {
    name?: string;
    subcats?: {
      name?: string;
    }[];
  }[];
  images: string[];
};

type FurnitureItemsResult = {
  furniture: ListingItem[];
};

class CatalogueClient {
  readonly #url = `${import.meta.env.VITE_API_URL}/api/products/furniture`;

  async getAll(): Promise<ListingItem[]> {
    const res = await fetch(this.#url, {
      credentials: "include",
    });

    if (!res.ok) {
      throw new Error(`Could not fetch furniture. Status: ${res.status}`);
    }

    const data = (await res.json()) as FurnitureItemsResult;

    return data.furniture;
  }
}

export default CatalogueClient;
