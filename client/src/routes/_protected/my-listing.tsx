import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/_protected/my-listing')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/_protected/my-listing"!</div>
}
