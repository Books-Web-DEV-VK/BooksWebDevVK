Dual-Category RBAC: The system distinguishes between Internal Staff (Admins, Employees) and External Clients (Customers, Company Admins).

Staff Management: Only Admins have the authority to create, update, or manage Admin and Employee accounts.

Data Governance: Both Admins and Employees have full CRUD (Create, Read, Update, Delete) permissions for Books, Companies, and User records.

Bulk Purchasing Power: Company Admins are specifically authorized to perform bulk book orders, whereas standard Customers are limited to retail quantities.

Financial Logic (Waivers): Company Admins enjoy a payment waiver (credit system), while standard Customers must provide immediate payment at checkout.

Logistics Control: Admins and Employees are the only roles permitted to move orders through the "Processing," "Shipping," and "Delivery" stages.

External Onboarding: Customers and Company Admins can self-register on the platform, whereas staff accounts must be provisioned.

Social Auth Integration: The platform supports Social Login (e.g., Facebook) to streamline the registration and login process for external users.

Company Relationships: The system tracks Company Admins as corporate entities separate from individual retail Customers.

Operational Flow: The workflow is designed so that clients (External) initiate the sales data, while staff (Internal) manage the inventory and fulfill the physical delivery.
