using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a read-only view of a location.
    /// </summary>
    public record WarehouseDTORead(
        int id,
        string warehouseName,
        string description);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for inserting or updating a location.
    /// </summary>
    public record WarehouseDTOInsertUpdate(
        [Required(ErrorMessage = "Warehouse name required")]
        string? warehouseName,

        string description
        );

    /// <summary>
    /// Data Transfer Object (DTO) representing a read-only view of a person
    /// </summary>
    public record EmployeeDTORead(
        int id,
        string firstName,
        string lastName,
        string email,
        string password);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for inserting or updating a person
    /// </summary>
    public record EmployeeDTOInsertUpdate(
        [Required(ErrorMessage = "First name required")]
        string firstName,

        [Required(ErrorMessage = "Last name required")]
        string lastName,

        [Required(ErrorMessage = "Email name required")]
        string email,

        [MinLength(8, ErrorMessage = "{0} must be at least {1} characters long")]
        [Required(ErrorMessage = "Password required")]
        string password);

    /// <summary>
    /// Data Transfer Object (DTO) representing a read-only view of a product
    /// </summary>
    public record ProductDTORead(
        int id,
        string productName,
        string? description,
        bool? isUnitary);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for inserting or updating a product
    /// </summary>
    public record ProductDTOInsertUpdate(
        [Required(ErrorMessage = "Product name required")]
        string productName,

        string description,

        bool isUnitary);

    /// <summary>
    /// Data Transfer Object (DTO) representing a read-only view of a inventory transaction
    /// </summary>
    public record InventoryTransactionDTORead(
        int id,
        string? employeeName,
        string? transactionStatusName,
        DateTime? transactionDate,
        string? additionalDetails);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for inserting or updating a inventory transaction
    /// </summary>
    public record InventoryTransactionDTOInsertUpdate(
        [Required(ErrorMessage = "Employee required")]
        int? employeeId,

        [Required(ErrorMessage = "Transaction status required")]
        int? transactionStatusId,

        DateTime? transactionDate,

        string? additionalDetails);

    /// <summary>
    /// Data Transfer Object (DTO) representing a read-only view of a inventory transaction items
    /// </summary>
    public record InventoryTransactionItemDTORead(
        int id,
        int? transactionId,
        string? warehouseName,
        string? productName ,
        int? quantity);

    public record ProductsOnTransactionDTORead(
        int id,
        string? productName,
        bool? isUnitary,
        int? quantity);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for inserting or updating a inventory transaction items
    /// </summary>
    public record InventoryTransactionItemDTOInsertUpdate(
        [Required(ErrorMessage = "Inventory Transaction required")]
        int? transactionId,

        [Required(ErrorMessage = "Warehouse required")]
        int? warehouseId,

        [Required(ErrorMessage = "Product required")]
        int? productId,

        [Required(ErrorMessage = "Quantity required")]
        int? quantity);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for operator
    /// </summary>
    public record EmployeeAuthDTO(
        [Required(ErrorMessage = "Email required")]
        string? email,

        [Required(ErrorMessage = "Password required")]
        string? password);
}
