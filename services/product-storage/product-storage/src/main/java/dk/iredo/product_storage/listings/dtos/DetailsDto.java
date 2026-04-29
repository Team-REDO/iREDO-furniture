package dk.iredo.product_storage.listings.dtos;

import dk.iredo.product_storage.listings.enums.Condition;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.Accessors;

import java.io.Serializable;
import java.math.BigDecimal;
import java.sql.Date;
import java.util.List;

/**
 * DTO for {@link dk.iredo.product_storage.listings.entities.ListingDetails}
 */
@Data
@AllArgsConstructor
@NoArgsConstructor
@Accessors(chain = true)
public class DetailsDto implements Serializable {
    private String title;
    private String description;
    private Integer x_length_in_mm;
    private Integer y_width_in_mm;
    private Integer z_height_in_mm;
    private Condition condition;
    private Integer quantity;
    private BigDecimal price_dkk;
    private String city;
    private Date modified_date;
    private List<String> colorHRefs;
    private List<String> subCategoryNames;
}