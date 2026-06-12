package dk.iredo.product_storage.listings.controllers;

import dk.iredo.product_storage.listings.dtos.ListingDto;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

import java.util.UUID;

public interface IListingsController {

    @PostMapping("/persons/{personGUID}/listings/{listingGUID}")
    public ResponseEntity<ListingDto> addListings(@RequestBody ListingDto listingDto,
                                                  @PathVariable UUID listingGUID,
                                                  @PathVariable UUID personGUID);
}
