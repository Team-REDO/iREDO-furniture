package dk.iredo.product_storage.colors;

import org.springframework.data.jpa.repository.JpaRepository;

import org.springframework.stereotype.Repository;

@Repository
public interface ColorsRepository extends JpaRepository<Color, Long> {
    public Color findByHref(String href);
}
