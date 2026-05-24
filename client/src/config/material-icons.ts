export const MATERIAL_ICONS = {
  // Shared utility classes for category icons.
  categoryClassName: "mb-2 leading-none",

  // Shared size class for category icons.
  categorySizeClassName: "text-[4rem]",

  // Material Symbols rendering styles.
  symbolStyle: {
    fontFamily: '"Material Symbols Rounded"',
    fontWeight: 300,
    fontStyle: "normal",
    lineHeight: 1,
    letterSpacing: "normal",
    textTransform: "none",
    display: "inline-block",
    whiteSpace: "nowrap",
    wordWrap: "normal",
    direction: "ltr",
    fontFeatureSettings: '"liga"',
    WebkitFontFeatureSettings: '"liga"',
    WebkitFontSmoothing: "antialiased",
  } as const,

  // Preferred icon per known top-level category label.
  categoryByName: {
    Chairs: "chair_Alt",
    Beds: "bed",
    Tables: "table_restaurant",
    Sofas: "weekend",
    Storage: "inventory_2",
    Lighting: "light",
  } as Record<string, string>,

  // Fallback cycle for categories not listed above.
  fallbackCycle: ["chair Alt", "bed", "table_restaurant", "weekend", "inventory_2", "light"],
} as const;
