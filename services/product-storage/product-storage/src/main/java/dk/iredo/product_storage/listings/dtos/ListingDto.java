package dk.iredo.product_storage.listings.dtos;

import dk.iredo.product_storage.listings.enums.Condition;
import lombok.Data;
import lombok.Getter;

import java.math.BigDecimal;
import java.sql.Date;
import java.util.List;
import java.util.Objects;
import java.util.UUID;

//TODO split up?
//TODO Keep VS. Changes VS. Delete ???

/**
 * DTO for {@link dk.iredo.product_storage.listings.entities.Listing}
 */
@Data
public class ListingDto {
    private UUID guid;
    private UUID personGUID;
    private ListingDetailsDto listingDetails; //TODO - Keep?

    public ListingDto() {
    }

    public ListingDto(UUID guid, UUID personGUID, ListingDetailsDto listingDetails) {
        this.guid = guid;
        this.personGUID = personGUID;
        this.listingDetails = listingDetails;
    }

    public ListingDto setGuid(UUID guid) {
        this.guid = guid;
        return this;
    }

    public ListingDto setPersonID(UUID personGUID) {
        this.personGUID = personGUID;
        return this;
    }

    public ListingDto setListingDetails(ListingDetailsDto listingDetails) {
        this.listingDetails = listingDetails;
        return this;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        ListingDto entity = (ListingDto) o;
        return Objects.equals(this.guid, entity.guid) &&
                Objects.equals(this.personGUID, entity.personGUID) &&
                Objects.equals(this.listingDetails, entity.listingDetails);
    }

    @Override
    public int hashCode() {
        return Objects.hash(guid, personGUID, listingDetails);
    }

    @Override
    public String toString() {
        return getClass().getSimpleName() + "(" +
                "guid = " + guid + ", " +
                "personGUID = " + personGUID + ", " +
                "listingDetails = " + listingDetails + ")";
    }

    /**
     * DTO for {@link dk.iredo.product_storage.listings.entities.ListingDetails}
     */
    @Data
    public static class ListingDetailsDto {
        private String title;
        private String description;
        private Integer x_length_in_mm;
        private Integer y_width_in_mm;
        private Integer z_height_in_mm;
        private Condition condition;
        private int quantity;
        private BigDecimal price;
        private String city;
        private Date modified_date;
        private List<ColorDto> colors;
        private List<SubCategoryDto> subCategories;
        private List<ImageDto> images;

        public ListingDetailsDto() {
        }

        public ListingDetailsDto(String title, String description, Integer x_length_in_mm, Integer y_width_in_mm, Integer z_height_in_mm, Condition condition, int quantity, BigDecimal price, String city, Date modified_date, List<ColorDto> colors, List<SubCategoryDto> subCategories, List<ImageDto> images) {
            this.title = title;
            this.description = description;
            this.x_length_in_mm = x_length_in_mm;
            this.y_width_in_mm = y_width_in_mm;
            this.z_height_in_mm = z_height_in_mm;
            this.condition = condition;
            this.quantity = quantity;
            this.price = price;
            this.city = city;
            this.modified_date = modified_date;
            this.colors = colors;
            this.subCategories = subCategories;
            this.images = images;
        }

        public ListingDetailsDto setTitle(String title) {
            this.title = title;
            return this;
        }

        public ListingDetailsDto setDescription(String description) {
            this.description = description;
            return this;
        }

        public ListingDetailsDto setX_length_in_mm(Integer x_length_in_mm) {
            this.x_length_in_mm = x_length_in_mm;
            return this;
        }

        public ListingDetailsDto setY_width_in_mm(Integer y_width_in_mm) {
            this.y_width_in_mm = y_width_in_mm;
            return this;
        }

        public ListingDetailsDto setZ_height_in_mm(Integer z_height_in_mm) {
            this.z_height_in_mm = z_height_in_mm;
            return this;
        }

        public ListingDetailsDto setCondition(Condition condition) {
            this.condition = condition;
            return this;
        }

        public ListingDetailsDto setQuantity(int quantity) {
            this.quantity = quantity;
            return this;
        }

        public ListingDetailsDto setPrice(BigDecimal price) {
            this.price = price;
            return this;
        }

        public ListingDetailsDto setCity(String city) {
            this.city = city;
            return this;
        }

        public ListingDetailsDto setModified_date(Date modified_date) {
            this.modified_date = modified_date;
            return this;
        }

        public ListingDetailsDto setImages(List<ImageDto> images) {
            this.images = images;
            return this;
        }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;
            ListingDetailsDto entity = (ListingDetailsDto) o;
            return Objects.equals(this.title, entity.title) &&
                    Objects.equals(this.description, entity.description) &&
                    Objects.equals(this.x_length_in_mm, entity.x_length_in_mm) &&
                    Objects.equals(this.y_width_in_mm, entity.y_width_in_mm) &&
                    Objects.equals(this.z_height_in_mm, entity.z_height_in_mm) &&
                    Objects.equals(this.condition, entity.condition) &&
                    Objects.equals(this.quantity, entity.quantity) &&
                    Objects.equals(this.price, entity.price) &&
                    Objects.equals(this.city, entity.city) &&
                    Objects.equals(this.modified_date, entity.modified_date) &&
                    Objects.equals(this.colors, entity.colors) &&
                    Objects.equals(this.subCategories, entity.subCategories) &&
                    Objects.equals(this.images, entity.images);
        }

        @Override
        public int hashCode() {
            return Objects.hash(title, description, x_length_in_mm, y_width_in_mm, z_height_in_mm, condition, quantity, price, city, modified_date, colors, subCategories, images);
        }

        @Override
        public String toString() {
            return getClass().getSimpleName() + "(" +
                    "title = " + title + ", " +
                    "description = " + description + ", " +
                    "x_length_in_mm = " + x_length_in_mm + ", " +
                    "y_width_in_mm = " + y_width_in_mm + ", " +
                    "z_height_in_mm = " + z_height_in_mm + ", " +
                    "condition = " + condition + ", " +
                    "quantity = " + quantity + ", " +
                    "price = " + price + ", " +
                    "city = " + city + ", " +
                    "modified_date = " + modified_date + ", " +
                    "colors = " + colors + ", " +
                    "subCategories = " + subCategories + ", " +
                    "images = " + images + ")";
        }

        /**
         * DTO for {@link dk.iredo.product_storage.colors.Color}
         */
        @Data
        public static class ColorDto{
            private String name;
            private String href;

            public ColorDto() {
            }

            public ColorDto(String name, String href) {
                this.name = name;
                this.href = href;
            }

            public ColorDto setName(String name) {
                this.name = name;
                return this;
            }

            public ColorDto setHref(String href) {
                this.href = href;
                return this;
            }

            @Override
            public boolean equals(Object o) {
                if (this == o) return true;
                if (o == null || getClass() != o.getClass()) return false;
                ColorDto entity = (ColorDto) o;
                return Objects.equals(this.name, entity.name) &&
                        Objects.equals(this.href, entity.href);
            }

            @Override
            public int hashCode() {
                return Objects.hash(name, href);
            }

            @Override
            public String toString() {
                return getClass().getSimpleName() + "(" +
                        "name = " + name + ", " +
                        "href = " + href + ")";
            }
        }

        /**
         * DTO for {@link dk.iredo.product_storage.categories.entities.SubCategory}
         */
        @Data
        public static class SubCategoryDto{
            private String name;
            private CategoryDto category;

            public SubCategoryDto() {
            }

            public SubCategoryDto(String name, CategoryDto category) {
                this.name = name;
                this.category = category;
            }

            public SubCategoryDto setName(String name) {
                this.name = name;
                return this;
            }

            public SubCategoryDto setCategory(CategoryDto category) {
                this.category = category;
                return this;
            }

            @Override
            public boolean equals(Object o) {
                if (this == o) return true;
                if (o == null || getClass() != o.getClass()) return false;
                SubCategoryDto entity = (SubCategoryDto) o;
                return Objects.equals(this.name, entity.name) &&
                        Objects.equals(this.category, entity.category);
            }

            @Override
            public int hashCode() {
                return Objects.hash(name, category);
            }

            @Override
            public String toString() {
                return getClass().getSimpleName() + "(" +
                        "name = " + name + ", " +
                        "category = " + category + ")";
            }

            /**
             * DTO for {@link dk.iredo.product_storage.categories.entities.Category}
             */
            @Getter
            public static class CategoryDto{
                private String name;

                public CategoryDto() {
                }

                public CategoryDto(String name) {
                    this.name = name;
                }

                public CategoryDto setName(String name) {
                    this.name = name;
                    return this;
                }

                @Override
                public boolean equals(Object o) {
                    if (this == o) return true;
                    if (o == null || getClass() != o.getClass()) return false;
                    CategoryDto entity = (CategoryDto) o;
                    return Objects.equals(this.name, entity.name);
                }

                @Override
                public int hashCode() {
                    return Objects.hash(name);
                }

                @Override
                public String toString() {
                    return getClass().getSimpleName() + "(" +
                            "name = " + name + ")";
                }
            }
        }

        /**
         * DTO for {@link dk.iredo.product_storage.images.Image}
         */
        @Data
        public static class ImageDto{
            private String url;

            public ImageDto() {
            }

            public ImageDto(String url) {
                this.url = url;
            }

            public ImageDto setUrl(String url) {
                this.url = url;
                return this;
            }

            @Override
            public boolean equals(Object o) {
                if (this == o) return true;
                if (o == null || getClass() != o.getClass()) return false;
                ImageDto entity = (ImageDto) o;
                return Objects.equals(this.url, entity.url);
            }

            @Override
            public int hashCode() {
                return Objects.hash(url);
            }

            @Override
            public String toString() {
                return getClass().getSimpleName() + "(" +
                        "url = " + url + ")";
            }
        }
    }
}