// MongoDB initialization script for seeding test data
// This script runs automatically when MongoDB container starts

db = db.getSiblingDB("furnitures");

db.SalesPost.drop();

const colorNames = ["Brown", "Black", "White", "Beige", "Grey"];
const colorHrefs = ["#8B5A2B", "#000000", "#FFFFFF", "#F5F5DC", "#808080"];

const createPost = (i, categoryName, subcategoryName, title, price, condition, city, imageKeyword) => ({
  sales_post_guid: `spg-${i}`,
  person_guid: `pg-${i}`,
  title,
  description: `${title} in ${condition.toLowerCase()} condition.`,
  size: "M",
  quantity: 1,
  price,
  condition,
  city,
  modifiedAt: new Date(),

  colors: [
    {
      colorid: `color-${i}`,
      colorGuid: `color-guid-${i}`,
      name: colorNames[i % colorNames.length],
      href: colorHrefs[i % colorHrefs.length],
    },
  ],

  categories: [
    {
      catid: `cat-${categoryName.toLowerCase().replaceAll(" ", "-")}`,
      name: categoryName,
      subcats: [
        {
          subid: `sub-${subcategoryName.toLowerCase().replaceAll(" ", "-")}`,
          name: subcategoryName,
        },
      ],
    },
  ],

  images: [
    {
      imageId: `img-${i}-1`,
      imageGuid: `image-guid-${i}-1`,
      url: `https://source.unsplash.com/800x600/?${imageKeyword}`,
    },
  ],
});

db.SalesPost.insertMany([
  createPost(1, "Couches", "U-shaped", "Large U-shaped Couch", 4500, "USED", "Copenhagen", "u-shaped-sofa"),
  createPost(2, "Couches", "3-person", "3-person Grey Sofa", 2800, "REFURBISHED", "Aarhus", "grey-sofa"),
  createPost(3, "Couches", "2-person", "Small 2-person Couch", 1600, "USED", "Odense", "small-sofa"),
  createPost(4, "Couches", "Chaiselong", "Couch with Chaiselong", 3500, "NEW", "Roskilde", "chaise-sofa"),
  createPost(5, "Couches", "Sofa bed", "Practical Sofa Bed", 2200, "USED", "Næstved", "sofa-bed"),

  createPost(6, "Chairs", "Dining chairs", "Set of Dining Chairs", 1200, "USED", "Copenhagen", "dining-chair"),
  createPost(7, "Chairs", "Armchairs", "Soft Armchair", 900, "REFURBISHED", "Aalborg", "armchair"),
  createPost(8, "Chairs", "Office chairs", "Ergonomic Office Chair", 1400, "USED", "Herlev", "office-chair"),
  createPost(9, "Chairs", "Kids chairs", "Small Kids Chair", 300, "NEW", "Hillerød", "kids-chair"),
  createPost(10, "Chairs", "Rocking chairs", "Wooden Rocking Chair", 1100, "USED", "Køge", "rocking-chair"),

  createPost(11, "Tables", "Dining tables", "Oak Dining Table", 3200, "USED", "Copenhagen", "dining-table"),
  createPost(12, "Tables", "Coffee tables", "Modern Coffee Table", 800, "REFURBISHED", "Aarhus", "coffee-table"),
  createPost(13, "Tables", "Side tables", "Small Side Table", 400, "USED", "Odense", "side-table"),
  createPost(14, "Tables", "Desks", "Work Desk", 1000, "NEW", "Roskilde", "desk"),
  createPost(15, "Tables", "Console tables", "Narrow Console Table", 700, "USED", "Næstved", "console-table"),

  createPost(16, "Beds", "180x200", "Large Double Bed 180x200", 4000, "USED", "Copenhagen", "bed"),
  createPost(17, "Beds", "160x200", "Double Bed 160x200", 3000, "REFURBISHED", "Aarhus", "double-bed"),
  createPost(18, "Beds", "120x200", "Single Bed 120x200", 1800, "USED", "Odense", "single-bed"),
  createPost(19, "Beds", "Kids beds", "Children's Bed", 900, "NEW", "Herlev", "kids-bed"),
  createPost(20, "Beds", "Baby beds", "Baby Crib", 700, "USED", "Køge", "baby-crib"),

  createPost(21, "Lamps", "Ceiling lamps", "Minimalist Ceiling Lamp", 600, "NEW", "Copenhagen", "ceiling-lamp"),
  createPost(22, "Lamps", "Wall lamps", "Brass Wall Lamp", 450, "USED", "Aalborg", "wall-lamp"),
  createPost(23, "Lamps", "Table lamps", "Ceramic Table Lamp", 350, "REFURBISHED", "Odense", "table-lamp"),
  createPost(24, "Lamps", "Floor lamps", "Tall Floor Lamp", 750, "USED", "Roskilde", "floor-lamp"),
  createPost(25, "Lamps", "Desk lamps", "Adjustable Desk Lamp", 250, "NEW", "Næstved", "desk-lamp"),
]);
