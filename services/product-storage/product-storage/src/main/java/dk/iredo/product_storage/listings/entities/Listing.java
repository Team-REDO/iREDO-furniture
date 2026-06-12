package dk.iredo.product_storage.listings.entities;

import dk.iredo.product_storage.categories.entities.SubCategory;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.listings.enums.Condition;
import jakarta.annotation.Nonnull;
import jakarta.annotation.Nullable;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.sql.Date;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "listing")
public class Listing {
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
    //TODO - Check guid and UUID - same?
    private UUID GUID;

    @Setter
    @Getter
    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    //TODO - Check guid and UUID - same?
    private UUID personGUID;

    @Setter
    @Getter
    @Nonnull()
    private Condition condition;

    //TODO correct fetch.type?
    @Getter
    @Setter
    @OneToOne()
    private ListingDetails listingDetails;

    //TODO correct fetch.type?
    @Getter
    @ManyToMany()
    private final List<Color> colors = new ArrayList<>();

    //TODO correct fetch.type?
    @Getter
    @ManyToMany()
    private final List<SubCategory> subCategories = new ArrayList<>();

    public Listing(@Nonnull UUID GUID, @Nonnull UUID personGUID) {
        this.GUID = GUID;
        this.personGUID = personGUID;
    }

    public Listing() {
        //Empty constructor for the ORM
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