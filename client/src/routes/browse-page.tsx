import { createFileRoute } from "@tanstack/react-router";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { Breadcrumb, BreadcrumbItem, BreadcrumbLink, BreadcrumbList, BreadcrumbPage, BreadcrumbSeparator } from "@/components/ui/breadcrumb";
import { Separator } from "@/components/ui/separator";
import { ListingCard, type ListingItem } from "@/components/listing-card";
import furnitureItemsRaw from "@/data/furniture-items.json?raw";

const furnitureItems = JSON.parse(furnitureItemsRaw) as ListingItem[];

export const Route = createFileRoute("/browse-page")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div>
      <header className="mb-4 flex h-8 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
        <div className="flex items-center gap-2 px-1">
          <Separator orientation="vertical" className="ml-3 data-[orientation=vertical]:h-8" />
          <SidebarTrigger className="-ml-0" />
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
        <div className="grid auto-rows-min grid-cols-[repeat(auto-fit,minmax(18rem,1fr))] gap-3 sm:gap-4 xl:gap-6">
          {furnitureItems.map((item) => (
            <ListingCard key={`${item.title}-${item.city}`} {...item} />
          ))}
        </div>
        <div className="min-h-[60vh] flex-1 rounded-xl bg-muted/50 md:min-h-min" />
      </div>
    </div>
  );
}
