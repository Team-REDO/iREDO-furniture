import { createFileRoute } from '@tanstack/react-router'

// to create a page where admin can manage all listings

export const Route = createFileRoute('/_protected/_admin/listings')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/_protected/_admin/listings"!</div>
}
