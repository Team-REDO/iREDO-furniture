package dk.iredo.product_storage.listings;

import dk.iredo.product_storage.categories.SubCategory;
import dk.iredo.product_storage.categories.SubCategoryRepository;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.colors.ColorsRepository;
import dk.iredo.product_storage.images.Image;
import dk.iredo.product_storage.images.ImageRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.NoSuchElementException;

@Service
public class ListingsService {

    @Autowired
    ListingRepository listingRepository;

    @Autowired
    ListingDetailsRepository listingDetailsRepository;

    @Autowired
    ImageRepository imageRepository;

    @Autowired
    SubCategoryRepository subCategoryRepository;

    @Autowired
    ColorsRepository colorsRepository;

    public ListingDetails addListing(Listing newListing, ListingDetails newListingDetails, List<Long> subCategoriesIDs,
                              List<String> imagesUrls, List<Long> colorsIDs) throws CloneNotSupportedException {

        if(listingRepository.existsListingByGuid(newListing.getGuid()) || listingDetailsRepository.existsListingDetailsByTitle(newListingDetails.getTitel())) {
            throw new CloneNotSupportedException("Listing to be added already exist");
        }

        Listing addedListing = listingRepository.save(newListing);
        newListingDetails.setListing(addedListing);

        subCategoriesIDs.forEach(subCategoryID -> {
            SubCategory subCategory = subCategoryRepository.findById(subCategoryID).orElseThrow(NoSuchElementException::new);
            newListingDetails.addSubCategories(subCategory);
        });

        colorsIDs.forEach(colorID -> {
            Color color = colorsRepository.findById(colorID).orElseThrow(NoSuchElementException::new);
            newListingDetails.addColors(color);
        });

        ListingDetails addedListingDetails = listingDetailsRepository.save(newListingDetails);

        imagesUrls.forEach(imageUrl -> {
            Image image = new Image();
            image.setListingDetails(addedListingDetails);
            image.setUrl(imageUrl);
            imageRepository.save(image);
        });

        return listingDetailsRepository.findListingDetailsByListingGuid(newListing.getGuid()).orElseThrow(NoSuchElementException::new);
    }
}
