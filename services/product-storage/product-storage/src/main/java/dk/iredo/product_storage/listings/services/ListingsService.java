package dk.iredo.product_storage.listings.services;

import dk.iredo.product_storage.categories.repositories.SubCategoryRepository;
import dk.iredo.product_storage.colors.ColorsRepository;
import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.entities.ListingDetails;
import dk.iredo.product_storage.listings.repositories.ListingRepository;
import jakarta.annotation.Nonnull;
import org.jspecify.annotations.NonNull;
import org.jspecify.annotations.Nullable;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class ListingsService implements IListingsLogic{

    private final ListingRepository listingRepository;

    private final SubCategoryRepository subCategoryRepository;

    private final ColorsRepository colorsRepository;

    public ListingsService(ListingRepository listingRepository, SubCategoryRepository subCategoryRepository, ColorsRepository colorsRepository) {
        this.listingRepository = listingRepository;
        this.subCategoryRepository = subCategoryRepository;
        this.colorsRepository = colorsRepository;
    }

    /**
     * @param details
     * @param listingGUID
     * @param personGUID
     * @param colorHRefs
     * @param subCategoryNames
     * @return {@link dk.iredo.product_storage.listings.entities.Listing}
     * @throws CloneNotSupportedException
     */
    @Override
    public Listing addListing(@NonNull ListingDetails details,
                              @Nonnull UUID listingGUID, @Nonnull UUID personGUID,
                              @Nullable List<String> colorHRefs, @Nullable List<String> subCategoryNames
    ) throws CloneNotSupportedException
    {
        if(listingRepository.existsListingByGuid(listingGUID)) {
            throw new CloneNotSupportedException("Listing to be added already exist");
        }
        else {
            Listing listing = new Listing(listingGUID, personGUID, details);

            if(subCategoryNames == null) { /* Do nothing */ }
            else {
                subCategoryNames.forEach(subCategoryName -> {
                    details.addSubCategory(
                            subCategoryRepository.findSubCategoryByName(subCategoryName)
                    );
                });
            }

            if(colorHRefs == null) { /* Do nothing */ }
            else {
                colorHRefs.forEach(colorHRef -> {
                    details.addColor(colorsRepository.findByHref(colorHRef));
                });
            }
            return listingRepository.save(listing);
        }
    }
}
/* TODO - Make this logic somewhere else:
           listingDTO.getImageUrls.forEach(imageUrl -> {
            Image image = new Image(listing);
            image.setUrl(imageUrl);
            imageRepository.save(image);
        });*/