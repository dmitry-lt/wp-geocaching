import com.geocaching.Login;
import com.geocaching.StatusCode;

public class Main {
    public static void main(String[] args) {
        StatusCode result = Login.login("test", "test");
        System.out.println(result);
    }
}
