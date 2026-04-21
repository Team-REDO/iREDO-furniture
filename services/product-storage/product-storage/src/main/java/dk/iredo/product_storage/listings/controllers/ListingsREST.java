package dk.iredo.product_storage.listings.controllers;

import dk.iredo.product_storage.listings.services.ListingsService;
import dk.iredo.product_storage.listings.dtos.ListingDto;
import dk.iredo.product_storage.listings.dtos.ListingDetailsDto;
import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.entities.ListingDetails;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping(path = ListingsREST.storage)
public class ListingsREST implements IListingsController{

    public static final String storage = "storage"; // TODO - Change or stay?

    private final ListingsService listingsService;

    private final ModelMapper modelMapper;

    public ListingsREST(ListingsService listingsService, ModelMapper modelMapper) {
        this.listingsService = listingsService;
        this.modelMapper = modelMapper;
    }

    @Override
    public ResponseEntity<ListingDto> addListings(@RequestBody ListingDetailsDto listingDetailsDto,
                                                         @PathVariable UUID listingGUID,
                                                         @PathVariable UUID personGUID){
        try {
            ListingDetails requestedDetails = modelMapper.map(listingDetailsDto, ListingDetails.class); // TODO - Dto handling here?
            Listing listing = listingsService.addListing(requestedDetails,
                    listingGUID, personGUID,
                    listingDetailsDto.getColorHRefs(), listingDetailsDto.getSubcategoryNames()
            );
            return ResponseEntity.ok(modelMapper.map(listing, ListingDto.class));
        }catch (CloneNotSupportedException e) {
            return ResponseEntity.badRequest().build();
        }
    }

}
