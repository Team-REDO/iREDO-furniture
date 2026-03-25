package dk.iredo.product_storage.categories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class CategoriesService {

    @Autowired
    SubCategoryRepository subCategoryRepository;

    @Autowired
    CategoryRepository categoryRepository;

    public List<SubCategory> getAllSubCategories() {
        return subCategoryRepository.getAll();
    }
}
