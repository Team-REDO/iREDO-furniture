import { LoginCard } from '@/components/login-card'
import { createFileRoute } from '@tanstack/react-router'



export const Route = createFileRoute('/login-page')({
  component: RouteComponent,
})

function RouteComponent() {
  return <> 
  <LoginCard/>

    </>
    
}


