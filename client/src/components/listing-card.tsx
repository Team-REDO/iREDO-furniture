import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardAction, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Carousel, CarouselContent, CarouselItem, useCarousel } from "@/components/ui/carousel";
import { ChevronLeftIcon, ChevronRightIcon } from "lucide-react";

const listingImages = [
  "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=1200&q=80",
  "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?auto=format&fit=crop&w=1200&q=80",
  "https://images.unsplash.com/photo-1519710164239-da123dc03ef4?auto=format&fit=crop&w=1200&q=80",
  "https://images.unsplash.com/photo-1493666438817-866a91353ca9?auto=format&fit=crop&w=1200&q=80",
];

function ListingCarouselControls() {
  const { scrollPrev, scrollNext } = useCarousel();

  return (
    <div className="pointer-events-none absolute inset-0 z-30 flex items-center justify-between px-2">
      <Button
        type="button"
        size="icon"
        variant="secondary"
        className="pointer-events-auto h-10 w-10 rounded-full border-0 bg-black/55 text-white hover:bg-black/70"
        onPointerDown={(event) => {
          event.preventDefault();
          event.stopPropagation();
        }}
        onClick={(event) => {
          event.preventDefault();
          event.stopPropagation();
          scrollPrev();
        }}
      >
        <ChevronLeftIcon className="h-5 w-5" />
        <span className="sr-only">Previous image</span>
      </Button>

      <Button
        type="button"
        size="icon"
        variant="secondary"
        className="pointer-events-auto h-10 w-10 rounded-full border-0 bg-black/55 text-white hover:bg-black/70"
        onPointerDown={(event) => {
          event.preventDefault();
          event.stopPropagation();
        }}
        onClick={(event) => {
          event.preventDefault();
          event.stopPropagation();
          scrollNext();
        }}
      >
        <ChevronRightIcon className="h-5 w-5" />
        <span className="sr-only">Next image</span>
      </Button>
    </div>
  );
}

export function ListingCard() {
  return (
    <Card className="relative mx-auto w-full max-w-sm pt-0">
      <div className="relative">
        <Carousel className="w-full select-none touch-pan-y" opts={{ loop: true, direction: "ltr", dragFree: false, skipSnaps: false }}>
          <CarouselContent className="ml-0">
            {listingImages.map((src, index) => (
              <CarouselItem key={src} className="pl-0">
                <div className="relative aspect-video w-full overflow-hidden">
                  <div className="absolute inset-0 z-10 bg-black/35" />
                  <img src={src} alt={`Listing image ${index + 1}`} className="relative z-0 h-full w-full object-cover" draggable={false} />
                </div>
              </CarouselItem>
            ))}
          </CarouselContent>
          <ListingCarouselControls />
        </Carousel>
      </div>
      <CardHeader>
        <CardAction>
          <Badge variant="secondary">Featured</Badge>
        </CardAction>
        <CardTitle>Design systems meetup</CardTitle>
        <CardDescription>A practical talk on component APIs, accessibility, and shipping faster.</CardDescription>
      </CardHeader>
      <CardFooter>
        <Button className="w-full">View Event</Button>
      </CardFooter>
    </Card>
  );
}
