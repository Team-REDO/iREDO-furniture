import "dotenv/config";
import app from "./app.js"
import { API_GATEWAY_PORT } from "./config/services.js";

const PORT = API_GATEWAY_PORT;

app.listen(PORT, () => {
    console.log(`Gateway running on ${PORT}`);
    
}); 