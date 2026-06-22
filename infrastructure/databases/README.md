# MongoDB Seeding & Setup

This directory contains MongoDB initialization and seeding utilities for the iREDO Furniture project.

## Files

- **`seed-data.json`** - Test data in JSON format with 6 sample furniture items
- **`init-mongo.js`** - MongoDB init script that auto-runs on container startup
- **`seed.js`** - Standalone Node.js script for manual seeding with various options
- **`package.json`** - Dependencies and npm scripts for the seeding tools

## Quick Start

### Automatic Seeding (Recommended)

When you run `docker compose up`, the MongoDB container automatically:

1. Starts the MongoDB service
2. Creates the `furnituredatabase` database
3. Runs `init-mongo.js` which seeds the `Furniture` collection with test data

**No additional steps needed** — your database is ready to use immediately.

```bash
# Start containers with automatic seeding
docker compose up --build

# MongoDB will have test data pre-loaded
```

### Manual Seeding (if needed)

If you need to reseed the database or run seeding manually:

### Reset the Docker volume and load fresh seed data

If the old data is stored in the Docker Mongo volume, remove the volume first and then start the stack again:

```bash
docker compose down -v
docker compose up --build -d
```

That deletes the old `mongodb-data` volume, so MongoDB starts empty and runs `init-mongo.js` again with the latest seed data.

If you only want to clear and repopulate the collection without deleting the volume:

```bash
cd infrastructure/databases
npm run seed:reset:local
```

Or from inside Docker:

```bash
docker exec iredo-mongodb mongosh furnituredatabase --eval "db.Furniture.deleteMany({})"
cd infrastructure/databases
npm run seed:reset
```

#### Option 1: Using npm scripts (easiest)

```bash
# Navigate to the databases directory
cd infrastructure/databases

# Install dependencies
npm install

# Seed with default MongoDB connection (for Docker)
npm run seed

# Seed to localhost (for local MongoDB)
npm run seed:local

# Clear database and reseed
npm run seed:clear

# Clear and reseed localhost
npm run seed:local:clear

# Reset and reseed localhost in one step
npm run seed:reset:local
```

#### Option 2: Direct node command

```bash
cd infrastructure/databases
npm install

# Default (connects to mongodb://mongodb:27017)
node seed.js

# Custom connection string
node seed.js mongodb://localhost:27017

# Clear before seeding
node seed.js --clear

# Show help
node seed.js --help
```

## Test Data

The seed includes 6 furniture items:

1. **Cloud Linen Sofa 3-Seater** - Oslo, €1890
2. **Compact Two-Seater Sofa** - Helsinki, €1390
3. **Velvet Lounge Armchair** - Berlin, €749
4. **Rattan Accent Chair** - Lisbon, €420
5. **Modern Dining Table** - Copenhagen, €1200
6. **White Bookshelf Unit** - Brussels, €350

Each item includes:

- Product details (title, description, size, price, condition)
- Category & subcategory information
- Color information
- Multiple images
- Status (active/sold flags)
- Location (city, zip code)
- Seller info (personId)

## MongoDB Connection

### Docker Environment

- **Host**: `mongodb` (service name)
- **Port**: `27017`
- **Connection String**: `mongodb://mongodb:27017`
- **Database**: `furnituredatabase`
- **Collection**: `Furniture`

### Local Environment

- **Host**: `localhost`
- **Port**: `27017`
- **Connection String**: `mongodb://localhost:27017`

## Database Schema (Furniture Collection)

```typescript
{
  _id: ObjectId,
  sales_post_guid: string,
  guid: string,
  personId: string,
  title: string,
  description: string,
  size: string,
  quantity: string,
  price: number,
  condition: "excellent" | "like-new" | "very-good" | "good" | "fair",
  zip_code: string,
  city: string,
  status: {
    removed: bool,
    date?: dateTime
  },
  color: {
    name: string,
    hex: string
  },
  category: {
    name: string
  },
  subcategory: {
    name: string
  },
  images: Array<{
    url: string,
    alt: string
  }>
}
```

## Troubleshooting

### "Database already contains X items. Skipping seed."

The auto-seed script only runs if the collection is empty. To reseed:

```bash
# Using npm script
cd infrastructure/databases
npm run seed:clear

# Or manually clear MongoDB
docker exec iredo-mongodb mongosh furnituredatabase --eval "db.Furniture.deleteMany({})"
```

### Connection refused

Ensure MongoDB is running:

```bash
# For Docker
docker compose ps  # Check if iredo-mongodb is running

# For local MongoDB
sudo systemctl start mongod  # Linux/Mac
# or use MongoDB GUI/Compass
```

### Script permission denied (Linux/Mac)

Make the script executable:

```bash
chmod +x infrastructure/databases/seed.js
```

## Adding Custom Test Data

To add more test data:

1. Edit `seed-data.json` with new furniture items following the schema above
2. Clear the database: `npm run seed:clear`
3. Or manually call:

   ```bash
   node seed.js --clear
   ```

## Verifying Seeded Data

Check what's in MongoDB:

```bash
# Via Docker exec
docker exec iredo-mongodb mongosh furnituredatabase --eval "db.Furniture.find().pretty()"

# Via MongoDB Compass
# Connect to mongodb://localhost:27017 and browse "furnituredatabase.Furniture"

# Via API (if GraphQL endpoint is running)
# Query: { products { title price city } }
```

## Notes

- The init script only runs **once** when the container is first created. Stopping/restarting the container won't re-run it.
- To re-seed after deletion, use the standalone `seed.js` script.
- Test data uses real image URLs from Unsplash (requires internet connection to display).
