package dk.iredo.product_storage.enums;

import java.util.HashMap;
import java.util.Map;
import java.util.stream.Stream;

public enum Condition { // Todo - wearRating instead??

    //TODO - Danish website for end-user? And what about expansion international? (WE KEEP IT ENGLISH)
    //TODO - fill text for enum and change to upper letter
    AsNew(Integer.toString(1)),
    Used(Integer.toString(2)),
    SomeWearAndTear(Integer.toString(3)),
    TLC(Integer.toString(4));

    private final String state;
    private static final Map<String, Condition> BY_STATE = new HashMap<>();

    static {
        for (Condition s: values()) {
            BY_STATE.put(s.state, s);
        }
    }
    Condition(String state) {
        this.state = state;
    }

    public static Condition valueOfState(String state) {
        return BY_STATE.get(state);
    }

    public String getState() {
        return this.state;
    }


    public static Condition of(String stateName) {
        return Stream.of(Condition.values())
                .filter(p -> p.getState() == stateName)
                .findFirst()
                .orElseThrow(IllegalArgumentException::new);
    }
}
