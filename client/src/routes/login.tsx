import { LoginCard } from '@/components/login-card'
import { createFileRoute } from '@tanstack/react-router'



export const Route = createFileRoute('/login')({
  component: RouteComponent,
})

function RouteComponent() {
  return <> 
  <LoginCard/>

    </>
    
}


