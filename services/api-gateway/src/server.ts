import "dotenv/config";
import app from "./app.js"

const PORT = process.env.POR || 3123; 

app.listen(PORT, () => {
    console.log(`Gateway running on ${PORT}`);
    
}); 