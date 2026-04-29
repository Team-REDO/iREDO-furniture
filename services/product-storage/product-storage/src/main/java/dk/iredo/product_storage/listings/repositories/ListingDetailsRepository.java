package dk.iredo.product_storage.listings.repositories;

import dk.iredo.product_storage.listings.entities.ListingDetails;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.Optional;
import java.util.UUID;
import org.springframework.stereotype.Repository;

@Repository
public interface ListingDetailsRepository extends JpaRepository<ListingDetails, Long> {

    public boolean existsListingDetailsByTitle(String title);

    @Query("SELECT ld FROM Listing l JOIN l.listingDetails ld WHERE l.GUID = :#{#listingGuid}")
    public Optional<ListingDetails> findListingDetailsByListingGuid(@Param("listingGuid") UUID listingGuid);

}
