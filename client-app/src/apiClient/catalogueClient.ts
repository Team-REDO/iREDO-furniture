
export type ListingItem = {
  title: string;
  price: number;
  city: string;
  category: string;
  subcategory: string;
  images: string[];
};


type FurnitureItemsResult = {
  furniture: ListingItem[];
  furnitureTotal?: number;
};

class CatalogueClient {
  readonly #url = `${import.meta.env.VITE_API_URL}/api/products/furniture`;

  async getAll(): Promise<ListingItem[]> {
    const res = await fetch(this.#url, {
      credentials: "include",
    });

    if (res.status !== 200) {
      throw new Error("could not fetch furniture");
    }

    const data = (await res.json()) as FurnitureItemsResult;

    return data.furniture;
  }
}

export default CatalogueClient;
