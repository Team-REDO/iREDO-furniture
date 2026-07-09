package dk.iredo.product_storage.listings.entities;

import jakarta.annotation.Nonnull;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.sql.Date;

@Entity
@Table(name = "listing_removed")
public class ListingRemoved {
    @Setter
    @Getter
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    //TODO - test for correct cascade.type and parrent child relation
    @Setter
    @Getter
    @OneToOne(cascade = CascadeType.REMOVE)
    @JoinColumn(name = "listingID", nullable = false)
    private Listing listing;

    //TODO - correct datetime format for DB?
    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private Date removed_date;

    @Nonnull
    public Date getRemoved_date() {
        return removed_date;
    }

    public void setRemoved_date(@Nonnull Date removed_date) {
        this.removed_date = removed_date;
    }

}