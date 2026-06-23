import { createFileRoute, Link } from "@tanstack/react-router";
import { useMemo, useState } from "react";
import { Breadcrumb, BreadcrumbItem, BreadcrumbLink, BreadcrumbList, BreadcrumbPage, BreadcrumbSeparator } from "@/components/ui/breadcrumb";
import { Separator } from "@/components/ui/separator";
import { ListingCard } from "@/components/listing-card";
import { MATERIAL_ICONS } from "@/config/material-icons";
import { cn } from "@/lib/utils";
import { furnitureItemsQueryOptions } from "@/features/furniture/queries";
import { useFurnitureItems } from "@/hooks/use-furniture";
import type { TListingItem } from "@/features/furniture/types";

type SortOption = "price-asc" | "price-desc" | "title-asc" | "title-desc";

export const Route = createFileRoute("/_public/catalogue/")({
  loader: async ({ context }) => {
    await context.queryClient.ensureQueryData(furnitureItemsQueryOptions());
  },
  component: RouteComponent,
});

function getCategory(item: TListingItem) {
  return item.categories[0]?.categoryName || "Uncategorized";
}

function getSubcategory(item: TListingItem) {
  return item.categories[0]?.subcategories[0]?.subcategoryName || "Uncategorized";
}

function RouteComponent() {
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [selectedSubcategory, setSelectedSubcategory] = useState<string | null>(null);
  const [sortBy, setSortBy] = useState<SortOption>("price-asc");

  const { data: furnitureItems = [] } = useFurnitureItems();

  const furnitureCategories = useMemo(() => {
    const categoryMap = new Map<string, Set<string>>();

    for (const item of furnitureItems) {
      const categoryName = getCategory(item);
      const subcategoryName = getSubcategory(item);

      if (!categoryMap.has(categoryName)) {
        categoryMap.set(categoryName, new Set());
      }

      categoryMap.get(categoryName)?.add(subcategoryName);
    }

    return Array.from(categoryMap.entries()).map(([category, subcategories]) => ({
      category,
      subcategories: Array.from(subcategories),
    }));
  }, [furnitureItems]);

  const selectedCategoryData = useMemo(() => furnitureCategories.find((category) => category.category === selectedCategory) ?? null, [furnitureCategories, selectedCategory]);

  const handleCategoryClick = (category: string) => {
    setSelectedCategory((current) => (current === category ? null : category));
    setSelectedSubcategory(null);
  };

  const filteredItems = useMemo(() => {
    if (!selectedCategoryData) return furnitureItems;

    const categoryFiltered = furnitureItems.filter((item) => getCategory(item) === selectedCategoryData.category);

    if (!selectedSubcategory) return categoryFiltered;
    return categoryFiltered.filter((item) => getSubcategory(item) === selectedSubcategory);
  }, [furnitureItems, selectedCategoryData, selectedSubcategory]);

  const sortedItems = useMemo(() => {
    const items = [...filteredItems];

    switch (sortBy) {
      case "price-asc":
        return items.sort((a, b) => a.price - b.price);
      case "price-desc":
        return items.sort((a, b) => b.price - a.price);
      case "title-asc":
        return items.sort((a, b) => a.title.localeCompare(b.title));
      case "title-desc":
        return items.sort((a, b) => b.title.localeCompare(a.title));
      default:
        return items;
    }
  }, [filteredItems, sortBy]);

  return (
    <div>
      <header className="mb-4 flex h-8 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
        <div className="flex items-center gap-2 px-1">
          <Separator orientation="vertical" className="ml-3 data-[orientation=vertical]:h-8" />
          <Separator orientation="vertical" className="mr-2 data-[orientation=vertical]:h-8" />
          <Breadcrumb>
            <BreadcrumbList>
              <BreadcrumbItem className="hidden md:block">
                <BreadcrumbLink href="#">Catalogue</BreadcrumbLink>
              </BreadcrumbItem>
              <BreadcrumbSeparator className="hidden md:block" />
              <BreadcrumbItem>
                <BreadcrumbPage>Furniture</BreadcrumbPage>
              </BreadcrumbItem>
            </BreadcrumbList>
          </Breadcrumb>
        </div>
      </header>

      <div className="flex flex-1 flex-col gap-4 px-2 py-2 sm:px-3 lg:px-4">
        <section className="rounded-xl p-3 sm:p-4">
          <div className="grid grid-cols-[repeat(auto-fit,minmax(9rem,1fr))] gap-2 sm:gap-3">
            {furnitureCategories.map((item, index) => {
              const iconName = MATERIAL_ICONS.categoryByName[item.category] ?? MATERIAL_ICONS.fallbackCycle[index % MATERIAL_ICONS.fallbackCycle.length];
              const isActive = selectedCategory === item.category;

              return (
                <button
                  key={item.category}
                  type="button"
                  aria-pressed={isActive}
                  onClick={() => handleCategoryClick(item.category)}
                  className={cn(
                    "flex w-full flex-col items-center justify-center rounded-lg border px-2 py-4 text-center transition-colors",
                    isActive ? "border-primary bg-primary/10 text-primary" : "border-transparent hover:bg-accent",
                  )}
                >
                  <span className={`${MATERIAL_ICONS.categoryClassName} ${MATERIAL_ICONS.categorySizeClassName}`} style={MATERIAL_ICONS.symbolStyle} aria-hidden="true">
                    {iconName}
                  </span>
                  <span className="text-sm font-medium">{item.category}</span>
                </button>
              );
            })}
          </div>
        </section>

        {selectedCategoryData && (
          <section className="rounded-lg bg-background/40 px-3 pb-3">
            <div className="mb-2 text-sm font-medium">Subcategories in {selectedCategoryData.category}</div>
            <div className="flex flex-wrap gap-2">
              <button
                type="button"
                onClick={() => setSelectedSubcategory(null)}
                className={cn(
                  "rounded-full border px-3 py-1.5 text-sm transition-colors",
                  selectedSubcategory === null ? "border-primary bg-primary/10 text-primary" : "border-border bg-background hover:bg-accent",
                )}
              >
                All
              </button>

              {selectedCategoryData.subcategories.map((subcategory) => (
                <button
                  key={subcategory}
                  type="button"
                  onClick={() => setSelectedSubcategory(subcategory)}
                  className={cn(
                    "rounded-full border px-3 py-1.5 text-sm transition-colors",
                    selectedSubcategory === subcategory ? "border-primary bg-primary/10 text-primary" : "border-border bg-background hover:bg-accent",
                  )}
                >
                  {subcategory}
                </button>
              ))}
            </div>
          </section>
        )}

        <div className="flex items-center justify-between px-1">
          <p className="text-sm text-muted-foreground">{sortedItems.length} items</p>
          <label className="flex items-center gap-2 text-sm">
            <span className="text-muted-foreground">Sort by</span>
            <select value={sortBy} onChange={(event) => setSortBy(event.target.value as SortOption)} className="rounded-md border border-border bg-background px-2 py-1 text-sm">
              <option value="price-asc">Price: low to high</option>
              <option value="price-desc">Price: high to low</option>
              <option value="title-asc">Title: A-Z</option>
              <option value="title-desc">Title: Z-A</option>
            </select>
          </label>
        </div>
        <div className="grid auto-rows-min grid-cols-[repeat(auto-fit,minmax(18rem,1fr))] gap-3 sm:gap-4 xl:gap-6">
          {sortedItems.map((item) => (
            <Link key={item.salesPostGuid} to="/catalogue/$salesPostGuid" params={{ salesPostGuid: item.salesPostGuid }} className="block">
              <ListingCard {...item} />
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
}
