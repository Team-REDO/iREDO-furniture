package dk.iredo.product_storage.images;

import dk.iredo.product_storage.listings.entities.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

@Entity
@Table(name = "image")
public class Image {
    @Setter
    @Getter
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    @Nonnull()
    private String url;

    @ManyToOne(cascade = CascadeType.DETACH)
    @JoinColumn(name = "listing_details_ID")
    private ListingDetails listingDetails;

    public Image(@Nonnull String url, @Nonnull ListingDetails listingDetails) {
        this.url = url;
        this.listingDetails = listingDetails;
        listingDetails.addImage(this);
    }

    public Image() {}

    @Nonnull
    public String getUrl() {
        return url;
    }

    public void setUrl(@Nonnull String url) {
        this.url = url;
    }

}