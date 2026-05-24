export type ListingItem = {
  title: string;
  price: number;
  city: string;
  subcategory: string;
  images: string[];
};

export type FurnitureCategory = {
  category: string;
  subcategories: string[];
};
