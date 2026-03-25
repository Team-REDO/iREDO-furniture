package dk.iredo.product_storage.colors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ColorsService {

    @Autowired
    private ColorsRepository colorsRepository;

    public Color addColor(Color color) {
        return colorsRepository.save(color);
    }

    public List<Color> getAllColors() {
        return colorsRepository.findAll();
    }
}
