import type { ReactNode } from 'react'
import { ThemeProvider } from './components/theme-provider'
import { ModeToggle } from './components/mode-toggle'



function App({ children }: { children?: ReactNode }) {

  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
   
   {children}
      <ModeToggle />
 
   
    </ThemeProvider>
  )
}

export default App
