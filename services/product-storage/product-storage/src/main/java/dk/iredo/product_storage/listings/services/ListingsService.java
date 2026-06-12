package dk.iredo.product_storage.listings.services;

import dk.iredo.product_storage.categories.repositories.SubCategoryRepository;
import dk.iredo.product_storage.colors.ColorsRepository;
import dk.iredo.product_storage.listings.dtos.ListingDto;
import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.repositories.ListingRepository;
import jakarta.annotation.Nonnull;
import lombok.Getter;
import org.jspecify.annotations.NonNull;
import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;

import java.util.EmptyStackException;
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
     * @return {@link dk.iredo.product_storage.listings.entities.Listing}
     */
    @Override
    public ListingDto addListing(@NonNull ListingDto listingDto) throws CloneNotSupportedException
    {
        if(listingRepository.existsListingByGuid(listingDto.getGUID())) {
            throw new CloneNotSupportedException("Listing to be added already exist");
        }
        else if(listingDto.getSubCategories() == null ||listingDto.getSubCategories().isEmpty()) {
            throw new EmptyStackException();
        }
        else if(listingDto.getColors() == null || listingDto.getColors().isEmpty()) {
            throw new EmptyStackException();
        }
        else {
            Listing listing = modelMapper.map(listingDto, Listing.class);

            listingDto.getSubCategories().forEach(subCategorydto -> {
                listing.addSubCategory(
                        subCategoryRepository.findSubCategoryByName(subCategorydto.getName())
                );
            });
            listingDto.getColors().forEach(colorDto -> {
                listing.addColor(colorsRepository.findByHref(colorDto.getHref()));
            });

            Listing addedListing = listingRepository.save(listing);
            return this.modelMapper.map(addedListing, ListingDto.class);
        }
    }
}
/* TODO - Make this logic somewhere else:
           listingDTO.getImageUrls.forEach(imageUrl -> {
            Image image = new Image(listing);
            image.setUrl(imageUrl);
            imageRepository.save(image);
        });*/