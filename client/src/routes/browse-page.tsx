import { createFileRoute } from '@tanstack/react-router'
import { SidebarTrigger } from '@/components/ui/sidebar'
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from '@/components/ui/breadcrumb'
import { Separator } from '@/components/ui/separator'
import { ListingCard } from '@/components/listing-card'

export const Route = createFileRoute('/browse-page')({
  component: RouteComponent,
})

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

      <div className="flex flex-1 flex-col gap-4 pt-0">
        <div className="grid auto-rows-min gap-4 md:grid-cols-3">
          <ListingCard />
          <ListingCard />
          <ListingCard />
        </div>
        <div className="min-h-[60vh] flex-1 rounded-xl bg-muted/50 md:min-h-min" />
      </div>
    </div>
  );
}
