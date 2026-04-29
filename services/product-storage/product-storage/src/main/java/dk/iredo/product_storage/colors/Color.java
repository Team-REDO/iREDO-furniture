package dk.iredo.product_storage.colors;

import dk.iredo.product_storage.listings.entities.ListingDetails;
import jakarta.annotation.Nonnull;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "color")
public class Color {
    @Setter
    @Getter
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

    public Color() {
    }


    public Color(@Nonnull String name, @Nonnull String href) {
        this.name = name;
        this.href = href;
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

}