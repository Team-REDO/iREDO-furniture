package dk.iredo.product_storage.listings.controllers;

import dk.iredo.product_storage.listings.ListingsService;
import dk.iredo.product_storage.listings.dtos.ListingDTO;
import dk.iredo.product_storage.listings.entities.Listing;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping(path = ListingsController.listings)
public class ListingsController implements IListingsController {

    public static final String listings = "listings";

    @Autowired
    private ListingsService listingsService;

    @Autowired
    private ModelMapper modelMapper;

    @Override
    public ResponseEntity<ListingDTO> addListings(ListingDTO listingDTO){
        try {
            Listing requestListing = modelMapper.map(listingDTO, Listing.class);
            System.out.println(requestListing.getListingDetails().getTitel() + " HEEEER");
            Listing responseListing = listingsService.addListing(requestListing);
            return ResponseEntity.ok(modelMapper.map(responseListing, ListingDTO.class)); // TODO - Dto?
        }catch (CloneNotSupportedException e) {
            return ResponseEntity.badRequest().build();
        }
    }

}
