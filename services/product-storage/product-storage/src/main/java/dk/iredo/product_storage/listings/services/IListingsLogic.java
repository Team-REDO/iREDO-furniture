package dk.iredo.product_storage.listings.services;

import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.entities.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.annotation.Nullable;
import org.antlr.v4.runtime.misc.NotNull;

import java.util.List;
import java.util.UUID;

public interface IListingsLogic {
    public Listing addListing(@Nonnull ListingDetails details,
                              @Nonnull UUID listingGUID, @Nonnull UUID personGUID,
                              @Nullable List<String> colorHRefs, @Nullable List<String> subCategoryNames)
            throws CloneNotSupportedException;
}
