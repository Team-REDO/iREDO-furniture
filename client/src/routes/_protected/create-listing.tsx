import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/_protected/create-listing')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/_protected/create-listing"!</div>
}
