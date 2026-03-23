import { SidebarTrigger } from '@/components/ui/sidebar'

export function Header() {
  return (
    <header className="w-full">
      {/* <header className="w-full bg-muted/30"> */}

      <div className="w-full px-4 flex justify-between items-center">
        <div className="flex items-center gap-2">
          <SidebarTrigger />
          <h1 className="">Furniture</h1>
        </div>
      </div>
    </header>
  );
}
