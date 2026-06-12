package dk.iredo.product_storage.listings.controllers;

import dk.iredo.product_storage.listings.dtos.ListingDto;
import dk.iredo.product_storage.listings.services.ListingsService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping(path = ListingsREST.storage)
public class ListingsREST implements IListingsController{

    public static final String storage = "storage";

    private final ListingsService listingsService;

    public ListingsREST(ListingsService listingsService) {
        this.listingsService = listingsService;
    }

    @Override
    public ResponseEntity<ListingDto> addListings(@RequestBody ListingDto listingDto,
                                                         @PathVariable UUID listingGUID,
                                                         @PathVariable UUID personGUID){
        try {
            ListingDto newListingDto = this.listingsService.addListing(listingDto);
            return ResponseEntity.ok(newListingDto);
        }catch (CloneNotSupportedException e) {
            return ResponseEntity.badRequest().build();
        }
    }

}
