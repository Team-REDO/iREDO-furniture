package dk.iredo.product_storage.categories;

import jakarta.annotation.Nonnull;
import jakarta.persistence.*;

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

    public SubCategory(@Nonnull String name, @Nonnull Category category) {
        this.name = name;
        this.category = category;
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

}