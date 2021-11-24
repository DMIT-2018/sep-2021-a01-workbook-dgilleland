---
title: Customer Orders
---
# Customer Orders

The customers of Northwind Traders place their orders by phone or fax. Employees then enter the details of the order into the system via the **Customer Order Form** (from the *Sales* menu item on the website).

The form allows employees to create new orders and view previous orders. Any order whose *Order Date* has been set cannot be modified, because that order has been *"Placed"*.

An order without an Order Date can be modified. It may be an order that is *"In Progress"* (saved, but not placed), or an entirely new order.

The screen mockups in the following sections describe the planned user experience (UX) in working with the form.

----

## Open the Sales Page

The first visit to the Sales page simply displays a list of existing customers that the employee can select from to view the customer's order history or to create a new order for the customer.

> ![Sales Page - First Visit](./images/First-Visit.png)

<!--
For the drop-down, only the Company Name and Customer Id are required on the form, so a simple POCO class geared to the needs of a drop-down is needed.

> ![Sales Page - Data Query for First Visit](./images/Query-First-Visit.png)

```csharp
public class KeyValueOption
{
    public string Key { get; set; }
    public string Text { get; set; }
}
```
-->

Clicking on the [Select] button will trigger the next major view of this page: the Selected Customer.

----

## Selected Customer

When a customer is selected, the general company information is displayed, along with the order history of the customer. Order history is divided into two sets - Orders that have been shipped, and orders that have not been shipped (i.e.: "Open" orders).

> ![Customer Order History](./images/Selected-Company.png)

<!--
When querying the database for this information, it quickly becomes clear that POCOs/DTOs are the most appropriate way to send information to the form.

> ![Customer Order History - Data Query](./images/Query-Selected-Company.png)

General customer information can be represented by a simple POCO, and the data is retrieved via code-behind. Part of the reason for this is that the data must be un-packed manually in the code-behind, since it is a *single* customer whose information is retrieved.

```csharp
public class CustomerSummary
{
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string Phone { get; set; }
    public string Fax { get; set; }
}
```

Likewise, each line of the order history details can also be represented by a simple POCO. In this case, however, the data can be retrieved either manually in code-behind or through an `<asp:ObjectDataSource>` control.

```csharp
public class CustomerOrder
{
    public int OrderId { get; set; }
    public string Employee { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public string Shipper { get; set; }
    public decimal? Freight { get; set; }
    public decimal OrderTotal { get; set; }
}
```
-->

----

## Edit New/Existing Order

When editing a new or existing order, **summary information** on the order needs to be available (relevant dates, shipping info, order total) as well as **detail information** on the order items.

> ![Edit Order View](./images/Edit-Order.png)

If the order is new or **not placed** (which means that there is no `OrderDate`), then the order can be edited and saved. Once an order is placed (i.e., the *Order Date* is set), then the order cannot be edited. This makes the form helpful for viewing details of placed orders (shipped or not shipped) while protecting the data from being inadvertantly changed.

----

<!--

## Query Responsibility

Loading the form with an order populates the summary and detail information. This information is obtained from a single call to the BLL, which retrieves a DTO constructed from parts of the entities. Drop-downs use the same general-purpose POCO as was used earlier when displaying customers.

> ![Query Open Order](./images/Query-Open-Order.png "Query Open Order")

The DTO/POCO used for populating the order summary/details uses the following classes.

```csharp
public class CustomerOrderWithDetails : CustomerOrder
{
    public IEnumerable<CustomerOrderItem> Details { get; set; }
        = new List<CustomerOrderItem>();
}
```

```csharp
public class CustomerOrderItem : ProductItem
{
    public int OrderId { get; set; }
    public short Quantity { get; set; }
    public float DiscountPercent { get; set; }
}
```

```csharp
public class ProductItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string QuantityPerUnit { get; set; }
    public decimal UnitPrice { get; set; }
    public short? InStockQuantity { get; set; }
}
```

You may notice that the `ProductItem` is the base class for the `CustomerOrderItem`. This class inheritance was introduced because the `ProductItem` information is needed in another code-behind query - the one used to add items to the order.

----
-->

## Form Behaviour

When editing the order, all changes are persisted on the form only, rather than making hits to the BLL/DAL on each minor change. The reason for this is that the edit form is functioning as a **working document** - changes are not persisted unless the user clicks the `[Save]` or `[Place Order]` buttons.

> ![Add/Remove Items](./images/Form-State-Edit-Order.png)

<!--
As such, this requires a good deal of code-behind processing on the form. For example, when the user selects a product to add to the order, the pricing information is queried from the database to fill in the suggested order price. But when the user clicks the [Add] button, all of this information is manually placed into the list view's data.

Processes like this can make use of additional queries to the database (as in the query for `ProductItem` details), but no commands are issued to the BLL.

```csharp
public class ProductItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string QuantityPerUnit { get; set; }
    public decimal UnitPrice { get; set; }
    public short? InStockQuantity { get; set; }
}
```
----

## Command Responsibility
-->

Editing the order is done by adding and removing order items as well as selecting the shipper, setting the freight, and entering the date that the order is required. None of these editing actions perform an update on the database directly. Rather, the changes are simply preserved on the form itself through the form's code-behind.

> ![Form State](./images/Form-State-Edit-Order.png)

Once all the editing is done, the user can either **Save** or **Place** the order. Both of these actions are performed as distinct transactions, meaning that all the relevant data from the form is sent to the BLL through a DTO.

<!--
> ![Save Order](./images/Command-Save-Order.png)

```csharp
public class EditOrderItem
{
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public short OrderQuantity { get; set; }
    public float DiscountPercent { get; set; }
}
```

```csharp
public class EditCustomerOrder
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public int? ShipperId { get; set; }
    public decimal? FreightCharge { get; set; }
    public IEnumerable<EditOrderItem> OrderItems { get; set; }
}
```
-->

----

## Saving Customer Orders

Saving a customer order is done to preserve changes to an order while keeping it open (i.e., not setting the `OrderDate`). The customer order to be saved may be a new order or an existing open order. Both of these are handled by a single public BLL method, which internally shunts the main work of saving to two alternate private methods. Each private method ensures that their specific tasks are performed as **transactions** that succeed or fail as a group. The following sequence diagram roughly illustrates the process.

![Save Customer Order - Sequence Diagram](./images/Northwind-CustomerSales-Save.png)

<!--
```csharp
public void Save(EditCustomerOrder order)
{
    // Always ensure you have been given data to work with
    if (order == null)
        throw new ArgumentNullException("order", "Cannot save order; order information was not supplied.");

    // Business validation rules
    if (order.OrderDate.HasValue)
        throw new Exception($"An order date of {order.OrderDate.Value.ToLongDateString()} has been supplied. The order date should only be supplied when placing orders, not saving them.");

    // Decide whether to add new or update
    //  NOTE: Notice that no db activity is occuring yet.
    if (order.OrderId == 0)
        AddPendingOrder(order);
    else
        UpdatePendingOrder(order);
}
```

The `Save()` method performs initial validation, ensuring that the order exists and that the order date is not set. In either case, the order is open (or "pending"), and the work of saving changes ir routed to two separate methods, each of which ensure that the order is **processed as a *single* transaction**.

```csharp
private void AddPendingOrder(EditCustomerOrder order)
{
    using (var context = new NorthwindContext())
    {
        var orderInProcess = context.Orders.Add(new Order());
        // Make the orderInProcess match the customer order as given...
        // A) The general order information
        orderInProcess.CustomerID = order.CustomerId;
        orderInProcess.EmployeeID = order.EmployeeId;
        orderInProcess.OrderDate = order.OrderDate;
        orderInProcess.ShipVia = order.ShipperId;
        orderInProcess.Freight = order.FreightCharge;
        // B) Add order details
        foreach (var item in order.OrderItems)
        {
            // Add as a new item
            var newItem = new OrderDetail
            {
                ProductID = item.ProductId,
                Quantity = item.OrderQuantity,
                UnitPrice = item.UnitPrice,
                Discount = item.DiscountPercent
            };
            orderInProcess.OrderDetails.Add(newItem);
        }

        // C) Save the changes (one save, one transaction)
        context.SaveChanges();
    }
}
```

```csharp
private void UpdatePendingOrder(EditCustomerOrder order)
{
    using (var context = new NorthwindContext())
    {
        var orderInProcess = context.Orders.Find(order.OrderId);
        // Make the orderInProcess match the customer order as given...
        // A) The general order information
        orderInProcess.CustomerID = order.CustomerId;
        orderInProcess.EmployeeID = order.EmployeeId;
        orderInProcess.OrderDate = order.OrderDate;
        orderInProcess.ShipVia = order.ShipperId;
        orderInProcess.Freight = order.FreightCharge;

        // B) Add/Update/Delete order details
        //    Loop through the items as known in the database (to update/remove)
        foreach (var detail in orderInProcess.OrderDetails)
        {
            var changes = order.OrderItems.SingleOrDefault(x => x.ProductId == detail.ProductID);
            if (changes == null)
                context.Entry(detail).State = EntityState.Deleted; // flag for deletion
            else
            {
                detail.Discount = changes.DiscountPercent;
                detail.Quantity = changes.OrderQuantity;
                detail.UnitPrice = changes.UnitPrice;
                context.Entry(detail).State = EntityState.Modified;
            }
        }
        //    Loop through the new items to add to the database
        foreach (var item in order.OrderItems)
        {
            bool notPresent = !orderInProcess.Order_Details.Any(x => x.ProductID == item.ProductId);
            if (notPresent)
            {
                // Add as a new item
                var newItem = new Order_Detail
                {
                    ProductID = item.ProductId,
                    Quantity = item.OrderQuantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.DiscountPercent
                };
                orderInProcess.Order_Details.Add(newItem);
            }
        }

        // C) Save the changes (one save, one transaction)
        context.Entry(orderInProcess).State = EntityState.Modified;
        context.SaveChanges();
    }
}
```
-->

----

## Placing Customer Orders

Placing customer orders is much like saving, except that the order date must be set.

<!--
```csharp
public void PlaceOrder(EditCustomerOrder order)
{
    // Always ensure you have been given data to work with
    if (order == null)
        throw new ArgumentNullException("order", "Cannot place order; order information was not supplied.");

    // Business validation rules
    if (!order.RequiredDate.HasValue)
        throw new Exception($"A  required date for the order is required when placing orders.");
    if (!order.OrderDate.HasValue)
        throw new Exception($"An order date is required when placing orders.");
    if (!order.ShipperId.HasValue)
        throw new Exception("A shipper must be identified before placing an order.");
    if (order.OrderItems.Count() == 0)
        throw new Exception("An order must have at least one item before it can be placed.");

    // Begin processing the order
    using (var context = new NorthwindContext())
    {
        // Prep for processing...
        var customer = context.Customers.Find(order.CustomerId);
        if (customer == null)
            throw new Exception("Customer does not exist");
        var orderInProcess = context.Orders.Find(order.OrderId);
        if (orderInProcess == null)
            orderInProcess = context.Orders.Add(new Order());
        else
            context.Entry(orderInProcess).State = EntityState.Modified;
        // Make the orderInProcess match the customer order as given...
        // A) The general order information
        orderInProcess.CustomerID = order.CustomerId;
        orderInProcess.EmployeeID = order.EmployeeId;
        orderInProcess.OrderDate = order.OrderDate;
        orderInProcess.RequiredDate = order.RequiredDate;
        orderInProcess.ShipVia = order.ShipperId;
        orderInProcess.Freight = order.FreightCharge;
        // B) Default the ship-to info to the customer's info
        orderInProcess.ShipName = customer.CompanyName;
        orderInProcess.ShipAddress = customer.Address;
        orderInProcess.ShipCity = customer.City;
        orderInProcess.ShipRegion = customer.Region;
        orderInProcess.ShipPostalCode = customer.PostalCode;

        // C) Add/Remove/Update order details
        //var toRemove = new List<OrderDetail>();
        foreach (var detail in orderInProcess.OrderDetails)
        {
            var changes = order.OrderItems.SingleOrDefault(x => x.ProductId == detail.ProductID);
            if (changes == null)
                //toRemove.Add(detail);
                context.Entry(detail).State = EntityState.Deleted; // flag for deletion
            else
            {
                detail.Discount = changes.DiscountPercent;
                detail.Quantity = changes.OrderQuantity;
                detail.UnitPrice = changes.UnitPrice;
                context.Entry(detail).State = EntityState.Modified;
            }
        }
        foreach (var item in order.OrderItems)
        {
            if (!orderInProcess.OrderDetails.Any(x => x.ProductID == item.ProductId))
            {
                // Add as a new item
                var newItem = new OrderDetail
                {
                    ProductID = item.ProductId,
                    Quantity = item.OrderQuantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.DiscountPercent
                };
                orderInProcess.OrderDetails.Add(newItem);
            }
        }

        // D) Save the changes (one save, one transaction)
        context.SaveChanges();
    }
}
```
-->