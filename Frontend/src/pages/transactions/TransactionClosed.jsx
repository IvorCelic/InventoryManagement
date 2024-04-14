import React, { useEffect, useState } from "react";
import { Col, Nav, NavDropdown, Row, Table } from "react-bootstrap";
import { useParams } from "react-router-dom";
import TransactionItemService from "../../services/TransactionItemService";
import useError from "../../hooks/useError";

function TransactionClosed({ activeTab, handleTabChange, isUnitary }) {
    // const navigate = useNavigate();
    const routeParams = useParams();

    const [associatedWarehouses, setAssociatedWarehouses] = useState([]);
    const [associatedProducts, setAssociatedProducts] = useState([]);
    const [productsOnWarehouse, setProductsOnWarehouse] = useState([]);

    const { showError } = useError();

    const [selectedWarehouseName, setSelectedWarehouseName] = useState("Select warehouse");

    const handleWarehouseSelect = (warehouseId, warehouseName) => {
        setSelectedWarehouseName(warehouseName);
        handleTabChange(warehouseId);
    };

    useEffect(() => {
        fetchInitialData();
    }, []);

    async function fetchInitialData() {
        await fetchAssociatedProducts();
        await fetchAssociatedWarehouses();
        await fetchProductsOnWarehouse();
    }

    async function fetchAssociatedProducts() {
        const response = await TransactionItemService.GetProducts(
            "InventoryTransactionItem",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setAssociatedProducts(response.data);
        // console.table(response.data);
    }

    async function fetchAssociatedWarehouses() {
        const response = await TransactionItemService.GetWarehouses(
            "InventoryTransactionItem",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setAssociatedWarehouses(response.data);
        console.log("warehouses ", response.data);
    }

    async function fetchProductsOnWarehouse(warehouseId, productId) {
        const response = await TransactionItemService.GetProductsOnWarehouse(
            routeParams.id,
            warehouseId,
            productId
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setProductsOnWarehouse(response.data);
    }

    return (
        <>
            {associatedWarehouses && associatedWarehouses.length > 0 ? (
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
                                associatedWarehouses.map((warehouse, index) => (
                                    <Nav.Item key={index}>
                                        <Nav.Link
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
                                        onClick={() =>
                                            handleWarehouseSelect("all", "All warehouses")
                                        }
                                    >
                                        All warehouses
                                    </NavDropdown.Item>
                                    {associatedWarehouses &&
                                        associatedWarehouses.map((warehouse, index) => (
                                            <NavDropdown.Item
                                                key={index}
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
                                        ? associatedProducts &&
                                          associatedProducts.map((product, index) => (
                                              <tr key={index}>
                                                  <td>{product.productName}</td>
                                                  <td>{isUnitary(product)}</td>
                                                  <td>{product.quantity}</td>
                                              </tr>
                                          ))
                                        : productsOnWarehouse &&
                                          productsOnWarehouse.map((product, index) => (
                                              <tr key={index}>
                                                  <td>{product.productName}</td>
                                                  <td>{isUnitary(product)}</td>
                                                  <td>{product.quantity}</td>
                                              </tr>
                                          ))}
                                </tbody>
                            </Table>
                        </Col>
                    </Row>
                </Col>
            ) : (
                <Col lg={8} md={12} sm={12} className="mt-5 transactionEditContainer">
                    <p>In this inventory transaction has not been added anything yet.</p>
                </Col>
            )}
        </>
    );
}

export default TransactionClosed;
