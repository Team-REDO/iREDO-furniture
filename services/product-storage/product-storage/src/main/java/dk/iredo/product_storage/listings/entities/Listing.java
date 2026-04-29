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
    @OneToOne(cascade = CascadeType.ALL)
    private ListingDetails listingDetails;

    public Listing(@Nonnull UUID GUID, @Nonnull UUID personGUID,
                   @Nonnull ListingDetails listingDetails) {
        this.GUID = GUID;
        this.personGUID = personGUID;
        listingDetails.setModified_date(new Date(System.currentTimeMillis()));
        this.listingDetails = listingDetails;
    }

    public Listing() {
        //Empty constructor for the ORM
    }

}