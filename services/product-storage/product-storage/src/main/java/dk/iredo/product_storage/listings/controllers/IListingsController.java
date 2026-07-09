package dk.iredo.product_storage.listings.controllers;

import dk.iredo.product_storage.listings.dtos.DetailsDto;
import dk.iredo.product_storage.listings.dtos.ListingDto;
import dk.iredo.product_storage.listings.entities.Listing;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

import java.util.UUID;

public interface IListingsController {

    @PostMapping("/persons/{personGUID}/listings/{listingGUID}") // TODO Change or stay???
    public ResponseEntity<ListingDto> addListings(@RequestBody DetailsDto listingDetailsDTO,
                                                  @PathVariable UUID listingGUID,
                                                  @PathVariable UUID personGUID);
}
