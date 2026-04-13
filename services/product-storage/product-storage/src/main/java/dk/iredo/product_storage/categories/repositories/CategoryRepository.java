package dk.iredo.product_storage.categories.repositories;

import dk.iredo.product_storage.categories.entities.Category;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface CategoryRepository extends CrudRepository<Category, Long> {

}
