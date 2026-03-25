package dk.iredo.product_storage.images;

import dk.iredo.product_storage.listings.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.persistence.*;

@Entity
@Table(name = "image")
public class Image {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    @Nonnull()
    private String url;

    @ManyToOne(cascade = CascadeType.DETACH)
    @JoinColumn(name = "listing_details_ID", nullable = false)
    private ListingDetails listingDetails;

    @Nonnull
    public String getUrl() {
        return url;
    }

    public void setUrl(@Nonnull String url) {
        this.url = url;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public ListingDetails getListingDetails() {
        return listingDetails;
    }

    public void setListingDetails(ListingDetails listingDetails) {
        this.listingDetails = listingDetails;
    }
}