package dk.iredo.product_storage.listings.entities;

import dk.iredo.product_storage.categories.entities.SubCategory;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.images.Image;
import dk.iredo.product_storage.listings.enums.Condition;
import jakarta.annotation.Nonnull;
import jakarta.annotation.Nullable;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import org.hibernate.annotations.ColumnDefault;

import java.math.BigDecimal;
import java.sql.Date;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "listing_details")
public class ListingDetails {
    @Setter
    @Getter
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    @Setter
    @Getter
    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String title;

    @Setter
    @Getter
    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String description;

    @Setter
    @Getter
    //TODO - Nullable works as nullable(true)?
    @Nonnull()
    private Integer x_length_in_mm;

    @Setter
    @Getter
    //TODO - Nullable works as nullable(true)?
    @Nonnull()
    private Integer y_width_in_mm;

    @Setter
    @Getter
    //TODO - Nullable works as nullable(true)?
    @Nonnull()
    private Integer z_height_in_mm;

    @Setter
    @Getter
    //TODO - Nonnull works as nullable(false)?
    //TODO - Change to Wear_rating??
    @Nonnull()
    private Condition condition;

    @Setter
    @Getter
    @ColumnDefault("1")
    private int quantity;

    //TODO ' precision = 10, ' in column? Only for big decimal? DKK?
    @Setter
    @Getter
    @Column(name = "price_dkk", nullable = false, scale = 2)
    private BigDecimal price_dkk;

    @Setter
    @Getter
    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String city;

    @Setter
    @Getter
    //TODO - correct datetime format for DB?
    private Date modified_date;

    //TODO correct fetch.type?
    @Getter
    @ManyToMany()
    private final List<Color> colors = new ArrayList<>();

    //TODO correct fetch.type?
    @Getter
    @ManyToMany()
    private final List<SubCategory> subCategories = new ArrayList<>();


    public ListingDetails(@Nonnull String title, @Nonnull String description,
                          @Nonnull Integer x_length_in_mm, @Nonnull Integer y_width_in_mm,
                          @Nonnull Integer z_height_in_mm, @Nonnull Condition condition,
                          @Nullable Integer quantity,
                          @Nonnull BigDecimal price_dkk, @Nonnull String city
    ) {
        this.title = title;
        this.description = description;
        this.x_length_in_mm = x_length_in_mm;
        this.y_width_in_mm = y_width_in_mm;
        this.z_height_in_mm = z_height_in_mm;
        this.condition = condition;
        if(quantity == null) this.quantity = 1; else this.quantity = quantity;
        this.price_dkk = price_dkk;
        this.city = city;
        this.modified_date = new Date(System.currentTimeMillis());
    }

    public ListingDetails() {
    }

    public void addSubCategory(SubCategory subCategory) {
        if (subCategory == null) {
            throw new NullPointerException("Given subcategory is null...");
        }else  {
            this.subCategories.add(subCategory);
        }
    }

    public void addColor(Color color) {
        if (color == null) {
            throw new NullPointerException("Give color is null...");
        }else  {
            this.colors.add(color);
        }
    }
}