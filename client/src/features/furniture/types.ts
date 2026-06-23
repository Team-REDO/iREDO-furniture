export type TListingItem = {
  id: string;
  salesPostGuid: string;
  personGuid: string;
  title: string;
  description: string;
  size: string;
  quantity: number;
  price: number;
  condition: string;
  city: string;
  modifiedAt: string;
  colors: Array<{
    name: string;
    href: string;
  }>;
  categories: Array<{
    categoryName: string;
    subcategories: Array<{
      subcategoryName: string;
    }>;
  }>;
  images: Array<{
    imageUrl: string;
  }>;
};

// export type ListingItem = {
//   title: string;
//   price: number;
//   city: string;
//   category: string;
//   subcategory: string;
//   images: string[];
// };
