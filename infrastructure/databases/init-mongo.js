// MongoDB initialization script for seeding test data
// This script runs automatically when MongoDB container starts

const seedData = [
  {
    sales_post_guid: "12345678-1234-1234-1234-123456789001",
    guid: "87654321-4321-4321-4321-987654321001",
    personId: "user123",
    title: "Cloud Linen Sofa 3-Seater",
    description: "Beautiful 3-seater sofa in light gray linen. Comfortable and modern design. Excellent condition.",
    size: "220x90x85cm",
    quantity: "1",
    price: 1890,
    condition: "like-new",
    zip_code: "0150",
    status: {
      removed: false,
      date: null
    },
    color: {
      id: "gray",
      name: "Gray",
      href: "#808080"
    },
    categories: [{ id: "living-room", name: "Living Room", subcats: [{ id: "sofas", name: "Sofas" }] }],
    images: [
      { id: "img-sofa-1", url: "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=1200&q=80" },
      { id: "img-sofa-2", url: "https://images.unsplash.com/photo-1567016432779-094069958ea5?auto=format&fit=crop&w=1200&q=80" },
      { id: "img-sofa-3", url: "https://images.unsplash.com/photo-1616486701797-0f33f61038aa?auto=format&fit=crop&w=1200&q=80" }
    ]
  },
  {
    sales_post_guid: "12345678-1234-1234-1234-123456789002",
    guid: "87654321-4321-4321-4321-987654321002",
    personId: "user456",
    title: "Compact Two-Seater Sofa",
    description: "Practical two-seater sofa, perfect for smaller spaces. Dark blue color. Some wear visible.",
    size: "160x80x80cm",
    quantity: "1",
    price: 1390,
    condition: "good",
    zip_code: "00100",
    status: {  removed: false, date: null  },
    color: { id: "navy-blue", name: "Navy Blue", href: "#000080" },
    categories: [{ id: "living-room", name: "Living Room", subcats: [{ id: "sofas", name: "Sofas" }] }],
    images: [
      { id: "img-sofa-4", url: "https://images.unsplash.com/photo-1616486701797-0f33f61038aa?auto=format&fit=crop&w=1200&q=80" },
      { id: "img-sofa-5", url: "https://images.unsplash.com/photo-1616486338812-3dadae4b4ace?auto=format&fit=crop&w=1200&q=80" }
    ]
  },
  {
    sales_post_guid: "12345678-1234-1234-1234-123456789003",
    guid: "87654321-4321-4321-4321-987654321003",
    personId: "user789",
    title: "Velvet Lounge Armchair",
    description: "Elegant armchair upholstered in soft burgundy velvet. Perfect for reading nook. Pristine condition.",
    size: "85x90x95cm",
    quantity: "1",
    price: 749,
    condition: "excellent",
    zip_code: "10115",
    status: { removed: false, date: null },
    color: { id: "burgundy", name: "Burgundy", href: "#800020" },
    categories: [{ id: "living-room", name: "Living Room", subcats: [{ id: "armchairs", name: "Armchairs" }] }],
    images: [
      { id: "img-armchair-1", url: "https://images.unsplash.com/photo-1519947486511-46149fa0a254?auto=format&fit=crop&w=1200&q=80" },
      { id: "img-armchair-2", url: "https://images.unsplash.com/photo-1519710164239-da123dc03ef4?auto=format&fit=crop&w=1200&q=80" }
    ]
  },
  {
    sales_post_guid: "12345678-1234-1234-1234-123456789004",
    guid: "87654321-4321-4321-4321-987654321004",
    personId: "user101",
    title: "Rattan Accent Chair",
    description: "Lightweight rattan chair with cushion. Scandinavian style. Perfect for any room.",
    size: "70x65x75cm",
    quantity: "1",
    price: 420,
    condition: "very-good",
    zip_code: "1200",
    status: { removed: false, date: null },
    color: { id: "natural", name: "Natural", href: "#D2B48C" },
    categories: [{ id: "living-room", name: "Living Room", subcats: [{ id: "armchairs", name: "Armchairs" }] }],
    images: [
      { id: "img-chair-1", url: "https://images.unsplash.com/photo-1506439773649-6e0eb8cfb237?auto=format&fit=crop&w=1200&q=80" }
    ]
  },
  {
    sales_post_guid: "12345678-1234-1234-1234-123456789005",
    guid: "87654321-4321-4321-4321-987654321005",
    personId: "user202",
    title: "Modern Dining Table",
    description: "Solid wood dining table, seats 6 comfortably. Walnut finish. Very sturdy construction.",
    size: "180x90x75cm",
    quantity: "1",
    price: 1200,
    condition: "excellent",
    zip_code: "3100",
    status: { removed: false, date: null },
    color: { id: "walnut", name: "Walnut", href: "#654321" },
    categories: [{ id: "dining-room", name: "Dining Room", subcats: [{ id: "tables", name: "Tables" }] }],
    images: [
      { id: "img-table-1", url: "https://images.unsplash.com/photo-1551632786-de41ec4a5fcd?auto=format&fit=crop&w=1200&q=80" }
    ]
  },
  {
    sales_post_guid: "12345678-1234-1234-1234-123456789006",
    guid: "87654321-4321-4321-4321-987654321006",
    personId: "user303",
    title: "White Bookshelf Unit",
    description: "Tall 5-shelf bookshelf in white lacquer. Perfect for storage and display.",
    size: "80x30x200cm",
    quantity: "1",
    price: 350,
    condition: "good",
    zip_code: "1050",
    status: { removed: false, date: null },
    color: { id: "white", name: "White", href: "#FFFFFF" },
    categories: [{ id: "storage", name: "Storage", subcats: [{ id: "shelves", name: "Shelves" }] }],
    images: [
      { id: "img-bookshelf-1", url: "https://images.unsplash.com/photo-1593642632630-e74f497e2d6c?auto=format&fit=crop&w=1200&q=80" }
    ]
  }
];

// Get the database
const db = db.getSiblingDB('furnituredatabase');

// Check if collection already has data
const count = db.furniture.countDocuments();

if (count === 0) {
  // Insert seed data
  db.furniture.insertMany(seedData);
  print(`✓ Successfully seeded ${seedData.length} furniture items to MongoDB`);
} else {
  print(`ℹ Database already contains ${count} items. Skipping seed.`);
}
