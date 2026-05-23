package dk.iredo.product_storage.listings.services;

import dk.iredo.product_storage.listings.dtos.DetailsDto;
import dk.iredo.product_storage.listings.dtos.ListingDto;
import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.entities.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.annotation.Nullable;
import org.antlr.v4.runtime.misc.NotNull;

import java.util.List;
import java.util.UUID;

public interface IListingsLogic {
    public Listing addListing(@Nonnull DetailsDto details,
                                 @Nonnull UUID listingGUID, @Nonnull UUID personGUID)
            throws CloneNotSupportedException;
}
