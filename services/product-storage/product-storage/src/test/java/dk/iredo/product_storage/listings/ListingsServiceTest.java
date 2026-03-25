package dk.iredo.product_storage.listings;

import dk.iredo.product_storage.categories.*;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.colors.ColorsRepository;
import dk.iredo.product_storage.enums.Condition;
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

    static long colorRedId;
    static long subCategoryArmChairId;

    @Autowired
    ListingsService listingsService;

    @BeforeEach()
    void setup() {
        Color red = colorsRepository.save(new Color("Red", "#f44336"));
        //colorsRepository.save(new Color("White", "#ffffff"));
        //colorsRepository.save(new Color("Blue", "#2f4df7"));
        //colorsRepository.save(new Color("Green", "#2ff757"));

        colorRedId = red.getId();

        Category chairs = categoryRepository.save(new Category("Chairs"));
        //Category sofas = categoryRepository.save(new Category("Sofas"));
        //Category beds = categoryRepository.save(new Category("Beds"));
        //Category tables = categoryRepository.save(new Category("Tables"));

        List<SubCategory> subCategories = new ArrayList<>();
        subCategories.add(new SubCategory("Arm chair", chairs));
        //subCategories.add(new SubCategory("Lounge chair", chairs));
        //subCategories.add(new SubCategory("Modular sofa", sofas));
        //subCategories.add(new SubCategory("Dream bed", beds));
        //subCategories.add(new SubCategory("Dinner table", tables));

        subCategoryRepository.saveAll(subCategories);
        subCategoryArmChairId = subCategoryRepository.findIDBySubCategoryName("Arm chair");
    }

    @AfterEach
    void tearDownEach() {
    }

    @Test
    void testPersistOfListingInAddListing() {
        //Arrange
        UUID personGuid = UUID.randomUUID(); //TODO - Not good practise?
        UUID listingGuid = UUID.randomUUID(); //TODO - Not good practise?
        Listing listing = new Listing(listingGuid, personGuid);

        String title = "For sale";
        String description = "Two red chairs for sale.";
        int x_length_in_mm = 900;
        int y_width_in_mm = 900;
        int z_height_in_mm = 1200;
        Condition condition = Condition.Used;
        int quantity = 2;
        BigDecimal priceDKK = BigDecimal.valueOf(100.00);
        String city = "Hellerup";

        ListingDetails listingDetails = new ListingDetails(listing, title, description, x_length_in_mm, y_width_in_mm,
                z_height_in_mm, condition, quantity, priceDKK, city);

        List<Long> subCategoryIDs = new ArrayList<>();
        subCategoryIDs.add(subCategoryArmChairId);

        List<Long> colorIDs = new ArrayList<>();
        colorIDs.add(colorRedId);

        //TODO Give correct example for url of image
        List<String> imagesUrls = new ArrayList<>();

        //Act
        ListingDetails actual;
        try {
            actual = listingsService.addListing(listing, listingDetails, subCategoryIDs, imagesUrls, colorIDs);
        } catch (CloneNotSupportedException e) {
            throw new RuntimeException(e);
        }

        //Assert
        Assertions.assertNotNull(actual);
        Assertions.assertNotNull(actual.getId());
        Assertions.assertNotNull(actual.getListing().getId());
        Assertions.assertEquals(actual.getListing().getGuid(), listingGuid);

    }
}