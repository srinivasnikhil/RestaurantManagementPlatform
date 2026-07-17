export interface Category {
  id: number;
  name: string;
  displayOrder: number;
  isActive: boolean;
}

export interface MenuItem {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl?: string;
  isVeg: boolean;
  spiceLevel: number;
  isAvailable: boolean;
  categoryId: number;
  categoryName: string;
  averageRating: number;
}

export interface CartItem {
  id: number;
  menuItemId: number;
  menuItemName: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
  selectedOptions?: string;
  notes?: string;
}

export interface Cart {
  id: number;
  items: CartItem[];
  subtotal: number;
}

export interface OrderItem {
  menuItemId: number;
  menuItemName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
  selectedOptions?: string;
}

export interface Order {
  id: number;
  status: string;
  type: string;
  subtotal: number;
  tax: number;
  total: number;
  createdAt: string;
  trackingCode: string;
  customerName?: string;
  customerPhone?: string;
  customerAddress?: string;
  items: OrderItem[];
}

export interface DailyRevenue {
  date: string;
  revenue: number;
}

export interface TopItem {
  name: string;
  quantitySold: number;
  revenue: number;
}

export interface StatusCount {
  status: string;
  count: number;
}

export interface Dashboard {
  revenueToday: number;
  revenueThisWeek: number;
  revenueThisMonth: number;
  revenueAllTime: number;
  ordersToday: number;
  ordersAllTime: number;
  averageOrderValue: number;
  revenueTrend: DailyRevenue[];
  topItems: TopItem[];
  statusBreakdown: StatusCount[];
}