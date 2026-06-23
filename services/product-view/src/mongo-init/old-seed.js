db = db.getSiblingDB("furnitures");

db.SalesPost.insertMany([
  {
    sales_post_guid: "spg-1",
    person_guid: "pg-1",
    title: "Oak Dining Chair",
    description: "Solid oak chair with natural finish",
    size: "M",
    quantity: 4,
    price: 120,
    condition: 1,

    colors: [
      {
        colorid: "c-1",
        colorGuid: "cg-1",
        name: "Brown",
        href: "#8B5A2B"
      },
      {
        colorid: "c-2",
        colorGuid: "cg-2",
        name: "Beige",
        href: "#F5F5DC"
      }
    ],

    categories: [
      {
        catid: "cat-1",
        name: "Furniture",
        subcats: [
          {
            subid: "sub-1",
            name: "Chairs"
          }
        ]
      }
    ],

    images: [
      {
        imageId: "img-1",
        imageGuid: "imgg-1",
        url: "https://example.com/images/chair1.jpg"
      },
      {
        imageId: "img-2",
        imageGuid: "imgg-2",
        url: "https://example.com/images/chair2.jpg"
      }
    ]
  },

  {
    sales_post_guid: "spg-2",
    person_guid: "pg-2",
    title: "Modern Coffee Table",
    description: "Glass top coffee table with metal frame",
    size: "L",
    quantity: 1,
    price: 300,
    condition: 2,

    colors: [
      {
        colorid: "c-3",
        colorGuid: "cg-3",
        name: "Black",
        href: "#000000"
      },
      {
        colorid: "c-4",
        colorGuid: "cg-4",
        name: "Clear",
        href: "#FFFFFF"
      }
    ],

    categories: [
      {
        catid: "cat-2",
        name: "Furniture",
        subcats: [
          {
            subid: "sub-2",
            name: "Tables"
          }
        ]
      }
    ],

    images: [
      {
        imageId: "img-3",
        imageGuid: "imgg-3",
        url: "https://example.com/images/table1.jpg"
      }
    ]
  },

  {
    sales_post_guid: "spg-3",
    person_guid: "pg-3",
    title: "Bookshelf Unit",
    description: "5-tier wooden bookshelf",
    size: "XL",
    quantity: 2,
    price: 180,
    condition: 0,

    colors: [
      {
        colorid: "c-5",
        colorGuid: "cg-5",
        name: "White",
        href: "#FFFFFF"
      }
    ],

    categories: [
      {
        catid: "cat-3",
        name: "Furniture",
        subcats: [
          {
            subid: "sub-3",
            name: "Storage"
          }
        ]
      }
    ],

    images: [
      {
        imageId: "img-4",
        imageGuid: "imgg-4",
        url: "https://example.com/images/shelf1.jpg"
      },
      {
        imageId: "img-5",
        imageGuid: "imgg-5",
        url: "https://example.com/images/shelf2.jpg"
      },
      {
        imageId: "img-6",
        imageGuid: "imgg-6",
        url: "https://example.com/images/shelf3.jpg"
      }
    ]
  }
]);