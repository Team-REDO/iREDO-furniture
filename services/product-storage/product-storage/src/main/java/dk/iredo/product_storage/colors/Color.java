package dk.iredo.product_storage.colors;

import dk.iredo.product_storage.listings.entities.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "color")
public class Color {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String name;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String href;

    //TODO correct fetch.type?
    @ManyToMany(cascade = {CascadeType.DETACH, CascadeType.MERGE, CascadeType.PERSIST, CascadeType.REFRESH})
    private List<ListingDetails> listingDetails;

    public Color() {
    }


    public Color(@Nonnull String name, @Nonnull String href) {
        this.name = name;
        this.href = href;
        this.listingDetails = new ArrayList<>();
    }

    @Nonnull
    public String getName() {
        return name;
    }

    public void setName(@Nonnull String name) {
        this.name = name;
    }

    @Nonnull
    public String getHref() {
        return href;
    }

    public void setHref(@Nonnull String href) {
        this.href = href;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public List<ListingDetails> getListingDetails() {
        return listingDetails;
    }

}