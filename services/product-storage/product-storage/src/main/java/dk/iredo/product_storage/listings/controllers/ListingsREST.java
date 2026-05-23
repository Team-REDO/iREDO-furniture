package dk.iredo.product_storage.listings.controllers;

import dk.iredo.product_storage.listings.services.ListingsService;
import dk.iredo.product_storage.listings.dtos.ListingDto;
import dk.iredo.product_storage.listings.dtos.DetailsDto;
import dk.iredo.product_storage.listings.entities.Listing;
import lombok.Getter;
import org.modelmapper.ModelMapper;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping(path = ListingsREST.storage)
public class ListingsREST implements IListingsController{

    public static final String storage = "storage"; // TODO - Change or stay?

    private final ListingsService listingsService;

    public ListingsREST(ListingsService listingsService) {
        this.listingsService = listingsService;
    }

    @Override
    public ResponseEntity<ListingDto> addListings(@RequestBody DetailsDto detailsDto,
                                                         @PathVariable UUID listingGUID,
                                                         @PathVariable UUID personGUID){
        try {
            //TODO - DTO Handling???? WHERE ??
            Listing listing = this.listingsService.addListing(detailsDto,
                    listingGUID, personGUID
            );
            ListingDto listingDto = this.listingsService.getModelMapper().map(listing, ListingDto.class);
            return ResponseEntity.ok(listingDto);
        }catch (CloneNotSupportedException e) {
            return ResponseEntity.badRequest().build();
        }
    }

}
