# Styling - iREDO Furniture Client

---
## TLDR
 
We use **Base UI + Tailwind CSS + Shadcn/UI** together. 

**Base UI** handles all the UI behaviour and accessibility (keyboard navigation, focus trapping, screen readers) invisibly under the hood. 

**Tailwind** handles all the visual styling via utility classes, with colours and fonts defined once in `tailwind.config.ts` and `index.css` so changes cascade everywhere. 

**Shadcn/UI** gives us prebuilt components (buttons, cards, dialogs etc.) that are copied directly into `src/components/ui/` — meaning we own and can edit the code freely. 

We chose Base UI over Radix UI (the other Shadcn/ui option) because it has a single `@base-ui/react` dependency instead of one package per component, and better long-term maintenance.
 
---

## Overview

The styling setup is built in three layers, each with a distinct responsibility:

```
Base UI       →  UI behaviour & accessibility  (invisible to the user)
Tailwind CSS  →  Visual styling & design tokens (colours, spacing, fonts)
Shadcn/UI     →  Prebuilt styled components     (buttons, dialogs, cards etc.)
```

These three work together but are independent UI parts. 

---

## Layer 1 — Base UI (Behaviour)

[Base UI](https://base-ui.com/) is a **headless component library** made by the MUI team. "Headless" means it provides behaviour and accessibility to UI components like a dropdown menu without styling.

It handles the parts of UI behaviour that are easy to forget:
- Dialog and modal focus trapping
- Dropdown keyboard navigation (arrow keys, escape, tab)
- Accessible checkboxes, switches, and selects

Shadcn/UI adds the actual styling on top of the UI behavior. This can be done using Radix UI or Base UI.

**Why Base UI over Radix UI**:

| | Radix UI | Base UI |
|---|---|---|
| **Dependency** | One package per component e.g. `@radix-ui/react-dialog`, `@radix-ui/react-select` | Single package: `@base-ui/react` |
| **Maintenance** | Acquired by WorkOS — community has raised concerns about long-term upkeep & bugs | Actively maintained by the MUI team |
| **Maturity** | Battle-tested, used widely for years | Reached v1.0 stable in December 2025 |
| **Component API** | Verbose HTML structure, strict and correct nesting necessary | Supports render prop pattern, for inserting custom UI elements |

We use Base UI because the single `@base-ui/react` dependency keeps `package.json` clean, and it has stronger long-term maintenance backing.

---

## Layer 2 — Tailwind CSS (Visual Styling)

[Tailwind](https://tailwindcss.com/) is a utility-first CSS framework. Instead of writing separate CSS files, you apply small utility classes directly in JSX:

```tsx
<button className="flex items-center gap-2 rounded-lg bg-walnut px-4 py-2 text-cream">
  Buy now
</button>
```

Tailwind is the **visual design** base — it controls the colours, spacing, typography, and layout. Shadcn/UI components are written in Tailwind, so the two work together.

**Pros:**
- **Global theme in one place** — colours, fonts, and spacing are defined once in `tailwind.config.ts` and available as classes everywhere
- **No naming CSS classes** — avoids the classic problem of inventing class names like `.card-inner-wrapper-left`
- **No unused CSS in production** — Tailwind only ships the classes actually used in the project
- **Consistent design system** — predefined spacing and colour scales keep the UI consistent across the project

**Cons:**
- JSX can look cluttered with many utility classes on one element
    - In practice this is mostly hidden in Shadcn components files
- Learning curve if the team hasn't used Tailwind before
- Requires setup and configuration compared to importing a stylesheet in HTML

---

## Layer 3 — Shadcn/UI (Prebuilt Components)

[Shadcn/UI](https://ui.shadcn.com/) is not a traditional component library you install as a package. Instead, you copy individual component source files directly into your project under `src/components/ui/`. Each component is a Base UI primitive wrapped in Tailwind styling — and since you own the code, you can edit anything freely.

```bash
npx shadcn@latest add button   # copies button.tsx into src/components/ui/
npx shadcn@latest add card     # great for listing cards
npx shadcn@latest add dialog   # for listing detail modals
npx shadcn@latest add input    # search bar
```

**Pros:**
- **You own the code** — no black box, every component is a file in your project you can read and edit freely
- **Accessibility for free** — Base UI primitives handle keyboard navigation, focus trapping, and screen reader support automatically
- **Global theming** — all components reference the CSS variables in `index.css`, so changing the theme cascades everywhere at once
- **No extra runtime dependency** — components are just files, not a package that ships to the browser
 
**Cons:**
- **More files in the project** — every component you add lives in `src/components/ui/`
- **Learning curve** — unfamiliar if the team has only used libraries like Bootstrap or Chakra before
- **Manual updates** — you are responsible for keeping copied components up to date if shadcn releases changes

## Install
 
```bash
# Tailwind
npm install tailwindcss @tailwindcss/vite
 
# Shadcn (also sets up Base UI and Tailwind integration interactively)
npx shadcn@latest init
```
 
`npx shadcn@latest init` walks through setup interactively — choose **Base UI** when prompted for the primitive library.
 
---


### Global Theme Example

Colours, fonts, and other design tokens are defined once in `tailwind.config.ts`:

```ts
export default {
  theme: {
    extend: {
      colors: {
        sand: '#e8d5b7',
        walnut: '#5c3d2e',
        cream: '#faf6f0',
        terracotta: '#c4622d',
      },
      fontFamily: {
        sans: ['Instrument Sans', 'sans-serif'],
        display: ['Playfair Display', 'serif'],
      },
    }
  }
}
```

Shadcn's component variables are defined once in `index.css`:

```css
:root {
  --background: 36 33% 97%;       /* warm off-white */
  --foreground: 20 25% 15%;       /* dark warm brown */
  --primary: 25 60% 35%;          /* walnut brown */
  --destructive: 10 72% 45%;      /* terracotta for cancel/delete buttons */
  --card: 36 25% 94%;             /* warm card background */
  --border: 30 20% 82%;           /* soft warm border */
}
```

Changing `--primary` once updates every primary button, link, and accent across the entire app automatically.

---
---

## Alternatives Considered

### Bootstrap
- Has a very recognizable, generic look that is clean but cold and corporate
- Easy to implement and includes both UI logic and visual styling in one package
- Not chosen because:
    - The goal is a warm, original-feeling marketplace — Bootstrap works against that aesthetic by default
    - Not built for React/TypeScript — requires `react-bootstrap` as a wrapper

### Chakra UI
- Uses an opinionated layout system (Stack, Grid) which is beginner-friendly but limiting for custom layouts
- Built-in TypeScript integration
- Not chosen because:
    - Shadcn/UI gives more control without added complexity
    - Can make the runtime slower
    - Higher chance of inconsistent custom styling
    - Fewer components available compared to Shadcn/UI
    - Less customizable than Shadcn/UI — harder to deviate from its default look

### Material UI (MUI)
- Very large component library, well maintained
- Based on Google's Material Design — opinionated, corporate look that is hard to customize away from
- Not chosen because:
    - the Material Design aesthetic works against the warm, original look we're going for
    - Large bundle size impact compared to Shadcn/UI

