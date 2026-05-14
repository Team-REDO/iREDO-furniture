#!/usr/bin/env node

/**
 * MongoDB Seeding Script for iREDO Furniture
 * 
 * Usage:
 *   node seed.js                    # Seed with default MongoDB connection
 *   node seed.js mongodb://localhost:27017  # Seed with custom connection string
 *   node seed.js --clear            # Clear database before seeding
 *   node seed.js --help             # Show this help message
 */

const { MongoClient } = require('mongodb');
const fs = require('fs');
const path = require('path');

// Color codes for console output
const colors = {
  reset: '\x1b[0m',
  green: '\x1b[32m',
  yellow: '\x1b[33m',
  red: '\x1b[31m',
  blue: '\x1b[34m',
};

const log = {
  info: (msg) => console.log(`${colors.blue}ℹ${colors.reset} ${msg}`),
  success: (msg) => console.log(`${colors.green}✓${colors.reset} ${msg}`),
  warn: (msg) => console.log(`${colors.yellow}⚠${colors.reset} ${msg}`),
  error: (msg) => console.log(`${colors.red}✗${colors.reset} ${msg}`),
};

// Parse command-line arguments
const args = process.argv.slice(2);
let mongoUrl = 'mongodb://mongodb:27017';
let shouldClear = false;
let showHelp = false;

for (const arg of args) {
  if (arg === '--help') showHelp = true;
  else if (arg === '--clear') shouldClear = true;
  else if (arg.startsWith('mongodb://') || arg.startsWith('mongodb+srv://')) mongoUrl = arg;
}

if (showHelp) {
  console.log(`
MongoDB Seeding Script for iREDO Furniture

Usage:
  node seed.js                                # Seed with default connection (mongodb://mongodb:27017)
  node seed.js mongodb://localhost:27017      # Seed with custom connection string
  node seed.js --clear                        # Clear database before seeding
  node seed.js --help                         # Show this help message

Options:
  --help                                      Show this help message
  --clear                                     Clear existing Furniture collection before seeding
  `);
  process.exit(0);
}

// Load seed data from JSON
const seedDataPath = path.join(__dirname, 'seed-data.json');
let seedData = [];

try {
  const rawData = fs.readFileSync(seedDataPath, 'utf-8');
  seedData = JSON.parse(rawData);
  log.info(`Loaded ${seedData.length} seed items from seed-data.json`);
} catch (error) {
  log.error(`Failed to load seed-data.json: ${error.message}`);
  process.exit(1);
}

// Connect and seed MongoDB
async function seedDatabase() {
  const client = new MongoClient(mongoUrl, {
    useNewUrlParser: true,
    useUnifiedTopology: true,
  });

  try {
    await client.connect();
    log.info('Connected to MongoDB');

    const db = client.db('furnituredatabase');
    const collection = db.collection('Furniture');

    if (shouldClear) {
      const deleteResult = await collection.deleteMany({});
      log.warn(`Cleared ${deleteResult.deletedCount} existing items`);
    }

    const insertResult = await collection.insertMany(seedData, { ordered: false });
    log.success(`Successfully seeded ${insertResult.insertedCount} furniture items`);

    const totalCount = await collection.countDocuments();
    log.info(`Total items in collection: ${totalCount}`);
  } catch (error) {
    if (error.code === 11000) {
      log.warn('Some documents already exist (duplicate key error). Use --clear flag to reset.');
    } else {
      log.error(`Database error: ${error.message}`);
    }
  } finally {
    await client.close();
    log.info('Connection closed');
  }
}

// Run the seeding
seedDatabase();
