package dk.iredo.product_storage.categories.entities;

import dk.iredo.product_storage.listings.entities.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "sub_category")
public class SubCategory {
    @Setter
    @Getter
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    @Nonnull()
    private String name;

    //TODO see - good practise?
    @Setter
    @Getter
    @ManyToOne
    @JoinColumn(name = "category_id")
    @Nonnull()
    private Category category;

    public SubCategory(@Nonnull String name, @Nonnull Category category) {
        this.name = name;
        this.category = category;
    }
    public SubCategory() {
    }

    @Nonnull
    public String getName() {
        return name;
    }
    public void setName(@Nonnull String name) {
        this.name = name;
    }
}