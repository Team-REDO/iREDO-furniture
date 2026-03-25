package dk.iredo.product_storage.categories;

import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface SubCategoryRepository extends JpaRepository<SubCategory, Long> {

    @Query("SELECT sc FROM SubCategory sc")
    public List<SubCategory> getAll();

    @Query("SELECT sc.id FROM SubCategory sc WHERE sc.name = :#{#name}")
    public long findIDBySubCategoryName(@Param("name") String name);
}
