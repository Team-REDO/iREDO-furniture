# Build stage
FROM node:20-alpine AS builder

WORKDIR /app

# Copy package files
COPY package.json package-lock.json ./

# Install dependencies (including dev deps so `vite preview` is available)
RUN npm ci

# Copy source code
COPY . .

# Build the application
RUN npm run build

# Runtime stage: use node to run `vite preview` so we don't need nginx
FROM node:20-alpine AS runtime
WORKDIR /app

# Copy built assets and node_modules from builder
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json

# Expose Vite preview default port
EXPOSE 5173

# Health check (check the preview server)
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --quiet --tries=1 --spider http://localhost:5173/ || exit 1

# Start vite preview on all interfaces so Docker can route to it
CMD ["npm", "run", "preview", "--", "--host", "0.0.0.0", "--port", "5173"]
