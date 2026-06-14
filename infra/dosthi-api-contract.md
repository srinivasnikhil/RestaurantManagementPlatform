# Dosthi Platform: API Contract and Wireframe Specs (Day 1 design output)

This is your reference for Days 2 through 8. Read it now; build it later.
Base URL (local): `https://localhost:5001/api`
Auth: JWT Bearer token in the `Authorization` header where marked **[auth]**. Admin-only marked **[admin]**.

All list endpoints return arrays; all create endpoints return the created resource with its new `id`.
Standard error shape: `{ "error": "message", "details": [...] }` with the right HTTP status code.

---

## Auth

| Method | Path | Auth | Purpose | Request body | Returns |
|---|---|---|---|---|---|
| POST | `/auth/register` | - | Create account | `{ name, email, password }` | `{ token, user }` |
| POST | `/auth/login` | - | Log in | `{ email, password }` | `{ token, user }` |
| GET | `/auth/me` | [auth] | Current user profile | - | `{ user }` |

---

## Categories

| Method | Path | Auth | Purpose |
|---|---|---|---|
| GET | `/categories` | - | List active categories, ordered |
| GET | `/categories/{id}` | - | One category |
| POST | `/categories` | [admin] | Create category |
| PUT | `/categories/{id}` | [admin] | Update category |
| DELETE | `/categories/{id}` | [admin] | Soft-delete (set IsActive false) |

---

## Menu items

| Method | Path | Auth | Purpose |
|---|---|---|---|
| GET | `/menu-items?categoryId=&isVeg=&search=` | - | List with optional filters |
| GET | `/menu-items/{id}` | - | One item with its options and avg rating |
| POST | `/menu-items` | [admin] | Create item |
| PUT | `/menu-items/{id}` | [admin] | Update item |
| DELETE | `/menu-items/{id}` | [admin] | Remove item |
| PATCH | `/menu-items/{id}/availability` | [admin] | Toggle IsAvailable |

Item options live under their item:
| POST | `/menu-items/{id}/options` | [admin] | Add an option |
| DELETE | `/menu-items/{id}/options/{optionId}` | [admin] | Remove an option |

---

## Cart

| Method | Path | Auth | Purpose |
|---|---|---|---|
| GET | `/cart` | [auth] | Get my cart with line items and totals |
| POST | `/cart/items` | [auth] | Add item `{ menuItemId, quantity, selectedOptions, notes }` |
| PUT | `/cart/items/{cartItemId}` | [auth] | Update quantity/options |
| DELETE | `/cart/items/{cartItemId}` | [auth] | Remove a line |
| DELETE | `/cart` | [auth] | Clear cart |

---

## Orders

| Method | Path | Auth | Purpose |
|---|---|---|---|
| POST | `/orders` | [auth] | Place order from cart `{ type, paymentIntentId }` |
| GET | `/orders` | [auth] | My order history |
| GET | `/orders/{id}` | [auth] | One order with items and status |
| GET | `/orders/all` | [admin] | All orders (admin board) |
| PATCH | `/orders/{id}/status` | [admin] | Update status `{ status }` |

---

## Payments

| Method | Path | Auth | Purpose |
|---|---|---|---|
| POST | `/payments/intent` | [auth] | Create Stripe payment intent for current cart total, returns `clientSecret` |
| POST | `/payments/webhook` | - | Stripe webhook (verify signature; mark payment paid/failed) |

---

## Reservations

| Method | Path | Auth | Purpose |
|---|---|---|---|
| POST | `/reservations` | [auth] | Book `{ reservationDateTime, partySize, notes }` |
| GET | `/reservations` | [auth] | My reservations |
| GET | `/reservations/all` | [admin] | All reservations |
| PATCH | `/reservations/{id}/status` | [admin] | Confirm or cancel |

---

## Reviews

| Method | Path | Auth | Purpose |
|---|---|---|---|
| GET | `/menu-items/{id}/reviews` | - | Reviews for an item |
| POST | `/menu-items/{id}/reviews` | [auth] | Add review `{ rating, comment }` |
| DELETE | `/reviews/{id}` | [auth] | Delete my own review |

---

## Restaurant info

| Method | Path | Auth | Purpose |
|---|---|---|---|
| GET | `/restaurant` | - | Name, address, phone, location |
| GET | `/restaurant/hours` | - | Operating hours grouped by day |

---

## AI service (separate FastAPI app, Days 9 to 10)

Base URL: `http://localhost:8000`

| Method | Path | Purpose |
|---|---|---|
| POST | `/recommend` | `{ preferences, budget }` returns suggested menu items |
| POST | `/chat` | `{ messages }` menu assistant reply (optionally streamed) |
| POST | `/search` | `{ query }` semantic menu search returns ranked item ids |
| POST | `/summarize-reviews` | `{ menuItemId, reviews }` returns a short summary |

The .NET API or the frontend calls these. Keep all AI logic here, not in the main backend.

---

# Wireframe screen specs

Sketch these in Figma as plain boxes. Goal is layout and flow, not visuals.

**Home / Menu**
- Top: logo, nav (Menu, Reservations, My Orders, Login/Profile), cart icon with count
- Hero strip: restaurant name, tagline, "Order now" button
- Category tabs or sidebar (Dosa, Biryani, Idly, Curries, Drinks, etc.)
- Grid of item cards: image, name, short description, price, veg/non-veg dot, "Add" button
- Search bar (later wired to AI semantic search)

**Item detail**
- Large image, name, description, price, rating
- Spice level selector, add-on options (checkboxes with extra price)
- Quantity stepper, notes field, "Add to cart" button
- Reviews list below

**Cart**
- Line items: name, options, quantity stepper, line total, remove
- Subtotal, tax, total
- "Proceed to checkout" button

**Checkout**
- Order type toggle (Pickup / Delivery)
- Delivery address field (if delivery)
- Order summary
- Stripe card element
- "Place order" button

**Order tracking**
- Order number, placed time
- Status stepper: Placed -> Confirmed -> Preparing -> Ready -> Completed
- Item list, total

**Reservation**
- Date/time picker, party size, notes
- "Book table" button
- My upcoming reservations list

**Admin orders**
- Table of orders: id, customer, total, status dropdown, time
- Filter by status
- Tab to manage menu items (add/edit/toggle availability)

---

## Notes to self while building

- DTOs are not entities. Never return EF entities straight from controllers.
- Validate every incoming body. Reject early with 400 and a clear message.
- Money is `decimal`, never `double` or `float`.
- Capture `UnitPrice` onto OrderItem at order time so old orders stay accurate.
- Protect the money paths (auth, place order, payment) with tests first.
