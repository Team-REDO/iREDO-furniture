package dk.iredo.product_storage.categories;

import dk.iredo.product_storage.categories.entities.SubCategory;
import dk.iredo.product_storage.categories.repositories.CategoryRepository;
import dk.iredo.product_storage.categories.repositories.SubCategoryRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class CategoriesService {

    @Autowired
    SubCategoryRepository subCategoryRepository;

    @Autowired
    CategoryRepository categoryRepository;

    //TODO make interface
    public List<SubCategory> getAllSubCategories() {
        return subCategoryRepository.getAll();
    }
}
