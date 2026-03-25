package dk.iredo.product_storage.listings;

import jakarta.annotation.Nonnull;
import jakarta.persistence.*;

import java.util.UUID;

@Entity
@Table(name = "listing")
public class Listing {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    //TODO - Check guid and UUID - same?
    private UUID guid;

    //TODO - Nonnull works as nullable(false)?
    @Nonnull()
    //TODO - Check guid and UUID - same?
    private UUID personID;

    public Listing(@Nonnull UUID guid, @Nonnull UUID personID) {
        this.guid = guid;
        this.personID = personID;
    }

    public Listing() {
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    @Nonnull
    public UUID getGuid() {
        return guid;
    }

    public void setGuid(@Nonnull UUID guid) {
        this.guid = guid;
    }

    @Nonnull
    public UUID getPersonID() {
        return personID;
    }

    public void setPersonID(@Nonnull UUID personID) {
        this.personID = personID;
    }
}