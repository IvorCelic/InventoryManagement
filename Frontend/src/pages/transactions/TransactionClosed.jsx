import React, { useEffect, useState } from "react";
import { Col, Nav, Row, Table } from "react-bootstrap";
import { useParams } from "react-router-dom";
import TransactionItemService from "../../services/TransactionItemService";
import useError from "../../hooks/useError";

function TransactionClosed() {
    const [associatedWarehouses, setAssociatedWarehouses] = useState([]);
    const [associatedProducts, setAssociatedProducts] = useState([]);
    const [productsOnWarehouse, setProductsOnWarehouse] = useState([]);
    const [selectedWarehouseId, setSelectedWarehouseId] = useState(null);

    const { showError } = useError();
    const routeParams = useParams();

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
    }

    async function fetchProductsOnWarehouse(warehouseId) {
        const response = await TransactionItemService.GetProductsOnWarehouse(
            "InventoryTransactionItem",
            routeParams.id,
            warehouseId
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setProductsOnWarehouse(response.data);
    }

    useEffect(() => {
        fetchInitialData();
    }, []);

    async function fetchInitialData() {
        await fetchAssociatedProducts();
        await fetchAssociatedWarehouses();
        // Select the "All" tab by default
        setSelectedWarehouseId(null);
        // Fetch products for the "All" tab initially
        setProductsOnWarehouse(associatedProducts);
    }

    function handleTabWarehouseChange(associatedWarehouseId) {
        setSelectedWarehouseId(associatedWarehouseId);
        if (associatedWarehouseId === null) {
            // If "All" tab is selected, set products to associatedProducts
            setProductsOnWarehouse(associatedProducts);
        } else {
            fetchProductsOnWarehouse(associatedWarehouseId);
        }
    }

    return (
        <>
            {associatedWarehouses.length > 0 ? (
                <Col lg={8} md={12} sm={12} className="mt-5 transactionEditContainer">
                    <Row className="horizontal-tabs-container">
                        <Nav className="horizontal-tabs d-none d-md-flex">
                            <Nav.Item key={null}>
                                <Nav.Link
                                    onClick={() => handleTabWarehouseChange(null)}
                                    className={selectedWarehouseId === null ? "active" : ""}
                                >
                                    All
                                </Nav.Link>
                            </Nav.Item>
                            {associatedWarehouses.map((warehouse) => (
                                <Nav.Item key={warehouse.id}>
                                    <Nav.Link
                                        onClick={() => handleTabWarehouseChange(warehouse.id)}
                                        className={
                                            selectedWarehouseId === warehouse.id ? "active" : ""
                                        }
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
                                        <th>Quantity</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {selectedWarehouseId === null
                                        ? associatedProducts.map((product, index) => (
                                              <tr key={index}>
                                                  <td>{product.productName}</td>
                                                  <td>{product.quantity}</td>
                                              </tr>
                                          ))
                                        : productsOnWarehouse.map((product, index) => (
                                              <tr key={index}>
                                                  <td>{product.productName}</td>
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
                    <p>In this inventory transaction, nothing has been added yet.</p>
                </Col>
            )}
        </>
    );
}

export default TransactionClosed;
