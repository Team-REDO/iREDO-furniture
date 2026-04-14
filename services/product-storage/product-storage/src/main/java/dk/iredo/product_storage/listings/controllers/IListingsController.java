package dk.iredo.product_storage.listings.controllers;

import dk.iredo.product_storage.listings.dtos.ListingDTO;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

public interface IListingsController {

    @PostMapping("/listing")
    public ResponseEntity<ListingDTO> addListings(@RequestBody ListingDTO listingDTO) throws Exception;
}
