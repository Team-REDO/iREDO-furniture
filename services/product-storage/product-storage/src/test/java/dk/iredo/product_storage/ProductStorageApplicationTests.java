package dk.iredo.product_storage;

import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

@SpringBootTest
@EnableJpaRepositories(basePackages = {"dk.iredo.product_storage"})
class ProductStorageApplicationTests {

	@Test
	void contextLoads() {
	}

}
