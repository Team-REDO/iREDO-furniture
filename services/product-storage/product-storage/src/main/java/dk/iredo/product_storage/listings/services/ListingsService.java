package dk.iredo.product_storage.listings.services;

import dk.iredo.product_storage.categories.entities.SubCategory;
import dk.iredo.product_storage.categories.repositories.SubCategoryRepository;
import dk.iredo.product_storage.colors.ColorsRepository;
import dk.iredo.product_storage.listings.dtos.DetailsDto;
import dk.iredo.product_storage.listings.dtos.ListingDto;
import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.entities.ListingDetails;
import dk.iredo.product_storage.listings.repositories.ListingRepository;
import jakarta.annotation.Nonnull;
import lombok.Getter;
import org.jspecify.annotations.NonNull;
import org.jspecify.annotations.Nullable;
import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.time.*;
import java.util.EmptyStackException;
import java.util.List;
import java.util.UUID;

@Service
public class ListingsService implements IListingsLogic{

    private final ListingRepository listingRepository;

    private final SubCategoryRepository subCategoryRepository;

    private final ColorsRepository colorsRepository;

    @Getter
    private final ModelMapper modelMapper;

    public ListingsService(ListingRepository listingRepository,
                           SubCategoryRepository subCategoryRepository,
                           ColorsRepository colorsRepository,
                           ModelMapper modelMapper
    ) {
        this.listingRepository = listingRepository;
        this.subCategoryRepository = subCategoryRepository;
        this.colorsRepository = colorsRepository;
        this.modelMapper = modelMapper;
    }

    /**
     * @param details
     * @param listingGUID
     * @param personGUID
     * @return {@link dk.iredo.product_storage.listings.entities.Listing}
     * @throws CloneNotSupportedException
     */
    @Override
    public Listing addListing(@NonNull DetailsDto details,
                                 @Nonnull UUID listingGUID, @Nonnull UUID personGUID
    ) throws CloneNotSupportedException
    {
        if(listingRepository.existsListingByGuid(listingGUID)) {
            throw new CloneNotSupportedException("Listing to be added already exist");
        }
        else if(details.getSubCategoryNames() == null ||details.getSubCategoryNames().isEmpty()) {
            throw new EmptyStackException();
        }
        else if(details.getColorHRefs() == null || details.getColorHRefs().isEmpty()) {
            throw new EmptyStackException();
        }
        else {
            ListingDetails ListlistingDetails = modelMapper.map(details, ListingDetails.class);
            details.getSubCategoryNames().forEach(subCategoryName -> {
                System.out.println(subCategoryName);
                System.out.println(subCategoryRepository.findSubCategoryByName(subCategoryName));
                ListlistingDetails.addSubCategory(
                        subCategoryRepository.findSubCategoryByName(subCategoryName)
                );
            });

            details.getColorHRefs().forEach(colorHRef -> {
                ListlistingDetails.addColor(colorsRepository.findByHref(colorHRef));
            });

            Listing listing = new Listing(listingGUID, personGUID, ListlistingDetails);
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