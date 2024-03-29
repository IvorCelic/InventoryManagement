import React from "react";
import { Col, Nav, Row, Table } from "react-bootstrap";

function TransactionClosed({
    activeTab,
    handleTabChange,
    warehouses,
    products,
    productsOnWarehouse,
    isUnitary,
}) {
    return (
        <Col lg={8} md={12} sm={12} className="mt-5 transactionEditContainer">
            <Row className="horizontal-tabs-container">
                <Nav className="horizontal-tabs">
                    <Nav.Item>
                        <Nav.Link
                            eventKey="all"
                            active={activeTab === "all"}
                            onClick={() => handleTabChange("all")}
                        >
                            All
                        </Nav.Link>
                    </Nav.Item>
                    {warehouses.map((warehouse) => (
                        <Nav.Item key={warehouse.id}>
                            <Nav.Link
                                eventKey={warehouse.id}
                                active={activeTab === warehouse.id}
                                onClick={() => handleTabChange(warehouse.id)}
                            >
                                {warehouse.warehouseName}
                            </Nav.Link>
                        </Nav.Item>
                    ))}
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
