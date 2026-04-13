package dk.iredo.product_storage.listings.entities;

import dk.iredo.product_storage.categories.entities.SubCategory;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.listings.entities.enums.Condition;
import jakarta.annotation.Nonnull;
import jakarta.annotation.Nullable;
import jakarta.persistence.*;

import java.math.BigDecimal;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "listing")
public class Listing {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    //TODO - Check guid and UUID - same?
    private UUID guid;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    //TODO - Check guid and UUID - same?
    private UUID personID;

    @OneToOne(cascade = CascadeType.ALL)
    private ListingDetails listingDetails;

    public Listing(@Nonnull UUID guid, @Nonnull UUID personID, @Nonnull String title, @Nonnull String description,
                   @Nullable Integer x_length_in_mm, @Nullable Integer y_width_in_mm,
                   @Nullable Integer z_height_in_mm, @Nonnull Condition condition, int quantity,
                   BigDecimal price, @Nonnull String city, @Nonnull List<SubCategory> subCategories,
                   @Nonnull List<Color> colors) {
        this.guid = guid;
        this.personID = personID;
        this.listingDetails = new ListingDetails(this, title, description, x_length_in_mm, y_width_in_mm,
                z_height_in_mm, condition, quantity, price, city, subCategories, colors
        );
    }

    public Listing() {
        //Empty constructor for the ORM
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    @Nonnull
    public UUID getGuid() {
        return guid;
    }

    public void setGuid(@Nonnull UUID guid) {
        this.guid = guid;
    }

    @Nonnull
    public UUID getPersonID() {
        return personID;
    }

    public void setPersonID(@Nonnull UUID personID) {
        this.personID = personID;
    }

    @Nonnull
    public ListingDetails getListingDetails() {
        return this.listingDetails;
    }

}