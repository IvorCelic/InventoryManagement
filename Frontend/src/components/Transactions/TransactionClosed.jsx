import React, { useState } from "react";
import { Col, Nav, NavDropdown, Row, Table } from "react-bootstrap";

function TransactionClosed({
    activeTab,
    handleTabChange,
    associatedWarehouses,
    products,
    productsOnWarehouse,
    isUnitary,
}) {
    const [selectedWarehouseName, setSelectedWarehouseName] = useState("Select warehouse");

    const handleWarehouseSelect = (warehouseId, warehouseName) => {
        setSelectedWarehouseName(warehouseName);
        handleTabChange(warehouseId);
    };

    return (
        <Col lg={8} md={12} sm={12} className="mt-5 transactionEditContainer">
            <Row className="horizontal-tabs-container">
                <Nav className="horizontal-tabs d-none d-md-flex">
                    <Nav.Item>
                        <Nav.Link
                            eventKey="all"
                            active={activeTab === "all"}
                            onClick={() => handleWarehouseSelect("all", "All warehouses")}
                        >
                            All
                        </Nav.Link>
                    </Nav.Item>
                    {associatedWarehouses &&
                        associatedWarehouses.map((warehouse) => (
                            <Nav.Item key={warehouse.id}>
                                <Nav.Link
                                    eventKey={warehouse.id}
                                    active={activeTab === warehouse.id}
                                    onClick={() =>
                                        handleWarehouseSelect(warehouse.id, warehouse.warehouseName)
                                    }
                                >
                                    {warehouse.warehouseName}
                                </Nav.Link>
                            </Nav.Item>
                        ))}
                </Nav>
                <Nav className="horizontal-tabs d-md-none">
                    <Nav.Item>
                        <NavDropdown title={selectedWarehouseName} id="nav-dropdown">
                            <NavDropdown.Item
                                eventKey="all"
                                active={activeTab === "all"}
                                onClick={() => handleWarehouseSelect("all", "All warehouses")}
                            >
                                All warehouses
                            </NavDropdown.Item>
                            {associatedWarehouses &&
                                associatedWarehouses.map((warehouse) => (
                                    <NavDropdown.Item
                                        key={warehouse.id}
                                        eventKey={warehouse.id}
                                        active={activeTab === warehouse.id}
                                        onClick={() =>
                                            handleWarehouseSelect(
                                                warehouse.id,
                                                warehouse.warehouseName
                                            )
                                        }
                                    >
                                        {warehouse.warehouseName}
                                    </NavDropdown.Item>
                                ))}
                        </NavDropdown>
                    </Nav.Item>
                </Nav>
            </Row>
            <Row className="mt-3">
                <Col>
                    <h5 className="mt-3">Products</h5>
                    <Table striped bordered hover size="sm" className="table-sm">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Is Unitary</th>
                                <th>Quantity</th>
                            </tr>
                        </thead>
                        <tbody>
                            {activeTab === "all"
                                ? products.map((product, index) => (
                                      <tr key={index}>
                                          <td>{product.productName}</td>
                                          <td>{isUnitary(product)}</td>
                                          <td>Quantity</td>
                                      </tr>
                                  ))
                                : productsOnWarehouse.map((product, index) => (
                                      <tr key={index}>
                                          <td>{product.productName}</td>
                                          <td>{isUnitary(product)}</td>
                                          <td>Quantity</td>
                                      </tr>
                                  ))}
                        </tbody>
                    </Table>
                </Col>
            </Row>
        </Col>
    );
}

export default TransactionClosed;
