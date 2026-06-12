package dk.iredo.product_storage.listings.dtos;

import lombok.*;
import lombok.experimental.Accessors;

import java.io.Serializable;
import java.math.BigDecimal;
import java.sql.Date;

/**
 * DTO for {@link dk.iredo.product_storage.listings.entities.ListingDetails}
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
@ToString
@Accessors(chain = true)
public class DetailsDto implements Serializable {
    private Long id;
    private String title;
    private String description;
    private Integer x_length_in_mm;
    private Integer y_width_in_mm;
    private Integer z_height_in_mm;
    private int quantity;
    private BigDecimal price_dkk;
    private String city;
    private Date modified_date;
}