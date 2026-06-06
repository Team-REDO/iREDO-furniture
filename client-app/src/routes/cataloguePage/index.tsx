import { createFileRoute } from "@tanstack/react-router";
import CataloguePage from "../../pages/CataloguePage";

export const Route = createFileRoute("/cataloguePage/")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <>
      <div>Hello "/cataloguePage/"!</div>
      <CataloguePage />
    </>
  );
}
