import express from "express";
import cors from "cors";
import axios from "axios";
import routes from "./routes/index.js";
import { CATALOGUE_GRAPHQL_URL, FRONTEND_ORIGIN } from "./config/services.js";

const app = express();
const CORS_OPTIONS = {
  origin: FRONTEND_ORIGIN,
  credentials: true,
};

app.use(cors(CORS_OPTIONS));
app.use(express.json());

app.options("/graphql", cors(CORS_OPTIONS));

app.all("/graphql", async (req, res, next) => {
  try {
    const response = await axios.request({
      method: req.method,
      url: CATALOGUE_GRAPHQL_URL,
      params: req.query,
      data: req.body,
      headers: {
        ...req.headers,
        host: undefined,
        origin: undefined,
      },
      validateStatus: () => true,
    });

    res.status(response.status);

    Object.entries(response.headers).forEach(([key, value]) => {
      if (value !== undefined) {
        res.setHeader(key, value);
      }
    });

    res.send(response.data);
  } catch (error) {
    next(error);
  }
});

app.use("/api", routes);

export default app;
