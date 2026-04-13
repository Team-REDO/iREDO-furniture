package dk.iredo.product_storage.categories.entities;

import dk.iredo.product_storage.listings.entities.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "sub_category")
public class SubCategory {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    @Nonnull()
    private String name;

    //TODO see - good practise?
    @ManyToOne
    @JoinColumn(name = "category_id")
    @Nonnull()
    private Category category;

    @ManyToMany()
    @JoinTable()
    List<ListingDetails> listingDetails;

    public SubCategory(@Nonnull String name, @Nonnull Category category) {
        this.name = name;
        this.category = category;
        this.listingDetails = new ArrayList<>();
    }
    public SubCategory() {
    }

    public Category getCategory() {
        return category;
    }

    public void setCategory(Category category) {
        this.category = category;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    @Nonnull
    public String getName() {
        return name;
    }
    public void setName(@Nonnull String name) {
        this.name = name;
    }
}