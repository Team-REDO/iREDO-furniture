import { createFileRoute } from "@tanstack/react-router";
import { useMemo, useState } from "react";
import { Breadcrumb, BreadcrumbItem, BreadcrumbLink, BreadcrumbList, BreadcrumbPage, BreadcrumbSeparator } from "@/components/ui/breadcrumb";
import { Separator } from "@/components/ui/separator";
import { ListingCard, type ListingItem } from "@/components/listing-card";
import furnitureItemsRaw from "@/data/furniture-items.json?raw";
import furnitureCategoriesRaw from "@/data/furniture-categories.json?raw";
import { MATERIAL_ICONS } from "@/config/material-icons";
import { cn } from "@/lib/utils";

const furnitureItems = JSON.parse(furnitureItemsRaw) as ListingItem[];
type FurnitureCategory = { category: string; subcategories: string[] };
const furnitureCategories = JSON.parse(furnitureCategoriesRaw) as FurnitureCategory[];
type SortOption = "price-asc" | "price-desc" | "city-asc" | "city-desc";

export const Route = createFileRoute("/browse-page")({
  component: RouteComponent,
});

function RouteComponent() {
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [selectedSubcategory, setSelectedSubcategory] = useState<string | null>(null);
  const [sortBy, setSortBy] = useState<SortOption>("price-asc");

  const selectedCategoryData = useMemo(() => furnitureCategories.find((category) => category.category === selectedCategory) ?? null, [selectedCategory]);

  const handleCategoryClick = (category: string) => {
    setSelectedCategory((current) => (current === category ? null : category));
    setSelectedSubcategory(null);
  };

  const filteredItems = useMemo(() => {
    if (!selectedCategoryData) return furnitureItems;

    const allowedSubcategories = new Set(selectedCategoryData.subcategories);
    const categoryFiltered = furnitureItems.filter((item) => allowedSubcategories.has(item.subcategory));

    if (!selectedSubcategory) return categoryFiltered;
    return categoryFiltered.filter((item) => item.subcategory === selectedSubcategory);
  }, [selectedCategoryData, selectedSubcategory]);

  const sortedItems = useMemo(() => {
    const items = [...filteredItems];

    switch (sortBy) {
      case "price-asc":
        return items.sort((a, b) => a.price - b.price);
      case "price-desc":
        return items.sort((a, b) => b.price - a.price);
      case "city-asc":
        return items.sort((a, b) => a.city.localeCompare(b.city));
      case "city-desc":
        return items.sort((a, b) => b.city.localeCompare(a.city));
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
                <BreadcrumbLink href="#">Build Your Application</BreadcrumbLink>
              </BreadcrumbItem>
              <BreadcrumbSeparator className="hidden md:block" />
              <BreadcrumbItem>
                <BreadcrumbPage>Data Fetching</BreadcrumbPage>
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
              <option value="city-asc">City: A-Z</option>
              <option value="city-desc">City: Z-A</option>
            </select>
          </label>
        </div>

        <div className="grid auto-rows-min grid-cols-[repeat(auto-fit,minmax(18rem,1fr))] gap-3 sm:gap-4 xl:gap-6">
          {sortedItems.map((item) => (
            <ListingCard key={`${item.title}-${item.city}`} {...item} />
          ))}
        </div>
        <div className="min-h-[60vh] flex-1 rounded-xl bg-muted/50 md:min-h-min" />
      </div>
    </div>
  );
}
