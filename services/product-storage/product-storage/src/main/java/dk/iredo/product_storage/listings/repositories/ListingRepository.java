package dk.iredo.product_storage.listings.repositories;

import dk.iredo.product_storage.listings.entities.Listing;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;
import java.util.UUID;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface ListingRepository extends JpaRepository<Listing, Long> {

    @Query("SELECT True FROM Listing l WHERE l.guid = :#{#guid}")
    default Boolean existsListingByGuid(@Param("guid") UUID guid) {
        return Boolean.FALSE;
    }

}
