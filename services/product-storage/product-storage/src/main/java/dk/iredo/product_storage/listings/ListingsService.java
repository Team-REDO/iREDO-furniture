package dk.iredo.product_storage.listings;

import dk.iredo.product_storage.categories.entities.SubCategory;
import dk.iredo.product_storage.categories.repositories.SubCategoryRepository;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.colors.ColorsRepository;
import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.repositories.ListingRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.xml.transform.Source;
import java.sql.SQLOutput;
import java.util.ArrayList;
import java.util.List;

@Service
public class ListingsService {

    @Autowired
    ListingRepository listingRepository;

    @Autowired
    SubCategoryRepository subCategoryRepository;

    @Autowired
    ColorsRepository colorsRepository;

    public Listing addListing(Listing listing) throws CloneNotSupportedException {

        List<SubCategory> subCategories = new ArrayList<>();
        List<Color> colors = new ArrayList<>();

        if(listingRepository.existsListingByGuid(listing.getGuid())) {
            throw new CloneNotSupportedException("Listing to be added already exist");
        }

        listing.getListingDetails().getSubCategories().forEach(subCategoryRequested -> {
            subCategories.add(subCategoryRepository.findSubCategoryByName(subCategoryRequested.getName()));
        });

        listing.getListingDetails().getColors().forEach(colorRequested -> {
            colors.add(colorsRepository.findByHref(colorRequested.getHref()));
        });

        listing.getListingDetails().setSubCategories(subCategories);
        listing.getListingDetails().setColors(colors);
        return listingRepository.save(listing);
    }
}
/* TODO - Make this logic somewhere else:
           listingDTO.getImageUrls.forEach(imageUrl -> {
            Image image = new Image(listing);
            image.setUrl(imageUrl);
            imageRepository.save(image);
        });*/