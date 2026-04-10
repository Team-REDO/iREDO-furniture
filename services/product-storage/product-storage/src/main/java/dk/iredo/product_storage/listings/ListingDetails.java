package dk.iredo.product_storage.listings;

import dk.iredo.product_storage.categories.SubCategory;
import dk.iredo.product_storage.colors.Color;
import dk.iredo.product_storage.enums.Condition;
import dk.iredo.product_storage.images.Image;
import jakarta.annotation.Nonnull;
import jakarta.annotation.Nullable;
import jakarta.persistence.*;
import org.hibernate.annotations.ColumnDefault;

import java.math.BigDecimal;
import java.sql.Date;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "listing_details")
public class ListingDetails {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    //TODO test if correct cascade.type and parrent child relation
    @OneToOne(cascade = CascadeType.REMOVE)
    @JoinColumn(name = "listingID", nullable = false)
    private Listing listing;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String title;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String description;

    //TODO - Nullable works as nullable(true)?
    @Nullable()
    private Integer x_length_in_mm;

    //TODO - Nullable works as nullable(true)?
    @Nullable()
    private Integer y_width_in_mm;

    //TODO - Nullable works as nullable(true)?
    @Nullable()
    private Integer z_height_in_mm;

    //TODO - Nonnull works as nullable(false)?
    //TODO - Change to Wear_rating
    @Nonnull()
    private Condition condition;

    @ColumnDefault("1")
    private int quantity;

    //TODO ' precision = 10, ' in column? Only for big decimal? DKK?
    @Column(name = "price", nullable = false, scale = 2)
    private BigDecimal price;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    private String city;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    //TODO - correct datetime format for DB?
    private Date Modified_date;

    //TODO correct fetch.type?
    @ManyToMany(fetch = FetchType.LAZY)
    @JoinTable(name = "listing_details_color")
    private List<Color> colors = new ArrayList<>();

    //TODO correct fetch.type?
    @ManyToMany(fetch = FetchType.LAZY)
    private List<SubCategory> subCategories = new ArrayList<>();

    public ListingDetails(Listing listing, @Nonnull String title, @Nonnull String description, @Nullable Integer x_length_in_mm, @Nullable Integer y_width_in_mm, @Nullable Integer z_height_in_mm, @Nonnull Condition condition, int quantity, BigDecimal price, @Nonnull String city) {
        this.listing = listing;
        this.title = title;
        this.description = description;
        this.x_length_in_mm = x_length_in_mm;
        this.y_width_in_mm = y_width_in_mm;
        this.z_height_in_mm = z_height_in_mm;
        this.condition = condition;
        this.quantity = quantity;
        this.price = price;
        this.city = city;
        Modified_date = new Date(System.currentTimeMillis());
    }

    public ListingDetails() {
    }

    public Listing getListing() {
        return listing;
    }

    public void setListing(Listing listing) {
        this.listing = listing;
    }

    @Nonnull
    public String getTitel() {
        return title;
    }

    public void setTitel(@Nonnull String titel) {
        this.title = titel;
    }

    @Nonnull
    public String getDescription() {
        return description;
    }

    public void setDescription(@Nonnull String description) {
        this.description = description;
    }

    @Nullable
    public Integer getX_length_in_mm() {
        return x_length_in_mm;
    }

    public void setX_length_in_mm(@Nullable Integer x_length_in_mm) {
        this.x_length_in_mm = x_length_in_mm;
    }

    @Nullable
    public Integer getY_width_in_mm() {
        return y_width_in_mm;
    }

    public void setY_width_in_mm(@Nullable Integer y_width_in_mm) {
        this.y_width_in_mm = y_width_in_mm;
    }

    @Nullable
    public Integer getZ_height_in_mm() {
        return z_height_in_mm;
    }

    public void setZ_height_in_mm(@Nullable Integer z_height_in_mm) {
        this.z_height_in_mm = z_height_in_mm;
    }

    @Nonnull
    public Condition getCondition() {
        return condition;
    }

    public void setCondition(@Nonnull Condition condition) {
        this.condition = condition;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public BigDecimal getPrice() {
        return price;
    }

    public void setPrice(BigDecimal price) {
        this.price = price;
    }

    @Nonnull
    public String getCity() {
        return city;
    }

    public void setCity(@Nonnull String city) {
        this.city = city;
    }

    @Nonnull
    public Date getModified_date() {
        return Modified_date;
    }

    public void setModified_date(@Nonnull Date modified_date) {
        Modified_date = modified_date;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public List<Color> getColors() {
        return colors;
    }

    public void setColors(List<Color> colors) {
        this.colors = colors;
    }

    public List<SubCategory> getSubCategories() {
        return subCategories;
    }

    public void setSubCategories(List<SubCategory> subCategories) {
        this.subCategories = subCategories;
    }

    public void addSubCategories(SubCategory subCategory) {
        if (subCategory == null) {
            throw new NullPointerException("SubCategory is null...");
        }else  {
            this.subCategories.add(subCategory);
        }
    }

    public void addColors(Color color) {
        if (color == null) {
            throw new NullPointerException("Color is null...");
        }else  {
            this.colors.add(color);
        }
    }
}