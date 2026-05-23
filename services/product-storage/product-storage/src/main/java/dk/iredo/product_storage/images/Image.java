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

    @Setter
    @Getter
    @Nonnull()
    private String url;

    @Setter
    @Getter
    @Nonnull()
    @ManyToOne()
    private ListingDetails listingDetails;

    public Image(@Nonnull String url, @Nonnull ListingDetails listingDetails) {
        this.url = url;
        this.listingDetails = listingDetails;
    }

    public Image() {}

}