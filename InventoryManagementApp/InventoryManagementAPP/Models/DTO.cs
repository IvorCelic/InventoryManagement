﻿namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a read-only view of a location.
    /// </summary>
    public record LocationDTORead(int id, string name, string description);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for inserting or updating a location.
    /// </summary>
    public record LocationDTOInsertUpdate(string name, string description);

    /// <summary>
    /// Data Transfer Object (DTO) representing a read-only view of a person
    /// </summary>
    public record PersonDTORead(int id, string firstName, string lastName, string email, string password);

    /// <summary>
    /// Data Transfer Object (DTO) representing the data needed for inserting or updating a person
    /// </summary>
    public record PersonDTOInsertUpdate(string firstName, string lastName, string email, string password);
}
