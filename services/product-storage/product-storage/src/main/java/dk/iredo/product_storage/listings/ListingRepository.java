package dk.iredo.product_storage.listings;

import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;
import org.springframework.stereotype.Repository;

@Repository
public interface ListingRepository extends JpaRepository<Listing, Long> {

    public Boolean existsListingByGuid(UUID gui);

    public Listing findListingByGuid(UUID guid);
}
