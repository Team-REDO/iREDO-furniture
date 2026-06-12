package dk.iredo.product_storage.listings.dtos;

import dk.iredo.product_storage.listings.enums.Condition;
import lombok.*;
import lombok.experimental.Accessors;

import java.io.Serializable;
import java.math.BigDecimal;
import java.sql.Date;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

/**
 * DTO for {@link dk.iredo.product_storage.listings.entities.Listing}
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
@ToString
@Accessors(chain = true)
public class ListingDto implements Serializable {
    private Long id;
    private UUID GUID;
    private UUID personGUID;
    private Condition condition;
    private DetailsDto listingDetails;
    private List<ColorDto> colors = new ArrayList<>();
    private List<SubCategoryDto> subCategories = new ArrayList<>();

    public ListingDto(UUID GUID, UUID personGUID) {
        this.GUID = GUID;
        this.personGUID = personGUID;
    }

    /**
     * DTO for {@link dk.iredo.product_storage.listings.entities.ListingDetails}
     */
    @AllArgsConstructor
    @NoArgsConstructor
    @Getter
    @Setter
    @ToString
    @Accessors(chain = true)
    public static class DetailsDto implements Serializable {
        private Long id;
        private String title;
        private String description;
        private Integer x_length_in_mm;
        private Integer y_width_in_mm;
        private Integer z_height_in_mm;
        private int quantity;
        private BigDecimal price_dkk;
        private String city;
        private Date modified_date;
    }

    /**
     * DTO for {@link dk.iredo.product_storage.colors.Color}
     */
    @AllArgsConstructor
    @NoArgsConstructor
    @Getter
    @Setter
    @ToString
    @Accessors(chain = true)
    public static class ColorDto implements Serializable {
        private Long id;
        private String name;
        private String href;
    }

    /**
     * DTO for {@link dk.iredo.product_storage.categories.entities.SubCategory}
     */
    @AllArgsConstructor
    @NoArgsConstructor
    @Getter
    @Setter
    @ToString
    @Accessors(chain = true)
    public static class SubCategoryDto implements Serializable {
        private Long id;
        private String name;
        private CategoryDto category;

        /**
         * DTO for {@link dk.iredo.product_storage.categories.entities.Category}
         */
        @AllArgsConstructor
        @NoArgsConstructor
        @Getter
        @Setter
        @ToString
        @Accessors(chain = true)
        public static class CategoryDto implements Serializable {
            private Long id;
            private String name;
        }
    }
}