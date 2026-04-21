package dk.iredo.product_storage.listings;

import dk.iredo.product_storage.categories.entities.Category;
import dk.iredo.product_storage.categories.entities.SubCategory;
import dk.iredo.product_storage.categories.repositories.CategoryRepository;
import dk.iredo.product_storage.categories.repositories.SubCategoryRepository;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.colors.ColorsRepository;
import dk.iredo.product_storage.images.ImageRepository;
import dk.iredo.product_storage.listings.entities.ListingDetails;
import dk.iredo.product_storage.listings.enums.Condition;
import dk.iredo.product_storage.listings.entities.Listing;
import dk.iredo.product_storage.listings.services.ListingsService;
import org.junit.jupiter.api.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.TestPropertySource;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@SpringBootTest
@TestPropertySource(
        locations = "classpath:testApplication.properties")
class ListingsServiceTest {

    @Autowired
    ColorsRepository colorsRepository;

    @Autowired
    CategoryRepository categoryRepository;

    @Autowired
    SubCategoryRepository subCategoryRepository;

    @Autowired
    ListingsService listingsService;

    @Autowired
    ImageRepository imageRepository;

    @BeforeEach()
    void setup() {
        /*Examples*/
        //colorsRepository.save(new Color("White", "#ffffff"));
        //colorsRepository.save(new Color("Blue", "#2f4df7"));
        //colorsRepository.save(new Color("Green", "#2ff757"));

        //Category sofas = categoryRepository.save(new Category("Sofas"));
        //Category beds = categoryRepository.save(new Category("Beds"));
        //Category tables = categoryRepository.save(new Category("Tables"));


        //subCategories.add(new SubCategory("Lounge chair", chairs));
        //subCategories.add(new SubCategory("Modular sofa", sofas));
        //subCategories.add(new SubCategory("Dream bed", beds));
        //subCategories.add(new SubCategory("Dinner table", tables));
    }

    @Test
    void testAddingFullListing() {
        /*Arrange*/
        Color colorDB = colorsRepository.save(new Color("Red", "#f44336"));
        Category categoryDB = categoryRepository.save(new Category("Chairs"));
        SubCategory subCategoryDB = subCategoryRepository.save(new SubCategory("Lounge chair", categoryDB));

        UUID personGuid = UUID.randomUUID(); //TODO - Not good practise?
        UUID listingGuid = UUID.randomUUID(); //TODO - Not good practise?

        String title = "For sale";
        String description = "Two red chairs for sale.";
        int x_length_in_mm = 900;
        int y_width_in_mm = 900;
        int z_height_in_mm = 1200;
        Condition condition = Condition.Used;
        int quantity = 2;
        BigDecimal priceDKK = BigDecimal.valueOf(100.00);
        String city = "Hellerup";

        List<String> subCategoryNames = new ArrayList<>();
        subCategoryNames.add("Lounge chair");

        List<String> colorHRefs = new ArrayList<>();
        colorHRefs.add("#f44336");

        ListingDetails listingDetails =new ListingDetails(
                title, description, x_length_in_mm, y_width_in_mm, z_height_in_mm,
                condition, quantity, priceDKK, city
        );

        /*Act*/
        Listing actual;
        try {
            actual = listingsService.addListing(listingDetails, listingGuid, personGuid,
                    colorHRefs, subCategoryNames
            );

        /*Assert*/
            Assertions.assertNotNull(actual); //if listing core is generated in db
            Assertions.assertNotNull(actual.getId()); //if listing core is generated in db
            Assertions.assertNotNull(actual.getListingDetails().getId()); //if details is generated in db
            Assertions.assertEquals(listingGuid, actual.getGUID()); //if new listing is generated

            Assertions.assertEquals(colorDB.getId(), actual.getListingDetails().getColors().getFirst().getId());//if reference is same object
            Assertions.assertEquals(subCategoryDB.getId(), actual.getListingDetails().getSubCategories().getFirst().getId());//if reference is same object
            Assertions.assertEquals(categoryDB.getId(), actual.getListingDetails().getSubCategories().getFirst().getCategory().getId());//if reference is same object
            Assertions.assertEquals( 1, colorsRepository.count()); //if no duplicates while persisting and only 1 in db
            Assertions.assertEquals( 1, subCategoryRepository.count()); //if no duplicates while persisting and only 1 in db
            Assertions.assertEquals( 1, categoryRepository.count());//if no duplicates while persisting and only 1 in db

            Assertions.assertEquals( 0, imageRepository.count()); //if no persisting of object when no images


        } catch (CloneNotSupportedException e) {
            System.out.println("Listing is duplicated: " + e.getMessage());
        }
    }
}