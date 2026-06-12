package dk.iredo.product_storage.listings.services;

import dk.iredo.product_storage.listings.dtos.ListingDto;
import jakarta.annotation.Nonnull;

public interface IListingsLogic {
    public ListingDto addListing(@Nonnull ListingDto listingDto) throws CloneNotSupportedException;
}
